using System;
using System.IO;
using System.Reflection;
using CommandsSystem;
using CommandsSystem.Commands;
using GameDevWare.Serialization;
using Interpolation;
using Interpolation.Properties;

namespace CommandsSystem {
    
    
    public class CommandsSystem {
        private void EncodeCommand<T>(T command, Stream stream) where T : ICommand {

            byte type;
            
/*BEGIN2*/            if (command is ApplyForceCommand) {
                    type = 0;
               } else             if (command is CreateGhostCommand) {
                    type = 1;
               } else             if (command is PickCoinCommand) {
                    type = 2;
               } else             if (command is PlayerPushCommand) {
                    type = 3;
               } else             if (command is SpawnPrefabCommand) {
                    type = 4;
               } else             if (command is StartGameCommand) {
                    type = 5;
               } else             if (command is ChangeGameObjectStateCommand<PlayerProperty>) {
                    type = 6;
               } else             if (command is ChangeGameObjectStateCommand<TransformProperty>) {
                    type = 7;
               } else /*END2*/
            {
                throw new ArgumentException("Unkwown command: " + command);
            }
                       
                  
            stream.WriteByte((byte)type);
            MsgPack.Serialize(command, stream);
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
            
            var commandType = stream.ReadByte();

             switch (commandType) {

/*BEGIN1*/             case 0:
                     return MsgPack.Deserialize<ApplyForceCommand>(stream);
             case 1:
                     return MsgPack.Deserialize<CreateGhostCommand>(stream);
             case 2:
                     return MsgPack.Deserialize<PickCoinCommand>(stream);
             case 3:
                     return MsgPack.Deserialize<PlayerPushCommand>(stream);
             case 4:
                     return MsgPack.Deserialize<SpawnPrefabCommand>(stream);
             case 5:
                     return MsgPack.Deserialize<StartGameCommand>(stream);
             case 6:
                     return MsgPack.Deserialize<ChangeGameObjectStateCommand<PlayerProperty>>(stream);
             case 7:
                     return MsgPack.Deserialize<ChangeGameObjectStateCommand<TransformProperty>>(stream);
/*END1*/

                 default:
                     throw new ArgumentOutOfRangeException("Command type is " + commandType);
             }
        }
    }
    
}

