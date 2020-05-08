using System;
using LightJson;
using Networking;
using UnityEngine;

namespace JsonRequest {
    public enum RequestType {
        GetMatchesList,
        CreateMatch,
        JoinMatch
    }
    
    public class Request {
        public int room;
        public int id;
        public string json;

        public bool isCompleted = false;
        public Action<JsonValue> callback;
        
        public float timeoutTime;
        public float timeout;
        public int retries;
        
        public Request(int room, RequestType type, JsonValue json, Action<JsonValue> callback, float timeout=4, int retries=2) {
            this.room = room;
            json["_id"] = this.id = ObjectID.RandomID;
            json["_type"] = type.ToString();
            this.json = json.ToString();
            this.callback = callback;
            this.timeout = timeout;
            this.retries = retries;
        }

        public Request(ClientCommandsRoom room, RequestType type, JsonValue json, Action<JsonValue> callback, float timeout=4, int retries=2) :
            this(room.roomID, type, json, callback, timeout, retries) {}
        
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

        public void GotResponse(Response response) {
            isCompleted = true;
            RequestsManager.openedRequests.Remove(id);
            callback(response.json);
        }
    }
}