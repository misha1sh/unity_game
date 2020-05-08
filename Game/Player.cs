﻿
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


        public int totalScore;
        public int placeInLastGame;
            
            
        
        public Player() {}

        public Player(int id, int owner, int controllerType) {
            this.id = id;
            this.score = 0;
            this.name = "Player#" + Random.Range(0, 100);
            this.owner = owner;
            this.controllerType = controllerType;
            this.placeInLastGame = -1;
            this.totalScore = 0;
        }

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