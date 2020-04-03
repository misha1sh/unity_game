using System;
using CommandsSystem.Commands;
using UnityEngine;

namespace Character.Guns {
    [Serializable]
    public class ShotGun : ReloadingGun {
        public float _fullReloadTime = 10.0f;
        public float _reloadTime = 0.3f;
        public int _bulletsInMagazine = 3;

        public float damage = 10;
        public int shootsCount = 5;
        public float accurancy = 12f;
        
        public override float GetFullReloadTime() => _fullReloadTime;
        public override float GetReloadTime() => _reloadTime;
        public override int GetBulletsInMagazine() => _bulletsInMagazine;
        
        protected override void DoShoot() {
            for (int i = 0; i < shootsCount; i++) {
                Vector3 random_delta = ShootSystem.RandomDelta(1 / accurancy);
                ShootSystem.ShootWithDamage(player, random_delta, damage);
            }
        }
        
        public void Run() {
            var go = Client.client.SpawnObject(new SpawnPrefabCommand("shotgun", position, Quaternion.identity, id));
            go.AddComponent<ShotgunController>().gun = this;
        }
    }
}