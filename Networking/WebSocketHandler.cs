using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using NativeWebSocket;

/// <summary>
///     Класс с дополнительными функциями для очериди
/// </summary>
public static class QueueExtension {
    /// <summary>
    ///     Пытается достать элемент из очереди
    /// </summary>
    /// <param name="queue">Очередь</param>
    /// <param name="res">Элемент</param>
    /// <typeparam name="T">Тип элемента</typeparam>
    /// <returns>true, если очердь была не пустая. Иначе false</returns>
    public static bool TryDequeue<T>(this Queue<T> queue, out T res) {
        if (queue.Count == 0) {
            res = default(T);
            return false;
        }

        res = queue.Dequeue();
        return true;
    }
}

/// <summary>
///     Класс для работы с WebSocket
/// </summary>
public class WebSocketHandler {
    /// <summary>
    ///     Очередь сообщений с клиента на сервер
    /// </summary>
    public Queue<byte[]> clientToServerMessages = new Queue<byte[]>();
    /// <summary>
    ///     Очередь сообщений с сервера на клиент
    /// </summary>
    public Queue<byte[]> serverToClientMessages = new Queue<byte[]>();
    
    /// <summary>
    ///     Ассинхронная задача подключиться к серверу
    /// </summary>
    private Task<WebSocket> connectTask;

    /// <summary>
    ///     Асинхронная задача отправить сообщение на сервер
    /// </summary>
    private Task sendTask = Task.CompletedTask;
    
    /// <summary>
    ///     Вебсокет для общения с сервером
    /// </summary>
    private WebSocket webSocket;

    /// <summary>
    ///     Отключается от сервера
    /// </summary>
    public void Stop() {
        Debug.Log("CLIENT: WebSocket closed");
        webSocket?.Close();
    }
    
    /// <summary>
    ///     Обновляет состояние вебсокета
    /// </summary>
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
        
        byte[] commands;
        while (sendTask.IsCompleted && clientToServerMessages.TryDequeue(out commands)) {
            /* #if UNITY_EDITOR
              Thread.Sleep(60);
            #endif  */
            var res = "";
            for (int i = 0; i < commands.Length; i++) {
                res += commands[i] +" ";
            }
            
            sendTask = webSocket.Send(commands);
        }
    }

    /// <summary>
    ///     Создаёт вебсокет
    /// </summary>
    /// <returns>Асинхронную задачу создания вебсокета</returns>
    private async Task<WebSocket> CreateWebSocket() {
        Debug.Log("CLIENT: Connecting");
        var webSocket = new WebSocket("ws://{host}/ws");
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



    /// <summary>
    ///     Обрабатывает сообщение, пришедшее в вебсокет
    /// </summary>
    /// <param name="data">Массив байт с данными</param>
    private void HandleWebSocketMessage(byte[] data)
    {/*
#if UNITY_EDITOR
        Thread.Sleep(60);
#endif*/
       serverToClientMessages.Enqueue(data);
//       Debug.Log("" + data.Length); //data[0] + data[1] + data[2] + data[3] + data[4]);
    }
    
    
    
 


}
