using System;
using CommandsSystem.Commands;
using UnityEngine;

namespace Character.Guns {
    /// <summary>
    ///     Класс для дробовика
    /// </summary>
    [Serializable]
    public partial class ShotGun : ReloadingGun {
        /// <summary>
        ///      Время перезарядки магазина
        /// </summary>
        public float _fullReloadTime = 5.0f;
        /// <summary>
        ///      Время перезарядки магазина
        /// </summary>
        public float _reloadTime = 0.3f;
        /// <summary>
        ///     Количество выстрелов в одном боекомплекте
        /// </summary>
        public int _bulletsInMagazine = 3;

        /// <summary>
        ///     Урон от выстрела
        /// </summary>
        public float damage = 18;
        /// <summary>
        ///     Количество дроби, вылетающее за один выстрел
        /// </summary>
        public int shootsCount = 5;
        /// <summary>
        ///     Точность (вычисляется по Гауссу)
        /// </summary>
        public float accurancy = 12f;
        
        /// <summary>
        ///     Возвращает время перезарядки магазина
        /// </summary>
        /// <returns>Время перезарядки магазина</returns>
        public override float GetFullReloadTime() => _fullReloadTime;
        /// <summary>
        ///     Возвращает время перезарядки между выстрелами
        /// </summary>
        /// <returns>Время перезарядки между выстрелами</returns>
        public override float GetReloadTime() => _reloadTime;
        /// <summary>
        ///     Возвращает количество выстрелов в одном боекомплекте
        /// </summary>
        /// <returns>Количество выстрелов в одном боекомплекте</returns>
        public override int GetBulletsInMagazine() => _bulletsInMagazine;
           
        
        // переменные для корректной работы сериализации
        ///  public int _bulletsCount;
        ///  public int _magazinesCount;
        ///  public Vector3 position;
        ///  public int id;
        ///  public int _state;


        /// <summary>
        ///     Производит выстрел из дробовика
        /// </summary>
        protected override void DoShoot() {
            for (int i = 0; i < shootsCount; i++) {
                Vector3 random_delta = ShootSystem.RandomDelta(1 / accurancy);
                ShootSystem.ShootWithDamage(player.gameObject, Quaternion.LookRotation(player.TargetRotation), random_delta, damage);
            }
        }
        
        /// <summary>
        ///     Создает дробовик на игровом поле
        /// </summary>
        public void Run() {
            var go = Client.client.SpawnObject(new SpawnPrefabCommand("shotgun", position, Quaternion.identity, id, 0, 0));
            go.GetComponent<ShotgunController>().gun = this;
        }
    }
}