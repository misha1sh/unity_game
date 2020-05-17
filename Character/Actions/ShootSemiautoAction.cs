using Character.Guns;
using UnityEngine;

namespace Character.Actions {
    /// <summary>
    ///     Действие для стрельбы из автомата (непрерывно производит выстрелы, пока действие выполняется)
    /// </summary>
    public class ShootSemiautoAction : ShootAction<ReloadingGun> {
        /// <summary>
        ///     Переенная показывающая, нужно ли производить выстрел
        /// </summary>
        private bool needShoot = false;
        
        /// <summary>
        ///     Начинает стрельбу из оружия
        /// </summary>
        public override void OnStartDoing() {
            needShoot = true;
        }
        
        /// <summary>
        ///     Прекращает стрельбу из оружия
        /// </summary>
        public override void OnStopDoing() {
            needShoot = false;
        }

        /// <summary>
        ///     Обновляет состояние оружия. Если была отдана команда стрелять и оружие заряжено -- стреляет
        /// </summary>
        void LateUpdate() {
            gun.Update(Time.deltaTime);
            if (needShoot && gun.state == GunState.READY) {
                gun.Shoot();
            }
        }
    }
}