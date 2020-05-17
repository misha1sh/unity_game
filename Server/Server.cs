using UnityEngine;
using System;
/////
/////     Message formats:
/////
/////     Client to Server
/////         1: simple message
/////             - 1 byte message type + 4 byte room number + 1 flags + data
/////             - send to all clients
/////         2: uniq message
/////             - 1 byte message type + 4 byte room number + 1 flags + 8 byte uniq code(4 byte code type, 4 byte code num) + data
/////             - if uniq code is new, message will be send to all clients. otherwise, message will be discarded
/////         3: ask for new messages
/////             - 1 byte message type + 4 byte room number + 1 flags + 4 byte first message + 4 byte last message
/////         4: join game room. if room does not exists, it must be created
/////             - 1 byte message type + 4 byte room number + 1 flags
/////             - optional: autoremove player if he not doing anything in room for some time (ping - pong)
/////         5: leave game room. if last player leaved room, it must be deleted;
/////             - 1 byte message type + 4 byte room number + 1 flags
///// 
/////    Server to Client:
/////         - 4 byte message id + 4 byte room number + message data
/////         if message id == -1 means client need to do command instantly
/////         - optional ping: 4 byte message id + 4 byte data
///// 
/////

/// <summary>
///     Перечиcления со видами сообщений, которые можно отправить на сервер
/// </summary>
public enum MessageType : byte {
    SimpleMessage = 1,
    UniqMessage = 2,
    AskMessage = 3,
    JoinGameRoom = 4,
    LeaveGameRoom = 5,
    JSON = 6,
};

/// <summary>
///     Флаги для отправки сообщений на сервер
/// </summary>
[Flags] 
public enum MessageFlags : byte {
    NONE = 0,
    IMPORTANT = 1 << 0,
    SEND_ONLY_IMPORTANT = 1 << 1
}