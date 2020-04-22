using System;
using System.Diagnostics;
using CommandsSystem.Commands;
using Networking;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace GameMode {
    public static class GameManager {

        private enum STATE {
            INIT,
            WAIT_OTHERS,
            CHOOSE_GAMEMODE,
            WAIT_CHOOSING_GAMEMODE,
            UPDATE_GAMEMODE,
            STOP_GAMEMODE,
            SHOW_RESULTS,
            AFTER_SHOW_RESULTS,
            FINISH
        }
        
        

        
        public static int gamesCount = 0;

        private static STATE _state;

        private static STATE state{
            get => _state;
            set {
                _state = value;
                DebugUI.debugText[1] = $"State: {state}. {gamesCount}. {PlayersManager.mainPlayer.id}";
            }
        }

        //public static Player currentPlayer;
        
        
        public static IGameMode gameMode;
        public static float timeEnd = -1;

        public static void SetGameMode(int gamemodenum, int roomId) {
            var ttest = new Stopwatch();
            ObjectID.Clear();
            SceneManager.LoadScene("neon_scene");
            Debug.LogError("Loaded scene in " + ttest.ElapsedMilliseconds);


            
            switch (gamemodenum) {
                case 1:
                    gameMode = new ShooterGameMode();
                    break;
                case 2:
                    gameMode = new PickCoinsGameMode();
                    break;
                default:
                    throw new ArgumentException($"Unknown gamemodenum: {gamemodenum}");
            }

            state = STATE.UPDATE_GAMEMODE;
            timeEnd = Time.time + gameMode.TimeLength();
        }
        
        
        public static void Update() {
            switch (state) {
                case STATE.INIT:

                    PlayersManager.mainPlayer = new Player(sClient.ID, sClient.ID, 0);
                    sClient.commandsHandler.RunSimpleCommand(new AddPlayerToGame(PlayersManager.mainPlayer), 1);
                    for (int i = 0; i < 2; i++) {
                        var ai = new Player(ObjectID.RandomID, sClient.ID, 1);
                        sClient.commandsHandler.RunUniqCommand(new AddPlayerToGame(ai), 1, UniqCodes.ADD_AI_PLAYER, i);
                    }
                    
                    state = STATE.WAIT_OTHERS;
                    break;
                case STATE.WAIT_OTHERS:
                    if (PlayersManager.playersCount == 4)
                        state = STATE.CHOOSE_GAMEMODE;
                    break;
                case STATE.CHOOSE_GAMEMODE: // choose gamemode
                    int gamemode = 2;
                    sClient.commandsHandler.RunSimpleCommand(new SetGameMode(gamemode), 1);
                    state = STATE.WAIT_CHOOSING_GAMEMODE;
                    break;
                case STATE.WAIT_CHOOSING_GAMEMODE:
                    break;
                case STATE.UPDATE_GAMEMODE: // update gamemode
                    var res = gameMode.Update();
                    if (!res) {
                        state = STATE.STOP_GAMEMODE;
                    }
                    if (timeEnd < Time.time) {
                        state = STATE.STOP_GAMEMODE;
                    }
                    break;
                case STATE.STOP_GAMEMODE: // stop gamemode
                    var res2 = gameMode.Stop();
                    if (!res2) {
                        state = STATE.SHOW_RESULTS;
                    }
                    break;
                case STATE.SHOW_RESULTS: // show results
                    // ???
                    state = STATE.AFTER_SHOW_RESULTS;
                    break;
                case STATE.AFTER_SHOW_RESULTS: // go to next
                    gamesCount++;
                    if (gamesCount > 2) {
                        state = STATE.FINISH;
                    } else {
                        state = STATE.CHOOSE_GAMEMODE;
                    }
                    break;
                case STATE.FINISH: // finish
                    // ???
                    Debug.Log("finish");
                    return;
                default:
                    throw new Exception($"Unknown GameManager state: {state}");
            }
        }

    }
}