
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

namespace Character.Guns {
    public partial class BombGun : ICommand  {

        public BombGun(){}
        
        public BombGun(float _fullReloadTime,float _reloadTime,int _bulletsInMagazine,int _bulletsCount,int _magazinesCount,Vector3 position,int id,int _state) {
            this._fullReloadTime = _fullReloadTime;
this._reloadTime = _reloadTime;
this._bulletsInMagazine = _bulletsInMagazine;
this._bulletsCount = _bulletsCount;
this._magazinesCount = _magazinesCount;
this.position = position;
this.id = id;
this._state = _state;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[40];
    float f__fullReloadTime = _fullReloadTime;
    int i__fullReloadTime = *((int*)&f__fullReloadTime);
arr[0] = (byte)(i__fullReloadTime & 0x000000ff);
   arr[1] = (byte)((i__fullReloadTime & 0x0000ff00) >> 8);
   arr[2] = (byte)((i__fullReloadTime & 0x00ff0000) >> 16);
   arr[3] = (byte)((i__fullReloadTime & 0xff000000) >> 24);

    float f__reloadTime = _reloadTime;
    int i__reloadTime = *((int*)&f__reloadTime);
arr[4] = (byte)(i__reloadTime & 0x000000ff);
   arr[5] = (byte)((i__reloadTime & 0x0000ff00) >> 8);
   arr[6] = (byte)((i__reloadTime & 0x00ff0000) >> 16);
   arr[7] = (byte)((i__reloadTime & 0xff000000) >> 24);

arr[8] = (byte)(_bulletsInMagazine & 0x000000ff);
   arr[9] = (byte)((_bulletsInMagazine & 0x0000ff00) >> 8);
   arr[10] = (byte)((_bulletsInMagazine & 0x00ff0000) >> 16);
   arr[11] = (byte)((_bulletsInMagazine & 0xff000000) >> 24);

arr[12] = (byte)(_bulletsCount & 0x000000ff);
   arr[13] = (byte)((_bulletsCount & 0x0000ff00) >> 8);
   arr[14] = (byte)((_bulletsCount & 0x00ff0000) >> 16);
   arr[15] = (byte)((_bulletsCount & 0xff000000) >> 24);

arr[16] = (byte)(_magazinesCount & 0x000000ff);
   arr[17] = (byte)((_magazinesCount & 0x0000ff00) >> 8);
   arr[18] = (byte)((_magazinesCount & 0x00ff0000) >> 16);
   arr[19] = (byte)((_magazinesCount & 0xff000000) >> 24);

    float f_position_x = position.x;
    int i_position_x = *((int*)&f_position_x);
arr[20] = (byte)(i_position_x & 0x000000ff);
   arr[21] = (byte)((i_position_x & 0x0000ff00) >> 8);
   arr[22] = (byte)((i_position_x & 0x00ff0000) >> 16);
   arr[23] = (byte)((i_position_x & 0xff000000) >> 24);

    float f_position_y = position.y;
    int i_position_y = *((int*)&f_position_y);
arr[24] = (byte)(i_position_y & 0x000000ff);
   arr[25] = (byte)((i_position_y & 0x0000ff00) >> 8);
   arr[26] = (byte)((i_position_y & 0x00ff0000) >> 16);
   arr[27] = (byte)((i_position_y & 0xff000000) >> 24);

    float f_position_z = position.z;
    int i_position_z = *((int*)&f_position_z);
arr[28] = (byte)(i_position_z & 0x000000ff);
   arr[29] = (byte)((i_position_z & 0x0000ff00) >> 8);
   arr[30] = (byte)((i_position_z & 0x00ff0000) >> 16);
   arr[31] = (byte)((i_position_z & 0xff000000) >> 24);


arr[32] = (byte)(id & 0x000000ff);
   arr[33] = (byte)((id & 0x0000ff00) >> 8);
   arr[34] = (byte)((id & 0x00ff0000) >> 16);
   arr[35] = (byte)((id & 0xff000000) >> 24);

arr[36] = (byte)(_state & 0x000000ff);
   arr[37] = (byte)((_state & 0x0000ff00) >> 8);
   arr[38] = (byte)((_state & 0x00ff0000) >> 16);
   arr[39] = (byte)((_state & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static BombGun DeserializeLittleEndian(byte[] arr) {
            var result = new BombGun();
            Assert.AreEqual(arr.Length, 40);
            unsafe {
int i_result__fullReloadTime;
i_result__fullReloadTime = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));
float f_result__fullReloadTime = *((float*)&i_result__fullReloadTime);
result._fullReloadTime = f_result__fullReloadTime;

int i_result__reloadTime;
i_result__reloadTime = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));
float f_result__reloadTime = *((float*)&i_result__reloadTime);
result._reloadTime = f_result__reloadTime;

result._bulletsInMagazine = (arr[8] | (arr[9] << 8) | (arr[10] << 16) | (arr[11] << 24));

result._bulletsCount = (arr[12] | (arr[13] << 8) | (arr[14] << 16) | (arr[15] << 24));

result._magazinesCount = (arr[16] | (arr[17] << 8) | (arr[18] << 16) | (arr[19] << 24));

result.position = new Vector3();
int i_result_position_x;
i_result_position_x = (arr[20] | (arr[21] << 8) | (arr[22] << 16) | (arr[23] << 24));
float f_result_position_x = *((float*)&i_result_position_x);
result.position.x = f_result_position_x;

int i_result_position_y;
i_result_position_y = (arr[24] | (arr[25] << 8) | (arr[26] << 16) | (arr[27] << 24));
float f_result_position_y = *((float*)&i_result_position_y);
result.position.y = f_result_position_y;

int i_result_position_z;
i_result_position_z = (arr[28] | (arr[29] << 8) | (arr[30] << 16) | (arr[31] << 24));
float f_result_position_z = *((float*)&i_result_position_z);
result.position.z = f_result_position_z;


result.id = (arr[32] | (arr[33] << 8) | (arr[34] << 16) | (arr[35] << 24));

result._state = (arr[36] | (arr[37] << 8) | (arr[38] << 16) | (arr[39] << 24));

             
                return result;
            }

        }
        
        public static BombGun Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'_fullReloadTime':{_fullReloadTime},'_reloadTime':{_reloadTime},'_bulletsInMagazine':{_bulletsInMagazine},'_bulletsCount':{_bulletsCount},'_magazinesCount':{_magazinesCount},'position':{position},'id':{id},'_state':{_state}}}";
        }
        
        public override string ToString() {
            return "BombGun " + AsJson();
        }
    }
}