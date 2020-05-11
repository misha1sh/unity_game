
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
    public partial class CreateChatMessageCommand : ICommand  {

        public CreateChatMessageCommand(){}
        
        public CreateChatMessageCommand(int playerid,string message) {
            this.playerid = playerid;
this.message = message;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[33];
arr[0] = (byte)(playerid & 0x000000ff);
   arr[1] = (byte)((playerid & 0x0000ff00) >> 8);
   arr[2] = (byte)((playerid & 0x00ff0000) >> 16);
   arr[3] = (byte)((playerid & 0xff000000) >> 24);

var bytes_message= Encoding.UTF8.GetBytes(message);
    Assert.IsTrue(bytes_message.Length <= 25);

arr[4] = (byte)(bytes_message.Length & 0x000000ff);
   arr[5] = (byte)((bytes_message.Length & 0x0000ff00) >> 8);
   arr[6] = (byte)((bytes_message.Length & 0x00ff0000) >> 16);
   arr[7] = (byte)((bytes_message.Length & 0xff000000) >> 24);
    Array.Copy(bytes_message ,0, arr, 8, bytes_message.Length);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static CreateChatMessageCommand DeserializeLittleEndian(byte[] arr) {
            var result = new CreateChatMessageCommand();
            Assert.AreEqual(arr.Length, 33);
            unsafe {
result.playerid = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

int len_result_message;
len_result_message = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));
result.message = Encoding.UTF8.GetString(arr, 8, len_result_message);

             
                return result;
            }

        }
        
        public static CreateChatMessageCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'playerid':{playerid},'message':{message}}}";
        }
        
        public override string ToString() {
            return "CreateChatMessageCommand " + AsJson();
        }
    }
}