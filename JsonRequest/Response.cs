using System;
using System.Text;
using CommandsSystem;
using Events;
using Game;

namespace JsonRequest {
    /// <summary>
    ///     Класс, представляющий ответ сервера на JSON запрос
    /// </summary>
    public class Response : ICommand {
        /// <summary>
        ///     ID запроса / ответа
        /// </summary>
        public int _id;
        /// <summary>
        ///     Ответ в JSON формате
        /// </summary>
        public LightJson.JsonValue json;

        /// <summary>
        ///     Конструктор ответа на запрос
        /// </summary>
        /// <param name="_id">ID ответа на запрос</param>
        /// <param name="json">Тело ответа в формате JSON</param>
        private Response(int _id, LightJson.JsonValue json) {
            this._id = _id;
            this.json = json;
        } 
        

        /// <summary>
        ///     Десериализует ответ на запрос из бинарного сообщения
        /// </summary>
        /// <param name="arr">Массив с ответом на запрос</param>
        /// <returns>Десериализованный ответ с свервера</returns>
        public static Response Deserialize(byte[] arr) {
            var jsonString = Encoding.UTF8.GetString(arr);
            var json = LightJson.JsonValue.Parse(jsonString);
            return new Response(json["_id"].AsInteger, json);
        }

        /// <summary>
        ///     Обрабатывает ответ с сервера
        /// </summary>
        public void Run() {
            switch (_id) {
                case -1: // changed match players
                    MatchesManager.HandleJsonMatchChanged(json);
                    break;
                default:
                    RequestsManager.openedRequests[_id].GotResponse(this);
                    break;
            }
        }

        /// <summary>
        ///     Выводит ответ с сервера в строку
        /// </summary>
        /// <returns>Ответ с сервера в виде строки</returns>
        public override string ToString() {
            return "JSON: " + json.ToString();
        }
    }
}