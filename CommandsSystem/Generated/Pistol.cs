
using System;
using System.Text;
using Character;
using Interpolation;
using Interpolation.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using Character.HP;
using CommandsSystem;

namespace Character.Guns {
    public partial class Pistol : ICommand  {

        public Pistol(){}
        
        public Pistol(float _fullReloadTime,float _reloadTime,int _bulletsInMagazine,float damage,int _bulletsCount,int _magazinesCount) {
            this._fullReloadTime = _fullReloadTime;
this._reloadTime = _reloadTime;
this._bulletsInMagazine = _bulletsInMagazine;
this.damage = damage;
this._bulletsCount = _bulletsCount;
this._magazinesCount = _magazinesCount;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[24];
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

arr[16] = (byte)(_bulletsCount & 0x000000ff);
   arr[17] = (byte)((_bulletsCount & 0x0000ff00) >> 8);
   arr[18] = (byte)((_bulletsCount & 0x00ff0000) >> 16);
   arr[19] = (byte)((_bulletsCount & 0xff000000) >> 24);

arr[20] = (byte)(_magazinesCount & 0x000000ff);
   arr[21] = (byte)((_magazinesCount & 0x0000ff00) >> 8);
   arr[22] = (byte)((_magazinesCount & 0x00ff0000) >> 16);
   arr[23] = (byte)((_magazinesCount & 0xff000000) >> 24);


                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static Pistol DeserializeLittleEndian(byte[] arr) {
            var result = new Pistol();
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

result._bulletsCount = (arr[16] | (arr[17] << 8) | (arr[18] << 16) | (arr[19] << 24));

result._magazinesCount = (arr[20] | (arr[21] << 8) | (arr[22] << 16) | (arr[23] << 24));

             
                return result;
            }

        }
        
        public static Pistol Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'_fullReloadTime':{_fullReloadTime},'_reloadTime':{_reloadTime},'_bulletsInMagazine':{_bulletsInMagazine},'damage':{damage},'_bulletsCount':{_bulletsCount},'_magazinesCount':{_magazinesCount}}}";
        }
        
        public override string ToString() {
            return "Pistol " + AsJson();
        }
    }
}