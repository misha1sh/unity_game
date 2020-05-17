using System;
using CommandsSystem.Commands;
using Events;
using Networking;
using UnityEngine;

namespace GameMode {
    /// <summary>
    ///     Класс для игрового режима со сбором монет
    /// </summary>
    public class PickCoinsGameMode : IGameMode {
        /// <summary>
        ///     Перечисление состояний режима
        /// </summary>
        private enum STATE {
            INIT,
            UPDATE
        }

        /// <summary>
        ///     Текущее состояние режима
        /// </summary>
        private STATE state = STATE.INIT;

        /// <summary>
        ///     Количество созданных монет
        /// </summary>
        public int coinsCount = 0;

        /// <summary>
        ///     Создает монету в случайном месте
        /// </summary>
        /// <param name="num">Порядковый номер монеты (нужен чтобы не создать одну и ту же два раза)</param>
        public void SpawnRandomCoin(int num) {
            var position = GameModeFunctions.FindPlaceForSpawn(10, 1);
            CommandsHandler.gameModeRoom.RunUniqCommand(new SpawnPrefabCommand("coin",
                    position, Quaternion.identity, ObjectID.RandomID, sClient.ID, 0),
                UniqCodes.SPAWN_COIN, num,
                MessageFlags.IMPORTANT);
        }


        /// <summary>
        ///     Обновляет состояние игрового режима
        /// </summary>
        /// <returns>false, если режим закончился. Иначе true</returns>
        public bool Update() {
            switch (state) {
                case STATE.INIT:
                    MainUIController.mainui.SetTask(" pick coin = <color=green>+1</color>");
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
        public float TimeLength => 30;
    }
}