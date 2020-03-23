using System;
using System.IO;
using System.Reflection;
using CommandsSystem;
using CommandsSystem.Commands;

namespace CommandsSystem {
    
    
    public class CommandsSystem {
        private void EncodeCommand<T>(T command, Stream stream) where T : ICommand {
            stream.WriteByte((byte)command.type());
            MessagePack.MessagePackSerializer..Serialize(command, stream);
        }
        
        public byte[] EncodeSimpleCommand<T>(T command) where T : ICommand {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            
            writer.Write((int) MessageType.SimpleMessage); 
            
            EncodeCommand(command, stream);
            return stream.ToArray();
        }

        public byte[] EncodeUniqCommand<T>(T command, int code1, int code2) where T : ICommand {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            
            writer.Write((int) MessageType.UniqMessage);
            writer.Write(code1);
            writer.Write(code2);
            
            EncodeCommand(command, stream);
            return stream.ToArray();
        }

        public byte[] EncodeAskMessage(int firstIndex, int lastIndex) {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            
            writer.Write((int) MessageType.AskMessage);
            writer.Write(firstIndex);
            writer.Write(lastIndex);

            return stream.ToArray();
        }

        
        
        public ICommand DecodeCommand(byte[] array, out int num) {
            var stream = new MemoryStream(array);
            var reader = new BinaryReader(stream);
            
            num = reader.ReadInt32();
            
            var commandType = (CommandType) stream.ReadByte();

             switch (commandType) {

// BEGIN
             case CommandType.CreateGhostCommand:
                     return MsgPack.Deserialize<CreateGhostCommand>(stream);
             case CommandType.PickCoinCommand:
                     return MsgPack.Deserialize<PickCoinCommand>(stream);
             case CommandType.SpawnPrefabCommand:
                     return MsgPack.Deserialize<SpawnPrefabCommand>(stream);
             case CommandType.StartGameCommand:
                     return MsgPack.Deserialize<StartGameCommand>(stream);

//  END

                 default:
                     throw new ArgumentOutOfRangeException("Command type is " + commandType);
             }
        }
    }
    
}

