namespace CommandsSystem.Commands {
    
    public partial class StartGameCommand {
        public int kek;

        public void Run() {
            Client.client.SetGameStarted();
        }
        
    }
}