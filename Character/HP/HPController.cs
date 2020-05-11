using CommandsSystem.Commands;
using Events;
using Networking;
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
            
            if (currentHp <= 0) {
                EventsManager.handler.OnObjectDead(gameObject, hpChange.source);
            }
        }
    }


}