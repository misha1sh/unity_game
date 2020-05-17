using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    /// <summary>
    ///     Компонента отладочного интерфейса
    /// </summary>
    public class DebugUI : MonoBehaviour {
        /// <summary>
        ///     Элемент интерфейса для отладочного текста
        /// </summary>
        public Text textPanel;
        
        /// <summary>
        ///     Флаг, сообщающий, что нужно перерисовать текст
        /// </summary>
        private static bool debugTextDirty = true;
        
        /// <summary>
        ///     Массив с отладочным текстом
        /// </summary>
        private static string[] _debugText = new string[10];

        /// <summary>
        ///     Отладочный текст
        /// </summary>
        public static string[] debugText {
            get {
                debugTextDirty = true;
                return _debugText;
            }
        }

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        private void Start() {
            debugText[0] = "DEBUG MODE";
        }

        /// <summary>
        ///     Перерисовывает отладочный текст, если требуется. Автоматически вызывается Unity каждый кадр
        /// </summary>
        private void Update() {
            if (debugTextDirty) {
                textPanel.text = String.Join("\n", debugText);
                debugTextDirty = false;
            }
        }
    }
}