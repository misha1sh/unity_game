
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
    public partial class ExplodeBombCommand : ICommand  {

        public ExplodeBombCommand(){}
        
        public ExplodeBombCommand(int bombId) {
            this.bombId = bombId;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[4];
arr[0] = (byte)(bombId & 0x000000ff);
   arr[1] = (byte)((bombId & 0x0000ff00) >> 8);
   arr[2] = (byte)((bombId & 0x00ff0000) >> 16);
   arr[3] = (byte)((bombId & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static ExplodeBombCommand DeserializeLittleEndian(byte[] arr) {
            var result = new ExplodeBombCommand();
            Assert.AreEqual(arr.Length, 4);
            unsafe {
result.bombId = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

             
                return result;
            }

        }
        
        public static ExplodeBombCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'bombId':{bombId}}}";
        }
        
        public override string ToString() {
            return "ExplodeBombCommand " + AsJson();
        }
    }
}