
using System;
using CommandsSystem.Commands;
using UnityEngine;

namespace Character.Guns {
    
    [Serializable]
    public partial class Pistol : ReloadingGun {
        public float _fullReloadTime = 10.0f;
        public float _reloadTime = 0.3f;
        public int _bulletsInMagazine = 8;

        public float damage = 10;
        
        public override float GetFullReloadTime() => _fullReloadTime;
        public override float GetReloadTime() => _reloadTime;
        public override int GetBulletsInMagazine() => _bulletsInMagazine;
   
        
   ///  public int _bulletsCount;
   ///  public int _magazinesCount;
   ///  public Vector3 position;
   ///  public int id;
   ///  public int _state;

   protected override void DoShoot() {
            ShootSystem.ShootWithDamage(player, Vector3.zero, damage);
        }

        public void Run() {
            var go = Client.client.SpawnObject(new SpawnPrefabCommand("pistol", position, Quaternion.identity, id, 0));
            go.GetComponent<PistolController>().gun = this;
        }
    }
}