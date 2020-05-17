using Character.Guns;
using Character.HP;
using Interpolation.Managers;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда рисования следа от пули между персонажами
    /// </summary>
    public partial class DrawTargetedTracerCommand {
        /// <summary>
        ///     Id персонажа, выпустившего пулю
        /// </summary>
        public int player;
        /// <summary>
        ///     Id персонажа, в которого попала пуля
        /// </summary>
        public int target;

        /// <summary>
        ///     Изменение здоровья персонажа, в которого попала пуля
        /// </summary>
        public HPChange HpChange;

        /// <summary>
        ///     Выполняет команду
        /// </summary>
        public void Run() {
            var target = ObjectID.GetObject(this.target);


            var player = ObjectID.GetObject(this.player);
            if (player.GetComponent<PlayerManagedGameObject>() != null) return;
            
            ShootSystem.DrawTracer(ShootSystem.GetGunPosition(player.transform.position),
                ShootSystem.GetGunPosition(target.transform.position));
            
            HPController.ApplyHPChange(target, HpChange);
        }
    }
}