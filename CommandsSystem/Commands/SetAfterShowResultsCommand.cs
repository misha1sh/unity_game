using GameMode;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда, сообщающая что GameManager должен перестать показывать результаты игры
    /// </summary>
    public partial class SetAfterShowResultsCommand {
        /// <summary>
        ///     Переменная для корректной работы сериализации
        /// </summary>
        public int _;
        
        /// <summary>
        ///     Изменяет состояние GameManager на соотвестующее команде
        /// </summary>
        public void Run() {
            GameManager.SetAfterShowResults();
        }
    }
}