using System;
using Character.Guns;
using CommandsSystem.Commands;
using UnityEngine;

namespace Character.Guns {
    /// <summary>
    ///     Класс для гранотомета
    /// </summary>
    [Serializable]
    public partial class BombGun : ReloadingGun  {
        /// <summary>
        ///      Время перезарядки магазина
        /// </summary>
        public float _fullReloadTime = 10.0f;
        /// <summary>
        ///     Время перезарядки между выстрелами
        /// </summary>
        public float _reloadTime = 0.3f;
        /// <summary>
        ///     Количество выстрелов в одном боекомплекте
        /// </summary>
        public int _bulletsInMagazine = 10;

        
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
        ///     Производит выстрел из гранатомета
        /// </summary>
        protected override void DoShoot() {
            ShootSystem.ShootWithBomb(player.gameObject, player.GetComponent<ActionController>().Target, "bomb");
        }

        /// <summary>
        ///     Создает гранотомет на игровом поле
        /// </summary>
        public void Run() {
            var go = Client.client.SpawnObject(new SpawnPrefabCommand("bombgun", position, Quaternion.identity, id, 0, 0));
            go.GetComponent<BombGunController>().gun = this;
        }
    }
}