using GameMode;

namespace CommandsSystem.Commands {
    public partial class AddPlayerToGame {
        public Player player;

        public void Run() {
            PlayersManager.players.Add(player);
            
            EventsManager.handler.OnPlayerScoreChanged(player, player.score);
        }
    }
}