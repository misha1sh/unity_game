using UnityEngine;
using System;

#if UNITY_EDITOR
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
///             - 1 byte message type + 4 byte room number + 1 flags + data
///             - send to all clients
///         2: uniq message
///             - 1 byte message type + 4 byte room number + 1 flags + 8 byte uniq code(4 byte code type, 4 byte code num) + data
///             - if uniq code is new, message will be send to all clients. otherwise, message will be discarded
///         3: ask for new messages
///             - 1 byte message type + 4 byte room number + 1 flags + 4 byte first message + 4 byte last message
///         4: join game room. if room does not exists, it must be created
///             - 1 byte message type + 4 byte room number + 1 flags
///             - optional: autoremove player if he not doing anything in room for some time (ping - pong)
///         5: leave game room. if last player leaved room, it must be deleted;
///             - 1 byte message type + 4 byte room number + 1 flags
/// 
///    Server to Client:
///         - 4 byte message id + 4 byte room number + message data
///         if message id == -1 means client need to do command instantly
///         - optional ping: 4 byte message id + 4 byte data
/// 
/// </summary>

public enum MessageType : byte {
    SimpleMessage = 1,
    UniqMessage = 2,
    AskMessage = 3,
    JoinGameRoom = 4,
    LeaveGameRoom = 5,
    JSON = 6,
};

[Flags] 
public enum MessageFlags : byte {
    NONE = 0,
    IMPORTANT = 1 << 0,
    SEND_ONLY_IMPORTANT = 1 << 1
}

public class Server : MonoBehaviour { } 
/*
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
/*


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
    

    
    public void BroadcastMessage(byte[] message, MessageFlags flags) {
        int messageId = lastMessageId + 1;
        lastMessageId++;

        message = CreateMessageWithId(message, messageId, id);
        
        if ((flags & MessageFlags.IMPORTANT) != 0 ) {
            lock (importantMessages) {
                importantMessages.Add(messageId, message);
            }
        } else {
            lock (notImportantMessages) {
                notImportantMessages.Add(messageId, message);                
                if (notImportantMessages.Count > 1000)
                    notImportantMessages.RemoveAt(0);
            }
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
                    if (flags.HasFlag(MessageFlags.IMPORTANT) || true)
                        UberDebug.LogChannel("SERVER", $"server{id}->client{client} {message.Length} {flags}");
                } 
            }
       
        }
    }

    public void HandleSimpleMessage(string client, MemoryStream message, MessageFlags needStore) {
        BroadcastMessage(message.ReadAllBytes(), needStore);
    }

    public void HandleUniqMessage(string client, long uid, MemoryStream message, MessageFlags needStore) {
        if (messagesUID.Contains(uid)) return;
        messagesUID.Add(uid);
        BroadcastMessage(message.ReadAllBytes(), needStore);
    }

    public void HandleAskMessage(string client, int startIndex, int endIndex, MessageFlags flags) {
        lock (importantMessages)
        lock (notImportantMessages) {
            if (startIndex < 0 || startIndex > endIndex || endIndex > lastMessageId && endIndex != int.MaxValue) {
                Debug.LogError($"SERVER AskMessage from {client} is incorrect. " +
                               $"startIndex: {startIndex}, endIndex: {endIndex}, messages. lastMessageId: {lastMessageId}");
                return;
            }

            if (endIndex == int.MaxValue)
                endIndex = lastMessageId;

            if (flags.HasFlag(MessageFlags.SEND_ONLY_IMPORTANT)) {
                for (int i = startIndex; i <= endIndex; i++) {
                    if (importantMessages.Contains(i)) {
                        sessions.SendTo((byte[]) importantMessages[i], client);
                    }
                }
            } else {
                for (int i = startIndex; i <= endIndex; i++) {
                    if (importantMessages.Contains(i)) {
                        sessions.SendTo((byte[])importantMessages[i], client);
                    } else {
                        sessions.SendTo((byte[])notImportantMessages[i], client);
                    }
                }
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

        byte messageTypeOrd = messageReader.ReadByte();


        if (!Enum.IsDefined(typeof(MessageType), messageTypeOrd)) {
            Debug.LogError($"SERVER got message with unknown message type: {messageTypeOrd}");
            return;
        }

        var messageType = (MessageType) messageTypeOrd;
        var roomId = messageReader.ReadInt32();
        MessageFlags flags = (MessageFlags) messageReader.ReadByte();
        if (flags.HasFlag(MessageFlags.IMPORTANT) || messageType!=MessageType.SimpleMessage || true)
            UberDebug.LogChannel("SERVER", $"client{ID}->SERVER: {messageType}  {roomId}");

        
        if (messageType == MessageType.JoinGameRoom) {
            if (memoryStream.ReadAllBytes().Length != 0) {
                Debug.LogError($"SERVER JoinGameRoom from {ID}" +
                               $" has incorrect length: {memoryStream.Length}");
            }

            GameServerRoom joinRoom;
            if (!Server.rooms.TryGetValue(roomId, out joinRoom)) {
                joinRoom = Server.rooms[roomId] = new GameServerRoom(roomId);
            }
            joinRoom.AddClient(ID, Sessions);
            rooms.Add(joinRoom);
            return;
        }
        
        GameServerRoom room = RoomById(roomId);

        if (room is null) {
            UberDebug.LogErrorChannel("SERVER", $"SERVER: client {ID} trying to send {messageType} to room {roomId} before joining it");
            return;
        }

        
        
        
        switch (messageType) {
            case MessageType.SimpleMessage: {
                
                room.HandleSimpleMessage(ID, memoryStream, flags);
                break;
            }

            case MessageType.UniqMessage: {
                long uid = messageReader.ReadInt64();
                room.HandleUniqMessage(ID, uid, memoryStream, flags);
                break;
            }
                
            case MessageType.AskMessage:
                int startIndex = messageReader.ReadInt32();
                int endIndex = messageReader.ReadInt32();
                
                if (memoryStream.ReadAllBytes().Length != 0) {
                    Debug.LogError($"SERVER AskMessage from {ID}" +
                                   $" has incorrect length: {memoryStream.Length}");
                }
                room.HandleAskMessage(ID, startIndex, endIndex, flags);
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
*/