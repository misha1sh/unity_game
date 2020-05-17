namespace GameMode {
    /// <summary>
    ///     Интерфейс для игрового режима
    /// </summary>
    public interface IGameMode {
        /// <summary>
        ///     Обновляет состояние игрового режима
        /// </summary>
        /// <returns>false, если режим закончился. Иначе true</returns>
        bool Update();
        /// <summary>
        ///     Завершает выполнение игрового режима
        /// </summary>
        /// <returns>false, если режим закончился. true, если надо ещё подождать</returns>
        bool Stop();
        /// <summary>
        ///    Время в секундах, которое длится игровой режим 
        /// </summary>
        float TimeLength { get; }
    }
}