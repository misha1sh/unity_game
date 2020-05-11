using Events;
using UnityEngine;

namespace CommandsSystem.Commands {
    public partial class PickCoinCommand
    {
        public int player;
        public int coin;


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