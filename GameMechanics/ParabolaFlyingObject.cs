using System;
using Interpolation;
using UnityEngine;

namespace GameMechanics {
    /// <summary>
    ///     Компонента для объекта, летящего по тректории параболы
    /// </summary>
    public class ParabolaFlyingObject : MonoBehaviour {
        /// <summary>
        ///     Стартовая позиция
        /// </summary>
        public Vector3 start;
        /// <summary>
        ///     Средняя позиция
        /// </summary>
        public Vector3 medium;
        /// <summary>
        ///     Конечная позиция
        /// </summary>
        public Vector3 stop;
        /// <summary>
        ///     Время полёта
        /// </summary>
        public float totalTime;

        /// <summary>
        ///     Время, когда начался полёт
        /// </summary>
        public float startTime = -1f;

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        public void Start() {
            startTime = Time.time;
        }

        /// <summary>
        ///     Автоматически прекращет движение при столкновении объектом
        /// </summary>
        /// <param name="other">Информация о столкновении</param>
        private void OnCollisionEnter(Collision other) {
            Destroy(this);
        }

        /// <summary>
        ///     Перемещает объект. Автоматически вызывается Unity каждый кадр
        /// </summary>
        public void Update() {
            float t = (Time.time - startTime) / totalTime;
            if (t > 1)
                t = 1;
            transform.position = InterpolationFunctions.BezierCurve(start, medium, stop, t);
        }
    }
}