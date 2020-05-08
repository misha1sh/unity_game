using System.Collections.Generic;
using CommandsSystem;
using CommandsSystem.Commands;
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
                    
                    
                    
                    currentMatch = MatchInfo.FromJson(response["match"]);
                    if (currentMatch.playersCount >= currentMatch.maxPlayersCount) {
                        CommandsHandler.gameRoom.RunSimpleCommand(new StartGameCommand(123), MessageFlags.NONE);
                    }
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
                        if (match.playersCount < match.maxPlayersCount) {
                            JoinMatch(match.roomid);
                            return;
                        }
                    }
                                        
                    var matchInfo = new MatchInfo("ser", ObjectID.RandomID, 2, 0);
                    CreateMatch(matchInfo);
                }));
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
    }
}