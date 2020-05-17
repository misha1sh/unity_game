using CommandsSystem;
using UnityEngine;

namespace Interpolation.Properties {
    /// <summary>
    ///     Интерфейс для состояния, которое можно синхронизировать по сети
    /// </summary>
    public interface IGameObjectProperty {
        /// <summary>
        ///     Копирует состояние из другого
        /// </summary>
        /// <param name="state">Состояние из которого нужно копировать</param>
        void CopyFrom(IGameObjectProperty state);
        
        /// <summary>
        ///     Получает состояние из объекта
        /// </summary>
        /// <param name="gameObject">Объект</param>
        void FromGameObject(GameObject gameObject);
        
        /// <summary>
        ///     Применяет состояние к объекту
        /// </summary>
        /// <param name="gameObject">Объект</param>
        void ApplyToObject(GameObject gameObject);

        /// <summary>
        ///     Интерполирует состояние между другими
        /// </summary>
        /// <param name="lastLastState">Предпредыдущее состояние</param>
        /// <param name="lastState">Предыдущее состояние</param>
        /// <param name="nextState">Следующее состояние</param>
        /// <param name="coef">Коэффициент интерполяции между состояниями (от 0 до 1)</param>
        void Interpolate(
            IGameObjectProperty lastLastState,
            IGameObjectProperty lastState,
            IGameObjectProperty nextState, 
            float coef);

        /// <summary>
        ///     Создаёт команду для отправки состояния другим клиентам
        /// </summary>
        /// <param name="deltaTime">Время, прошедшее с прошлой отправки</param>
        /// <returns>Команду</returns>
        ICommand CreateChangedCommand(float deltaTime);
    }
}