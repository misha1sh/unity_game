using LightJson;

namespace Game {
    public class MatchInfo {
        public int roomid;
        public string name;
        public int maxPlayersCount;
        public int playersCount;
        
        public MatchInfo(string name, int roomid, int maxPlayersCount, int playersCount) {
            this.playersCount = playersCount;
            this.maxPlayersCount = maxPlayersCount;
            this.roomid = roomid;
            this.name = name;
        }

        public JsonValue ToJson() {
            //return $"{{'roomid':{roomid}, 'name':'{name}', 'maxPlayersCount':{maxPlayersCount}, 'playersCount':{playersCount} }}";
            var res = new JsonObject();
            res["roomid"] = roomid;
            res["name"] = name;
            res["maxPlayersCount"] = maxPlayersCount;
            res["playersCount"] = playersCount;
            return res;
        }

        public static MatchInfo FromJson(JsonValue json) {
            return new MatchInfo(json["name"].AsString, json["roomid"].AsInteger, 
                json["maxPlayersCount"].AsInteger, json["playersCount"].AsInteger);
        }
    }
}