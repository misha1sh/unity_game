using Events;
using GameMode;

namespace CommandsSystem.Commands {
    public partial class AddPlayerToGame {
        public Player player;

        public void Run() {
            if (PlayersManager.GetPlayerById(player.id) != null) return;
            PlayersManager.players.Add(player);
            
            EventsManager.handler.OnPlayerScoreChanged(player, player.score);
        }
    }
}