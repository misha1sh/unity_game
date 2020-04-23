
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
    public partial class DrawPositionTracerCommand : ICommand  {

        public DrawPositionTracerCommand(){}
        
        public DrawPositionTracerCommand(int player,Vector3 target) {
            this.player = player;
this.target = target;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[16];
arr[0] = (byte)(player & 0x000000ff);
   arr[1] = (byte)((player & 0x0000ff00) >> 8);
   arr[2] = (byte)((player & 0x00ff0000) >> 16);
   arr[3] = (byte)((player & 0xff000000) >> 24);

    float f_target_x = target.x;
    int i_target_x = *((int*)&f_target_x);
arr[4] = (byte)(i_target_x & 0x000000ff);
   arr[5] = (byte)((i_target_x & 0x0000ff00) >> 8);
   arr[6] = (byte)((i_target_x & 0x00ff0000) >> 16);
   arr[7] = (byte)((i_target_x & 0xff000000) >> 24);

    float f_target_y = target.y;
    int i_target_y = *((int*)&f_target_y);
arr[8] = (byte)(i_target_y & 0x000000ff);
   arr[9] = (byte)((i_target_y & 0x0000ff00) >> 8);
   arr[10] = (byte)((i_target_y & 0x00ff0000) >> 16);
   arr[11] = (byte)((i_target_y & 0xff000000) >> 24);

    float f_target_z = target.z;
    int i_target_z = *((int*)&f_target_z);
arr[12] = (byte)(i_target_z & 0x000000ff);
   arr[13] = (byte)((i_target_z & 0x0000ff00) >> 8);
   arr[14] = (byte)((i_target_z & 0x00ff0000) >> 16);
   arr[15] = (byte)((i_target_z & 0xff000000) >> 24);



                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static DrawPositionTracerCommand DeserializeLittleEndian(byte[] arr) {
            var result = new DrawPositionTracerCommand();
            Assert.AreEqual(arr.Length, 16);
            unsafe {
result.player = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.target = new Vector3();
int i_result_target_x;
i_result_target_x = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));
float f_result_target_x = *((float*)&i_result_target_x);
result.target.x = f_result_target_x;

int i_result_target_y;
i_result_target_y = (arr[8] | (arr[9] << 8) | (arr[10] << 16) | (arr[11] << 24));
float f_result_target_y = *((float*)&i_result_target_y);
result.target.y = f_result_target_y;

int i_result_target_z;
i_result_target_z = (arr[12] | (arr[13] << 8) | (arr[14] << 16) | (arr[15] << 24));
float f_result_target_z = *((float*)&i_result_target_z);
result.target.z = f_result_target_z;


             
                return result;
            }

        }
        
        public static DrawPositionTracerCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'player':{player},'target':{target}}}";
        }
        
        public override string ToString() {
            return "DrawPositionTracerCommand " + AsJson();
        }
    }
}