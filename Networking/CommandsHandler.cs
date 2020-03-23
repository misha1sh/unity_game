using System.Collections.Generic;
using System.Collections.Specialized;
using CommandsSystem;
using UnityEngine;

namespace Networking {
    public class CommandsHandler {
        private WebSocketHandler webSocketHandler;

        private int lastMessage = -1;
        private OrderedDictionary losedMessages = new OrderedDictionary();
        private CommandsSystem.CommandsSystem commandsSystem = new CommandsSystem.CommandsSystem();

        public CommandsHandler(WebSocketHandler webSocketHandler) {
            this.webSocketHandler = webSocketHandler;
        }

        public void Start() {
            webSocketHandler.Start();
            RunAskMessage(0, int.MaxValue);
        }

        public void Update() {
            webSocketHandler.Update();
        }
        
        public void RunSimpleCommand(ICommand command) {
            var data = commandsSystem.EncodeSimpleCommand(command);
            webSocketHandler.clientToServerMessages.Enqueue(data);
        }

        public void RunUniqCommand(ICommand command, int i1, int i2) {
            var data = commandsSystem.EncodeUniqCommand(command, i1, i2);
            webSocketHandler.clientToServerMessages.Enqueue(data);
        }

        private void RunAskMessage(int firstIndex, int lastIndex) {
            var data = commandsSystem.EncodeAskMessage(firstIndex, lastIndex);
            webSocketHandler.clientToServerMessages.Enqueue(data);
        }

        public IEnumerable<ICommand> GetCommands() {
            byte[] data;
            while (webSocketHandler.serverToClientMessages.TryDequeue(out data)) {
                int commandId;
                ICommand command = commandsSystem.DecodeCommand(data, out commandId);
                if (commandId <= lastMessage || losedMessages.Contains(commandId)) {
                    Debug.LogWarning("CLIENT: Got message twice.");
                    continue;
                }
                losedMessages.Add(commandId, command);
            }

            while (losedMessages.Contains(lastMessage + 1)) {
                lastMessage++;
                ICommand command = (ICommand) losedMessages[(object) lastMessage];
                losedMessages.Remove(lastMessage);
                
                yield return command;
            }

            if (losedMessages.Count != 0) {
                var enumerator = losedMessages.GetEnumerator();
                enumerator.MoveNext();
                int currentId = (int) enumerator.Key;
                
                RunAskMessage(lastMessage + 1, currentId - 1);
                
                Debug.LogWarning($"Server loosed messages from {lastMessage + 1} to {currentId - 1}");
            }
        } 
        
        
        public void Stop() {
            webSocketHandler.Stop();
        }
    }
    
  
}