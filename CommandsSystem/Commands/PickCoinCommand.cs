using Events;
using UnityEngine;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда, сообщающая, что игрок подобрал монетку
    /// </summary>
    public partial class PickCoinCommand {
        /// <summary>
        ///     Id игрока, подобравшего монетку
        /// </summary>
        public int player;
        /// <summary>
        ///     Id подобранной монетки
        /// </summary>
        public int coin;

        /// <summary>
        ///     Подбирает монетку
        /// </summary>
        public void Run()
        {
            var player = ObjectID.GetObject(this.player);
            var coin = ObjectID.GetObject(this.coin);
            if (player == null || coin == null)
            {
                Debug.LogWarning("Player or coin null.");
                return;
            }

            EventsManager.handler.OnPlayerPickedUpCoin(player, coin);

            Client.client.RemoveObject(coin);
        }
        
    }
}