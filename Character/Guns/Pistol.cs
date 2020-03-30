
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Character.Guns {
    
    [Serializable]
    public class Pistol : IGun {
        public GunState state;

        
        public float _fullReloadTime = 3.0f;
        
        public float _reloadTime = 1.0f;
        
        public int _bulletsInMagazine = 8;
        

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
                    EventsManager.handler.OnPlayerMagazinesCountChanged(player, _bulletsCount);
                }
            }
        }
        
        public float GetFullReloadTime() => _fullReloadTime;

        public float GetReloadTime() => _reloadTime;

        

        public Pistol() {
            bulletsCount = _bulletsInMagazine;
            state = GunState.READY;
        }


        private GameObject player;
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
                    bulletsCount = _bulletsInMagazine;
                    magazinesCount--;
                }
            }
        }

        public void Shoot() {
            if (state == GunState.READY) {
                bulletsCount--;
                SetReloadBullet();

                if (GetBulletsCount() > 0) {
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
        
  /*      private float needTime = 0;

        public float RemainingWaitTime() => Mathf.Max(0, needTime - Client.client.GameTime);
        
        /// <returns> time to wait </returns>
        public float ReloadMagazine() {
            magazinesCount--;
            if (magazinesCount <= 0) return 0; // means we dropped last magazine
            
            bulletsCount = bulletsInMagazine;

            float reloadTime = GetFullReloadTime();
            needTime = Client.client.GameTime + reloadTime;
            
            return RemainingWaitTime();
        }

        public float ReloadBullet() {
            bulletsCount--;
            if (bulletsCount <= 0) return 0; // means we shotted last bullet last time
            
            float reloadTime = GetReloadTime();
            needTime = Client.client.GameTime + reloadTime;
            
            return RemainingWaitTime();
        }*/

    }
}