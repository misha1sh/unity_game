
using UnityEngine;

namespace GameMode {
    /// <summary>
    ///     Класс для игрока
    /// </summary>
    public class Player {
        /// <summary>
        ///     ID игрока
        /// </summary>
        public int id;
        /// <summary>
        ///     Количество очков у игрока
        /// </summary>
        public int score;
        /// <summary>
        ///     Имя игрока
        /// </summary>
        public string name;
        /// <summary>
        ///     Владелец игрока (id instance, который управляет игроком)
        /// </summary>
        public int owner;

        /// <summary>
        ///     Кем управляется игрок. 0 -- человеком, 1 -- компьютером
        /// </summary>
        public int controllerType;

        /// <summary>
        ///     Суммарное количество очков за все игровые режимы
        /// </summary>
        public int totalScore;
        /// <summary>
        ///     Место, занятое в последней игре
        /// </summary>
        public int placeInLastGame;
            
            
        /// <summary>
        ///     Конструктор для сериализации
        /// </summary>
        public Player() {}

        /// <summary>
        ///     Конструктор игрока
        /// </summary>
        /// <param name="id">ID игрока</param>
        /// <param name="owner">Владелец игрока</param>
        /// <param name="controllerType">Кем управляется игрок. 0 -- человеком, 1 -- компьютером</param>
        public Player(int id, int owner, int controllerType) {
            this.id = id;
            this.score = 0;
            this.name = "Player#" + Random.Range(0, 100);
            this.owner = owner;
            this.controllerType = controllerType;
            this.placeInLastGame = -1;
            this.totalScore = 0;
        }

        /// <summary>
        ///     Записывает информацию об игроке в строку
        /// </summary>
        /// <returns>Строку с информацией об игроке</returns>
        public override string ToString() {
            string controllerName;
            if (controllerType == 0) {
                controllerName = "player";
            } else if (controllerType == 1) {
                controllerName = "AI";
            } else {
                controllerName = "unknown: " + controllerType;
            }
            return $"Player#{id} name:{name} score:{score} owner:{owner}  controller: {controllerName}";
        }
    }
}