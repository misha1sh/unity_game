namespace Networking {
    public static class CommandsHandler {
        public static WebSocketHandler webSocketHandler;

        public static void Init() {
            webSocketHandler = new WebSocketHandler();
            webSocketHandler.Start();
        }

        public static void Update() {
            
        }
    }
}