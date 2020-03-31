﻿
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Character.Guns {
    
    [Serializable]
    public class Pistol : ReloadingGun {
        public float _fullReloadTime = 10.0f;
        public float _reloadTime = 0.3f;
        public int _bulletsInMagazine = 8;

        public float damage = 10;
        
        public override float GetFullReloadTime() => _fullReloadTime;
        public override float GetReloadTime() => _reloadTime;
        public override int GetBulletsInMagazine() => _bulletsInMagazine;
        
        protected override void DoShoot() {
            ShootSystem.ShootWithDamage(player, Vector3.zero, damage);
        }
    }
}