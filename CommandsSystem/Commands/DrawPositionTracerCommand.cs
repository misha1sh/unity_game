using Character.Guns;
using Interpolation.Managers;
using UnityEngine;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда рисования следа от пули между персонажем и координатой
    /// </summary>
    public partial class DrawPositionTracerCommand {
        /// <summary>
        ///     id персонажа, выпустивший пулю
        /// </summary>
        public int player;
        /// <summary>
        ///     Координата, в которую попала пуля
        /// </summary>
        public Vector3 target;
        
        /// <summary>
        ///     Выполняет команду
        /// </summary>
        public void Run() {
            var player = ObjectID.GetObject(this.player);
            if (player.GetComponent<PlayerManagedGameObject>() != null) return;
            ShootSystem.DrawTracer(ShootSystem.GetGunPosition(player.transform.position), 
                target);
        }
    }
}