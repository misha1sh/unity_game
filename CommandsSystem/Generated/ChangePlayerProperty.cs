
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
    public partial class ChangePlayerProperty : ICommand  {

        public ChangePlayerProperty(){}
        
        public ChangePlayerProperty(PlayerProperty property,float deltaTime) {
            this.property = property;
this.deltaTime = deltaTime;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[45];
arr[0] = (byte)(property.id & 0x000000ff);
   arr[1] = (byte)((property.id & 0x0000ff00) >> 8);
   arr[2] = (byte)((property.id & 0x00ff0000) >> 16);
   arr[3] = (byte)((property.id & 0xff000000) >> 24);

    float f_property_position_x = property.position.x;
    int i_property_position_x = *((int*)&f_property_position_x);
arr[4] = (byte)(i_property_position_x & 0x000000ff);
   arr[5] = (byte)((i_property_position_x & 0x0000ff00) >> 8);
   arr[6] = (byte)((i_property_position_x & 0x00ff0000) >> 16);
   arr[7] = (byte)((i_property_position_x & 0xff000000) >> 24);

    float f_property_position_y = property.position.y;
    int i_property_position_y = *((int*)&f_property_position_y);
arr[8] = (byte)(i_property_position_y & 0x000000ff);
   arr[9] = (byte)((i_property_position_y & 0x0000ff00) >> 8);
   arr[10] = (byte)((i_property_position_y & 0x00ff0000) >> 16);
   arr[11] = (byte)((i_property_position_y & 0xff000000) >> 24);

    float f_property_position_z = property.position.z;
    int i_property_position_z = *((int*)&f_property_position_z);
arr[12] = (byte)(i_property_position_z & 0x000000ff);
   arr[13] = (byte)((i_property_position_z & 0x0000ff00) >> 8);
   arr[14] = (byte)((i_property_position_z & 0x00ff0000) >> 16);
   arr[15] = (byte)((i_property_position_z & 0xff000000) >> 24);


    float f_property_rotation_x = property.rotation.x;
    int i_property_rotation_x = *((int*)&f_property_rotation_x);
arr[16] = (byte)(i_property_rotation_x & 0x000000ff);
   arr[17] = (byte)((i_property_rotation_x & 0x0000ff00) >> 8);
   arr[18] = (byte)((i_property_rotation_x & 0x00ff0000) >> 16);
   arr[19] = (byte)((i_property_rotation_x & 0xff000000) >> 24);

    float f_property_rotation_y = property.rotation.y;
    int i_property_rotation_y = *((int*)&f_property_rotation_y);
arr[20] = (byte)(i_property_rotation_y & 0x000000ff);
   arr[21] = (byte)((i_property_rotation_y & 0x0000ff00) >> 8);
   arr[22] = (byte)((i_property_rotation_y & 0x00ff0000) >> 16);
   arr[23] = (byte)((i_property_rotation_y & 0xff000000) >> 24);

    float f_property_rotation_z = property.rotation.z;
    int i_property_rotation_z = *((int*)&f_property_rotation_z);
arr[24] = (byte)(i_property_rotation_z & 0x000000ff);
   arr[25] = (byte)((i_property_rotation_z & 0x0000ff00) >> 8);
   arr[26] = (byte)((i_property_rotation_z & 0x00ff0000) >> 16);
   arr[27] = (byte)((i_property_rotation_z & 0xff000000) >> 24);

    float f_property_rotation_w = property.rotation.w;
    int i_property_rotation_w = *((int*)&f_property_rotation_w);
arr[28] = (byte)(i_property_rotation_w & 0x000000ff);
   arr[29] = (byte)((i_property_rotation_w & 0x0000ff00) >> 8);
   arr[30] = (byte)((i_property_rotation_w & 0x00ff0000) >> 16);
   arr[31] = (byte)((i_property_rotation_w & 0xff000000) >> 24);


arr[32] = (property.animationState.idle?(byte)1: (byte)0);

    float f_property_animationState_speed = property.animationState.speed;
    int i_property_animationState_speed = *((int*)&f_property_animationState_speed);
arr[33] = (byte)(i_property_animationState_speed & 0x000000ff);
   arr[34] = (byte)((i_property_animationState_speed & 0x0000ff00) >> 8);
   arr[35] = (byte)((i_property_animationState_speed & 0x00ff0000) >> 16);
   arr[36] = (byte)((i_property_animationState_speed & 0xff000000) >> 24);

    float f_property_animationState_rotationSpeed = property.animationState.rotationSpeed;
    int i_property_animationState_rotationSpeed = *((int*)&f_property_animationState_rotationSpeed);
arr[37] = (byte)(i_property_animationState_rotationSpeed & 0x000000ff);
   arr[38] = (byte)((i_property_animationState_rotationSpeed & 0x0000ff00) >> 8);
   arr[39] = (byte)((i_property_animationState_rotationSpeed & 0x00ff0000) >> 16);
   arr[40] = (byte)((i_property_animationState_rotationSpeed & 0xff000000) >> 24);



    float f_deltaTime = deltaTime;
    int i_deltaTime = *((int*)&f_deltaTime);
arr[41] = (byte)(i_deltaTime & 0x000000ff);
   arr[42] = (byte)((i_deltaTime & 0x0000ff00) >> 8);
   arr[43] = (byte)((i_deltaTime & 0x00ff0000) >> 16);
   arr[44] = (byte)((i_deltaTime & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static ChangePlayerProperty DeserializeLittleEndian(byte[] arr) {
            var result = new ChangePlayerProperty();
            Assert.AreEqual(arr.Length, 45);
            unsafe {
result.property = new PlayerProperty();
result.property.id = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

result.property.position = new Vector3();
int i_result_property_position_x;
i_result_property_position_x = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));
float f_result_property_position_x = *((float*)&i_result_property_position_x);
result.property.position.x = f_result_property_position_x;

int i_result_property_position_y;
i_result_property_position_y = (arr[8] | (arr[9] << 8) | (arr[10] << 16) | (arr[11] << 24));
float f_result_property_position_y = *((float*)&i_result_property_position_y);
result.property.position.y = f_result_property_position_y;

int i_result_property_position_z;
i_result_property_position_z = (arr[12] | (arr[13] << 8) | (arr[14] << 16) | (arr[15] << 24));
float f_result_property_position_z = *((float*)&i_result_property_position_z);
result.property.position.z = f_result_property_position_z;


result.property.rotation = new Quaternion();
int i_result_property_rotation_x;
i_result_property_rotation_x = (arr[16] | (arr[17] << 8) | (arr[18] << 16) | (arr[19] << 24));
float f_result_property_rotation_x = *((float*)&i_result_property_rotation_x);
result.property.rotation.x = f_result_property_rotation_x;

int i_result_property_rotation_y;
i_result_property_rotation_y = (arr[20] | (arr[21] << 8) | (arr[22] << 16) | (arr[23] << 24));
float f_result_property_rotation_y = *((float*)&i_result_property_rotation_y);
result.property.rotation.y = f_result_property_rotation_y;

int i_result_property_rotation_z;
i_result_property_rotation_z = (arr[24] | (arr[25] << 8) | (arr[26] << 16) | (arr[27] << 24));
float f_result_property_rotation_z = *((float*)&i_result_property_rotation_z);
result.property.rotation.z = f_result_property_rotation_z;

int i_result_property_rotation_w;
i_result_property_rotation_w = (arr[28] | (arr[29] << 8) | (arr[30] << 16) | (arr[31] << 24));
float f_result_property_rotation_w = *((float*)&i_result_property_rotation_w);
result.property.rotation.w = f_result_property_rotation_w;


result.property.animationState = new PlayerAnimationState();
result.property.animationState.idle = (arr[32] == 1);

int i_result_property_animationState_speed;
i_result_property_animationState_speed = (arr[33] | (arr[34] << 8) | (arr[35] << 16) | (arr[36] << 24));
float f_result_property_animationState_speed = *((float*)&i_result_property_animationState_speed);
result.property.animationState.speed = f_result_property_animationState_speed;

int i_result_property_animationState_rotationSpeed;
i_result_property_animationState_rotationSpeed = (arr[37] | (arr[38] << 8) | (arr[39] << 16) | (arr[40] << 24));
float f_result_property_animationState_rotationSpeed = *((float*)&i_result_property_animationState_rotationSpeed);
result.property.animationState.rotationSpeed = f_result_property_animationState_rotationSpeed;



int i_result_deltaTime;
i_result_deltaTime = (arr[41] | (arr[42] << 8) | (arr[43] << 16) | (arr[44] << 24));
float f_result_deltaTime = *((float*)&i_result_deltaTime);
result.deltaTime = f_result_deltaTime;

             
                return result;
            }

        }
        
        public static ChangePlayerProperty Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'property':{property},'deltaTime':{deltaTime}}}";
        }
        
        public override string ToString() {
            return "ChangePlayerProperty " + AsJson();
        }
    }
}