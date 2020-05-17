using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CommandsSystem;
using CommandsSystem.Commands;
using Game;
using UnityEngine;

namespace Networking {
    /// <summary>
    ///     Перчисление с кодами для уникальных команд
    /// </summary>
    public static class UniqCodes {
        public static int PICK_UP_COIN = 0;
        public static int PICK_UP_GUN = 1;
        public static int SPAWN_COIN = 2;
        public static int SPAWN_GUN = 3;

        public static int ADD_AI_PLAYER = 4;
        public static int CHOOSE_GAMEMODE = 5;
        public static int EXPLODE_BOMB = 6;
        public static int START_GAME = 7;

        public static int SPAWN_PISTOL = 8;
        public static int SPAWN_SEMIAUTO = 9;
        public static int SPAWN_SHOTGUN = 10;
        public static int SPAWN_BOMBGUN = 11;
    }
    
    /// <summary>
    ///     Класс комнаты для общения с сервером
    /// </summary>
    public class ClientCommandsRoom {
        /// <summary>
        ///     Номер последней обработанной команды
        /// </summary>
        private int lastMessage = -1;
        /// <summary>
        ///     Слоаврь с потерянными сообщениями
        /// </summary>
        private OrderedDictionary losedMessages = new OrderedDictionary();
        /// <summary>
        ///     CommandsSystem для кодирования сообщений
        /// </summary>
        private CommandsSystem.CommandsSystem commandsSystem = new CommandsSystem.CommandsSystem();
        /// <summary>
        ///     Когда последний раз был отправлен запрос на переотправку команды
        /// </summary>
        private float lastTimeRequestSended = Time.time;

        /// <summary>
        ///     ID комнаты
        /// </summary>
        public int roomID;
        /// <summary>
        ///     Конструктор комнаты
        /// </summary>
        /// <param name="roomID">ID комнаты</param>
        /// <param name="alreadyJoined">true, если клиент уже приосоединился к комнате</param>
        public ClientCommandsRoom(int roomID, bool alreadyJoined=false) {
            this.roomID = roomID;
            if (!alreadyJoined)
               RunJoinMessage();
            RunAskMessage(0, -1, MessageFlags.NONE); // MessageFlags.SEND_ONLY_IMPORTANT);
        }


        /// <summary>
        ///     Деструктор комнаты. Отправляет команду выхода из комнаты
        /// </summary>
        ~ClientCommandsRoom() {
            RunLeaveMessage();
        }

        /// <summary>
        ///     Отправляет простую команду на сервер
        /// </summary>
        /// <param name="command">Команда</param>
        /// <param name="flags">Флаги</param>
        public void RunSimpleCommand(ICommand command, MessageFlags flags) {
            var data = commandsSystem.EncodeSimpleCommand(command, roomID, flags);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            if (flags.HasFlag(MessageFlags.IMPORTANT))
                UberDebug.LogChannel("SendCommand", "room#" +roomID+$" SimpleCommand {command} {flags}");
        }

        /// <summary>
        ///     Отправляет уникальную команду на сервер
        /// </summary>
        /// <param name="command">Команда</param>
        /// <param name="i1">Первая часть уникального кода</param>
        /// <param name="i2">Вторая часть уникального кода</param>
        /// <param name="flags">Флаги</param>
        public void RunUniqCommand(ICommand command, int i1, int i2, MessageFlags flags) {
            var data = commandsSystem.EncodeUniqCommand(command, roomID, i1, i2, flags);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            if (flags.HasFlag(MessageFlags.IMPORTANT))
                UberDebug.LogChannel("SendCommand", "room#" +roomID+$" UniqCommand {command} {i1} {i2} {flags}");
        }

        /// <summary>
        ///     Отправляет JSON-запрос на сервер
        /// </summary>
        /// <param name="json">JSON строка</param>
        /// <param name="flags">Флаги</param>
        public void RunJsonMessage(string json, MessageFlags flags=MessageFlags.NONE) {
            var data = commandsSystem.EncodeJsonMessage(json, roomID, MessageFlags.NONE);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            UberDebug.LogChannel("SendCommand", "room#" +roomID+$" JsonMessage {json} {flags}");
        }
        
        /// <summary>
        ///     Отправляет команду с запросом сообщений на сервер
        /// </summary>
        /// <param name="firstIndex">Номер первого сообщения, которое нужно переотправить</param>
        /// <param name="lastIndex">Номер последнего сообщения</param>
        /// <param name="flags">Флаги</param>
        private void RunAskMessage(int firstIndex, int lastIndex, MessageFlags flags) {
            var data = commandsSystem.EncodeAskMessage(roomID, firstIndex, lastIndex, flags);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            UberDebug.LogChannel("SendCommand", "room#" +roomID+$" AskMessage {firstIndex} {lastIndex} {flags}");
        }

        /// <summary>
        ///     Отправляет команду присоединения к комнате
        /// </summary>
        private void RunJoinMessage() {
            var data = commandsSystem.EncodeJoinGameRoomMessage(roomID, MessageFlags.NONE);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            UberDebug.LogChannel("Client", $"JoinRoom {roomID}");
        }

        /// <summary>
        ///     Отправляет команду покидания комнаты
        /// </summary>
        private void RunLeaveMessage() {
            var data = commandsSystem.EncodeLeaveGameRoomMessage(roomID, MessageFlags.NONE);
            CommandsHandler.webSocketHandler.clientToServerMessages.Enqueue(data);
            UberDebug.LogChannel("Client", $"LeaveMessage {roomID}");
        }

        /// <summary>
        ///     Выполняет команду
        /// </summary>
        /// <param name="command">Команда</param>
        private void ReceiveCommand(ICommand command) {
            if (command == null) return;

            if (command is ChangePlayerProperty || command is DrawPositionTracerCommand ||
                command is DrawTargetedTracerCommand || command is SetPlatformStateCommand) { } else {
                UberDebug.LogChannel("ReceiveCommand", command.ToString());
            }
            //try {
                command.Run();
            /*} catch (Exception e) {
                Debug.LogException(e);
            }*/
        }

        /// <summary>
        ///     Обрабатывает полученную команду
        /// </summary>
        /// <param name="commandId">Номер команды</param>
        /// <param name="command">Команда</param>
        public void HandleCommand(int commandId, ICommand command) {
            if (commandId == -1) {
                ReceiveCommand(command);
                return;
            }
            
            if (commandId <= lastMessage || losedMessages.Contains(commandId)) {
                UberDebug.LogWarningChannel("ReceiveCommand", "Got message twice. " + command.ToString());
                return;
            }



            if (commandId == lastMessage + 1) {
                lastMessage++;
                ReceiveCommand(command);
            } else {
                losedMessages.Add(commandId, command);
            }
            
            while (losedMessages.Contains(lastMessage + 1)) {
                lastMessage++;
                var c = ((ICommand) losedMessages[(object) lastMessage]);
                ReceiveCommand(c);
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