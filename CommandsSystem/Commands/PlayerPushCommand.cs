using Character;
using Interpolation.Managers;
using UnityEngine;

namespace CommandsSystem.Commands {
    public class PlayerPushCommand : Command<PlayerPushCommand> {
        public int playerId;
        
        public PlayerPushCommand() {}

        public PlayerPushCommand(int playerId) {
            this.playerId = playerId;
        }
        
        public override void Run() {
            var player = ObjectID.GetObject(playerId);
            if (player == null) {
                Debug.LogWarning($"Player#{playerId} was null ");
                return;
            }

            var manager = player.GetComponent<PlayerUnmanagedGameObject>();
            if (manager == null) return; // means we own this player
            var characterAnimator = player.GetComponent<CharacterAnimator>();
            characterAnimator.SetPush();
        }
    }
}