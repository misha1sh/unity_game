using System.Collections.Generic;
using LightJson;

namespace Game {
    public class MatchInfo {
        public int roomid;
        public string name;
        public int maxPlayersCount;
        public List<string> players;

        // 0 -- waiting for players
        // 1 -- already started
        public int state;
        
        public MatchInfo(string name, int roomid, int maxPlayersCount, List<string> players, int state) {
            this.maxPlayersCount = maxPlayersCount;
            this.roomid = roomid;
            this.name = name;
            this.players = players;
            this.state = state;
        }

        public JsonValue ToJson() {
            //return $"{{'roomid':{roomid}, 'name':'{name}', 'maxPlayersCount':{maxPlayersCount}, 'playersCount':{playersCount} }}";
            var res = new JsonObject();
            res["roomid"] = roomid;
            res["name"] = name;
            res["maxPlayersCount"] = maxPlayersCount;
            res["state"] = state;
            /*var playersJson = new JsonArray();
            foreach (var playerName in players) {
                playersJson.Add(new JsonValue(playerName));    
            }

            res["players"] = playersJson;*/
            return res;
        }

        public static MatchInfo FromJson(JsonValue json) {
            var players = new List<string>();
            foreach (var value in json["players"].AsJsonArray) {
                players.Add(value.AsString);
            }
            return new MatchInfo(json["name"].AsString, json["roomid"].AsInteger, 
                json["maxPlayersCount"].AsInteger, players, json["state"]);
        }
    }
}