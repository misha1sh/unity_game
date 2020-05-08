using System.Collections.Generic;
using System.Linq;
using CommandsSystem.Commands;
using Networking;
using UnityEngine;

namespace GameMode {
    public static class PlayersManager {
        public static List<Player> players = new List<Player>();

        public static List<Player> playersSortedByScore {
            get {
                var players2 = players.ToList();
                players2.Sort((player1, player2) =>
                    player2.score.CompareTo(player1.score));
                return players2;
            }
        }
        public static Player mainPlayer;

        public static int playersCount => players.Count;

        public static Player GetPlayerById(int id) {
            for (int i = 0; i < players.Count; i++) {
                if (players[i].id == id) {
                    return players[i];
                }
            }

            return null;
        }

        public static void AddScoreToPlayer(GameObject player, int score) {
            AddScoreToPlayer(player.GetComponent<PlayerStorage>().Player, score);
        }        
        
        public static void AddScoreToPlayer(Player player, int score) {
            CommandsHandler.gameRoom.RunSimpleCommand(new ChangePlayerScore(player.id, score), MessageFlags.IMPORTANT);
        }


        public static bool IsMainPlayer(Player player) {
            return PlayersManager.mainPlayer != null && player.id == PlayersManager.mainPlayer.id;
        }

    }
}