using System;
using CommandsSystem;

namespace Networking {
    public static class CommandsHandler {
        public static WebSocketHandler webSocketHandler = new WebSocketHandler();
        private static CommandsSystem.CommandsSystem commandsSystem = new CommandsSystem.CommandsSystem();


        public static ClientCommandsRoom matchmakingRoom;

        public static ClientCommandsRoom gameRoom; // current game
        public static ClientCommandsRoom gameModeRoom; // current gamemode
        
        public static void Init() {
            webSocketHandler.Start();
        }

        public static ClientCommandsRoom RoomById(int id) {
            if (matchmakingRoom?.roomID == id)
                return matchmakingRoom;
            if (gameRoom?.roomID == id)
                return gameRoom;
            if (gameModeRoom?.roomID == id)
                return gameModeRoom;
            return null;
        }

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

        public static void Stop() {
            webSocketHandler?.Stop();
        }
    }
}