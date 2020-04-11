
using System;

namespace CommandsSystem.Commands {
    public partial class SetGameMode : ICommand  {

        public SetGameMode(){}
        
        public SetGameMode(int gamemode) {
            this.gamemode = gamemode;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[4];
arr[0] = (byte)(gamemode & 0x000000ff);
   arr[1] = (byte)((gamemode & 0x0000ff00) >> 8);
   arr[2] = (byte)((gamemode & 0x00ff0000) >> 16);
   arr[3] = (byte)((gamemode & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static SetGameMode DeserializeLittleEndian(byte[] arr) {
            var result = new SetGameMode();
            unsafe {
result.gamemode = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

             
                return result;
            }

        }
        
        public static SetGameMode Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'gamemode':{gamemode}}}";
        }
        
        public override string ToString() {
            return "SetGameMode " + AsJson();
        }
    }
}