using UnityEngine;

namespace Events {
    /// <summary>
    ///     Класс для управления обработкой событии
    /// </summary>
    public class EventsManager : MonoBehaviour {

        /// <summary>
        ///     Текущий обработчик событий
        /// </summary>
        public static EventsHandler handler;

        /// <summary>
        ///     Инициализирует переменные. Автоматически вызывается Unity
        /// </summary>
        private void Awake() {
            handler = new EventsHandler();
        }

        /// <summary>
        ///     Устанавливает обработчики событий
        /// </summary>
        private void Start() {
            MainUIController.mainui.SetupHandlers();
        }
    }
}
