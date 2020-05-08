using System;
using System.Text;
using CommandsSystem;

namespace JsonRequest {
    public class Response : ICommand {
        public int _id;
        public LightJson.JsonValue json;

        private Response(int _id, LightJson.JsonValue json) {
            this._id = _id;
            this.json = json;
        } 
        
        
        private static Response DeserializeLittleEndian(byte[] arr) { 
            var jsonString = Encoding.UTF8.GetString(arr);
            var json = LightJson.JsonValue.Parse(jsonString);
            return new Response(json["_id"].AsInteger, json);
        }

        public static Response Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }

        public void Run() {
            RequestsManager.openedRequests[_id].GotResponse(this);
        }

        public override string ToString() {
            return "JSON: " + json.ToString();
        }
    }
}