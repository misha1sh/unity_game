using CommandsSystem;

namespace Networking {
    public static class CommandsHandler {
        public static WebSocketHandler webSocketHandler = new WebSocketHandler();
        private static CommandsSystem.CommandsSystem commandsSystem = new CommandsSystem.CommandsSystem();

        public static ClientCommandsRoom gameRoom; // current game
        public static ClientCommandsRoom gameModeRoom; // current gamemode
        
        public static void Init() {
            webSocketHandler.Start();
        }

        public static void Update() {
            webSocketHandler.Update();
            byte[] data;
            
            while (CommandsHandler.webSocketHandler.serverToClientMessages.TryDequeue(out data)) {
                int commandId, roomId;
                ICommand command = commandsSystem.DecodeCommand(data, out commandId, out roomId);
                if (gameRoom?.roomID == roomId) {
                    gameRoom.HandleCommand(commandId, command);
                } else if (gameModeRoom?.roomID == roomId) {
                    gameModeRoom.HandleCommand(commandId, command);
                }
            }
        }

        public static void Stop() {
            webSocketHandler?.Stop();
        }
    }
}