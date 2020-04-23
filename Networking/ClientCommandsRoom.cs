using System.Collections.Generic;
using System.Collections.Specialized;
using CommandsSystem;
using CommandsSystem.Commands;
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
            RunAskMessage(0, int.MaxValue, MessageFlags.SEND_ONLY_IMPORTANT);
        }

        ~ClientCommandsRoom() {
            RunLeaveMessage();
        }

        
        public void RunSimpleCommand(ICommand command, MessageFlags flags) {
            var data = commandsSystem.EncodeSimpleCommand(command, roomID, flags);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            if (flags.HasFlag(MessageFlags.IMPORTANT))
                UberDebug.LogChannel("SendCommand", $"SimpleCommand {command}");
        }

        public void RunUniqCommand(ICommand command, int i1, int i2, MessageFlags flags) {
            var data = commandsSystem.EncodeUniqCommand(command, roomID, i1, i2, flags);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            if (flags.HasFlag(MessageFlags.IMPORTANT))
                UberDebug.LogChannel("SendCommand", $"UniqCommand {command} {i1} {i2}");
        }

        private void RunAskMessage(int firstIndex, int lastIndex, MessageFlags flags) {
            var data = commandsSystem.EncodeAskMessage(roomID, firstIndex, lastIndex, flags);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            UberDebug.LogChannel("SendCommand", $"AskMessage {firstIndex} {lastIndex} {flags}");
        }

        private void RunJoinMessage() {
            var data = commandsSystem.EncodeJoinGameRoomMessage(roomID, MessageFlags.NONE);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            UberDebug.LogChannel("Client", $"JoinRoom {roomID}");
        }

        private void RunLeaveMessage() {
            var data = commandsSystem.EncodeLeaveGameRoomMessage(roomID, MessageFlags.NONE);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            UberDebug.LogChannel("Client", $"LeaveMessage {roomID}");
        }
        
        
        public void HandleCommand(int commandId, ICommand command) {
            if (commandId <= lastMessage || losedMessages.Contains(commandId)) {
                Debug.LogWarning("CLIENT: Got message twice.");
                return;
            }

            if (commandId == lastMessage + 1) {
                lastMessage++;
                if (command is ChangePlayerProperty || command is DrawPositionTracerCommand ||
                    command is DrawTargetedTracerCommand) {
                    // skip
                } else {
                    UberDebug.LogChannel("RecieveCommand", command.ToString());
                }
                command.Run();
            } else {
                losedMessages.Add(commandId, command);
            }
            
            while (losedMessages.Contains(lastMessage + 1)) {
                lastMessage++;
                ((ICommand) losedMessages[(object) lastMessage]).Run();
                losedMessages.Remove(lastMessage);
            }
            
            
            if (losedMessages.Count != 0 && Time.time - lastTimeRequestSended > 2f) {
                lastTimeRequestSended = Time.time;

                var enumerator = losedMessages.GetEnumerator();
                enumerator.MoveNext();
                int currentId = (int) enumerator.Key;

                RunAskMessage(lastMessage + 1, currentId - 1, MessageFlags.NONE);
                
                Debug.LogWarning($"Server loosed messages from {lastMessage + 1} to {currentId - 1}");
            }
        }
    }
    
  
}