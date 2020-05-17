using System.Collections.Generic;
using CommandsSystem;
using CommandsSystem.Commands;
using Events;
using GameMode;
using JsonRequest;
using LightJson;
using Networking;
using UI;
using UnityEngine;

namespace Game {
    /// <summary>
    ///     Класс для управления и присоединения к игровым матчам
    /// </summary>
    public static class MatchesManager {
        /// <summary>
        ///     Перечисление возможных состояний менеджера матчей
        /// </summary>
        public enum STATE {
            START_FIND,
            WAIT_MATCHES_INFO,
            WAIT_PLAYERS_IN_MATCH,
            WAIT_STARTING_MATCH,
            PLAYING_MATCH
        }
        
        /// <summary>
        ///     Переменная для хранения состояния менеджера матчей
        /// </summary>        
        private static STATE _state = STATE.START_FIND;

        /// <summary>
        ///     Состояние менеджера матчей
        /// </summary>
        private static STATE state {
            get => _state;
            set {
                _state = value;
                DebugUI.debugText[1] = $"MatchesManager.State: {state}.";
            }
        }

        /// <summary>
        ///     Информация о текущем матче
        /// </summary>
        public static MatchInfo currentMatch;

        /// <summary>
        ///     Создаёт матч с заданными параметрами
        /// </summary>
        /// <param name="matchInfo">Параметры создаваемого матча</param>
        private static void CreateMatch(MatchInfo matchInfo) {
            UberDebug.LogChannel("Matchmaking", "Creating match with params: " + matchInfo.ToJson().ToString());
            RequestsManager.Send(new Request(CommandsHandler.matchmakingRoom,  RequestType.CreateMatch, 
                matchInfo.ToJson(), response => {
                    if (response["result"] != "success") {
                        UberDebug.LogErrorChannel("Matchmaking", "Error in json request: " + response.ToString());
                        state = STATE.START_FIND;
                        return;
                    }
                    UberDebug.LogChannel("Matchmaking", "Created match");
                    JoinMatch(matchInfo.roomid);
                } ));
        }

        /// <summary>
        ///     Присоединяется к заданному матчу
        /// </summary>
        /// <param name="matchid">ID матча, к которому нужно присоединиться</param>
        private static void JoinMatch(int matchid) {
            UberDebug.LogChannel("Matchmaking", "Joining match#" + matchid);
            var json = new JsonObject();
            json["matchid"] = matchid;
            json["name"] = PlayersManager.mainPlayer.name;
            RequestsManager.Send(new Request(CommandsHandler.matchmakingRoom, RequestType.JoinMatch,
                json, response => {
                    if (response["result"] != "success") {
                        UberDebug.LogErrorChannel("Matchmaking", "Error in json request: " + response.ToString());
                        state = STATE.START_FIND;
                        return;
                    }
                    UberDebug.LogChannel("Matchmaking", "Joined match#" + matchid);
                    
                    state = STATE.WAIT_PLAYERS_IN_MATCH;
                    
                    CommandsHandler.gameRoom = new ClientCommandsRoom(matchid);
                    
                    HandleJsonMatchChanged(response);
                    
                }));
        }

        /// <summary>
        ///     Получает список матчей и автоматически присоединяется к одному из возможных
        /// </summary>
        private static void GetMatchesList() {
            RequestsManager.Send(new Request(CommandsHandler.matchmakingRoom, RequestType.GetMatchesList, new JsonObject(), 
                response => {
                    if (response["result"] != "success") {
                        UberDebug.LogErrorChannel("Matchmaking", "Error in json request: " + response.ToString());
                        state = STATE.START_FIND;
                        return;
                    }

                    UberDebug.LogChannel("Matchmaking", "Available matches: " + response["matches"].ToString());
                    foreach (var matchJson in response["matches"].AsJsonArray) {
                        var match = MatchInfo.FromJson(matchJson);
                        if (match.players.Count < match.maxPlayersCount) {
                            JoinMatch(match.roomid);
                            return;
                        }
                    }

                    int mid = ObjectID.RandomID;
                    var matchInfo = new MatchInfo("Match#" + mid, mid, 2, new List<string>(), 0);
                    CreateMatch(matchInfo);
                }));
        }

        /// <summary>
        ///     Посылает сообщение о старте матча
        /// </summary>
        public static void SendStartGame() {
            if (state == STATE.WAIT_STARTING_MATCH) return;
            if (state == STATE.PLAYING_MATCH) return;
            if (state != STATE.WAIT_PLAYERS_IN_MATCH) {
                UberDebug.LogErrorChannel("Matchmaking", $"Invalid state of MatchesManager: {state} while trying to start match!");
                return;
            }

            state = STATE.WAIT_STARTING_MATCH;
            
            var json = new JsonObject();
            json["matchid"] = currentMatch.roomid;
            json["state"] = 1;
            UberDebug.LogChannel("Matchmaking", "Starting game");
            RequestsManager.Send(new Request(CommandsHandler.matchmakingRoom, RequestType.ChangeMatchState,
                json, response => {
                    if (response["result"] != "success") {
                        UberDebug.LogErrorChannel("Matchmaking", "Error in json request: " + response.ToString());
                        GameManager.Reset();
                        return;
                    }
                    UberDebug.LogChannel("Matchmaking", "Started match: " + response["match"].ToString());
                    CommandsHandler.gameRoom.RunUniqCommand(new StartGameCommand(123), UniqCodes.START_GAME, 0,
                        MessageFlags.NONE);
                }));

        }

        /// <summary>
        ///     Устанавливает локальное состояние матча на PLAYING
        /// </summary>
        public static void SetMatchIsPlaying() {
            state = STATE.PLAYING_MATCH;
        }
        
        /// <summary>
        ///     Обрабатывает JSON-сообщение, что текущий матч изменился
        /// </summary>
        /// <param name="json">JSON-сообщение</param>
        public static void HandleJsonMatchChanged(JsonValue json) {
            UberDebug.LogChannel("Matchmaking", "Match changed " + json.ToString());
            var mi = MatchInfo.FromJson(json["match"]);

            var last = currentMatch;
            currentMatch = mi;

            EventsManager.handler.OnCurrentMatchChanged(last, currentMatch);
        }
        
        /// <summary>
        ///     Обновляет состояние менеджера матчей
        /// </summary>
        public static void Update() {
            switch (state) {
                case STATE.START_FIND:
                    CommandsHandler.matchmakingRoom = new ClientCommandsRoom(42);
                    GetMatchesList();
                    
                    state = STATE.WAIT_MATCHES_INFO;
                    break;
                case STATE.WAIT_MATCHES_INFO:
                    break;
                case STATE.WAIT_PLAYERS_IN_MATCH:
                    break;
                case STATE.WAIT_STARTING_MATCH:
                    break;
                case STATE.PLAYING_MATCH:
                    break;
            }
        }

        public static void Reset() {
            _state = STATE.START_FIND;
        }
    }
}