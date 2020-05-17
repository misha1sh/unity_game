using GameMechanics;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда для синхронизации состояния подвижной платформы
    /// </summary>
    public partial class SetPlatformStateCommand {
        /// <summary>
        ///     Id подвижной платформы
        /// </summary>
        public int id;
        /// <summary>
        ///     Направление, в котором должна двигаться платформа сейчас
        /// </summary>
        public int direction;
        
        /// <summary>
        ///     Синхронизирует состояние платформы с заданным
        /// </summary>
        public void Run() {
            var platform = ObjectID.GetObject(id);
            platform.GetComponent<MovingPlatform>().SetMoveState(direction);
        }
    }
}