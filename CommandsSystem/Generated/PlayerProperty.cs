
using System;
using System.Text;
using Character;
using Interpolation;
using Interpolation.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using Character.HP;
using CommandsSystem;

namespace CommandsSystem.Commands {
    public partial class PlayerProperty : ICommand  {

        public PlayerProperty(){}
        
        public PlayerProperty(int id,Vector3 position,Quaternion rotation,PlayerAnimationState animationState) {
            this.id = id;
this.position = position;
this.rotation = rotation;
this.animationState = animationState;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[41];
arr[0] = (byte)(id & 0x000000ff);
   arr[1] = (byte)((id & 0x0000ff00) >> 8);
   arr[2] = (byte)((id & 0x00ff0000) >> 16);
   arr[3] = (byte)((id & 0xff000000) >> 24);

    float f_position_x = position.x;
    int i_position_x = *((int*)&f_position_x);
arr[4] = (byte)(i_position_x & 0x000000ff);
   arr[5] = (byte)((i_position_x & 0x0000ff00) >> 8);
   arr[6] = (byte)((i_position_x & 0x00ff0000) >> 16);
   arr[7] = (byte)((i_position_x & 0xff000000) >> 24);

    float f_position_y = position.y;
    int i_position_y = *((int*)&f_position_y);
arr[8] = (byte)(i_position_y & 0x000000ff);
   arr[9] = (byte)((i_position_y & 0x0000ff00) >> 8);
   arr[10] = (byte)((i_position_y & 0x00ff0000) >> 16);
   arr[11] = (byte)((i_position_y & 0xff000000) >> 24);

    float f_position_z = position.z;
    int i_position_z = *((int*)&f_position_z);
arr[12] = (byte)(i_position_z & 0x000000ff);
   arr[13] = (byte)((i_position_z & 0x0000ff00) >> 8);
   arr[14] = (byte)((i_position_z & 0x00ff0000) >> 16);
   arr[15] = (byte)((i_position_z & 0xff000000) >> 24);


    float f_rotation_x = rotation.x;
    int i_rotation_x = *((int*)&f_rotation_x);
arr[16] = (byte)(i_rotation_x & 0x000000ff);
   arr[17] = (byte)((i_rotation_x & 0x0000ff00) >> 8);
   arr[18] = (byte)((i_rotation_x & 0x00ff0000) >> 16);
   arr[19] = (byte)((i_rotation_x & 0xff000000) >> 24);

    float f_rotation_y = rotation.y;
    int i_rotation_y = *((int*)&f_rotation_y);
arr[20] = (byte)(i_rotation_y & 0x000000ff);
   arr[21] = (byte)((i_rotation_y & 0x0000ff00) >> 8);
   arr[22] = (byte)((i_rotation_y & 0x00ff0000) >> 16);
   arr[23] = (byte)((i_rotation_y & 0xff000000) >> 24);

    float f_rotation_z = rotation.z;
    int i_rotation_z = *((int*)&f_rotation_z);
arr[24] = (byte)(i_rotation_z & 0x000000ff);
   arr[25] = (byte)((i_rotation_z & 0x0000ff00) >> 8);
   arr[26] = (byte)((i_rotation_z & 0x00ff0000) >> 16);
   arr[27] = (byte)((i_rotation_z & 0xff000000) >> 24);

    float f_rotation_w = rotation.w;
    int i_rotation_w = *((int*)&f_rotation_w);
arr[28] = (byte)(i_rotation_w & 0x000000ff);
   arr[29] = (byte)((i_rotation_w & 0x0000ff00) >> 8);
   arr[30] = (byte)((i_rotation_w & 0x00ff0000) >> 16);
   arr[31] = (byte)((i_rotation_w & 0xff000000) >> 24);


arr[32] = (animationState.idle?(byte)1: (byte)0);

    float f_animationState_speed = animationState.speed;
    int i_animationState_speed = *((int*)&f_animationState_speed);
arr[33] = (byte)(i_animationState_speed & 0x000000ff);
   arr[34] = (byte)((i_animationState_speed & 0x0000ff00) >> 8);
   arr[35] = (byte)((i_animationState_speed & 0x00ff0000) >> 16);
   arr[36] = (byte)((i_animationState_speed & 0xff000000) >> 24);

    float f_animationState_rotationSpeed = animationState.rotationSpeed;
    int i_animationState_rotationSpeed = *((int*)&f_animationState_rotationSpeed);
arr[37] = (byte)(i_animationState_rotationSpeed & 0x000000ff);
   arr[38] = (byte)((i_animationState_rotationSpeed & 0x0000ff00) >> 8);
   arr[39] = (byte)((i_animationState_rotationSpeed & 0x00ff0000) >> 16);
   arr[40] = (byte)((i_animationState_rotationSpeed & 0xff000000) >> 24);



                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static PlayerProperty DeserializeLittleEndian(byte[] arr) {
            var result = new PlayerProperty();
            unsafe {
result.id = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

int i_result_position_x;
i_result_position_x = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));
float f_result_position_x = *((float*)&i_result_position_x);
result.position.x = f_result_position_x;

int i_result_position_y;
i_result_position_y = (arr[8] | (arr[9] << 8) | (arr[10] << 16) | (arr[11] << 24));
float f_result_position_y = *((float*)&i_result_position_y);
result.position.y = f_result_position_y;

int i_result_position_z;
i_result_position_z = (arr[12] | (arr[13] << 8) | (arr[14] << 16) | (arr[15] << 24));
float f_result_position_z = *((float*)&i_result_position_z);
result.position.z = f_result_position_z;


int i_result_rotation_x;
i_result_rotation_x = (arr[16] | (arr[17] << 8) | (arr[18] << 16) | (arr[19] << 24));
float f_result_rotation_x = *((float*)&i_result_rotation_x);
result.rotation.x = f_result_rotation_x;

int i_result_rotation_y;
i_result_rotation_y = (arr[20] | (arr[21] << 8) | (arr[22] << 16) | (arr[23] << 24));
float f_result_rotation_y = *((float*)&i_result_rotation_y);
result.rotation.y = f_result_rotation_y;

int i_result_rotation_z;
i_result_rotation_z = (arr[24] | (arr[25] << 8) | (arr[26] << 16) | (arr[27] << 24));
float f_result_rotation_z = *((float*)&i_result_rotation_z);
result.rotation.z = f_result_rotation_z;

int i_result_rotation_w;
i_result_rotation_w = (arr[28] | (arr[29] << 8) | (arr[30] << 16) | (arr[31] << 24));
float f_result_rotation_w = *((float*)&i_result_rotation_w);
result.rotation.w = f_result_rotation_w;


result.animationState.idle = (arr[32] == 1);

int i_result_animationState_speed;
i_result_animationState_speed = (arr[33] | (arr[34] << 8) | (arr[35] << 16) | (arr[36] << 24));
float f_result_animationState_speed = *((float*)&i_result_animationState_speed);
result.animationState.speed = f_result_animationState_speed;

int i_result_animationState_rotationSpeed;
i_result_animationState_rotationSpeed = (arr[37] | (arr[38] << 8) | (arr[39] << 16) | (arr[40] << 24));
float f_result_animationState_rotationSpeed = *((float*)&i_result_animationState_rotationSpeed);
result.animationState.rotationSpeed = f_result_animationState_rotationSpeed;


             
                return result;
            }

        }
        
        public static PlayerProperty Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'id':{id},'position':{position},'rotation':{rotation},'animationState':{animationState}}}";
        }
        
        public override string ToString() {
            return "PlayerProperty " + AsJson();
        }
    }
}