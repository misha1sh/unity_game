using CommandsSystem.Commands;
using Events;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Character.HP {
    /// <summary>
    ///     Класс для компоненты здоровья игрового объекта
    /// </summary>
    public class HPController : MonoBehaviour {
        /// <summary>
        ///     Максимальное количество здоровья у объекта
        /// </summary>
        public float MaxHP = 100f;
        /// <summary>
        ///     Скорость анимирования изменения здоровья у объекта
        /// </summary>
        public float HPAnimationSpeed = 140;

        /// <summary>
        ///     Изображение, на котором рисуется полоска со здоровьем
        /// </summary>
        public Image hpImage;
        
        /// <summary>
        ///     Переменная, хранящая был ли убит данный объект
        /// </summary>
        private bool dead = false;

        /// <summary>
        ///     Текущее количество здоровья
        /// </summary>
        public float currentHp;

        /// <summary>
        ///     Иництализирует переменные
        /// </summary>
        void Start() {
            currentHp = MaxHP;
            hpOnBar = currentHp;
        }

        /// <summary>
        ///     Переменная для хранения hpOnBar
        /// </summary>
        private float _hpOnBar;
        /// <summary>
        ///     Количество здоровья, которое сейчас отображается на полоске здоровья
        /// </summary>
        private float hpOnBar {
            get => _hpOnBar;
            set {
                _hpOnBar = value;
                hpImage.fillAmount = value / MaxHP;
            }
        }

        /// <summary>
        ///     Анимирует полоску здоровья. Автоматически вызывается Unity каждый кадр
        /// </summary>
        void Update() {
            if (hpOnBar != currentHp) {
                hpOnBar = Mathf.MoveTowards(hpOnBar, currentHp, HPAnimationSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        ///     Наносит урон по здоровью данного объекта
        /// </summary>
        /// <param name="damage">Наносимый урон</param>
        /// <param name="source">Источник урона</param>
        /// <param name="autoSendChange">Нужно ли отослать команду с изменением здоровья на сервер. Если false, то изменения будут применены локально</param>
        /// <returns>Урон, который был нанесён</returns>
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

        /// <summary>
        ///     Применяет изменения здоровья
        /// </summary>
        /// <param name="hpChange">Изменение здровья</param>
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
        
        
        /// <summary>
        ///     Применяет изменение здоровья к объекту
        /// </summary>
        /// <param name="target">Объект</param>
        /// <param name="hpChange">Изменение здоровья</param>
        public static void ApplyHPChange(GameObject target, HPChange hpChange) {
            if (hpChange.source == DamageSource.None()) return;
            var hp = target.GetComponent<HPController>();
            hp._applyHpChange(hpChange);
        }
    }


}