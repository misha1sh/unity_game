using Character.Guns;
using UnityEngine;

namespace Character.Actions {
    /// <summary>
    ///     Действие для стрельбы из пистолета (делает один выстрел за клик)
    /// </summary>
    public class ShootPistolAction : ShootAction<ReloadingGun> {
        /// <summary>
        ///     Время, когда была отдана последняя команда стрелять.
        ///     Нужно для возможности отдать команду стрелять, если до перезарядки орудия осталось меньше 150 мсек
        /// </summary>
        private float needShoot = -100;
        
        /// <summary>
        ///    Отдает команду стрелять
        /// </summary>
        public override void OnStartDoing() {
            needShoot = Time.time;
        }
        
        /// <summary>
        ///     Вызывается при прекращении действия
        /// </summary>
        public override void OnStopDoing() {}

        /// <summary>
        ///     Обновляет состояние оружия. Если была отдана команда стрелять и оружие заряжено -- стреляет
        /// </summary>
        void LateUpdate() {
            gun.Update(Time.deltaTime);
            if (Time.time - needShoot < 0.15f && gun.state == GunState.READY) {
                gun.Shoot();
                needShoot = -100;
            }
        }
    }
}
