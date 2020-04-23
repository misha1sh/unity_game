
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
    public partial class PlayerPushCommand : ICommand  {

        public PlayerPushCommand(){}
        
        public PlayerPushCommand(int playerId) {
            this.playerId = playerId;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[4];
arr[0] = (byte)(playerId & 0x000000ff);
   arr[1] = (byte)((playerId & 0x0000ff00) >> 8);
   arr[2] = (byte)((playerId & 0x00ff0000) >> 16);
   arr[3] = (byte)((playerId & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static PlayerPushCommand DeserializeLittleEndian(byte[] arr) {
            var result = new PlayerPushCommand();
            Assert.AreEqual(arr.Length, 4);
            unsafe {
result.playerId = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

             
                return result;
            }

        }
        
        public static PlayerPushCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'playerId':{playerId}}}";
        }
        
        public override string ToString() {
            return "PlayerPushCommand " + AsJson();
        }
    }
}