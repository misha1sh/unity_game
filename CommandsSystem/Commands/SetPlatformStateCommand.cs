using GameMechanics;

namespace CommandsSystem.Commands {
    public partial class SetPlatformStateCommand {
        public int id;
        public int direction;
        
        public void Run() {
            var platform = ObjectID.GetObject(id);
            platform.GetComponent<MovingPlatform>().SetMoveState(direction);
        }
    }
}