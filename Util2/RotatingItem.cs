using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util2 {
    /// <summary>
    ///     Компонента для вращающегося игрвого объекта
    /// </summary>
    public class RotatingItem : UnityEngine.MonoBehaviour {
        /// <summary>
        ///     Скорость вращения
        /// </summary>
        public float rotationSpeed = 70;
        /// <summary>
        ///     Скорость передвижения вверх-вниз
        /// </summary>
        public float upDownSpeed = 3f;
        /// <summary>
        ///     Амплитуда перемещения вверх-вниз
        /// </summary>
        public float upDownAmplitude = 0.25f;
        /// <summary>
        ///     Сдвиг в фазе перемещения вверх вниз
        /// </summary>
        private float phase;

        /// <summary>
        ///     Стартовая координата y
        /// </summary>
        private float startingY;

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        public void Start() {
            startingY = transform.position.y;
            phase  = Random.value * Mathf.PI * 2;
            transform.Rotate(0, Random.value * 2 * Mathf.PI, 0);
        }
        
        /// <summary>
        ///     Вращает объект и перемещает его вверх-вниз
        /// </summary>
        public void Update() {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            var pos = transform.localPosition;
            pos.y = upDownAmplitude * Mathf.Sin(upDownSpeed * Time.time + phase);
            transform.localPosition = pos;
        }
    }
}