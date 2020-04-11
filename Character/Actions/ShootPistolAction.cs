using Character.Guns;
using UnityEngine;

namespace Character.Actions {
    public class ShootPistolAction : ShootAction<ReloadingGun> {
        private float needShoot = -100;
        public override void OnStartDoing() {
            needShoot = Time.time;
        }
        
        public override void OnStopDoing() {
        }

        void LateUpdate() {
            gun.Update(Time.deltaTime);
            if (Time.time - needShoot < 0.15f && gun.state == GunState.READY) {
                gun.Shoot();
                needShoot = -100;
            }
        }
    }
}
