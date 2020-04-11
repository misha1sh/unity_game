
using UnityEngine;

namespace GameMode {
    public class Player {
        public int id;
        public int score;
        public string name;
        public int owner;
        
        // 0 -- controlled by player
        // 1 -- controlled by AI
        public int controllerType;
        
        public Player() {}

        public Player(int id, int owner, int controllerType) {
            this.id = id;
            this.score = 0;
            this.name = "Player#" + Random.Range(0, 100);
            this.owner = owner;
            this.controllerType = controllerType;
        }
    }
}