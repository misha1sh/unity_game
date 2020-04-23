
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
    public partial class PickCoinCommand : ICommand  {

        public PickCoinCommand(){}
        
        public PickCoinCommand(int player,int coin) {
            this.player = player;
this.coin = coin;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[8];
arr[0] = (byte)(player & 0x000000ff);
   arr[1] = (byte)((player & 0x0000ff00) >> 8);
   arr[2] = (byte)((player & 0x00ff0000) >> 16);
   arr[3] = (byte)((player & 0xff000000) >> 24);

arr[4] = (byte)(coin & 0x000000ff);
   arr[5] = (byte)((coin & 0x0000ff00) >> 8);
   arr[6] = (byte)((coin & 0x00ff0000) >> 16);
   arr[7] = (byte)((coin & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static PickCoinCommand DeserializeLittleEndian(byte[] arr) {
            var result = new PickCoinCommand();
            Assert.AreEqual(arr.Length, 8);
            unsafe {
result.player = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.coin = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));

             
                return result;
            }

        }
        
        public static PickCoinCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'player':{player},'coin':{coin}}}";
        }
        
        public override string ToString() {
            return "PickCoinCommand " + AsJson();
        }
    }
}