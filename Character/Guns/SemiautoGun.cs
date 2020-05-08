using System;
using CommandsSystem.Commands;
using UnityEngine;

namespace Character.Guns {
    [Serializable]
    public partial class  SemiautoGun : ReloadingGun {
        public float _fullReloadTime = 10.0f;
        public float _reloadTime = 0.06f;
        public int _bulletsInMagazine = 50;

        public float damage = 2;
        public float accurancy = 50f;
        
        public override float GetFullReloadTime() => _fullReloadTime;
        public override float GetReloadTime() => _reloadTime;
        public override int GetBulletsInMagazine() => _bulletsInMagazine;
       
   
        ///  public int _bulletsCount;
        ///  public int _magazinesCount;
        ///  public Vector3 position;
        ///  public int id;
        ///  public int _state;

        
        protected override void DoShoot() {
            Vector3 random_delta = ShootSystem.RandomDelta(1 / accurancy);
            ShootSystem.ShootWithDamage(player, random_delta, damage);
        }
        
        public void Run() {
            var go = Client.client.SpawnObject(new SpawnPrefabCommand("semiauto", position, Quaternion.identity, id, 0, 0));
            go.GetComponent<SemiautoController>().gun = this;
        }
    }
}