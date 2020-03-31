using System;
using UnityEngine;

namespace Character.Guns {
    [Serializable]
    public class SemiautoGun : ReloadingGun {
        public float _fullReloadTime = 10.0f;
        public float _reloadTime = 0.06f;
        public int _bulletsInMagazine = 50;

        public float damage = 2;
        public float accurancy = 50f;
        
        public override float GetFullReloadTime() => _fullReloadTime;
        public override float GetReloadTime() => _reloadTime;
        public override int GetBulletsInMagazine() => _bulletsInMagazine;
       
        
        protected override void DoShoot() {
            Vector3 random_delta = ShootSystem.RandomDelta(1 / accurancy);
            ShootSystem.ShootWithDamage(player, random_delta, damage);
        }
    }
}