using System;
using System.IO;
using System.Linq;
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
            
/*BEGIN2*/
            if (command is ApplyForceCommand applyforcecommand) {
                stream.WriteByte((byte)0);
                var buf = applyforcecommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is CreateGhostCommand createghostcommand) {
                stream.WriteByte((byte)1);
                var buf = createghostcommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is DrawPositionTracerCommand drawpositiontracercommand) {
                stream.WriteByte((byte)2);
                var buf = drawpositiontracercommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is DrawTargetedTracerCommand drawtargetedtracercommand) {
                stream.WriteByte((byte)3);
                var buf = drawtargetedtracercommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is PickCoinCommand pickcoincommand) {
                stream.WriteByte((byte)4);
                var buf = pickcoincommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is PickUpGunCommand pickupguncommand) {
                stream.WriteByte((byte)5);
                var buf = pickupguncommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is PlayerProperty playerproperty) {
                stream.WriteByte((byte)6);
                var buf = playerproperty.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is PlayerPushCommand playerpushcommand) {
                stream.WriteByte((byte)7);
                var buf = playerpushcommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is SpawnPrefabCommand spawnprefabcommand) {
                stream.WriteByte((byte)8);
                var buf = spawnprefabcommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is StartGameCommand startgamecommand) {
                stream.WriteByte((byte)9);
                var buf = startgamecommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
/*END2*/
            {
                throw new ArgumentException("Unkwown command: " + command);
            }
                       
            
        }
        
        private static MemoryStream _stream = new MemoryStream();
        private static BinaryWriter _writer = new BinaryWriter(_stream);

        private void ResetWriteStreams() {
            _stream.SetLength(0);
            _writer.Seek(0, SeekOrigin.Begin);
        }
        
        public byte[] EncodeSimpleCommand<T>(T command) where T : ICommand {
          /*  var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            */
          ResetWriteStreams();
            _writer.Write((int) MessageType.SimpleMessage); 
            
            EncodeCommand(command, _stream);
            return _stream.ToArray();
        }

        public byte[] EncodeUniqCommand<T>(T command, int code1, int code2) where T : ICommand {
            ResetWriteStreams();
            _writer.Write((int) MessageType.UniqMessage);
            _writer.Write(code1);
            _writer.Write(code2);
            
            EncodeCommand(command, _stream);
            return _stream.ToArray();
        }

        public byte[] EncodeAskMessage(int firstIndex, int lastIndex) {
            ResetWriteStreams();
            
            _writer.Write((int) MessageType.AskMessage);
            _writer.Write(firstIndex);
            _writer.Write(lastIndex);

            return _stream.ToArray();
        }

        private static MemoryStream _read_stream = new MemoryStream();
        
        public ICommand DecodeCommand(byte[] array, out int num) {
            var stream = new MemoryStream(array);
            
            var reader = new BinaryReader(stream);
            
            num = reader.ReadInt32();
            
            var commandType = stream.ReadByte();
            byte[] arr = array.Skip(5).ToArray(); // TODO: fix perfomance

         /*   
            _read_stream.SetLength(0);
            _read_stream*/
             switch (commandType) {

/*BEGIN1*/
             case 0:
                     return ApplyForceCommand.Deserialize(arr);
                 case 1:
                     return CreateGhostCommand.Deserialize(arr);
                 case 2:
                     return DrawPositionTracerCommand.Deserialize(arr);
                 case 3:
                     return DrawTargetedTracerCommand.Deserialize(arr);
                 case 4:
                     return PickCoinCommand.Deserialize(arr);
                 case 5:
                     return PickUpGunCommand.Deserialize(arr);
                 case 6:
                     return PlayerProperty.Deserialize(arr);
                 case 7:
                     return PlayerPushCommand.Deserialize(arr);
                 case 8:
                     return SpawnPrefabCommand.Deserialize(arr);
                 case 9:
                     return StartGameCommand.Deserialize(arr);
    /*END1*/

                 default:
                     throw new ArgumentOutOfRangeException("Command type is " + commandType);
             }
        }
    }
    
}

