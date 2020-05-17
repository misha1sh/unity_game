namespace Character.Actions {
    /// <summary>
    ///     Интерфейс действия пользователя
    /// </summary>
    public interface IAction {
        /// <summary>
        ///     Функция, которая вызывается, когда игрок начинает делать действие
        /// </summary>
        void OnStartDoing();
        
        /// <summary>
        ///     Функция, которая вызывается, когда игрок прекращает делать действие
        /// </summary>
        void OnStopDoing();
    }
}