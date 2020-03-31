using Character.Guns;
using UnityEngine;

namespace Character.Actions {
    public class ShootSemiautoAction : ShootAction<ReloadingGun> {
        private bool needShoot = false;
        public override void OnStartDoing() {
            needShoot = true;
        }
        
        public override void OnStopDoing() {
            needShoot = false;
        }

        void LateUpdate() {
            gun.Update(Time.deltaTime);
            if (needShoot && gun.state == GunState.READY) {
                gun.Shoot();
            }
        }
    }
}