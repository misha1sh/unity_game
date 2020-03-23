using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentCollections;
using Fleck;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using Debug = UnityEngine.Debug;
using Random = System.Random;
using WebSocketServer = Fleck.WebSocketServer;


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

/// <summary>
///     Message formats:
///
///     Client to Server
///         1: simple message
///             - 4 byte message type + data
///             - send to all clients
///         2: uniq message
///             - 4 byte message type + 8 byte uniq code(4 byte code type, 4 byte code num) + data
///             - if uniq code is new, message will be sended to all clients. otherwise, message will be discarded
///         3: ask for new messages
///             - 4 byte message type + 4 byte first message + 4 byte last message
///
///    Server to Client:
///         - 4 byte message id + message data
/// 
/// </summary>

public enum MessageType {
    SimpleMessage = 1,
    UniqMessage = 2,
    AskMessage = 3
}

class GameServerRoom {
    private static Random random = new Random();
    
    ConcurrentHashSet<string> clients;
    List<byte[]> messages;
    ConcurrentHashSet<long> messagesUID;

    private WebSocketSessionManager sessions;

    public GameServerRoom() {
        clients = new ConcurrentHashSet<string>();
        messages = new List<byte[]>();
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
    }
    
    private byte[] CreateMessageWithId(byte[] message, int messageId) {
            
        byte[] messageWithId = new byte[message.Length + 4];
        BitConverter.GetBytes(messageId).CopyTo(messageWithId, 0);
        message.CopyTo(messageWithId, 4);

        return messageWithId;
    }
    

    
    public void BroadcastMessage(byte[] message) {
        lock (messages) {
            int messageId = messages.Count;
            message = CreateMessageWithId(message, messageId);
            messages.Add(message);
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

    public void HandleSimpleMessage(string client, MemoryStream message) {
        BroadcastMessage(message.ReadAllBytes());
    }

    public void HandleUniqMessage(string client, long uid, MemoryStream message) {
        if (messagesUID.Contains(uid)) return;
        messagesUID.Add(uid);
        GameServerConnection con;
        BroadcastMessage(message.ReadAllBytes());
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

    private GameServerRoom room;
    public void Init(GameServerRoom room) {
        this.room = room;
    }
    
    protected override void OnOpen() {
        Debug.LogWarning($"SERVER client {ID} connected");
        room.AddClient(ID, Sessions);
    }
    
    protected override void OnClose(CloseEventArgs e) {
        Debug.LogWarning($"SERVER client {ID} disconnected");
        room.RemoveClient(ID);
    }

    protected override void OnMessage(MessageEventArgs e) {
//        Debug.Log("SERVER got message");
       // Debug.Log("SERVER got message: " + e.RawData.Length + " from " + ID);

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

        switch (messageType) {
            case MessageType.SimpleMessage:
                room.HandleSimpleMessage(ID, memoryStream);
                break;
            
            case MessageType.UniqMessage:
                long uid = messageReader.ReadInt64();
                room.HandleUniqMessage(ID, uid, memoryStream);
                break;
                    
            case MessageType.AskMessage:
                int startIndex = messageReader.ReadInt32();
                int endIndex = messageReader.ReadInt32();
                
                if (memoryStream.ReadAllBytes().Length != 0) {
                    Debug.LogError($"SERVER AskMessage from {ID}" +
                                   $" has incorrect length: {memoryStream.Length}");
                }
                room.HandleAskMessage(ID, startIndex, endIndex);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
     
    
    }
}

public class Server : MonoBehaviour
{
    // Start is called before the first frame update
#if UNITY_EDITOR
    private int safeChannelId;
    private Thread serverThread;
    void Start() {
        serverThread = new Thread(RunServer);
        serverThread.IsBackground = true;
        serverThread.Start();
    }
    

    private HttpServer httpServer;
    void RunServer() {
        
        Debug.Log("SERVER starting...");
        httpServer = new HttpServer ("http://0.0.0.0:8887");
        
        var room = new GameServerRoom();
        httpServer.AddWebSocketService<GameServerConnection>("/ws", 
            connection => connection.Init(room));
        
        httpServer.Start();
        Debug.Log("SERVER started");

     
    }



    private void OnApplicationQuit()
    {
        httpServer?.Stop();

        if (serverThread != null && serverThread.IsAlive)
        {
            serverThread.Abort();
            serverThread = null;
        }

    
    }


    void Update()
    {
  
    }
#endif
}
