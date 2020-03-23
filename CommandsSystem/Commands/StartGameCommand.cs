using System;

namespace CommandsSystem {
    
    [Serializable]
    public class StartGameCommand: Command<StartGameCommand> {
        public StartGameCommand() {
            
        }
        
        public override CommandType type() => CommandType.StartGameCommand;
        

        public override void Run() {
            Client.client.SetGameStarted();
        }
        
    }
}