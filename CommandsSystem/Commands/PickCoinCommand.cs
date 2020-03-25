
using System;
using UnityEngine;

namespace CommandsSystem.Commands {
    [Serializable]
    public class PickCoinCommand : Command<PickCoinCommand>
    {
        public int player;
        public int coin;

        
        public PickCoinCommand() {}
        public PickCoinCommand(int player, int coin) {
            this.player = player;
            this.coin = coin;
        }

        public override void Run()
        {
            var player = ObjectID.GetObject(this.player);
            var coin = ObjectID.GetObject(this.coin);
            if (player == null || coin == null)
            {
                Debug.LogWarning("Player or coin null.");
                return;
            }
            Client.client.RemoveObject(coin);
        }
        
    }
}