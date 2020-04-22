using UnityEngine;

#if UNITY_EDITOR
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using ConcurrentCollections;
using UnityEngine.Assertions;
using WebSocketSharp;
using WebSocketSharp.Server;
using Debug = UnityEngine.Debug;
using Random = System.Random;
#endif

/// <summary>
///     Message formats:
///
///     Client to Server
///         1: simple message
///             - 4 byte message type + 4 byte room number + 1 byte need store + data
///             - send to all clients
///         2: uniq message
///             - 4 byte message type + 4 byte room number + 1 byte need store + 8 byte uniq code(4 byte code type, 4 byte code num) + data
///             - if uniq code is new, message will be send to all clients. otherwise, message will be discarded
///         3: ask for new messages
///             - 4 byte message type + 4 byte room number + 4 byte first message + 4 byte last message
///         4: join game room. if room does not exists, it must be created
///             - 4 byte message type + 4 byte room number
///             - optional: autoremove player if he not doing anything in room for some time (ping - pong)
///         5: leave game room. if last player leaved room, it must be deleted;
///             - 4 byte message type + 4 byte room number
/// 
///    Server to Client:
///         - 4 byte message id + 4 byte room number + message data
///         - optional ping: 4 byte message id + 4 byte data
/// 
/// </summary>

public enum MessageType {
    SimpleMessage = 1,
    UniqMessage = 2,
    AskMessage = 3,
    JoinGameRoom = 4,
    LeaveGameRoom = 5
};


#if UNITY_EDITOR
public static class ArrayExtension {
    public static T[] SubArray<T>(this T[] data, long index)
    {
        T[] result = new T[data.Length - index];
        Array.Copy(data, index, result, 0, data.Length - index);
        return result;
    }
}

public static class MemoryStreamExtension {
    public static byte[] ReadAllBytes(this MemoryStream memoryStream) {
        return memoryStream.ToArray().SubArray(memoryStream.Position);

    }
}

/*
public static class BinaryReaderExtension {
    public static byte[] ReadAllBytes(this BinaryReader binaryReader) {
        return binaryReader.ReadBytes(int.MaxValue);
        
    }
}*/



internal class GameServerRoom {
    private static Random random = new Random();


    public int id;
    public int clientsCount => clients.Count;
    ConcurrentHashSet<string> clients;

    private int lastMessageId = -1;
    
    OrderedDictionary importantMessages;
    OrderedDictionary notImportantMessages;
    ConcurrentHashSet<long> messagesUID;

    private WebSocketSessionManager sessions;

    public GameServerRoom(int id) {
        this.id = id;
        clients = new ConcurrentHashSet<string>();
        importantMessages = new OrderedDictionary();
        notImportantMessages = new OrderedDictionary();
        messagesUID = new ConcurrentHashSet<long>();
    }
    
    public void AddClient(string id, WebSocketSessionManager sessions) {
        this.sessions = sessions;
        this.clients.Add(id);
    }

    public void RemoveClient(string id) {
        Assert.IsTrue(
            clients.TryRemove(id)
        );
        if (clientsCount == 0)
            Assert.IsTrue(Server.rooms.TryRemove(this.id, out _));
    }
    
    private byte[] CreateMessageWithId(byte[] message, int messageId, int roomId) {
            
        byte[] messageWithId = new byte[message.Length + 8];
        BitConverter.GetBytes(messageId).CopyTo(messageWithId, 0);
        BitConverter.GetBytes(roomId).CopyTo(messageWithId, 4);
        message.CopyTo(messageWithId, 8);

        return messageWithId;
    }
    

    
    public void BroadcastMessage(byte[] message, byte needStore) {
        if (needStore != 0) {
            lock (messages) {
                int messageId = messages.Count;
                message = CreateMessageWithId(message, messageId, id);
                messages.Add(message);
            }
        } else {
            message = CreateMessageWithId(message, -1, id);
        }


        lock (clients) {
            for (int i = 0; i < 1; i++) { // random.Next(0, 2)
                foreach (var client in clients) {

                    int i1 = i;
                    bool completed = false;
                    sessions.SendToAsync(message, client, b => {
               //         Debug.Log("sended " + i1);
                        completed = true;
                    });
                    while (!completed) ;
                } 
            }
       
        }
    }

    public void HandleSimpleMessage(string client, MemoryStream message, byte needStore) {
        BroadcastMessage(message.ReadAllBytes(), needStore);
    }

    public void HandleUniqMessage(string client, long uid, MemoryStream message, byte needStore) {
        if (messagesUID.Contains(uid)) return;
        messagesUID.Add(uid);
        GameServerConnection con;
        BroadcastMessage(message.ReadAllBytes(), needStore);
    }

    public void HandleAskMessage(string client, int startIndex, int endIndex) {
        lock (messages) {
            if (startIndex < 0 || startIndex > endIndex || endIndex >= messages.Count && endIndex != int.MaxValue) {
                Debug.LogError($"SERVER AskMessage from {client} is incorrect. " +
                               $"startIndex: {startIndex}, endIndex: {endIndex}, messages.Count: {messages.Count}");
                return;
            }

            if (endIndex == int.MaxValue)
                endIndex = messages.Count - 1;
            
            for (int i = startIndex; i <= endIndex; i++) {
                sessions.SendTo(messages[i], client);
            }
        }
    }
}

