using System.Collections.Generic;
using System.Linq;
using CommandsSystem.Commands;
using Networking;
using UnityEngine;

namespace GameMode {
    /// <summary>
    ///     Класс для хранения информации об игроках
    /// </summary>
    public static class PlayersManager {
        /// <summary>
        ///     Список игроков в текущем матче
        /// </summary>
        public static List<Player> players = new List<Player>();

        /// <summary>
        ///     Возвращает список игроков, отсортированный по очкам
        /// </summary>
        public static List<Player> playersSortedByScore {
            get {
                var players2 = players.ToList();
                players2.Sort((player1, player2) =>
                    player2.score.CompareTo(player1.score));
                return players2;
            }
        }
        
        /// <summary>
        ///     Возвращает список игроков, отсортированный по суммрному количеству очков
        /// </summary>
        public static List<Player> playersSortedByTotalScore {
            get {
                var players2 = players.ToList();
                players2.Sort((player1, player2) =>
                    player2.totalScore.CompareTo(player1.totalScore));
                return players2;
            }
        }
        
        /// <summary>
        ///     Главный игрок, которым упраляет человек
        /// </summary>
        public static Player mainPlayer;

        /// <summary>
        ///     Количество игроков в игре
        /// </summary>
        public static int playersCount => players.Count;

        /// <summary>
        ///     Возвращает игрока с заданным ID
        /// </summary>
        /// <param name="id">ID игрока</param>
        /// <returns>Игрока с заданным ID</returns>
        public static Player GetPlayerById(int id) {
            for (int i = 0; i < players.Count; i++) {
                if (players[i].id == id) {
                    return players[i];
                }
            }

            return null;
        }

        /// <summary>
        ///     Начисляет очки игроку
        /// </summary>
        /// <param name="player">Персонаж игрока</param>
        /// <param name="score">Количество очков</param>
        public static void AddScoreToPlayer(GameObject player, int score) {
            AddScoreToPlayer(player.GetComponent<PlayerStorage>().Player, score);
        }        
        
        /// <summary>
        ///     Начисляет очки игроку
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <param name="score">Количество очков</param>
        public static void AddScoreToPlayer(Player player, int score) {
            CommandsHandler.gameRoom.RunSimpleCommand(new ChangePlayerScore(player.id, score), MessageFlags.IMPORTANT);
        }

        /// <summary>
        ///     Проверяет, является ли данный игрок главным
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <returns>true, если является главным. Иначе false</returns>
        public static bool IsMainPlayer(Player player) {
            return PlayersManager.mainPlayer != null && player.id == PlayersManager.mainPlayer.id;
        }

        /// <summary>
        ///     Сбрасывает значения переменных
        /// </summary>
        public static void Reset() {
            players.Clear();
            if (mainPlayer != null) {
                mainPlayer.totalScore = 0;
                players.Add(mainPlayer);
            }
        }
    }
}