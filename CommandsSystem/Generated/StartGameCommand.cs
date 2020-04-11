
using System;

namespace CommandsSystem.Commands {
    public partial class StartGameCommand : ICommand  {

        public StartGameCommand(){}
        
        public StartGameCommand(int kek) {
            this.kek = kek;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[4];
arr[0] = (byte)(kek & 0x000000ff);
   arr[1] = (byte)((kek & 0x0000ff00) >> 8);
   arr[2] = (byte)((kek & 0x00ff0000) >> 16);
   arr[3] = (byte)((kek & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static StartGameCommand DeserializeLittleEndian(byte[] arr) {
            var result = new StartGameCommand();
            unsafe {
result.kek = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

             
                return result;
            }

        }
        
        public static StartGameCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'kek':{kek}}}";
        }
        
        public override string ToString() {
            return "StartGameCommand " + AsJson();
        }
    }
}