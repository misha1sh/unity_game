using System.Collections.Generic;
using Networking;
using UnityEngine;

namespace JsonRequest {
    /// <summary>
    ///     Класс для управления JSON запросами
    /// </summary>
    public static class RequestsManager {
        /// <summary>
        ///     Словарь с запросами
        /// </summary>
        public static SortedDictionary<int, Request> openedRequests = new SortedDictionary<int, Request>();

        /// <summary>
        ///     Обновляет состояния запросов
        /// </summary>
        public static void Update() {
            foreach (var request in openedRequests.Values) {
                request.Update();
            }
        }

        /// <summary>
        ///     Отправляет запрос на сервер
        /// </summary>
        /// <param name="request">Запрос</param>
        public static void Send(Request request) {
            if (!openedRequests.ContainsKey(request.id))
                openedRequests.Add(request.id, request);
            request.timeoutTime = Time.time + request.timeout;
            CommandsHandler.RoomById(request.room).RunJsonMessage(request.json);
        }
    }
}