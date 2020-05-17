using System;
using Interpolation.Properties;
using UnityEngine;

namespace Interpolation {
    /// <summary>
    ///     Компонента для объекта, управляемого из другого клиента
    /// </summary>
    public class UnmanagedGameObject<T> : MonoBehaviour
        where T : IGameObjectProperty, new() {

        /// <summary>
        ///     Данные для синхронизации объекта
        /// </summary>
        private class Data {
            /// <summary>
            ///     Состояние объекта
            /// </summary>
            public IGameObjectProperty s;
            /// <summary>
            ///     Время, прошедшее между отправками ссостояний
            /// </summary>
            public float timeSinceLast;

            public Data(IGameObjectProperty property, float timeSinceLast) {
                this.s = property;
                this.timeSinceLast = timeSinceLast;
            }
        }
        
        /// <summary>
        ///     Период синхронизации состояния
        /// </summary>
        private float timePerFrame = 1f / sClient.NETWORK_FPS;

        /// <summary>
        ///     Состояния объекта
        /// </summary>
        private Data lastlastState, lastState, nextState;
        /// <summary>
        ///     Текущее состояние объекта
        /// </summary>
        private IGameObjectProperty state;
        /// <summary>
        ///     Состояние объекта после следующего
        /// </summary>
        private Data nextNextState;
        /// <summary>
        ///     Время, когда было получено последнее обновление состояния
        /// </summary>
        private float lastMessageTime;

        /// <summary>
        ///     Инициализирует переменные    
        /// </summary>
        void Init() {
            lastlastState = lastState = nextState = new Data(new T(), 1);
            lastlastState.s.FromGameObject(gameObject);
            state = new T();
#if DEBUG_INTERPOLATION            
            DebugGUI.SetGraphProperties("dx", "dx", -15f, 15f, 0, new Color(0, 1, 1), false);
            DebugGUI.SetGraphProperties("x", "x", 5f, 25f, 1, new Color(0, 1, 1), true);
#endif
        }
        
        
        

        /// <summary>
        ///     Коэффициент между предпредыдущим и предыдущим состоянием
        /// </summary>
        private float P0P1InterpolationCoef = 1;
        /// <summary>
        ///     Коэффициент между предыдущим и следующим состоянием
        /// </summary>
        private float P1P2InterpolationCoef = 1;

        /// <summary>
        ///     Переключается на следующее состояние
        /// </summary>
        private void SwitchToNextState() {
            lastlastState = lastState;
            lastState = nextState;
            nextState = nextNextState;
            nextNextState = null;
            
            
            lastMessageTime = Time.realtimeSinceStartup;//-1f;
            P0P1InterpolationCoef = P1P2InterpolationCoef;
        }


        /// <summary>
        ///     Устанавливает состояние объекта с плаынм переходом
        /// </summary>
        /// <param name="newState">Новое состояние</param>
        /// <param name="deltaSinceLast">Время, прошедшее между отправками состояний</param>
        public void SetStateAnimated(T newState, float deltaSinceLast) {
            if (lastlastState is null) Init();
            nextNextState = new Data(newState, deltaSinceLast);
        }
        
        /// <summary>
        ///     Текущее время интерполяции
        /// </summary>
        private float interpolationTime;

        /// <summary>
        ///     Интерполирует и применяет состояние
        /// </summary>
        /// <param name="coef">Коэф. интерполяции</param>
        private void Interpolate(float coef) {
            var pos = transform.position.x;
            state.Interpolate(lastlastState.s, lastState.s, nextState.s, coef);
            state.ApplyToObject(gameObject);
            // Debug.Log(transform.position.x - pos);
#if DEBUG_INTERPOLATION
            DebugGUI.Graph("dx", (transform.position.x - pos) / Time.deltaTime);
            DebugGUI.Graph("x", transform.position.x);
#endif
        }
        
        /// <summary>
        ///     Анимирует состояние
        /// </summary>
        /// <param name="delta">Прошедшее количество времени</param>
        private void Animate(float delta) {
           /* if (lastMessageTime < 0) lastMessageTime = Time.realtimeSinceStartup;/* - 
                                                       Math.Min(Time.deltaTime, timePerFrame)*/; // endTime: lastMessageTime + timePerFrame
            // (beginTime, endTime]

           // var interpolationTime = Time.realtimeSinceStartup  - lastMessageTime;
           interpolationTime += delta;
           float coef = interpolationTime / nextState.timeSinceLast;
            if (interpolationTime > nextState.timeSinceLast) {
                
              /*  state.Interpolate(lastlastState.s, lastState.s, nextState.s, 1f);
                state.ApplyToObject(gameObject);*/
                
                // TODO adaptive correct
                if (nextNextState == null) {
                    DebugExtension.DebugPoint(transform.position, Color.blue, 0.1f, 3);
                    var waitTime = Math.Round((interpolationTime - nextState.timeSinceLast) * 1000);
#if DEBUG_INTERPOLATION                    
                    Debug.LogWarning($"Where is message? Waiting {waitTime} msec.");
#endif

                    Interpolate(coef);
                    
                    interpolationTime = nextState.timeSinceLast + 0.0000001f;
                    
                    return;
                }

                interpolationTime -= nextState.timeSinceLast;
                SwitchToNextState();
                Animate(0);
                return;
            }


            
            Interpolate(coef);
          //  P1P2InterpolationCoef = coef;
        }

        /// <summary>
        ///     Обновляет состояние. Автоматически вызывается Unity каждый кадр
        /// </summary>
        void Update() {
            if (lastlastState is null) Init();
            Animate(Time.deltaTime);
        }
    }
}