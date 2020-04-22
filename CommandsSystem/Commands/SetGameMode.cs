using GameMode;

namespace CommandsSystem.Commands {
    public partial class SetGameMode {
        public int gamemode;
        public int roomId;

        public void Run() {
            GameManager.SetGameMode(gamemode, roomId);
        }
    }
}