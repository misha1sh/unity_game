using System;
using UnityEngine;
using UnityEngine.UI;

namespace Character {
    public class HPController : MonoBehaviour {
        public float MaxHP;

        public Image hpImage;

        private float _currentHp;

        public float currentHp {
            get => _currentHp;
            private set {
                _currentHp = value;
                hpImage.fillAmount = _currentHp / MaxHP;
            }
        }

        void Start() {
            currentHp = MaxHP;
        }

        // returns real taken damage
        public float TakeDamage(float damage, int source) {
            float realDamage = Mathf.Min(currentHp, damage);
            currentHp -= realDamage;

            if (currentHp == 0) {
                // TODO        
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