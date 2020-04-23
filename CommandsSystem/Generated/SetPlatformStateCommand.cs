
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
    public partial class SetPlatformStateCommand : ICommand  {

        public SetPlatformStateCommand(){}
        
        public SetPlatformStateCommand(int id,int direction) {
            this.id = id;
this.direction = direction;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[8];
arr[0] = (byte)(id & 0x000000ff);
   arr[1] = (byte)((id & 0x0000ff00) >> 8);
   arr[2] = (byte)((id & 0x00ff0000) >> 16);
   arr[3] = (byte)((id & 0xff000000) >> 24);

arr[4] = (byte)(direction & 0x000000ff);
   arr[5] = (byte)((direction & 0x0000ff00) >> 8);
   arr[6] = (byte)((direction & 0x00ff0000) >> 16);
   arr[7] = (byte)((direction & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static SetPlatformStateCommand DeserializeLittleEndian(byte[] arr) {
            var result = new SetPlatformStateCommand();
            Assert.AreEqual(arr.Length, 8);
            unsafe {
result.id = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.direction = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));

             
                return result;
            }

        }
        
        public static SetPlatformStateCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'id':{id},'direction':{direction}}}";
        }
        
        public override string ToString() {
            return "SetPlatformStateCommand " + AsJson();
        }
    }
}