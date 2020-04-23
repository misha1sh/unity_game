
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
    public partial class DrawTargetedTracerCommand : ICommand  {

        public DrawTargetedTracerCommand(){}
        
        public DrawTargetedTracerCommand(int player,int target,HPChange HpChange) {
            this.player = player;
this.target = target;
this.HpChange = HpChange;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[16];
arr[0] = (byte)(player & 0x000000ff);
   arr[1] = (byte)((player & 0x0000ff00) >> 8);
   arr[2] = (byte)((player & 0x00ff0000) >> 16);
   arr[3] = (byte)((player & 0xff000000) >> 24);

arr[4] = (byte)(target & 0x000000ff);
   arr[5] = (byte)((target & 0x0000ff00) >> 8);
   arr[6] = (byte)((target & 0x00ff0000) >> 16);
   arr[7] = (byte)((target & 0xff000000) >> 24);

    float f_HpChange_delta = HpChange.delta;
    int i_HpChange_delta = *((int*)&f_HpChange_delta);
arr[8] = (byte)(i_HpChange_delta & 0x000000ff);
   arr[9] = (byte)((i_HpChange_delta & 0x0000ff00) >> 8);
   arr[10] = (byte)((i_HpChange_delta & 0x00ff0000) >> 16);
   arr[11] = (byte)((i_HpChange_delta & 0xff000000) >> 24);

arr[12] = (byte)(HpChange.source & 0x000000ff);
   arr[13] = (byte)((HpChange.source & 0x0000ff00) >> 8);
   arr[14] = (byte)((HpChange.source & 0x00ff0000) >> 16);
   arr[15] = (byte)((HpChange.source & 0xff000000) >> 24);



                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static DrawTargetedTracerCommand DeserializeLittleEndian(byte[] arr) {
            var result = new DrawTargetedTracerCommand();
            Assert.AreEqual(arr.Length, 16);
            unsafe {
result.player = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.target = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));

result.HpChange = new HPChange();
int i_result_HpChange_delta;
i_result_HpChange_delta = (arr[8] | (arr[9] << 8) | (arr[10] << 16) | (arr[11] << 24));
float f_result_HpChange_delta = *((float*)&i_result_HpChange_delta);
result.HpChange.delta = f_result_HpChange_delta;

result.HpChange.source = (arr[12] | (arr[13] << 8) | (arr[14] << 16) | (arr[15] << 24));


             
                return result;
            }

        }
        
        public static DrawTargetedTracerCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'player':{player},'target':{target},'HpChange':{HpChange}}}";
        }
        
        public override string ToString() {
            return "DrawTargetedTracerCommand " + AsJson();
        }
    }
}