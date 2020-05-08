
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
        
        public SetGameMode(int gamemodeCode,int roomId,int currentGameNum) {
            this.gamemodeCode = gamemodeCode;
this.roomId = roomId;
this.currentGameNum = currentGameNum;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[12];
arr[0] = (byte)(gamemodeCode & 0x000000ff);
   arr[1] = (byte)((gamemodeCode & 0x0000ff00) >> 8);
   arr[2] = (byte)((gamemodeCode & 0x00ff0000) >> 16);
   arr[3] = (byte)((gamemodeCode & 0xff000000) >> 24);

arr[4] = (byte)(roomId & 0x000000ff);
   arr[5] = (byte)((roomId & 0x0000ff00) >> 8);
   arr[6] = (byte)((roomId & 0x00ff0000) >> 16);
   arr[7] = (byte)((roomId & 0xff000000) >> 24);

arr[8] = (byte)(currentGameNum & 0x000000ff);
   arr[9] = (byte)((currentGameNum & 0x0000ff00) >> 8);
   arr[10] = (byte)((currentGameNum & 0x00ff0000) >> 16);
   arr[11] = (byte)((currentGameNum & 0xff000000) >> 24);


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
            Assert.AreEqual(arr.Length, 12);
            unsafe {
result.gamemodeCode = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.roomId = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));

result.currentGameNum = (arr[8] | (arr[9] << 8) | (arr[10] << 16) | (arr[11] << 24));

             
                return result;
            }

        }
        
        public static SetGameMode Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'gamemodeCode':{gamemodeCode},'roomId':{roomId},'currentGameNum':{currentGameNum}}}";
        }
        
        public override string ToString() {
            return "SetGameMode " + AsJson();
        }
    }
}