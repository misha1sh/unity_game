
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
    public partial class SpawnParabolaFlyingCommand : ICommand  {

        public SpawnParabolaFlyingCommand(){}
        
        public SpawnParabolaFlyingCommand(SpawnPrefabCommand command,Vector3 medium,Vector3 target,float totalTime) {
            this.command = command;
this.medium = medium;
this.target = target;
this.totalTime = totalTime;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[97];
var bytes_command_prefabName= Encoding.UTF8.GetBytes(command.prefabName);
    Assert.IsTrue(bytes_command_prefabName.Length <= 25);

arr[0] = (byte)(bytes_command_prefabName.Length & 0x000000ff);
   arr[1] = (byte)((bytes_command_prefabName.Length & 0x0000ff00) >> 8);
   arr[2] = (byte)((bytes_command_prefabName.Length & 0x00ff0000) >> 16);
   arr[3] = (byte)((bytes_command_prefabName.Length & 0xff000000) >> 24);
    Array.Copy(bytes_command_prefabName ,0, arr, 4, bytes_command_prefabName.Length);

    float f_command_position_x = command.position.x;
    int i_command_position_x = *((int*)&f_command_position_x);
arr[29] = (byte)(i_command_position_x & 0x000000ff);
   arr[30] = (byte)((i_command_position_x & 0x0000ff00) >> 8);
   arr[31] = (byte)((i_command_position_x & 0x00ff0000) >> 16);
   arr[32] = (byte)((i_command_position_x & 0xff000000) >> 24);

    float f_command_position_y = command.position.y;
    int i_command_position_y = *((int*)&f_command_position_y);
arr[33] = (byte)(i_command_position_y & 0x000000ff);
   arr[34] = (byte)((i_command_position_y & 0x0000ff00) >> 8);
   arr[35] = (byte)((i_command_position_y & 0x00ff0000) >> 16);
   arr[36] = (byte)((i_command_position_y & 0xff000000) >> 24);

    float f_command_position_z = command.position.z;
    int i_command_position_z = *((int*)&f_command_position_z);
arr[37] = (byte)(i_command_position_z & 0x000000ff);
   arr[38] = (byte)((i_command_position_z & 0x0000ff00) >> 8);
   arr[39] = (byte)((i_command_position_z & 0x00ff0000) >> 16);
   arr[40] = (byte)((i_command_position_z & 0xff000000) >> 24);


    float f_command_rotation_x = command.rotation.x;
    int i_command_rotation_x = *((int*)&f_command_rotation_x);
arr[41] = (byte)(i_command_rotation_x & 0x000000ff);
   arr[42] = (byte)((i_command_rotation_x & 0x0000ff00) >> 8);
   arr[43] = (byte)((i_command_rotation_x & 0x00ff0000) >> 16);
   arr[44] = (byte)((i_command_rotation_x & 0xff000000) >> 24);

    float f_command_rotation_y = command.rotation.y;
    int i_command_rotation_y = *((int*)&f_command_rotation_y);
arr[45] = (byte)(i_command_rotation_y & 0x000000ff);
   arr[46] = (byte)((i_command_rotation_y & 0x0000ff00) >> 8);
   arr[47] = (byte)((i_command_rotation_y & 0x00ff0000) >> 16);
   arr[48] = (byte)((i_command_rotation_y & 0xff000000) >> 24);

    float f_command_rotation_z = command.rotation.z;
    int i_command_rotation_z = *((int*)&f_command_rotation_z);
arr[49] = (byte)(i_command_rotation_z & 0x000000ff);
   arr[50] = (byte)((i_command_rotation_z & 0x0000ff00) >> 8);
   arr[51] = (byte)((i_command_rotation_z & 0x00ff0000) >> 16);
   arr[52] = (byte)((i_command_rotation_z & 0xff000000) >> 24);

    float f_command_rotation_w = command.rotation.w;
    int i_command_rotation_w = *((int*)&f_command_rotation_w);
arr[53] = (byte)(i_command_rotation_w & 0x000000ff);
   arr[54] = (byte)((i_command_rotation_w & 0x0000ff00) >> 8);
   arr[55] = (byte)((i_command_rotation_w & 0x00ff0000) >> 16);
   arr[56] = (byte)((i_command_rotation_w & 0xff000000) >> 24);


arr[57] = (byte)(command.id & 0x000000ff);
   arr[58] = (byte)((command.id & 0x0000ff00) >> 8);
   arr[59] = (byte)((command.id & 0x00ff0000) >> 16);
   arr[60] = (byte)((command.id & 0xff000000) >> 24);

arr[61] = (byte)(command.owner & 0x000000ff);
   arr[62] = (byte)((command.owner & 0x0000ff00) >> 8);
   arr[63] = (byte)((command.owner & 0x00ff0000) >> 16);
   arr[64] = (byte)((command.owner & 0xff000000) >> 24);

arr[65] = (byte)(command.creator & 0x000000ff);
   arr[66] = (byte)((command.creator & 0x0000ff00) >> 8);
   arr[67] = (byte)((command.creator & 0x00ff0000) >> 16);
   arr[68] = (byte)((command.creator & 0xff000000) >> 24);


    float f_medium_x = medium.x;
    int i_medium_x = *((int*)&f_medium_x);
arr[69] = (byte)(i_medium_x & 0x000000ff);
   arr[70] = (byte)((i_medium_x & 0x0000ff00) >> 8);
   arr[71] = (byte)((i_medium_x & 0x00ff0000) >> 16);
   arr[72] = (byte)((i_medium_x & 0xff000000) >> 24);

    float f_medium_y = medium.y;
    int i_medium_y = *((int*)&f_medium_y);
arr[73] = (byte)(i_medium_y & 0x000000ff);
   arr[74] = (byte)((i_medium_y & 0x0000ff00) >> 8);
   arr[75] = (byte)((i_medium_y & 0x00ff0000) >> 16);
   arr[76] = (byte)((i_medium_y & 0xff000000) >> 24);

    float f_medium_z = medium.z;
    int i_medium_z = *((int*)&f_medium_z);
arr[77] = (byte)(i_medium_z & 0x000000ff);
   arr[78] = (byte)((i_medium_z & 0x0000ff00) >> 8);
   arr[79] = (byte)((i_medium_z & 0x00ff0000) >> 16);
   arr[80] = (byte)((i_medium_z & 0xff000000) >> 24);


    float f_target_x = target.x;
    int i_target_x = *((int*)&f_target_x);
arr[81] = (byte)(i_target_x & 0x000000ff);
   arr[82] = (byte)((i_target_x & 0x0000ff00) >> 8);
   arr[83] = (byte)((i_target_x & 0x00ff0000) >> 16);
   arr[84] = (byte)((i_target_x & 0xff000000) >> 24);

    float f_target_y = target.y;
    int i_target_y = *((int*)&f_target_y);
arr[85] = (byte)(i_target_y & 0x000000ff);
   arr[86] = (byte)((i_target_y & 0x0000ff00) >> 8);
   arr[87] = (byte)((i_target_y & 0x00ff0000) >> 16);
   arr[88] = (byte)((i_target_y & 0xff000000) >> 24);

    float f_target_z = target.z;
    int i_target_z = *((int*)&f_target_z);
arr[89] = (byte)(i_target_z & 0x000000ff);
   arr[90] = (byte)((i_target_z & 0x0000ff00) >> 8);
   arr[91] = (byte)((i_target_z & 0x00ff0000) >> 16);
   arr[92] = (byte)((i_target_z & 0xff000000) >> 24);


    float f_totalTime = totalTime;
    int i_totalTime = *((int*)&f_totalTime);
arr[93] = (byte)(i_totalTime & 0x000000ff);
   arr[94] = (byte)((i_totalTime & 0x0000ff00) >> 8);
   arr[95] = (byte)((i_totalTime & 0x00ff0000) >> 16);
   arr[96] = (byte)((i_totalTime & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static SpawnParabolaFlyingCommand DeserializeLittleEndian(byte[] arr) {
            var result = new SpawnParabolaFlyingCommand();
            Assert.AreEqual(arr.Length, 97);
            unsafe {
result.command = new SpawnPrefabCommand();
int len_result_command_prefabName;
len_result_command_prefabName = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));
result.command.prefabName = Encoding.UTF8.GetString(arr, 4, len_result_command_prefabName);

result.command.position = new Vector3();
int i_result_command_position_x;
i_result_command_position_x = (arr[29] | (arr[30] << 8) | (arr[31] << 16) | (arr[32] << 24));
float f_result_command_position_x = *((float*)&i_result_command_position_x);
result.command.position.x = f_result_command_position_x;

int i_result_command_position_y;
i_result_command_position_y = (arr[33] | (arr[34] << 8) | (arr[35] << 16) | (arr[36] << 24));
float f_result_command_position_y = *((float*)&i_result_command_position_y);
result.command.position.y = f_result_command_position_y;

int i_result_command_position_z;
i_result_command_position_z = (arr[37] | (arr[38] << 8) | (arr[39] << 16) | (arr[40] << 24));
float f_result_command_position_z = *((float*)&i_result_command_position_z);
result.command.position.z = f_result_command_position_z;


result.command.rotation = new Quaternion();
int i_result_command_rotation_x;
i_result_command_rotation_x = (arr[41] | (arr[42] << 8) | (arr[43] << 16) | (arr[44] << 24));
float f_result_command_rotation_x = *((float*)&i_result_command_rotation_x);
result.command.rotation.x = f_result_command_rotation_x;

int i_result_command_rotation_y;
i_result_command_rotation_y = (arr[45] | (arr[46] << 8) | (arr[47] << 16) | (arr[48] << 24));
float f_result_command_rotation_y = *((float*)&i_result_command_rotation_y);
result.command.rotation.y = f_result_command_rotation_y;

int i_result_command_rotation_z;
i_result_command_rotation_z = (arr[49] | (arr[50] << 8) | (arr[51] << 16) | (arr[52] << 24));
float f_result_command_rotation_z = *((float*)&i_result_command_rotation_z);
result.command.rotation.z = f_result_command_rotation_z;

int i_result_command_rotation_w;
i_result_command_rotation_w = (arr[53] | (arr[54] << 8) | (arr[55] << 16) | (arr[56] << 24));
float f_result_command_rotation_w = *((float*)&i_result_command_rotation_w);
result.command.rotation.w = f_result_command_rotation_w;


result.command.id = (arr[57] | (arr[58] << 8) | (arr[59] << 16) | (arr[60] << 24));

result.command.owner = (arr[61] | (arr[62] << 8) | (arr[63] << 16) | (arr[64] << 24));

result.command.creator = (arr[65] | (arr[66] << 8) | (arr[67] << 16) | (arr[68] << 24));


result.medium = new Vector3();
int i_result_medium_x;
i_result_medium_x = (arr[69] | (arr[70] << 8) | (arr[71] << 16) | (arr[72] << 24));
float f_result_medium_x = *((float*)&i_result_medium_x);
result.medium.x = f_result_medium_x;

int i_result_medium_y;
i_result_medium_y = (arr[73] | (arr[74] << 8) | (arr[75] << 16) | (arr[76] << 24));
float f_result_medium_y = *((float*)&i_result_medium_y);
result.medium.y = f_result_medium_y;

int i_result_medium_z;
i_result_medium_z = (arr[77] | (arr[78] << 8) | (arr[79] << 16) | (arr[80] << 24));
float f_result_medium_z = *((float*)&i_result_medium_z);
result.medium.z = f_result_medium_z;


result.target = new Vector3();
int i_result_target_x;
i_result_target_x = (arr[81] | (arr[82] << 8) | (arr[83] << 16) | (arr[84] << 24));
float f_result_target_x = *((float*)&i_result_target_x);
result.target.x = f_result_target_x;

int i_result_target_y;
i_result_target_y = (arr[85] | (arr[86] << 8) | (arr[87] << 16) | (arr[88] << 24));
float f_result_target_y = *((float*)&i_result_target_y);
result.target.y = f_result_target_y;

int i_result_target_z;
i_result_target_z = (arr[89] | (arr[90] << 8) | (arr[91] << 16) | (arr[92] << 24));
float f_result_target_z = *((float*)&i_result_target_z);
result.target.z = f_result_target_z;


int i_result_totalTime;
i_result_totalTime = (arr[93] | (arr[94] << 8) | (arr[95] << 16) | (arr[96] << 24));
float f_result_totalTime = *((float*)&i_result_totalTime);
result.totalTime = f_result_totalTime;

             
                return result;
            }

        }
        
        public static SpawnParabolaFlyingCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'command':{command},'medium':{medium},'target':{target},'totalTime':{totalTime}}}";
        }
        
        public override string ToString() {
            return "SpawnParabolaFlyingCommand " + AsJson();
        }
    }
}