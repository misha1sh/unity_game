using System;
using Character;
using Character.Actions;
using Character.Guns;
using Character.HP;
using CommandsSystem.Commands;
using Events;
using Networking;
using UnityEngine;
using UnityEngine.Animations;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GameMode {
    public class ShooterGameMode : IGameMode {

        private enum STATE {
            INIT,
            UPDATE
        }

        private STATE state = STATE.INIT;

       /* private int spawnedPistolsCount = 0;
        private int spawnedShotgunsCount = 0;
        private int spawnedSemiAutoCount = 0;
        */
       private int spawnedGunsCount = 0;
       private float timeToSpawnNextGun = 0f;

       private void SpawnRandomGun(int id) {
           var position = GameModeFunctions.FindPlaceForSpawn(0.1f, 1);
           
           int gunType = Random.Range(0, 3);
           string gunName;
           switch (gunType) {
               case 0:
                   gunName = "pistol";
                   break;
               case 1:
                   gunName = "semiauto";
                   break;
               case 2:
                   gunName = "shotgun";
                   break;
               default:
                   throw new Exception("Unknown gun type");
           }
           CommandsHandler.gameModeRoom.RunUniqCommand(new SpawnPrefabCommand(gunName,
                   position, Quaternion.identity, ObjectID.RandomID, sClient.ID, 0),
               UniqCodes.SPAWN_GUN, id,
               MessageFlags.IMPORTANT);
       }

       public bool Update() {
        

            switch (state) {
                case STATE.INIT:
                    MainUIController.mainui.SetTask( "   Kill enemy = <color=green>+100</color>\n" +
                                                     "   Deal <color=red>1</color> damage = <color=green>+1</color>");
                    MainUIController.mainui.gunsPanel.SetActive(true);
                    GameModeFunctions.SpawnPlayers();
                    EventsManager.handler.OnSpawnedPlayer += (gameObject, player) => {
                        if (player.owner == sClient.ID) {
                            gameObject.GetComponent<ActionController>().SetAction<ShootPistolAction>(action => 
                                action.gun = new Pistol());
                        }
                    };

                    EventsManager.handler.OnObjectDead += (go, source) => {
                        if (!go.TryGetComponent<PlayerStorage>(out _)) return; // check that it is player

                        var killedPlayer = go.GetComponent<PlayerStorage>().Player;
                        
                        if (killedPlayer.owner == sClient.ID)
                            GameModeFunctions.SpawnPlayer(killedPlayer.id);
                        Client.client.RemoveObject(go);

                        
                        UberDebug.LogChannel("GameMode", $"Shooter: player#{killedPlayer.id} '{killedPlayer.name}' dead. respawning");
                        
                        var player = DamageSource.GetSourceGO(source)?.GetComponent<PlayerStorage>()?.Player;
                        
                        if (player == null) return;
                        if (player.owner == sClient.ID)
                            PlayersManager.AddScoreToPlayer(player, 100);
                    };

                    EventsManager.handler.OnObjectChangedHP += (go, delta, source) => {
                        if (!go.TryGetComponent<PlayerStorage>(out _)) return; // check that it is player
                        
                        
                        var player = DamageSource.GetSourceGO(source)?.GetComponent<PlayerStorage>()?.Player;
                        if (player == null) return;
                        if (player.owner == sClient.ID)
                            PlayersManager.AddScoreToPlayer(player, Mathf.RoundToInt(-delta));
                    };

                    for (int i = 0; i < 3; i++) {
                        SpawnRandomGun(spawnedGunsCount++);
                    }
                    timeToSpawnNextGun = 10f;
                    
                    state = STATE.UPDATE;
                    break;
                case STATE.UPDATE:
                    timeToSpawnNextGun -= Time.deltaTime;
                    if (timeToSpawnNextGun < 0) {
                        SpawnRandomGun(spawnedGunsCount++);
                        timeToSpawnNextGun = 10f;
                    }

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
            return 140;
        }
    }
}