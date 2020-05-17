
using System;
using System.Text;
using Character;
using Interpolation;
using Interpolation.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using Character.HP;
using CommandsSystem;
using GameMode;

namespace CommandsSystem.Commands {
    public partial class StartGameCommand : ICommand  {

        public StartGameCommand(){}
        
        public StartGameCommand(int _) {
            this._ = _;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[4];
arr[0] = (byte)(_ & 0x000000ff);
   arr[1] = (byte)((_ & 0x0000ff00) >> 8);
   arr[2] = (byte)((_ & 0x00ff0000) >> 16);
   arr[3] = (byte)((_ & 0xff000000) >> 24);


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
            Assert.AreEqual(arr.Length, 4);
            unsafe {
result._ = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

             
                return result;
            }

        }
        
        public static StartGameCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'_':{_}}}";
        }
        
        public override string ToString() {
            return "StartGameCommand " + AsJson();
        }
    }
}