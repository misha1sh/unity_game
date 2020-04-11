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
            Client.client.commandsHandler.RunUniqCommand(new SpawnPrefabCommand("coin",
                position, Quaternion.identity, ObjectID.RandomID, Client.client.ID),
                UniqCodes.SPAWN_COIN, id);
        }
        
        
        public bool Update() {
            switch (state) {
                case STATE.INIT:                    
                    GameModeFunctions.SpawnPlayers();

                    EventsManager.handler.OnPlayerPickedUpCoin += (playerObj, coin) => {
                        SpawnRandomCoin(coinsCount++);
                        var player = playerObj.GetComponent<PlayerStorage>().Player;
                        if (player.owner == Client.client.ID)
                            PlayersManager.AddScoreToPlayer(player, 1);
                    };

                    for (int i = 0; i < 1; i++) {
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
            return 600f;
        }
    }
}