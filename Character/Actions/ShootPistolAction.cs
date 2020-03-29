using System.Collections;
using Character.Guns;
using UnityEngine;

namespace Character.Actions {
    public class ShootPistolAction : ShootAction {

        private Pistol pistol;

        public void Init(Pistol pistol) {
            this.pistol = pistol;
        }

        private float needShoot = -100;
        public override void OnStartDoing() {
            needShoot = Time.time;
        }
        
        public override void OnStopDoing() {
        }

        void LateUpdate() {
            pistol.Update(Time.deltaTime);
            if (Time.time - needShoot < 0.15f && pistol.state == GunState.READY) {
                pistol.Shoot();
                ShootWithDamage(Vector3.zero, 10);
                needShoot = -100;
            }
        }
    }
}
