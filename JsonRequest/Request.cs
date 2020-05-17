using System;
using LightJson;
using Networking;
using UnityEngine;

namespace JsonRequest {
    /// <summary>
    ///     Тип JSON запроса к серверу
    /// </summary>
    public enum RequestType {
        GetMatchesList,
        CreateMatch,
        JoinMatch,
        ChangeMatchState
    }
    
    /// <summary>
    ///     Класс для JSON запроса к серверу
    /// </summary>
    public class Request {
        /// <summary>
        ///     Комната для запроса
        /// </summary>
        public int room;
        /// <summary>
        ///     ID запроса
        /// </summary>
        public int id;
        /// <summary>
        ///     Запрос в JSON формате
        /// </summary>
        public string json;

        /// <summary>
        ///     Выполнен ли запрос
        /// </summary>
        public bool isCompleted = false;
        /// <summary>
        ///     Действие, которое нужно сделать после получения ответа
        /// </summary>
        public Action<JsonValue> callback;
        
        /// <summary>
        ///     Время в которое запрос будет признан невыполненным
        /// </summary>
        public float timeoutTime;
        /// <summary>
        ///     Время, через которое запрос будет признан невыполненным
        /// </summary>
        public float timeout;
        /// <summary>
        ///     Количество попыток повторить запрос
        /// </summary>
        public int retries;
        
        /// <summary>
        ///     Конструктор JSON запроса
        /// </summary>
        /// <param name="room">ID комнаты для запроса</param>
        /// <param name="type">Тип запроса</param>
        /// <param name="json">Запрос в JSON формате</param>
        /// <param name="callback">Действие, которое нужно сделать после получения ответа</param>
        /// <param name="timeout">Время, через которое запрос будет признан невыполненным</param>
        /// <param name="retries">Количество попыток повторить запрос</param>
        public Request(int room, RequestType type, JsonValue json, Action<JsonValue> callback, float timeout=4, int retries=2) {
            this.room = room;
            json["_id"] = this.id = ObjectID.RandomID;
            json["_type"] = type.ToString();
            this.json = json.ToString();
            this.callback = callback;
            this.timeout = timeout;
            this.retries = retries;
        }

        /// <summary>
        ///     Конструктор JSON запроса
        /// </summary>
        /// <param name="room">Комната для запроса</param>
        /// <param name="type">Тип запроса</param>
        /// <param name="json">Запрос в JSON формате</param>
        /// <param name="callback">Действие, которое нужно сделать после получения ответа</param>
        /// <param name="timeout">Время, через которое запрос будет признан невыполненным</param>
        /// <param name="retries">Количество попыток повторить запрос</param>
        public Request(ClientCommandsRoom room, RequestType type, JsonValue json, Action<JsonValue> callback, float timeout=4, int retries=2) :
            this(room.roomID, type, json, callback, timeout, retries) {}
        
        /// <summary>
        ///     Обновляет состояние запроса
        /// </summary>
        public void Update() {
            if (Time.time > timeoutTime) {
                if (retries <= 0) {
                    RequestsManager.openedRequests.Remove(id);
                    throw new Exception($"Timeout error for request#{id} to room {room} {json}");
                }
                retries--;
                RequestsManager.Send(this);
            }
        }

        /// <summary>
        ///     Обрабатывает получение ответа на запрос
        /// </summary>
        /// <param name="response">Ответ на запрос</param>
        public void GotResponse(Response response) {
            isCompleted = true;
            RequestsManager.openedRequests.Remove(id);
            callback(response.json);
        }
    }
}