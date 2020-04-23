
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
    public partial class SetGameMode : ICommand  {

        public SetGameMode(){}
        
        public SetGameMode(int gamemode,int roomId) {
            this.gamemode = gamemode;
this.roomId = roomId;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[8];
arr[0] = (byte)(gamemode & 0x000000ff);
   arr[1] = (byte)((gamemode & 0x0000ff00) >> 8);
   arr[2] = (byte)((gamemode & 0x00ff0000) >> 16);
   arr[3] = (byte)((gamemode & 0xff000000) >> 24);

arr[4] = (byte)(roomId & 0x000000ff);
   arr[5] = (byte)((roomId & 0x0000ff00) >> 8);
   arr[6] = (byte)((roomId & 0x00ff0000) >> 16);
   arr[7] = (byte)((roomId & 0xff000000) >> 24);


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
            Assert.AreEqual(arr.Length, 8);
            unsafe {
result.gamemode = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.roomId = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));

             
                return result;
            }

        }
        
        public static SetGameMode Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'gamemode':{gamemode},'roomId':{roomId}}}";
        }
        
        public override string ToString() {
            return "SetGameMode " + AsJson();
        }
    }
}