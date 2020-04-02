using System;
using UnityEngine;
using UnityEngine.UI;

namespace Character.HP {
    public class HPController : MonoBehaviour {
        public float MaxHP;

        public Image hpImage;

        private float _currentHp;

        public float currentHp {
            get => _currentHp;
            set {
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


}