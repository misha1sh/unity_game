using GameMode;

namespace CommandsSystem.Commands {
    public partial class CreateChatMessageCommand {
        public int playerid;
        public string message;

        public void Run() {
            var player = PlayersManager.GetPlayerById(playerid);
            MainUIController.mainui.AddChatMessage(player, message);
        }
    }
}