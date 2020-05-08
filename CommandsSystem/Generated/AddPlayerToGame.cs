
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
    public partial class AddPlayerToGame : ICommand  {

        public AddPlayerToGame(){}
        
        public AddPlayerToGame(Player player) {
            this.player = player;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[53];
arr[0] = (byte)(player.id & 0x000000ff);
   arr[1] = (byte)((player.id & 0x0000ff00) >> 8);
   arr[2] = (byte)((player.id & 0x00ff0000) >> 16);
   arr[3] = (byte)((player.id & 0xff000000) >> 24);

arr[4] = (byte)(player.score & 0x000000ff);
   arr[5] = (byte)((player.score & 0x0000ff00) >> 8);
   arr[6] = (byte)((player.score & 0x00ff0000) >> 16);
   arr[7] = (byte)((player.score & 0xff000000) >> 24);

var bytes_player_name= Encoding.UTF8.GetBytes(player.name);
    Assert.IsTrue(bytes_player_name.Length <= 25);

arr[8] = (byte)(bytes_player_name.Length & 0x000000ff);
   arr[9] = (byte)((bytes_player_name.Length & 0x0000ff00) >> 8);
   arr[10] = (byte)((bytes_player_name.Length & 0x00ff0000) >> 16);
   arr[11] = (byte)((bytes_player_name.Length & 0xff000000) >> 24);
    Array.Copy(bytes_player_name ,0, arr, 12, bytes_player_name.Length);

arr[37] = (byte)(player.owner & 0x000000ff);
   arr[38] = (byte)((player.owner & 0x0000ff00) >> 8);
   arr[39] = (byte)((player.owner & 0x00ff0000) >> 16);
   arr[40] = (byte)((player.owner & 0xff000000) >> 24);

arr[41] = (byte)(player.controllerType & 0x000000ff);
   arr[42] = (byte)((player.controllerType & 0x0000ff00) >> 8);
   arr[43] = (byte)((player.controllerType & 0x00ff0000) >> 16);
   arr[44] = (byte)((player.controllerType & 0xff000000) >> 24);

arr[45] = (byte)(player.totalScore & 0x000000ff);
   arr[46] = (byte)((player.totalScore & 0x0000ff00) >> 8);
   arr[47] = (byte)((player.totalScore & 0x00ff0000) >> 16);
   arr[48] = (byte)((player.totalScore & 0xff000000) >> 24);

arr[49] = (byte)(player.placeInLastGame & 0x000000ff);
   arr[50] = (byte)((player.placeInLastGame & 0x0000ff00) >> 8);
   arr[51] = (byte)((player.placeInLastGame & 0x00ff0000) >> 16);
   arr[52] = (byte)((player.placeInLastGame & 0xff000000) >> 24);



                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static AddPlayerToGame DeserializeLittleEndian(byte[] arr) {
            var result = new AddPlayerToGame();
            Assert.AreEqual(arr.Length, 53);
            unsafe {
result.player = new Player();
result.player.id = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.player.score = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));

int len_result_player_name;
len_result_player_name = (arr[8] | (arr[9] << 8) | (arr[10] << 16) | (arr[11] << 24));
result.player.name = Encoding.UTF8.GetString(arr, 12, len_result_player_name);

result.player.owner = (arr[37] | (arr[38] << 8) | (arr[39] << 16) | (arr[40] << 24));

result.player.controllerType = (arr[41] | (arr[42] << 8) | (arr[43] << 16) | (arr[44] << 24));

result.player.totalScore = (arr[45] | (arr[46] << 8) | (arr[47] << 16) | (arr[48] << 24));

result.player.placeInLastGame = (arr[49] | (arr[50] << 8) | (arr[51] << 16) | (arr[52] << 24));


             
                return result;
            }

        }
        
        public static AddPlayerToGame Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'player':{player}}}";
        }
        
        public override string ToString() {
            return "AddPlayerToGame " + AsJson();
        }
    }
}