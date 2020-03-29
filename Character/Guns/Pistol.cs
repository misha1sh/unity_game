
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Character.Guns {
    
    [Serializable]
    [DataContract]
    public class Pistol : IGun {
        [DataMember, SerializeField]
        public GunState state { get; private set; }

        
        [DataMember, SerializeField]
        private float fullReloadTime = 3.0f;
        [DataMember, SerializeField]
        private float reloadTime = 1.0f;
        [DataMember, SerializeField]
        private int bulletsInMagazine = 8;
        [DataMember, SerializeField]
        private int magazinesCount = 3;

        [DataMember]
        private int bulletsCount;

        
        public float GetFullReloadTime() => fullReloadTime;

        public float GetReloadTime() => reloadTime;

        // includes current bullet
        public int GetBulletsCount() => bulletsCount;
        // not includes current magazine
        public int GetMagazinesCount() => magazinesCount;

        public Pistol() {
            bulletsCount = bulletsInMagazine;
            state = GunState.READY;
        }


        public void OnPickedUp() {
        }

        public void OnDropped() {
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
                    bulletsCount = bulletsInMagazine;
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