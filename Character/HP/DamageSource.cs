using UnityEngine;

namespace Character.HP {
    public static class DamageSource {

        public static int None() {
            return 0;
        }

        public static int InstaKill() {
            return 1;
        }
        
        public static int Player(int id) {
            return id;
        }

        public static int Player(GameObject gameObject) {
            return Player(ObjectID.GetID(gameObject));
        }

        public static int Bomb() {
            return 2;
        }
    }
}