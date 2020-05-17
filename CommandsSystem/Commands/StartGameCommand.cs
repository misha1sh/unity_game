namespace CommandsSystem.Commands {
    
    /// <summary>
    ///     Команда начать игру
    /// </summary>
    public partial class StartGameCommand {
        public int _;

        /// <summary>
        ///     Начинает игру
        /// </summary>
        public void Run() {
            sClient.SetGameStarted();
        }
    }
}