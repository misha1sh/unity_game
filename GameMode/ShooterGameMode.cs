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
    /// <summary>
    ///     Класс для игрового режима в формате шутера
    /// </summary>
    public class ShooterGameMode : IGameMode {
        /// <summary>
        ///     Перечисление состояния игрового режима
        /// </summary>
        private enum STATE {
            INIT,
            UPDATE
        }

        /// <summary>
        ///     Текущее состояние игрового режима
        /// </summary>
        private STATE state = STATE.INIT;

        /// <summary>
        ///     Количество созданного на игровом поле оружия
        /// </summary>
        private int spawnedGunsCount = 0;
        /// <summary>
        ///     Время, через которое нужно создать следующее оружие
        /// </summary>
        private float timeToSpawnNextGun = 0f;

        /// <summary>
        ///     Создаёт случайное оружие на игровом поле
        /// </summary>
        /// <param name="num">Порядковый номер создаваемого оружия (нужен чтобы не создать одно и то же два раза)</param>
        private void SpawnRandomGun(int num) {
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
               UniqCodes.SPAWN_GUN, num,
               MessageFlags.IMPORTANT);
       }

        /// <summary>
        ///     Обновляет состояние игрового режима
        /// </summary>
        /// <returns>false, если режим закончился. Иначе true</returns>
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

        /// <summary>
        ///     Завершает выполнение игрового режима
        /// </summary>
        /// <returns>false, если режим закончился. true, если надо ещё подождать</returns>
        public bool Stop() {
            return false;
        }

        /// <summary>
        ///    Время в секундах, которое длится игровой режим 
        /// </summary>
        public float TimeLength => 140;
    }
}