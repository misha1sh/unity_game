using System;
using Events;
using Game;
using UnityEngine;

namespace Util2 {
    /// <summary>
    ///     Компонента для автоматического поиска и входа в матч
    /// </summary>
    public class AutoMatchJoiner : MonoBehaviour {
        /// <summary>
        ///     Запущен ли автовыбор матча
        /// </summary>
        public static bool isRunning = false;
        /// <summary>
        ///     Нужно ли ждать другого игрока для старта игры
        /// </summary>
        public static bool sneedWaitOtherPlayers;
        
        /// <summary>
        ///     Нужно ли ждать другого игрока для старта игры
        /// </summary>
        public bool needWaitOtherPlayers = false;

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        public void Awake() {
            isRunning = true;
            sneedWaitOtherPlayers = needWaitOtherPlayers;
        }

        /// <summary>
        ///     Автоматически входит в матч
        /// </summary>
        public void Start() {
            EventsManager.handler.OnCurrentMatchChanged += (last, currentMatch) => {

                if (!needWaitOtherPlayers || currentMatch.players.Count >= currentMatch.maxPlayersCount) {
                    MatchesManager.SendStartGame();
                }
            };
            
            sClient.StartFindingMatch();

        }
    }
}