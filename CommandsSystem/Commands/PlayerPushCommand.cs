using Character;
using Interpolation.Managers;
using UnityEngine;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда сообщающая, что для данного игрока нужно показать анимацию толкания
    /// </summary>
    public partial class PlayerPushCommand {
        /// <summary>
        ///     id игрока, для которого нужно показать анимацию
        /// </summary>
        public int playerId;
        
        
        /// <summary>
        ///     Показывает анимацию толкания для соотвествующего игрока
        /// </summary>
        public void Run() {
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