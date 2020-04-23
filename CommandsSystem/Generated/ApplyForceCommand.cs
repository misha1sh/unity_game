
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
    public partial class ApplyForceCommand : ICommand  {

        public ApplyForceCommand(){}
        
        public ApplyForceCommand(int objectId,Vector3 force) {
            this.objectId = objectId;
this.force = force;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[16];
arr[0] = (byte)(objectId & 0x000000ff);
   arr[1] = (byte)((objectId & 0x0000ff00) >> 8);
   arr[2] = (byte)((objectId & 0x00ff0000) >> 16);
   arr[3] = (byte)((objectId & 0xff000000) >> 24);

    float f_force_x = force.x;
    int i_force_x = *((int*)&f_force_x);
arr[4] = (byte)(i_force_x & 0x000000ff);
   arr[5] = (byte)((i_force_x & 0x0000ff00) >> 8);
   arr[6] = (byte)((i_force_x & 0x00ff0000) >> 16);
   arr[7] = (byte)((i_force_x & 0xff000000) >> 24);

    float f_force_y = force.y;
    int i_force_y = *((int*)&f_force_y);
arr[8] = (byte)(i_force_y & 0x000000ff);
   arr[9] = (byte)((i_force_y & 0x0000ff00) >> 8);
   arr[10] = (byte)((i_force_y & 0x00ff0000) >> 16);
   arr[11] = (byte)((i_force_y & 0xff000000) >> 24);

    float f_force_z = force.z;
    int i_force_z = *((int*)&f_force_z);
arr[12] = (byte)(i_force_z & 0x000000ff);
   arr[13] = (byte)((i_force_z & 0x0000ff00) >> 8);
   arr[14] = (byte)((i_force_z & 0x00ff0000) >> 16);
   arr[15] = (byte)((i_force_z & 0xff000000) >> 24);



                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static ApplyForceCommand DeserializeLittleEndian(byte[] arr) {
            var result = new ApplyForceCommand();
            Assert.AreEqual(arr.Length, 16);
            unsafe {
result.objectId = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.force = new Vector3();
int i_result_force_x;
i_result_force_x = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));
float f_result_force_x = *((float*)&i_result_force_x);
result.force.x = f_result_force_x;

int i_result_force_y;
i_result_force_y = (arr[8] | (arr[9] << 8) | (arr[10] << 16) | (arr[11] << 24));
float f_result_force_y = *((float*)&i_result_force_y);
result.force.y = f_result_force_y;

int i_result_force_z;
i_result_force_z = (arr[12] | (arr[13] << 8) | (arr[14] << 16) | (arr[15] << 24));
float f_result_force_z = *((float*)&i_result_force_z);
result.force.z = f_result_force_z;


             
                return result;
            }

        }
        
        public static ApplyForceCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'objectId':{objectId},'force':{force}}}";
        }
        
        public override string ToString() {
            return "ApplyForceCommand " + AsJson();
        }
    }
}