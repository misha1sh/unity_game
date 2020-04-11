using Character.Guns;
using Interpolation.Managers;
using UnityEngine;

namespace CommandsSystem.Commands {
    public partial class DrawPositionTracerCommand {
        public int player;
        public Vector3 target;
        
        
        public void Run() {
            var player = ObjectID.GetObject(this.player);
            if (player.GetComponent<PlayerManagedGameObject>() != null) return;
            ShootSystem.DrawTracer(ShootSystem.GetGunPosition(player.transform.position), 
                target);
        }
    }
}