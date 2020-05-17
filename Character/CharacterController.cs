using UnityEngine;

namespace Character {
    /// <summary>
    ///     Базовый класс для компоненты, управляющей персонажем
    /// </summary>
    public class CharacterController : MonoBehaviour {
        /// <summary>
        ///     Объект, которым управляет данный контроллер
        /// </summary>
        public GameObject target;
        
        /// <summary>
        ///     Ссылка на motionController у управляемого персонажа
        /// </summary>
        protected MotionController motionController;
        /// <summary>
        ///     Ссылка на actionController у управляемого персонажа
        /// </summary>
        protected ActionController actionController;

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        protected virtual void Start() {
            motionController = target.GetComponent<MotionController>();
            actionController = target.GetComponent<ActionController>();
        }
    }
}