
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
    public partial class ChangeHPCommand : ICommand  {

        public ChangeHPCommand(){}
        
        public ChangeHPCommand(int id,HPChange HpChange) {
            this.id = id;
this.HpChange = HpChange;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[12];
arr[0] = (byte)(id & 0x000000ff);
   arr[1] = (byte)((id & 0x0000ff00) >> 8);
   arr[2] = (byte)((id & 0x00ff0000) >> 16);
   arr[3] = (byte)((id & 0xff000000) >> 24);

    float f_HpChange_delta = HpChange.delta;
    int i_HpChange_delta = *((int*)&f_HpChange_delta);
arr[4] = (byte)(i_HpChange_delta & 0x000000ff);
   arr[5] = (byte)((i_HpChange_delta & 0x0000ff00) >> 8);
   arr[6] = (byte)((i_HpChange_delta & 0x00ff0000) >> 16);
   arr[7] = (byte)((i_HpChange_delta & 0xff000000) >> 24);

arr[8] = (byte)(HpChange.source & 0x000000ff);
   arr[9] = (byte)((HpChange.source & 0x0000ff00) >> 8);
   arr[10] = (byte)((HpChange.source & 0x00ff0000) >> 16);
   arr[11] = (byte)((HpChange.source & 0xff000000) >> 24);



                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static ChangeHPCommand DeserializeLittleEndian(byte[] arr) {
            var result = new ChangeHPCommand();
            Assert.AreEqual(arr.Length, 12);
            unsafe {
result.id = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.HpChange = new HPChange();
int i_result_HpChange_delta;
i_result_HpChange_delta = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));
float f_result_HpChange_delta = *((float*)&i_result_HpChange_delta);
result.HpChange.delta = f_result_HpChange_delta;

result.HpChange.source = (arr[8] | (arr[9] << 8) | (arr[10] << 16) | (arr[11] << 24));


             
                return result;
            }

        }
        
        public static ChangeHPCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'id':{id},'HpChange':{HpChange}}}";
        }
        
        public override string ToString() {
            return "ChangeHPCommand " + AsJson();
        }
    }
}