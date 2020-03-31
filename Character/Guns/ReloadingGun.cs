using System;
using UnityEngine;

namespace Character.Guns {
    [Serializable]
    public abstract class ReloadingGun : IGun {
        public GunState state { get; set; }

        
     
        
        public abstract float GetFullReloadTime();
        public abstract float GetReloadTime();
        public abstract int GetBulletsInMagazine();

        // includes current bullet
        public int _bulletsCount;
        public int bulletsCount { 
            get => _bulletsCount;
            private set {
                _bulletsCount = value;
                if (player != null) {
                    EventsManager.handler.OnPlayerBulletsCountChanged(player, _bulletsCount);
                }
            } 
        }

        // not includes current magazine
        public int _magazinesCount = 3;
        public int magazinesCount {
            get => _magazinesCount;
            private set {
                _magazinesCount = value;
                if (player != null) {
                    EventsManager.handler.OnPlayerMagazinesCountChanged(player, _magazinesCount);
                }
            }
        }
        


        

        public ReloadingGun() {
            bulletsCount = GetBulletsInMagazine();
            state = GunState.READY;
        }


        protected GameObject player;
        public void OnPickedUp(GameObject player) {
            this.player = player;
        }

        public void OnDropped() {
            this.player = null;
            // drop reloading state
            if (state == GunState.RELOADING_MAGAZINE)
                state = GunState.READY;
        }

        private float needTime = 0;
        public void Update(float dt) {
            if (state == GunState.RELOADING_BULLET) {
                needTime -= dt;
                if (needTime <= 0)
                    state = GunState.READY;
            }
            if (state == GunState.RELOADING_MAGAZINE) {
                needTime -= dt;
                if (needTime <= 0) {
                    state = GunState.READY;
                    bulletsCount = GetBulletsInMagazine();
                    magazinesCount--;
                }
            }
        }

        protected abstract void DoShoot();
        
        public void Shoot() {
            if (state == GunState.READY) {
                DoShoot();
                bulletsCount--;
                SetReloadBullet();
          
                if (bulletsCount > 0) {
                    SetReloadBullet();
                } else {
                    SetReloadMagazine();
                }
            } else {
                throw new InvalidOperationException("Cannot shoot without bullet");
            }
        }

        private void SetReloadBullet() {
            state = GunState.RELOADING_BULLET;
            needTime = GetReloadTime();
        }

        public void SetReloadMagazine() {
            if (magazinesCount == 0) return;
            bulletsCount = 0;
            state = GunState.RELOADING_MAGAZINE;
            needTime = GetFullReloadTime();
        }
    }
}