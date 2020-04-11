using System.Collections.Generic;
using CommandsSystem.Commands;
using UnityEngine;

namespace GameMode {
    public static class PlayersManager {
        public static List<Player> players = new List<Player>();
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
            Client.client.commandsHandler.RunSimpleCommand(new ChangePlayerScore(player.id, score));
        }
    }
}