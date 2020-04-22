using System.Collections.Generic;
using System.Collections.Specialized;
using CommandsSystem;
using UnityEngine;

namespace Networking {
    public static class UniqCodes {
        public static int PICK_UP_COIN = 0;
        public static int PICK_UP_GUN = 1;
        public static int SPAWN_COIN = 2;
        public static int SPAWN_GUN = 3;

        public static int ADD_AI_PLAYER = 4;
    }
    
    public class ClientCommandsRoom {

        private int lastMessage = -1;
        private OrderedDictionary losedMessages = new OrderedDictionary();
        private CommandsSystem.CommandsSystem commandsSystem = new CommandsSystem.CommandsSystem();
        private float lastTimeRequestSended = Time.time;

        public int roomID;
        public ClientCommandsRoom(int roomID) {
            this.roomID = roomID;
            RunJoinMessage();
        }

        ~ClientCommandsRoom() {
            RunLeaveMessage();
        }

        
        public void RunSimpleCommand(ICommand command, byte needStore) {
            var data = commandsSystem.EncodeSimpleCommand(command, roomID, needStore);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            UberDebug.LogChannel("SendCommand", $"SimpleCommand{command}");
        }

        public void RunUniqCommand(ICommand command, byte needStore, int i1, int i2) {
            var data = commandsSystem.EncodeUniqCommand(command, roomID, needStore, i1, i2);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
        }

        private void RunAskMessage(int firstIndex, int lastIndex) {
            var data = commandsSystem.EncodeAskMessage(roomID, firstIndex, lastIndex);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
        }

        private void RunJoinMessage() {
            UberDebug.LogChannel("Client", $"JoinRoom{roomID}");
            var data = commandsSystem.EncodeJoinGameRoomMessage(roomID);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
        }

        private void RunLeaveMessage() {
            var data = commandsSystem.EncodeLeaveGameRoomMessage(roomID);
            sClient.webSocketHandler.clientToServerMessages.Enqueue(data);
        }
        
        void HandleCommand(int commandId, ICommand command) {
            
        }    
        
        public IEnumerable<ICommand> GetCommands() {
            bool loggedTwice = false;
            byte[] data;
            while (sClient.webSocketHandler.serverToClientMessages.TryDequeue(out data)) {
                int commandId, roomId;
                ICommand command = commandsSystem.DecodeCommand(data, out commandId, out roomId);
                if (commandId <= lastMessage || losedMessages.Contains(commandId)) {
                    if (!loggedTwice) {
                        loggedTwice = true;
                        Debug.LogWarning("CLIENT: Got message twice.");
                    }
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
            
            if (losedMessages.Count != 0 && Time.time - lastTimeRequestSended > 1.0f) {
                lastTimeRequestSended = Time.time;

                var enumerator = losedMessages.GetEnumerator();
                    enumerator.MoveNext();
                    int currentId = (int) enumerator.Key;
 
                    RunAskMessage(lastMessage + 1, currentId - 1);
                
                Debug.LogWarning($"Server loosed messages from {lastMessage + 1} to {currentId - 1}");
            }
        } 
        
        
    }
    
  
}