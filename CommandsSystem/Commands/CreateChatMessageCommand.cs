using GameMode;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда для отправки сообщения в чат
    /// </summary>
    public partial class CreateChatMessageCommand {
        /// <summary>
        ///     Игрок, отправивший сообщение
        /// </summary>
        public int playerid;
        /// <summary>
        ///     Сообщение
        /// </summary>
        public string message;

        /// <summary>
        ///     Отображает сообщение в чате
        /// </summary>
        public void Run() {
            var player = PlayersManager.GetPlayerById(playerid);
            MainUIController.mainui.AddChatMessage(player, message);
        }
    }
}