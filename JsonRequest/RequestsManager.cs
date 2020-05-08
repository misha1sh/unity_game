using System.Collections.Generic;
using Networking;
using UnityEngine;

namespace JsonRequest {
    public class RequestsManager {
        public static SortedDictionary<int, Request> openedRequests = new SortedDictionary<int, Request>();

        public static void Update() {
            foreach (var request in openedRequests.Values) {
                request.Update();
            }
        }

        public static void Send(Request request) {
            if (!openedRequests.ContainsKey(request.id))
                openedRequests.Add(request.id, request);
            request.timeoutTime = Time.time + request.timeout;
            CommandsHandler.RoomById(request.room).RunJsonMessage(request.json);
        }
    }
}