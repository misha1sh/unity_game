using GameMode;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда, сообщающая, что нужно запускать заданный игровой режим
    /// </summary>
    public partial class SetGameMode {
        /// <summary>
        ///     Код игрового режима
        /// </summary>
        public int gamemodeCode;
        /// <summary>
        ///     Номер комнаты, в которой будет проводиться данный игровой режим
        /// </summary>
        public int roomId;
        /// <summary>
        ///     Номер игры по порядку
        /// </summary>
        public int currentGameNum;

        /// <summary>
        ///     Загружает заданный игровой режим
        /// </summary>
        public void Run() {
            GameManager.SetGameMode(gamemodeCode, roomId, currentGameNum);
        }
    }
}