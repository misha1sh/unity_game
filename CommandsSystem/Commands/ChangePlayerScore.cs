using Events;
using GameMode;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда для изменения очков игрока
    /// </summary>
    public partial class ChangePlayerScore {
        /// <summary>
        ///     Игрок, у которого нужно изменить очки
        /// </summary>
        public int player;
        /// <summary>
        ///     Новое количество очков
        /// </summary>
        public int newScore;

        /// <summary>
        ///     Применяет изменения
        /// </summary>
        public void Run() {
            var player = PlayersManager.GetPlayerById(this.player);
            player.score += newScore;

            EventsManager.handler.OnPlayerScoreChanged(player, player.score);
        }
    }
}