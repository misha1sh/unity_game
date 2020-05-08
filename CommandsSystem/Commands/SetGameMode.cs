using GameMode;

namespace CommandsSystem.Commands {
    public partial class SetGameMode {
        public int gamemodeCode;
        public int roomId;
        public int currentGameNum;

        public void Run() {
            GameManager.SetGameMode(gamemodeCode, roomId, currentGameNum);
        }
    }
}