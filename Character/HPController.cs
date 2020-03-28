using System;
using UnityEngine;

namespace Character {
    public class HPController : MonoBehaviour {
        public float MaxHP;

        public float CurrentHp => currentHp;
        private float currentHp;

        void Start() {
            currentHp = MaxHP;
        }

        // returns real taken damage
        public float TakeDamage(float damage, int source) {
            float realDamage = Mathf.Min(currentHp, damage);
            currentHp -= realDamage;

            if (currentHp == 0) {
                
            }
            
            return realDamage;
        }
        
    }

    public static class DamageSource {
        public static int Player(int id) {
            return id;
        }

        public static int Player(GameObject gameObject) {
            return Player(ObjectID.GetID(gameObject));
        }
    }
}