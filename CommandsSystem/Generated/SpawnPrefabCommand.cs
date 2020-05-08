
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
    public partial class SpawnPrefabCommand : ICommand  {

        public SpawnPrefabCommand(){}
        
        public SpawnPrefabCommand(string prefabName,Vector3 position,Quaternion rotation,int id,int owner,int creator) {
            this.prefabName = prefabName;
this.position = position;
this.rotation = rotation;
this.id = id;
this.owner = owner;
this.creator = creator;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[69];
var bytes_prefabName= Encoding.UTF8.GetBytes(prefabName);
    Assert.IsTrue(bytes_prefabName.Length <= 25);

arr[0] = (byte)(bytes_prefabName.Length & 0x000000ff);
   arr[1] = (byte)((bytes_prefabName.Length & 0x0000ff00) >> 8);
   arr[2] = (byte)((bytes_prefabName.Length & 0x00ff0000) >> 16);
   arr[3] = (byte)((bytes_prefabName.Length & 0xff000000) >> 24);
    Array.Copy(bytes_prefabName ,0, arr, 4, bytes_prefabName.Length);

    float f_position_x = position.x;
    int i_position_x = *((int*)&f_position_x);
arr[29] = (byte)(i_position_x & 0x000000ff);
   arr[30] = (byte)((i_position_x & 0x0000ff00) >> 8);
   arr[31] = (byte)((i_position_x & 0x00ff0000) >> 16);
   arr[32] = (byte)((i_position_x & 0xff000000) >> 24);

    float f_position_y = position.y;
    int i_position_y = *((int*)&f_position_y);
arr[33] = (byte)(i_position_y & 0x000000ff);
   arr[34] = (byte)((i_position_y & 0x0000ff00) >> 8);
   arr[35] = (byte)((i_position_y & 0x00ff0000) >> 16);
   arr[36] = (byte)((i_position_y & 0xff000000) >> 24);

    float f_position_z = position.z;
    int i_position_z = *((int*)&f_position_z);
arr[37] = (byte)(i_position_z & 0x000000ff);
   arr[38] = (byte)((i_position_z & 0x0000ff00) >> 8);
   arr[39] = (byte)((i_position_z & 0x00ff0000) >> 16);
   arr[40] = (byte)((i_position_z & 0xff000000) >> 24);


    float f_rotation_x = rotation.x;
    int i_rotation_x = *((int*)&f_rotation_x);
arr[41] = (byte)(i_rotation_x & 0x000000ff);
   arr[42] = (byte)((i_rotation_x & 0x0000ff00) >> 8);
   arr[43] = (byte)((i_rotation_x & 0x00ff0000) >> 16);
   arr[44] = (byte)((i_rotation_x & 0xff000000) >> 24);

    float f_rotation_y = rotation.y;
    int i_rotation_y = *((int*)&f_rotation_y);
arr[45] = (byte)(i_rotation_y & 0x000000ff);
   arr[46] = (byte)((i_rotation_y & 0x0000ff00) >> 8);
   arr[47] = (byte)((i_rotation_y & 0x00ff0000) >> 16);
   arr[48] = (byte)((i_rotation_y & 0xff000000) >> 24);

    float f_rotation_z = rotation.z;
    int i_rotation_z = *((int*)&f_rotation_z);
arr[49] = (byte)(i_rotation_z & 0x000000ff);
   arr[50] = (byte)((i_rotation_z & 0x0000ff00) >> 8);
   arr[51] = (byte)((i_rotation_z & 0x00ff0000) >> 16);
   arr[52] = (byte)((i_rotation_z & 0xff000000) >> 24);

    float f_rotation_w = rotation.w;
    int i_rotation_w = *((int*)&f_rotation_w);
arr[53] = (byte)(i_rotation_w & 0x000000ff);
   arr[54] = (byte)((i_rotation_w & 0x0000ff00) >> 8);
   arr[55] = (byte)((i_rotation_w & 0x00ff0000) >> 16);
   arr[56] = (byte)((i_rotation_w & 0xff000000) >> 24);


arr[57] = (byte)(id & 0x000000ff);
   arr[58] = (byte)((id & 0x0000ff00) >> 8);
   arr[59] = (byte)((id & 0x00ff0000) >> 16);
   arr[60] = (byte)((id & 0xff000000) >> 24);

arr[61] = (byte)(owner & 0x000000ff);
   arr[62] = (byte)((owner & 0x0000ff00) >> 8);
   arr[63] = (byte)((owner & 0x00ff0000) >> 16);
   arr[64] = (byte)((owner & 0xff000000) >> 24);

arr[65] = (byte)(creator & 0x000000ff);
   arr[66] = (byte)((creator & 0x0000ff00) >> 8);
   arr[67] = (byte)((creator & 0x00ff0000) >> 16);
   arr[68] = (byte)((creator & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static SpawnPrefabCommand DeserializeLittleEndian(byte[] arr) {
            var result = new SpawnPrefabCommand();
            Assert.AreEqual(arr.Length, 69);
            unsafe {
int len_result_prefabName;
len_result_prefabName = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));
result.prefabName = Encoding.UTF8.GetString(arr, 4, len_result_prefabName);

result.position = new Vector3();
int i_result_position_x;
i_result_position_x = (arr[29] | (arr[30] << 8) | (arr[31] << 16) | (arr[32] << 24));
float f_result_position_x = *((float*)&i_result_position_x);
result.position.x = f_result_position_x;

int i_result_position_y;
i_result_position_y = (arr[33] | (arr[34] << 8) | (arr[35] << 16) | (arr[36] << 24));
float f_result_position_y = *((float*)&i_result_position_y);
result.position.y = f_result_position_y;

int i_result_position_z;
i_result_position_z = (arr[37] | (arr[38] << 8) | (arr[39] << 16) | (arr[40] << 24));
float f_result_position_z = *((float*)&i_result_position_z);
result.position.z = f_result_position_z;


result.rotation = new Quaternion();
int i_result_rotation_x;
i_result_rotation_x = (arr[41] | (arr[42] << 8) | (arr[43] << 16) | (arr[44] << 24));
float f_result_rotation_x = *((float*)&i_result_rotation_x);
result.rotation.x = f_result_rotation_x;

int i_result_rotation_y;
i_result_rotation_y = (arr[45] | (arr[46] << 8) | (arr[47] << 16) | (arr[48] << 24));
float f_result_rotation_y = *((float*)&i_result_rotation_y);
result.rotation.y = f_result_rotation_y;

int i_result_rotation_z;
i_result_rotation_z = (arr[49] | (arr[50] << 8) | (arr[51] << 16) | (arr[52] << 24));
float f_result_rotation_z = *((float*)&i_result_rotation_z);
result.rotation.z = f_result_rotation_z;

int i_result_rotation_w;
i_result_rotation_w = (arr[53] | (arr[54] << 8) | (arr[55] << 16) | (arr[56] << 24));
float f_result_rotation_w = *((float*)&i_result_rotation_w);
result.rotation.w = f_result_rotation_w;


result.id = (arr[57] | (arr[58] << 8) | (arr[59] << 16) | (arr[60] << 24));

result.owner = (arr[61] | (arr[62] << 8) | (arr[63] << 16) | (arr[64] << 24));

result.creator = (arr[65] | (arr[66] << 8) | (arr[67] << 16) | (arr[68] << 24));

             
                return result;
            }

        }
        
        public static SpawnPrefabCommand Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'prefabName':{prefabName},'position':{position},'rotation':{rotation},'id':{id},'owner':{owner},'creator':{creator}}}";
        }
        
        public override string ToString() {
            return "SpawnPrefabCommand " + AsJson();
        }
    }
}