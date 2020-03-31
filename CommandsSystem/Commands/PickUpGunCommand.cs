using System;
using Character;
using Interpolation;
using Interpolation.Managers;
using UnityEngine;

namespace CommandsSystem.Commands {
    [Serializable]
    public class PickUpGunCommand : Command<PickUpGunCommand> {
        public int player;
        public int gun;

        
        public PickUpGunCommand() {}
        public PickUpGunCommand(int player, int gun) {
            this.player = player;
            this.gun = gun;
        }

        public override void Run()
        {
            var player = ObjectID.GetObject(this.player);
            var gunObject = ObjectID.GetObject(this.gun);
            if (player == null || gunObject == null)
            {
                Debug.LogWarning("Player or gun null.");
                return;
            }
            Client.client.RemoveObject(gunObject);

            var managed = player.GetComponent<ActionController>();
            if (managed == null) return;
            
            var gun = gunObject.GetComponent<>()
           // managed.SetAction<();
        }
    }
}