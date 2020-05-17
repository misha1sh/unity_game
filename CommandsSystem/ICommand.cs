namespace CommandsSystem {
    /// <summary>
    ///     Интерфейс для команды
    /// </summary>
    public interface ICommand {
        /// <summary>
        ///     Выполняет команду
        /// </summary>
        void Run();
    }
}