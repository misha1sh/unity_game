
using System;
using CommandsSystem.Commands;
using UnityEngine;

namespace Character.Guns {
    
    /// <summary>
    ///     Класс для пистолета
    /// </summary>
    [Serializable]
    public partial class Pistol : ReloadingGun {
        /// <summary>
        ///      Время перезарядки магазина
        /// </summary>
        public float _fullReloadTime = 4.0f;
        /// <summary>
        ///     Время перезарядки между выстрелами
        /// </summary>
        public float _reloadTime = 0.3f;
        /// <summary>
        ///     Количество выстрелов в одном боекомплекте
        /// </summary>
        public int _bulletsInMagazine = 5;

        /// <summary>
        ///     Урон от выстрела
        /// </summary>
        public float damage = 25;
        
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
        ///     Производит выстрел из пистолета
        /// </summary>
        protected override void DoShoot() {
            ShootSystem.ShootWithDamage(player.gameObject, Quaternion.LookRotation(player.TargetRotation), Vector3.zero, damage);
        }

       /// <summary>
       ///     Создает пистолет на игровом поле
       /// </summary>
        public void Run() {
            var go = Client.client.SpawnObject(new SpawnPrefabCommand("pistol", position, Quaternion.identity, id, 0, 0));
            go.GetComponent<PistolController>().gun = this;
        }
    }
}