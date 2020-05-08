using System;
using CommandsSystem.Commands;
using Networking;
using UnityEngine;

namespace GameMode {
    public class PickCoinsGameMode : IGameMode {
        private enum STATE {
            INIT,
            UPDATE
        }

        private STATE state = STATE.INIT;

        public int coinsCount = 0;
        
        public void SpawnRandomCoin(int id) {
            var position = GameModeFunctions.FindPlaceForSpawn(10, 1);
            CommandsHandler.gameModeRoom.RunUniqCommand(new SpawnPrefabCommand("coin",
                position, Quaternion.identity, ObjectID.RandomID, sClient.ID, 0),
                UniqCodes.SPAWN_COIN, id,
                MessageFlags.IMPORTANT);
        }
        
        
        public bool Update() {
            switch (state) {
                case STATE.INIT:                    
                    MainUIController.mainui.SetTask(" -pick coin = <color=green>+1</color>");
                    MainUIController.mainui.gunsPanel.SetActive(false);
                    GameModeFunctions.SpawnPlayers();

                    EventsManager.handler.OnPlayerPickedUpCoin += (playerObj, coin) => {
                        SpawnRandomCoin(coinsCount++);
                        var player = playerObj.GetComponent<PlayerStorage>().Player;
                        if (player.owner == sClient.ID)
                            PlayersManager.AddScoreToPlayer(player, 1);
                    };

                    for (int i = 0; i < 20; i++) {
                        SpawnRandomCoin(coinsCount++);
                    }
                    
                    
                    state = STATE.UPDATE;
                    break;
                case STATE.UPDATE:

                    break;
                default:
                    throw new Exception($"Unknown state: {state}");
            }

            return true;
        }

        public bool Stop() {
            return false;
        }

        public float TimeLength() {
            return 7f;
        }
    }
}