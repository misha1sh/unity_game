using GameMechanics;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда для взрыва указанной бомбы
    /// </summary>
    public partial class ExplodeBombCommand {
        /// <summary>
        ///     id бомбы, которую нужно взорвать
        /// </summary>
        public int bombId;

        /// <summary>
        ///     Взрывает указанную бомбу
        /// </summary>
        public void Run() {
            var bomb = ObjectID.GetObject(bombId);
            if (bomb == null) return;
            bomb.GetComponent<Bomb>().RealExplode();
            Client.client.RemoveObject(bomb);
        }
    }
}