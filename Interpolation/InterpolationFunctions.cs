using Character;
using UnityEngine;

namespace Interpolation {
    /// <summary>
    ///     Класс с функциями для интерполяции
    /// </summary>
    public static class InterpolationFunctions {
        /// <summary>
        ///     Вычисляет сплайн Эрмита
        /// </summary>
        /// <param name="start">Стартовая позиция</param>
        /// <param name="stop">Конечная позиция</param>
        /// <param name="m0">tan угла наклона в start</param>
        /// <param name="m1">tan угла наклона в stop</param>
        /// <param name="t">Коэф. интерполяции (от 0 до 1)</param>
        /// <returns>Значение в сплайне Эрмита</returns>
        public static float CubicHermiteSpline(float start, float stop, float m0, float m1, float t) {
            return (2*t*t*t - 3*t*t + 1) * start + (t*t*t - 2*t*t + t)*m0 + (-2*t*t*t + 3*t*t) * stop + (t*t*t - t*t) * m1;
        }

        /// <summary>
        ///     Вычисляет сплайн Эрмита для трёх точек
        /// </summary>
        /// <param name="p0">Предпредыдущая позиция</param>
        /// <param name="p1">Предыдущая позиция</param>
        /// <param name="p2">Следующая позиция</param>
        /// <param name="dt01">Время между p0 и p1</param>
        /// <param name="dt12">Время между p1 и p2</param>
        /// <param name="t">Коэф. интерполяции (от 0 до 1)</param>
        /// <returns>Значение в сплайне Эрмита</returns>
        public static float CubicHermiteSpline3(float p0, float p1, float p2, float dt01, float dt12, float t) {
            var m0 = (p1 - p0) / dt01;
            var m1 = (p2 - p1) / dt12;
            if (p0 + 0.01f >= p1 && p1 <= p2 + 0.01f) {
                m0 = 0;
            }

            if (p0 <= p1 + 0.01f && p1 + 0.01f >= p2) {
                m0 = 0;
            }
            /*if (t > 1)
                 Debug.LogError("time is " + t);*/
            var res = CubicHermiteSpline(p1, p2, m0, m1, t);
         /*   if (p1 <= p2 && res > p2)
                return p2;
            if (p1 >= p2 && res < p2)
                return p2;*/
            return res;
        }


        /// <summary>
        ///     Интерполирует вектор
        /// </summary>
        /// <param name="p0">Предпредыдущий вектор</param>
        /// <param name="p1">Предыдущий вектор</param>
        /// <param name="p2">Следующий вектор</param>
        /// <param name="dt01">Время между p0 и p1</param>
        /// <param name="dt12">Время между p1 и p2</param>
        /// <param name="t">Коэф. интерполяции (от 0 до 1)</param>
        /// <returns>Интерполированный вектор</returns>
        public static Vector3 Lerp3Points(Vector3 p0, Vector3 p1, Vector3 p2, float dt01, float dt12, float t) {
            float x = CubicHermiteSpline3(p0.x, p1.x, p2.x, dt01, dt12, t);
            float y = CubicHermiteSpline3(p0.y, p1.y, p2.y, dt01, dt12, t);
            float z = CubicHermiteSpline3(p0.z, p1.z, p2.z, dt01, dt12, t);
            return new Vector3(x, y, z);
        }

        /// <summary>
        ///     Интерполирет позицию между точкам
        /// </summary>
        /// <param name="lastlastPosition">Предпредыдущая позиция</param>
        /// <param name="lastPosition">Предыдущая позиция</param>
        /// <param name="nextPosition">Следующая позиция</param>
        /// <param name="coef">Коэф. интерполяции (от 0 до 1)</param>
        /// <returns>Интерполированную позицию</returns>
        public static Vector3 InterpolatePosition(Vector3 lastlastPosition, Vector3 lastPosition, Vector3 nextPosition, 
            float coef) {
            return Lerp3Points(lastlastPosition, lastPosition, nextPosition,
                1, 1,
                coef);/**/ /* * Client.client.interpolationCoef
                    +*/
               /* Vector3.LerpUnclamped(lastPosition, nextPosition, coef);/**/// * (1.0f - Client.client.interpolationCoef)/*)*/;
        }

        /// <summary>
        ///     Интерполирует поворот
        /// </summary>
        /// <param name="lastRotation">Предыдущий поворот</param>
        /// <param name="nextRotation">Следующий поворот</param>
        /// <param name="coef">Коэф. интерполяции (от 0 до 1)</param>
        /// <returns>Интерполированный поворот</returns>
        public static Quaternion InterpolateRotation(Quaternion lastRotation, Quaternion nextRotation, float coef) {
            return Quaternion.Lerp(lastRotation, nextRotation, coef);
        }

        /// <summary>
        ///     Интерполирует анимацию персонажа
        /// </summary>
        /// <param name="last">Предыдущая анимация</param>
        /// <param name="next">Следующая анимация</param>
        /// <param name="coef">Коэф. интерполяции (от 0 до 1)</param>
        /// <returns>Интерполированную анимацию</returns>
        public static PlayerAnimationState InterpolatePlayerAnimationState(PlayerAnimationState last,
            PlayerAnimationState next, float coef) {
            bool idle = next.idle;//InterpolateBool(last.idle, next.idle, coef);
            float speed = next.speed;//InterpolateFloat(last.speed, next.speed, coef);
            float rotationSpeed = InterpolateFloat(last.speed, next.speed, coef);
            return new PlayerAnimationState() {
                idle = idle,
                speed = speed,
                rotationSpeed = rotationSpeed
            };
        }

        /// <summary>
        ///     Интерполирует логическую переменную
        /// </summary>
        /// <param name="last">Предыдущее значение</param>
        /// <param name="next">Следующее значение</param>
        /// <param name="coef">Коэф. интерполяции (от 0 до 1)</param>
        /// <returns>Интерполированную логическую переменную</returns>
        public static bool InterpolateBool(bool last, bool next, float coef) {
            if (last == next) return last;
            if (coef < 0.5f) return last;
            return next;
        }

        /// <summary>
        ///     Интерполирует число с плавающей запятой
        /// </summary>
        /// <param name="last"></param>
        /// <param name="next"></param>
        /// <param name="coef"></param>
        /// <returns>Результат интерполяции</returns>
        public static float InterpolateFloat(float last, float next, float coef) {
            return (next - last) * coef + last;
        }

        /// <summary>
        ///     Получает значение в кривой Безье
        /// </summary>
        /// <param name="p0">Первая точка</param>
        /// <param name="p1">Вторая точка</param>
        /// <param name="p2">Третья точка</param>
        /// <param name="t">Коэффициент интерполяции (от 0 до 1)</param>
        /// <returns>Значение в кривой Безье</returns>
        public static float BezierCurve(float p0, float p1, float p2, float t) {
            return (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
        }

        /// <summary>
        ///     Получает координаты вектора в кривой Безье
        /// </summary>
        /// <param name="p0">Первая точка</param>
        /// <param name="p1">Вторая точка</param>
        /// <param name="p2">Третья точка</param>
        /// <param name="t">Коэффициент интерполяции (от 0 до 1)</param>
        /// <returns>Значение вектора в кривой Безье</returns>
        public static Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
            float x = BezierCurve(p0.x, p1.x, p2.x, t);
            float y = BezierCurve(p0.y, p1.y, p2.y, t);
            float z = BezierCurve(p0.z, p1.z, p2.z, t);
            return new Vector3(x, y, z);
        }
    }
}