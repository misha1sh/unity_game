using System;
using CommandsSystem;
using UnityEngine;

namespace Interpolation.Properties {
    
    /// <summary>
    ///     Базовый класс для состояния, которое можно синхронизировать по сети
    /// </summary>
    /// <typeparam name="T">Тип состояния</typeparam>
    [Serializable]
    public abstract class GameObjectProperty<T> : IGameObjectProperty
        where T : GameObjectProperty<T>, new() { 

        /// <summary>
        ///     Копирует состояние из другого
        /// </summary>
        /// <param name="state">Состояние из которого нужно копировать</param>
        public abstract void CopyFrom(T state);

        /// <summary>
        ///     Копирует состояние из другого
        /// </summary>
        /// <param name="state">Состояние из которого нужно копировать</param>
        public void CopyFrom(IGameObjectProperty state) {
            CopyFrom(state as T);
        }
        
        /// <summary>
        ///     Интерполирует состояние между другими
        /// </summary>
        /// <param name="lastLastState">Предпредыдущее состояние</param>
        /// <param name="lastState">Предыдущее состояние</param>
        /// <param name="nextState">Следующее состояние</param>
        /// <param name="coef">Коэффициент интерполяции между состояниями (от 0 до 1)</param>
        public abstract void Interpolate(
            T lastLastState,
            T lastState,
            T nextState, 
            float coef);
        
        /// <summary>
        ///     Интерполирует состояние между другими
        /// </summary>
        /// <param name="lastLastState">Предпредыдущее состояние</param>
        /// <param name="lastState">Предыдущее состояние</param>
        /// <param name="nextState">Следующее состояние</param>
        /// <param name="coef">Коэффициент интерполяции между состояниями (от 0 до 1)</param>
        public void Interpolate(IGameObjectProperty lastLastState, IGameObjectProperty lastState, IGameObjectProperty nextState,
            float coef) {
            Interpolate(lastLastState as T, lastState as T, nextState as T, coef);
        }

        /// <summary>
        ///     Создаёт команду для отправки состояния другим клиентам
        /// </summary>
        /// <param name="deltaTime">Время, прошедшее с прошлой отправки</param>
        /// <returns>Команду</returns>
        public abstract ICommand CreateChangedCommand(float deltaTime);


        /// <summary>
        ///     Получает состояние из объекта
        /// </summary>
        /// <param name="gameObject">Объект</param>
        public abstract void FromGameObject(GameObject gameObject);
        
        /// <summary>
        ///     Применяет состояние к объекту
        /// </summary>
        /// <param name="gameObject">Объект</param>
        public abstract void ApplyToObject(GameObject gameObject);


    }
}