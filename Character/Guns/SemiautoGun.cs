using System;
using CommandsSystem.Commands;
using UnityEngine;

namespace Character.Guns {
    /// <summary>
    ///     Класс для автомата
    /// </summary>
    [Serializable]
    public partial class  SemiautoGun : ReloadingGun {
        /// <summary>
        ///      Время перезарядки магазина
        /// </summary>
        public float _fullReloadTime = 7.0f;
        /// <summary>
        ///      Время перезарядки магазина
        /// </summary>
        public float _reloadTime = 0.06f;
        /// <summary>
        ///     Количество выстрелов в одном боекомплекте
        /// </summary>
        public int _bulletsInMagazine = 50;

        /// <summary>
        ///     Урон от выстрела
        /// </summary>
        public float damage = 3;
        /// <summary>
        ///     Точность (вычисляется по Гауссу)
        /// </summary>
        public float accurancy = 50f;
        
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
        ///     Производит выстрел из автомата
        /// </summary>
        protected override void DoShoot() {
            Vector3 random_delta = ShootSystem.RandomDelta(1 / accurancy);
            ShootSystem.ShootWithDamage(player.gameObject, Quaternion.LookRotation(player.TargetRotation), random_delta, damage);
        }
        
        /// <summary>
        ///     Создает автомат на игровом поле
        /// </summary>
        public void Run() {
            var go = Client.client.SpawnObject(new SpawnPrefabCommand("semiauto", position, Quaternion.identity, id, 0, 0));
            go.GetComponent<SemiautoController>().gun = this;
        }
    }
}