using Events;
using GameMode;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Добавляет игрока в игру
    /// </summary>
    public partial class AddPlayerToGame {
        /// <summary>
        ///     Добавляемый игрок
        /// </summary>
        public Player player;

        /// <summary>
        ///     Добавляет игрока в игру
        /// </summary>
        public void Run() {
            if (PlayersManager.GetPlayerById(player.id) != null) return;
            PlayersManager.players.Add(player);
            
            EventsManager.handler.OnPlayerScoreChanged(player, player.score);
        }
    }
}