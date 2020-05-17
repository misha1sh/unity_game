using System;
using CommandsSystem;

namespace Networking {
    /// <summary>
    ///     Класс для обработки команд с сервера
    /// </summary>
    public static class CommandsHandler {
        /// <summary>
        ///     Обработчик WebSocket
        /// </summary>
        public static WebSocketHandler webSocketHandler = new WebSocketHandler();
        /// <summary>
        ///     CommandsSystem для сериализации команд
        /// </summary>
        private static CommandsSystem.CommandsSystem commandsSystem = new CommandsSystem.CommandsSystem();


        /// <summary>
        ///     Комната для поиска матча
        /// </summary>
        public static ClientCommandsRoom matchmakingRoom;

        /// <summary>
        ///     Комната для матча (игры)
        /// </summary>
        public static ClientCommandsRoom gameRoom;
        /// <summary>
        ///     Комната для игрового режима
        /// </summary>
        public static ClientCommandsRoom gameModeRoom;
        

        /// <summary>
        ///     Сбрасывает значения переменных
        /// </summary>
        public static void Reset() {
            matchmakingRoom = gameRoom = gameModeRoom = null;
        }

        /// <summary>
        ///     Полчает комнату по ID
        /// </summary>
        /// <param name="id">ID комнаты</param>
        /// <returns>Комнату</returns>
        public static ClientCommandsRoom RoomById(int id) {
            if (matchmakingRoom?.roomID == id)
                return matchmakingRoom;
            if (gameRoom?.roomID == id)
                return gameRoom;
            if (gameModeRoom?.roomID == id)
                return gameModeRoom;
            return null;
        }

        /// <summary>
        ///     Получает и отправляет команды
        /// </summary>
        public static void Update() {
            webSocketHandler.Update();
            byte[] data;
            
            while (CommandsHandler.webSocketHandler.serverToClientMessages.TryDequeue(out data)) {
                int commandId, roomId;
                ICommand command = commandsSystem.DecodeCommand(data, out commandId, out roomId);
                var room = RoomById(roomId);
                if (room != null) {
                    room.HandleCommand(commandId, command);
                } else {
                    throw new Exception($"unhandled command to room {roomId}");
                }
            }
        }

        /// <summary>
        ///     Отключается от сервера
        /// </summary>
        public static void Stop() {
            webSocketHandler?.Stop();
        }
    }
}