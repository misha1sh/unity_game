
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
    public partial class ShotGun : ICommand  {

        public ShotGun(){}
        
        public ShotGun(float _fullReloadTime,float _reloadTime,int _bulletsInMagazine,float damage,int shootsCount,float accurancy,int _bulletsCount,int _magazinesCount,Vector3 position,int id,int _state) {
            this._fullReloadTime = _fullReloadTime;
this._reloadTime = _reloadTime;
this._bulletsInMagazine = _bulletsInMagazine;
this.damage = damage;
this.shootsCount = shootsCount;
this.accurancy = accurancy;
this._bulletsCount = _bulletsCount;
this._magazinesCount = _magazinesCount;
this.position = position;
this.id = id;
this._state = _state;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[52];
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

    float f_damage = damage;
    int i_damage = *((int*)&f_damage);
arr[12] = (byte)(i_damage & 0x000000ff);
   arr[13] = (byte)((i_damage & 0x0000ff00) >> 8);
   arr[14] = (byte)((i_damage & 0x00ff0000) >> 16);
   arr[15] = (byte)((i_damage & 0xff000000) >> 24);

arr[16] = (byte)(shootsCount & 0x000000ff);
   arr[17] = (byte)((shootsCount & 0x0000ff00) >> 8);
   arr[18] = (byte)((shootsCount & 0x00ff0000) >> 16);
   arr[19] = (byte)((shootsCount & 0xff000000) >> 24);

    float f_accurancy = accurancy;
    int i_accurancy = *((int*)&f_accurancy);
arr[20] = (byte)(i_accurancy & 0x000000ff);
   arr[21] = (byte)((i_accurancy & 0x0000ff00) >> 8);
   arr[22] = (byte)((i_accurancy & 0x00ff0000) >> 16);
   arr[23] = (byte)((i_accurancy & 0xff000000) >> 24);

arr[24] = (byte)(_bulletsCount & 0x000000ff);
   arr[25] = (byte)((_bulletsCount & 0x0000ff00) >> 8);
   arr[26] = (byte)((_bulletsCount & 0x00ff0000) >> 16);
   arr[27] = (byte)((_bulletsCount & 0xff000000) >> 24);

arr[28] = (byte)(_magazinesCount & 0x000000ff);
   arr[29] = (byte)((_magazinesCount & 0x0000ff00) >> 8);
   arr[30] = (byte)((_magazinesCount & 0x00ff0000) >> 16);
   arr[31] = (byte)((_magazinesCount & 0xff000000) >> 24);

    float f_position_x = position.x;
    int i_position_x = *((int*)&f_position_x);
arr[32] = (byte)(i_position_x & 0x000000ff);
   arr[33] = (byte)((i_position_x & 0x0000ff00) >> 8);
   arr[34] = (byte)((i_position_x & 0x00ff0000) >> 16);
   arr[35] = (byte)((i_position_x & 0xff000000) >> 24);

    float f_position_y = position.y;
    int i_position_y = *((int*)&f_position_y);
arr[36] = (byte)(i_position_y & 0x000000ff);
   arr[37] = (byte)((i_position_y & 0x0000ff00) >> 8);
   arr[38] = (byte)((i_position_y & 0x00ff0000) >> 16);
   arr[39] = (byte)((i_position_y & 0xff000000) >> 24);

    float f_position_z = position.z;
    int i_position_z = *((int*)&f_position_z);
arr[40] = (byte)(i_position_z & 0x000000ff);
   arr[41] = (byte)((i_position_z & 0x0000ff00) >> 8);
   arr[42] = (byte)((i_position_z & 0x00ff0000) >> 16);
   arr[43] = (byte)((i_position_z & 0xff000000) >> 24);


arr[44] = (byte)(id & 0x000000ff);
   arr[45] = (byte)((id & 0x0000ff00) >> 8);
   arr[46] = (byte)((id & 0x00ff0000) >> 16);
   arr[47] = (byte)((id & 0xff000000) >> 24);

arr[48] = (byte)(_state & 0x000000ff);
   arr[49] = (byte)((_state & 0x0000ff00) >> 8);
   arr[50] = (byte)((_state & 0x00ff0000) >> 16);
   arr[51] = (byte)((_state & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static ShotGun DeserializeLittleEndian(byte[] arr) {
            var result = new ShotGun();
            Assert.AreEqual(arr.Length, 52);
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

int i_result_damage;
i_result_damage = (arr[12] | (arr[13] << 8) | (arr[14] << 16) | (arr[15] << 24));
float f_result_damage = *((float*)&i_result_damage);
result.damage = f_result_damage;

result.shootsCount = (arr[16] | (arr[17] << 8) | (arr[18] << 16) | (arr[19] << 24));

int i_result_accurancy;
i_result_accurancy = (arr[20] | (arr[21] << 8) | (arr[22] << 16) | (arr[23] << 24));
float f_result_accurancy = *((float*)&i_result_accurancy);
result.accurancy = f_result_accurancy;

result._bulletsCount = (arr[24] | (arr[25] << 8) | (arr[26] << 16) | (arr[27] << 24));

result._magazinesCount = (arr[28] | (arr[29] << 8) | (arr[30] << 16) | (arr[31] << 24));

result.position = new Vector3();
int i_result_position_x;
i_result_position_x = (arr[32] | (arr[33] << 8) | (arr[34] << 16) | (arr[35] << 24));
float f_result_position_x = *((float*)&i_result_position_x);
result.position.x = f_result_position_x;

int i_result_position_y;
i_result_position_y = (arr[36] | (arr[37] << 8) | (arr[38] << 16) | (arr[39] << 24));
float f_result_position_y = *((float*)&i_result_position_y);
result.position.y = f_result_position_y;

int i_result_position_z;
i_result_position_z = (arr[40] | (arr[41] << 8) | (arr[42] << 16) | (arr[43] << 24));
float f_result_position_z = *((float*)&i_result_position_z);
result.position.z = f_result_position_z;


result.id = (arr[44] | (arr[45] << 8) | (arr[46] << 16) | (arr[47] << 24));

result._state = (arr[48] | (arr[49] << 8) | (arr[50] << 16) | (arr[51] << 24));

             
                return result;
            }

        }
        
        public static ShotGun Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'_fullReloadTime':{_fullReloadTime},'_reloadTime':{_reloadTime},'_bulletsInMagazine':{_bulletsInMagazine},'damage':{damage},'shootsCount':{shootsCount},'accurancy':{accurancy},'_bulletsCount':{_bulletsCount},'_magazinesCount':{_magazinesCount},'position':{position},'id':{id},'_state':{_state}}}";
        }
        
        public override string ToString() {
            return "ShotGun " + AsJson();
        }
    }
}