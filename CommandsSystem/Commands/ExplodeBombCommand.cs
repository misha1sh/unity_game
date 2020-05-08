using GameMechanics;

namespace CommandsSystem.Commands {
    public partial class ExplodeBombCommand {
        public int bombId;

        public void Run() {
            var bomb = ObjectID.GetObject(bombId);
            if (bomb == null) return;
            bomb.GetComponent<Bomb>().RealExplode();
            Client.client.RemoveObject(bomb);
        }
    }
}