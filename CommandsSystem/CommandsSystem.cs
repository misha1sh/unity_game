using System;
using System.IO;
using System.Linq;
using Character.Guns;
using CommandsSystem.Commands;

namespace CommandsSystem {
    
    
    public class CommandsSystem {
        private void EncodeCommand<T>(T command, Stream stream) where T : ICommand {

            
/*BEGIN2*/
            if (command is AddPlayerToGame addplayertogame) {
                stream.WriteByte((byte)0);
                var buf = addplayertogame.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is ApplyForceCommand applyforcecommand) {
                stream.WriteByte((byte)1);
                var buf = applyforcecommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is ChangePlayerProperty changeplayerproperty) {
                stream.WriteByte((byte)2);
                var buf = changeplayerproperty.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is ChangePlayerScore changeplayerscore) {
                stream.WriteByte((byte)3);
                var buf = changeplayerscore.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is CreateGhostCommand createghostcommand) {
                stream.WriteByte((byte)4);
                var buf = createghostcommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is DrawPositionTracerCommand drawpositiontracercommand) {
                stream.WriteByte((byte)5);
                var buf = drawpositiontracercommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is DrawTargetedTracerCommand drawtargetedtracercommand) {
                stream.WriteByte((byte)6);
                var buf = drawtargetedtracercommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is PickCoinCommand pickcoincommand) {
                stream.WriteByte((byte)7);
                var buf = pickcoincommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is PickUpGunCommand pickupguncommand) {
                stream.WriteByte((byte)8);
                var buf = pickupguncommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is PlayerPushCommand playerpushcommand) {
                stream.WriteByte((byte)9);
                var buf = playerpushcommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is SetGameMode setgamemode) {
                stream.WriteByte((byte)10);
                var buf = setgamemode.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is SetPlatformStateCommand setplatformstatecommand) {
                stream.WriteByte((byte)11);
                var buf = setplatformstatecommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is SpawnPlayerCommand spawnplayercommand) {
                stream.WriteByte((byte)12);
                var buf = spawnplayercommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is SpawnPrefabCommand spawnprefabcommand) {
                stream.WriteByte((byte)13);
                var buf = spawnprefabcommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is StartGameCommand startgamecommand) {
                stream.WriteByte((byte)14);
                var buf = startgamecommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is TakeOwnCommand takeowncommand) {
                stream.WriteByte((byte)15);
                var buf = takeowncommand.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is Pistol pistol) {
                stream.WriteByte((byte)16);
                var buf = pistol.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is ShotGun shotgun) {
                stream.WriteByte((byte)17);
                var buf = shotgun.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
            if (command is SemiautoGun semiautogun) {
                stream.WriteByte((byte)18);
                var buf = semiautogun.Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
/*END2*/
            {
                throw new ArgumentException("Unkwown command: " + command.GetType());
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
                     return AddPlayerToGame.Deserialize(arr);
                 case 1:
                     return ApplyForceCommand.Deserialize(arr);
                 case 2:
                     return ChangePlayerProperty.Deserialize(arr);
                 case 3:
                     return ChangePlayerScore.Deserialize(arr);
                 case 4:
                     return CreateGhostCommand.Deserialize(arr);
                 case 5:
                     return DrawPositionTracerCommand.Deserialize(arr);
                 case 6:
                     return DrawTargetedTracerCommand.Deserialize(arr);
                 case 7:
                     return PickCoinCommand.Deserialize(arr);
                 case 8:
                     return PickUpGunCommand.Deserialize(arr);
                 case 9:
                     return PlayerPushCommand.Deserialize(arr);
                 case 10:
                     return SetGameMode.Deserialize(arr);
                 case 11:
                     return SetPlatformStateCommand.Deserialize(arr);
                 case 12:
                     return SpawnPlayerCommand.Deserialize(arr);
                 case 13:
                     return SpawnPrefabCommand.Deserialize(arr);
                 case 14:
                     return StartGameCommand.Deserialize(arr);
                 case 15:
                     return TakeOwnCommand.Deserialize(arr);
                 case 16:
                     return Pistol.Deserialize(arr);
                 case 17:
                     return ShotGun.Deserialize(arr);
                 case 18:
                     return SemiautoGun.Deserialize(arr);
    /*END1*/

                 default:
                     throw new ArgumentOutOfRangeException("Command type is " + commandType);
             }
        }
    }
    
}

