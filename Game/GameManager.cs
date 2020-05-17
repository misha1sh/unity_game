using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Character;
using CommandsSystem.Commands;
using Events;
using Networking;
using UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Util2;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace GameMode {
    public static class GameManager {
        private const int TOTAL_GAMES_COUNT = 2;
        
        
        private enum STATE {
            INIT,
            WAIT_OTHERS,
            CHOOSE_GAMEMODE,
            WAIT_CHOOSING_GAMEMODE,
            WAIT_FOR_ALL_LOAD_GAMEMODE,
            UPDATE_GAMEMODE,
            STOP_GAMEMODE,
            SHOW_RESULTS,
            WAIT_SHOW_RESULTS,
            WAIT_AFTER_SHOW_RESULTS,
            AFTER_SHOW_RESULTS,
            FINISH
        }


        public static bool sceneReloaded = false;
        
        public static int gamesCount = 0;
        
        private static STATE _state;

        private static STATE state {
            get => _state;
            set {
                _state = value;
                DebugUI.debugText[1] = $"GameManager.State: {state}. {gamesCount}. {PlayersManager.mainPlayer.id}";
            }
        }

        //public static Player currentPlayer;
        
        
        public static IGameMode gameMode;
        public static float timeEnd = -1;

        public static void SetGameMode(int gamemodeCode, int roomId, int currentGameNum) {
            CommandsHandler.gameModeRoom = new ClientCommandsRoom(roomId);
            
            
            foreach (var player in PlayersManager.players) {
                player.score = 0;
                EventsManager.handler.OnPlayerScoreChanged(player, player.score);
            }
            
            
            var ttest = new Stopwatch();
            ObjectID.Clear();
            sceneReloaded = true;
            sClient.LoadScene("new_scene2");
            Debug.LogError("Loaded scene in " + ttest.ElapsedMilliseconds);


            switch (gamemodeCode) {
                case 0:
                    gameMode = new ShooterGameMode();
                    break;
                case 1:
                    gameMode = new PickCoinsGameMode();
                    break;
                default:
                    throw new ArgumentException($"Unknown gamemodeCode: {gamemodeCode}");
            }

            availableGameModes.Remove(gamemodeCode);
  
           
            InstanceManager.currentInstance.currentLoadedGamemodeNum++;
            InstanceManager.currentInstance.Send();
           
            
            MainUIController.mainui.HideTotalScore();

      
            
            
            state = STATE.WAIT_FOR_ALL_LOAD_GAMEMODE;
            
            Assert.AreEqual(InstanceManager.currentInstance.currentLoadedGamemodeNum, currentGameNum);

        }

        public static void SetAfterShowResults() {
            if (state == STATE.WAIT_AFTER_SHOW_RESULTS)
                state = STATE.AFTER_SHOW_RESULTS;
            MainUIController.mainui.HideTotalScore();
        }
        

        private static float showResultsWaitTime;

        public static void Reset() {
            state = STATE.INIT;
            gamesCount = 0;
            gameMode = null;
        }
        
        
        
        private static List<int> availableGameModes = new List<int>();

        private static int ChooseGameMode() {
            if (availableGameModes.Count == 0) {
                Debug.LogError("no available game modes. choosing random one");
                return Random.Range(0, 2);
            }

            var index = Random.Range(0, availableGameModes.Count);
            return availableGameModes[index];
        }

        public static void Update() {
            switch (state) {
                case STATE.INIT:
                    //   CommandsHandler.gameRoom = new ClientCommandsRoom(137);
                    //    CommandsHandler.gameRoom.RunUniqCommand(new StartGameCommand(), 1, 1, MessageFlags.IMPORTANT);

                    availableGameModes.Clear();
                    availableGameModes.Add(0);
                    availableGameModes.Add(1);
                    
                    InstanceManager.currentInstance.Send();



                    CommandsHandler.gameRoom.RunSimpleCommand(new AddPlayerToGame(PlayersManager.mainPlayer),
                        MessageFlags.IMPORTANT);
                    int cnt = 2;
                    if (AutoMatchJoiner.isRunning && !AutoMatchJoiner.sneedWaitOtherPlayers)
                        cnt = 3;
                    for (int i = 0; i < cnt; i++) {
                        var ai = new Player(ObjectID.RandomID, sClient.ID, 1);
                        CommandsHandler.gameRoom.RunUniqCommand(new AddPlayerToGame(ai), UniqCodes.ADD_AI_PLAYER, i,
                            MessageFlags.IMPORTANT);
                    }

                    state = STATE.WAIT_OTHERS;
                    break;
                case STATE.WAIT_OTHERS:
                    if (PlayersManager.playersCount == 4)
                        state = STATE.CHOOSE_GAMEMODE;
                    break;
                case STATE.CHOOSE_GAMEMODE:
                    int gamemode = ChooseGameMode();
                    CommandsHandler.gameRoom.RunUniqCommand(new SetGameMode(gamemode, sClient.random.Next(),
                            InstanceManager.currentInstance.currentLoadedGamemodeNum + 1),
                        UniqCodes.CHOOSE_GAMEMODE, InstanceManager.currentInstance.currentLoadedGamemodeNum + 1,
                        MessageFlags.IMPORTANT);
                    state = STATE.WAIT_CHOOSING_GAMEMODE;
                    break;
                case STATE.WAIT_CHOOSING_GAMEMODE:
                    break;
                case STATE.WAIT_FOR_ALL_LOAD_GAMEMODE:
                    bool allLoaded = true;
                    foreach (var instance in InstanceManager.instances) {
                        if (instance.currentLoadedGamemodeNum !=
                            InstanceManager.currentInstance.currentLoadedGamemodeNum)
                            allLoaded = false;
                    }

                    if (allLoaded) {
                        timeEnd = Time.time + gameMode.TimeLength();
                        state = STATE.UPDATE_GAMEMODE;
                    }

                    break;
                case STATE.UPDATE_GAMEMODE:
                    sceneReloaded = false;

                    var res = gameMode.Update();
                    if (!res) {
                        state = STATE.STOP_GAMEMODE;
                    }
                    
                    MainUIController.mainui.SetTimerTime((int) (timeEnd - Time.time));

                    if (timeEnd < Time.time) {
                        state = STATE.STOP_GAMEMODE;
                    }

                    break;
                case STATE.STOP_GAMEMODE:
                    var res2 = gameMode.Stop();
                    if (!res2) {
                        state = STATE.SHOW_RESULTS;
                    }

                    break;
                case STATE.SHOW_RESULTS: 
                    var players2 = PlayersManager.playersSortedByScore;

                    for (int i = 0; i < players2.Count; i++) {
                        players2[i].placeInLastGame = i + 1;
                        players2[i].totalScore += players2.Count - i;
                    }

                    gamesCount++;
                    if (gamesCount == TOTAL_GAMES_COUNT) {
                        MainUIController.mainui.ShowFinalResults();
                        state = STATE.FINISH;
                        break;
                    }

                    showResultsWaitTime = 8;
                    MainUIController.mainui.ShowTotalScore(TOTAL_GAMES_COUNT - gamesCount,
                        (int) Math.Ceiling(showResultsWaitTime));
                    state = STATE.WAIT_SHOW_RESULTS;
                    break;

                case STATE.WAIT_SHOW_RESULTS:
                    showResultsWaitTime -= Time.deltaTime;
                    MainUIController.mainui.SetTotalScoreTimeRemaining((int) Math.Ceiling(showResultsWaitTime));
                    if (showResultsWaitTime < 0) {
                        state = STATE.WAIT_AFTER_SHOW_RESULTS;
                        CommandsHandler.gameRoom.RunSimpleCommand(new SetAfterShowResultsCommand(),
                            MessageFlags.IMPORTANT);
                    }

                    break;
                case STATE.WAIT_AFTER_SHOW_RESULTS:
                    break;

                case STATE.AFTER_SHOW_RESULTS:
                    if (gamesCount >= TOTAL_GAMES_COUNT) {
                        state = STATE.FINISH;
                        UberDebug.Log("finish");
                    } else {
                        state = STATE.CHOOSE_GAMEMODE;
                    }

                    break;
                case STATE.FINISH:
                    // ???
                    return;
                default:
                    throw new Exception($"Unknown GameManager state: {state}");
            }
        }

    }
}