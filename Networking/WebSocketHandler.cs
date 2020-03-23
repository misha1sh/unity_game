using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using NativeWebSocket;
using System.IO;
using Random = UnityEngine.Random;

public class WebSocketHandler
{


    public ConcurrentQueue<byte[]> clientToServerMessages = new ConcurrentQueue<byte[]>();
    public ConcurrentQueue<byte[]> serverToClientMessages = new ConcurrentQueue<byte[]>();

    
    private Task<WebSocket> connectTask;
    private Task recieveTask = Task.CompletedTask;
    private Task sendTask = Task.CompletedTask;
    
    private WebSocket webSocket;

    public void Start() {}

    public void Stop() {
        Debug.Log("CLIENT: WebSocket closed");
        webSocket?.Close();
    }
    
    public void Update() {
        if (webSocket is null || webSocket.State == WebSocketState.Closed) {
            if (connectTask is null) {
                connectTask = CreateWebSocket();
            }

            if (connectTask.IsCompleted) {
                webSocket = connectTask.Result;
                connectTask = null;
            }

            return;
        }

        //if (!socketTask.IsCompleted) return;
       // recieveTask = webSocket.Receive();
        
        byte[] commands;
        while (sendTask.IsCompleted && clientToServerMessages.TryDequeue(out commands)) {
//            Debug.Log("CLIENT: start sending"); 
            /* #if UNITY_EDITOR
              Thread.Sleep(60);
            #endif  */
            sendTask = webSocket.Send(commands);
        }
        

    }


    private async Task<WebSocket> CreateWebSocket()
    {
        Debug.Log("CLIENT: Connecting");
        var webSocket = new WebSocket("ws://localhost:8887/ws");
        lock (webSocket)
        {
            webSocket.OnOpen += () => { Debug.LogWarning("CLIENT: connected"); };
            webSocket.OnClose += e =>
            {
                Debug.LogWarning("CLIENT: disconnected. " + e);
            };
            webSocket.OnMessage += HandleWebSocketMessage;
            webSocket.OnError += msg => { Debug.LogError("CLIENT: Websocket error. " + msg); };
        }


        await webSocket.Connect();
        return webSocket;
    }



    private void HandleWebSocketMessage(byte[] data)
    {/*
#if UNITY_EDITOR
        Thread.Sleep(60);
#endif*/
       serverToClientMessages.Enqueue(data);
//       Debug.Log("" + data.Length); //data[0] + data[1] + data[2] + data[3] + data[4]);
    }


}
