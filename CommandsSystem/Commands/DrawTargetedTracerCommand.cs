using System;
using Character.Guns;
using Character.HP;
using Interpolation.Managers;

namespace CommandsSystem.Commands {
    public partial class DrawTargetedTracerCommand {
        public int player;
        public int target;

        public HPChange HpChange;

        public void Run() {
            var target = ObjectID.GetObject(this.target);


            var player = ObjectID.GetObject(this.player);
            if (player.GetComponent<PlayerManagedGameObject>() != null) return;
            
            ShootSystem.DrawTracer(ShootSystem.GetGunPosition(player.transform.position),
                ShootSystem.GetGunPosition(target.transform.position));
        }
    }
}