class GameServerConnection : WebSocketBehavior {

    private List<GameServerRoom> rooms = new List<GameServerRoom>();
    
    
    protected override void OnOpen() {
        Debug.LogWarning($"SERVER client {ID} connected");
       // room.AddClient(ID, Sessions);
    }
    
    protected override void OnClose(CloseEventArgs e) {
        Debug.LogWarning($"SERVER client {ID} disconnected");
        foreach (var room in rooms) {
            room.RemoveClient(ID);
        }
    }

    private GameServerRoom RoomById(int id) {
        foreach (var room in rooms) {
            if (room.id == id) return room;
        }
        return null;
    }

    private void HandleMessage(MessageEventArgs e) {
        if (!e.IsBinary) {
            Debug.LogError("SERVER got not binary message.");
            return;
        }
                
        var memoryStream = new MemoryStream(e.RawData);
        var messageReader = new BinaryReader(memoryStream);

        int messageTypeOrd = messageReader.ReadInt32();

        if (!Enum.IsDefined(typeof(MessageType), messageTypeOrd)) {
            Debug.LogError($"SERVER got message with unknown message type: {messageTypeOrd}");
            return;
        }

        var messageType = (MessageType) messageTypeOrd;
        var roomId = messageReader.ReadInt32();

        if (messageType == MessageType.JoinGameRoom) {
            Debug.LogError($"SERVER JoinGameRoom from {ID}" +
                           $" has incorrect length: {memoryStream.Length}");
            
            if (!Server.rooms.ContainsKey(roomId))
                Server.rooms[roomId] = new GameServerRoom(roomId);
            Server.rooms[roomId].AddClient(ID, Sessions);
            return;
        }
        
        GameServerRoom room = RoomById(roomId);
        if (room is null) {
            Debug.LogError($"SERVER: client {ID} trying to send {messageType} to room {roomId} before joining it");
            return;
        }
        
        switch (messageType) {
            case MessageType.SimpleMessage: {
                byte needStore = messageReader.ReadByte();
                room.HandleSimpleMessage(ID, memoryStream, needStore);
                break;
            }

            case MessageType.UniqMessage: {
                byte needStore = messageReader.ReadByte();
                long uid = messageReader.ReadInt64();
                room.HandleUniqMessage(ID, uid, memoryStream, needStore);
                break;
            }
                
            case MessageType.AskMessage:
                int startIndex = messageReader.ReadInt32();
                int endIndex = messageReader.ReadInt32();
                
                if (memoryStream.ReadAllBytes().Length != 0) {
                    Debug.LogError($"SERVER AskMessage from {ID}" +
                                   $" has incorrect length: {memoryStream.Length}");
                }
                room.HandleAskMessage(ID, startIndex, endIndex);
                break;
            
            case MessageType.LeaveGameRoom:
                if (memoryStream.ReadAllBytes().Length != 0) {
                    Debug.LogError($"SERVER LeaveGameRoom from {ID}" +
                                   $" has incorrect length: {memoryStream.Length}");
                }
                room.RemoveClient(ID);
                break;
            
            default:
                throw new ArgumentOutOfRangeException($"{messageType}");
        }
    }

    protected override void OnMessage(MessageEventArgs e) {
//        Debug.Log("SERVER got message");
       // Debug.Log("SERVER got message: " + e.RawData.Length + " from " + ID);
      /* Task.Run(async () => {
          // await Task.Delay(100);-*/
           HandleMessage(e);
      // });
    }
}


public class Server : MonoBehaviour
{
    internal static ConcurrentDictionary<int, GameServerRoom> rooms = new ConcurrentDictionary<int, GameServerRoom>();
    
    
    // Start is called before the first frame update

    private int safeChannelId;
    private static Thread serverThread;
    void Start() {
        Debug.LogError("start");
        if (serverThread != null && serverThread.IsAlive) return;
        
        serverThread = new Thread(RunServer);
        serverThread.IsBackground = true;
        serverThread.Start();
    }
    

    private static HttpServer httpServer;
    void RunServer() {
        
        Debug.Log("SERVER starting...");
        
        if (httpServer != null && httpServer.IsListening) return;
        httpServer = new HttpServer("http://0.0.0.0:8887");

        httpServer.AddWebSocketService<GameServerConnection>("/ws",
            connection =>{});
        
        try {
            httpServer.Start();
        }
        catch (Exception ex) {
            Debug.LogException(ex);
        }

        Debug.Log("SERVER started");

     
    }



    private void OnApplicationQuit() {
        Debug.LogError("appquit");
        httpServer?.Stop();

        if (serverThread != null && serverThread.IsAlive)
        {
            serverThread.Abort();
            serverThread = null;
        }

    
    }

    
}
#else
public class Server : MonoBehaviour { }
#endif
