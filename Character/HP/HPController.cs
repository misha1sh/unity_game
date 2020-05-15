using CommandsSystem.Commands;
using Events;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Character.HP {
    public class HPController : MonoBehaviour {
        public float MaxHP = 100f;
        public float HPAnimationSpeed = 140;

        public Image hpImage;

        

        private bool dead = false;

        public float currentHp;

        void Start() {
            currentHp = MaxHP;
            hpOnBar = currentHp;
        }

        private float _hpOnBar;
        private float hpOnBar {
            get => _hpOnBar;
            set {
                _hpOnBar = value;
                hpImage.fillAmount = value / MaxHP;
            }
        }

        void Update() {
            if (hpOnBar != currentHp) {
                hpOnBar = Mathf.MoveTowards(hpOnBar, currentHp, HPAnimationSpeed * Time.deltaTime);
            }
        }

        // returns real taken damage
        public float TakeDamage(float damage, int source, bool autoSendChange) {
            float realDamage = Mathf.Min(currentHp, damage);

            if (autoSendChange) {
                CommandsHandler.gameModeRoom.RunSimpleCommand(new ChangeHPCommand(ObjectID.GetID(gameObject), 
                    new HPChange(-realDamage, source)), MessageFlags.IMPORTANT);
            } else {
                _applyHpChange(new HPChange(-realDamage, source));
            }
        
            

            return realDamage;
        }

        public void _applyHpChange(HPChange hpChange) {
            currentHp += hpChange.delta;
            if (currentHp > MaxHP)
                currentHp = MaxHP;

            EventsManager.handler.OnObjectChangedHP(gameObject, hpChange.delta, hpChange.source);
            
            if (!dead && currentHp <= 0) {
                dead = true;
                EventsManager.handler.OnObjectDead(gameObject, hpChange.source);
            }
        }
    }


}