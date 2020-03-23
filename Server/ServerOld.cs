/*using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentCollections;
using Fleck;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;


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



public class ServerOld : MonoBehaviour
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

    

    ConcurrentHashSet<IWebSocketConnection> clients;
    List<byte[]> messages;
    ConcurrentHashSet<long> messagesUID;

    private byte[] CreateMessageWithId(byte[] message, int messageId) {
            
        byte[] messageWithId = new byte[message.Length + 4];
        BitConverter.GetBytes(messageId).CopyTo(messageWithId, 0);
        message.CopyTo(messageWithId, 4);

        return messageWithId;
    }
    
    
    private void BroadcastMessage(byte[] message) {
        lock (messages) {
            int messageId = messages.Count;
            message = CreateMessageWithId(message, messageId);
            messages.Add(message);
        }

        
//        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < 1000; i++) {
            foreach (var otherClient in clients)
            {
                var task = otherClient.Send(message);
                task.Wait();
            }
        }
//        sw.Stop();
        
//        Debug.Log("sending in " + sw.ElapsedMilliseconds + "ms");
        
    }

    void RunServer() {
        clients = new ConcurrentHashSet<IWebSocketConnection>();
        messages = new List<byte[]>();
        messagesUID = new ConcurrentHashSet<long>();

        FleckLog.Level = LogLevel.Error;
        Debug.Log("SERVER starting websocket server");
        var ip = "ws://0.0.0.0:8887";
        var server = new WebSocketServer(ip);



        server.Start(client => {
            Debug.Log($"SERVER connecting client {client.ConnectionInfo.Id}");

            
            client.OnOpen = () => {
                clients.Add(client);

                Debug.Log($"SERVER client {client.ConnectionInfo.Id} connected");
            };

            client.OnClose = () => {
                Assert.IsTrue(
                    clients.TryRemove(client)
                    );
                Debug.Log($"SERVER client {client.ConnectionInfo.Id} disconnected");
            };
            
            client.OnBinary = message => {
                Debug.Log($"SERVER got message: {message.Length} from {client.ConnectionInfo.Id}");

                
                var memoryStream = new MemoryStream(message);
                var messageReader = new BinaryReader(memoryStream);

                int messageTypeOrd = messageReader.ReadInt32();

                if (!Enum.IsDefined(typeof(MessageType), messageTypeOrd)) {
                    Debug.LogError($"SERVER got message with unknown message type: {messageTypeOrd}");
                    return;
                }

                var messageType = (MessageType) messageTypeOrd;

                switch (messageType) {
                    case MessageType.SimpleMessage:
                        BroadcastMessage(memoryStream.ReadAllBytes());                        
                        break;
                    
                    case MessageType.UniqMessage:
                        long uid = messageReader.ReadInt64();
                        
                        if (messagesUID.Contains(uid)) return;
                        messagesUID.Add(uid);

                        BroadcastMessage(memoryStream.ReadAllBytes());
                        
                        break;
                    
                    case MessageType.AskMessage:


                        int startIndex = messageReader.ReadInt32();
                        int endIndex = messageReader.ReadInt32();

                        if (memoryStream.ReadAllBytes().Length != 0) {
                            Debug.LogError($"SERVER AskMessage from {client.ConnectionInfo.Id}" +
                                           $" has incorrect length: {message.Length}");
                        }

                        lock (messages) {
                            if (startIndex < 0 || startIndex > endIndex || endIndex >= messages.Count) {
                                Debug.LogError($"SERVER AskMessage from {client.ConnectionInfo.Id} is incorrect. " +
                                               $"startIndex: {startIndex}, endIndex: {endIndex}, messages.Count: {messages.Count}");
                                return;
                            }

                            for (int i = startIndex; i <= endIndex; i++) {
                                client.Send(messages[i]);
                            }
                        }
          
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };

        });
    }



    private void OnApplicationQuit()
    {
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
*/