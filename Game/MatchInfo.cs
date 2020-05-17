using System.Collections.Generic;
using LightJson;

namespace Game {
    /// <summary>
    ///     Класс для информации о матче
    /// </summary>
    public class MatchInfo {
        /// <summary>
        ///     id комнаты, в которой проводится матч
        /// </summary>
        public int roomid;
        /// <summary>
        ///     Имя матча
        /// </summary>
        public string name;
        /// <summary>
        ///     Максимальное количество игроков в матче
        /// </summary>
        public int maxPlayersCount;
        /// <summary>
        ///     Список игроков в матче
        /// </summary>
        public List<string> players;

        /// <summary>
        ///     Текущее состояние матча. 0 означает, что матч ещё не начался. 1 -- начался.
        /// </summary>
        public int state;

        /// <summary>
        ///     Конструктор информации о матче
        /// </summary>
        /// <param name="name">Имя матча</param>
        /// <param name="roomid">id комнаты, в которой проводится матч</param>
        /// <param name="maxPlayersCount">Максимальное количество игроков в матче</param>
        /// <param name="players">Список игроков в матче</param>
        /// <param name="state">Текущее состояние матча</param>
        public MatchInfo(string name, int roomid, int maxPlayersCount, List<string> players, int state) {
            this.maxPlayersCount = maxPlayersCount;
            this.roomid = roomid;
            this.name = name;
            this.players = players;
            this.state = state;
        }

        /// <summary>
        ///     Записывает информацию о матче в формате JSON
        /// </summary>
        /// <returns>Информациюю о матче в формате JSON</returns>
        public JsonValue ToJson() {
            //return $"{{'roomid':{roomid}, 'name':'{name}', 'maxPlayersCount':{maxPlayersCount}, 'playersCount':{playersCount} }}";
            var res = new JsonObject();
            res["roomid"] = roomid;
            res["name"] = name;
            res["maxPlayersCount"] = maxPlayersCount;
            res["state"] = state;
            return res;
        }

        /// <summary>
        ///     Создаёт информацию о матче из JSON-объекта
        /// </summary>
        /// <param name="json">JSON-объект</param>
        /// <returns>Информацию о матче</returns>
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