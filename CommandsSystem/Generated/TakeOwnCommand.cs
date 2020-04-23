
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
    public partial class TakeOwnCommand : ICommand  {

        public TakeOwnCommand(){}
        
        public TakeOwnCommand(int objectId,int owner) {
            this.objectId = objectId;
this.owner = owner;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[8];
arr[0] = (byte)(objectId & 0x000000ff);
   arr[1] = (byte)((objectId & 0x0000ff00) >> 8);
   arr[2] = (byte)((objectId & 0x00ff0000) >> 16);
   arr[3] = (byte)((objectId & 0xff000000) >> 24);

arr[4] = (byte)(owner & 0x000000ff);
   arr[5] = (byte)((owner & 0x0000ff00) >> 8);
   arr[6] = (byte)((owner & 0x00ff0000) >> 16);
   arr[7] = (byte)((owner & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static TakeOwnCommand DeserializeLittleEndian(byte[] arr) {
            var result = new TakeOwnCommand();
            Assert.AreEqual(arr.Length, 8);
            unsafe {
result.objectId = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.owner = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));

             
                return result;
            }

        }
        
        public static TakeOwnCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'objectId':{objectId},'owner':{owner}}}";
        }
        
        public override string ToString() {
            return "TakeOwnCommand " + AsJson();
        }
    }
}