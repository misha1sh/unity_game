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

    public static class MatchesManager {
        public enum STATE {
            START_FIND,
            WAIT_MATCHES_INFO,
            IN_MATCH
        }
        
        
        
        
        public static List<MatchInfo> matches = new List<MatchInfo>();
        private static SortedSet<int> bannedMatches = new SortedSet<int>();
        
        private static STATE _state = STATE.START_FIND;

        private static STATE state {
            get => _state;
            set {
                _state = value;
                DebugUI.debugText[1] = $"MatchesManager.State: {state}.";
            }
        }

     //   private static float waitTime = -1f;

        public static MatchInfo currentMatch;

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
                    
                    state = STATE.IN_MATCH;
                    
                    CommandsHandler.gameRoom = new ClientCommandsRoom(matchid);
                    
                    HandleJsonMatchChanged(response);
                    
                   /* currentMatch = MatchInfo.FromJson(response["match"]);
                    EventsManager.handler.OnCurrentMatchChanged(currentMatch);*/
                }));
        }

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

        public static void StartGame() {
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
                    UberDebug.LogChannel("Matchmaking", "Started match: " + response["match"]);
                }));

        }
        
        public static void HandleJsonMatchChanged(JsonValue json) {
            currentMatch = MatchInfo.FromJson(json["match"]);
            EventsManager.handler.OnCurrentMatchChanged(currentMatch);
        }
        
        public static void Update() {
            switch (state) {
                case STATE.START_FIND:
                    CommandsHandler.matchmakingRoom = new ClientCommandsRoom(42);
                    GetMatchesList();
                    
                    state = STATE.WAIT_MATCHES_INFO;
                    break;
                case STATE.WAIT_MATCHES_INFO:
                    break;
                case STATE.IN_MATCH:
                    break;
            }
           /* switch (state) {
                case STATE.START_FINDING:
                    hostMatch = new MatchInfo(1, 2, ObjectID.RandomID);
                    matches = new List<MatchInfo>();
                    
                    CommandsHandler.matchmakingRoom = new ClientCommandsRoom(42);
                    CommandsHandler.matchmakingRoom.RunSimpleCommand(new AskMatches(), MessageFlags.NONE);
                    waitTime = 1;
                    break;
                
                case STATE.WAITING_RESPONSES:
                    foreach (var match in matches) {
                        if (bannedMatches.Contains(match.roomId)) continue;
                        if (match.roomId == hostMatch.roomId) continue;
                        if (match.playersCount >= match.maxPlayersCount) continue;
                        CommandsHandler.matchmakingRoom.RunSimpleCommand(new );
                    }
                    
                    waitTime -= Time.deltaTime;
                    if (waitTime < 0) {
                        CommandsHandler.matchmakingRoom.RunSimpleCommand(new AskMatches(), MessageFlags.NONE);
                        waitTime = 1;
                    }
                    break;
                
                case STATE.MATCH_FOUND:
                    break;
            }*/

        }

        public static void Reset() {
            _state = STATE.START_FIND;
        }
    }
}