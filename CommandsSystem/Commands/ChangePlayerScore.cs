using GameMode;

namespace CommandsSystem.Commands {
    public partial class ChangePlayerScore {
        public int player;
        public int newScore;

        public void Run() {
            /*   var playerObj = ObjectID.GetObject(this.player);
               var player = playerObj.GetComponent<PlayerStorage>().Player;*/
            var player = PlayersManager.GetPlayerById(this.player);
            player.score += newScore;

            EventsManager.handler.OnPlayerScoreChanged(player, player.score);
        }
    }
}