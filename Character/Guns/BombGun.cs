using System;
using Character.Guns;
using CommandsSystem.Commands;
using UnityEngine;

namespace Character.Guns {
    [Serializable]
    public partial class BombGun : ReloadingGun  {
        public float _fullReloadTime = 10.0f;
        public float _reloadTime = 0.3f;
        public int _bulletsInMagazine = 10;

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
            ShootSystem.ShootWithBomb(player.gameObject, player.GetComponent<ActionController>().Target, "bomb");
        }

        public void Run() {
            var go = Client.client.SpawnObject(new SpawnPrefabCommand("bombgun", position, Quaternion.identity, id, 0, 0));
            go.GetComponent<BombGunController>().gun = this;
        }
    }
}