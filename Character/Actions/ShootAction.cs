using Character.Guns;
using Events;
using GameMode;
using UnityEngine;

namespace Character.Actions {
    /// <summary>
    ///     Действие персонажа, при котором он стреляет по врагам
    /// </summary>
    /// <typeparam name="T">Оружие, из которго игрок стреляет</typeparam>
    public abstract class ShootAction<T> : MonoBehaviour, IAction 
        where T: IGun {
        
        /// <summary>
        ///     Ссылка на CharacterAnimator у игрока
        /// </summary>
        private CharacterAnimator animator;

        /// <summary>
        ///     Оружие, из которого игрок стреляет
        /// </summary>
        public T gun;

        /// <summary>
        ///     Обрабатывает подбор оружия игроком. Вызывается при включении скрипта
        /// </summary>
        void OnEnable() {
            if (gun != null) {
                gun.OnPickedUp(gameObject);
                EventsManager.handler.OnPlayerPickedUpGun(gameObject, gun);
            }
        }

        /// <summary>
        ///    Обрабатывает потерю оружия игроком. Вызывается при выключении скрипта
        /// </summary>
        private void OnDisable() {
            if (GameManager.sceneReloaded) return;
            if (gun != null) {
                gun.OnDropped();
                EventsManager.handler.OnPlayerDroppedGun(gameObject, gun);
            }
        }
        
        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        void Start() {
            animator = gameObject.GetComponent<CharacterAnimator>();
        }

        /// <summary>
        ///     Функция, которая вызывается, когда игрок начинает делать действие
        /// </summary>
        public abstract void OnStartDoing();

        /// <summary>
        ///     Функция, которая вызывается, когда игрок прекращает делать действие
        /// </summary>
        public abstract void OnStopDoing();
    }
}