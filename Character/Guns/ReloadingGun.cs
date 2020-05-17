using System;
using CommandsSystem;
using Events;
using Interpolation.Managers;
using Networking;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character.Guns {
    /// <summary>
    ///     Базовый класс для оружия, которое может перезаряжаться
    /// </summary>
    [Serializable]
    public abstract class ReloadingGun : IGun {
        /// <summary>
        ///     Состояние оружия в виде целого числа (нужно для сериализации)
        /// </summary>
        public int _state = (int)GunState.READY;
        /// <summary>
        ///     Состояние оружия
        /// </summary>
        public GunState state {
            get => (GunState) _state;
            set => _state = (int) value;
        }
        
        /// <summary>
        ///     Позиция на игровом поле, на котором должно появиться оружие
        /// </summary>
        public Vector3 position;
        /// <summary>
        ///    id оружия
        /// </summary>
        public int id;

        /// <summary>
        ///     Возвращает время перезарядки магазина
        /// </summary>
        /// <returns>Время перезарядки магазина</returns>
        public abstract float GetFullReloadTime();
        /// <summary>
        ///     Возвращает время перезарядки между выстрелами
        /// </summary>
        /// <returns>Время перезарядки между выстрелами</returns>
        public abstract float GetReloadTime();
        /// <summary>
        ///     Возвращает количество выстрелов в одном боекомплекте
        /// </summary>
        /// <returns>Количество выстрелов в одном боекомплекте</returns>
        public abstract int GetBulletsInMagazine();

        /// <summary>
        ///     Переменная для хранения bulletsCount
        /// </summary>
        public int _bulletsCount;
        /// <summary>
        ///     Текущее количество патронов, которое заряжено (включая тот, которым будет произведен выстрел)
        /// </summary>
        public int bulletsCount { 
            get => _bulletsCount;
            private set {
                _bulletsCount = value;
                if (player != null) {
                    EventsManager.handler.OnPlayerBulletsCountChanged(player.gameObject, _bulletsCount);
                }
            } 
        }

        /// <summary>
        ///     Переенная для хранения magazinesCount
        /// </summary>
        public int _magazinesCount = 1;
        /// <summary>
        ///     Количество полных магазинов с патронами, которое осталось
        /// </summary>
        public int magazinesCount {
            get => _magazinesCount;
            private set {
                _magazinesCount = value;
                if (player != null) {
                    EventsManager.handler.OnPlayerMagazinesCountChanged(player.gameObject, _magazinesCount);
                }
            }
        }

        /// <summary>
        ///     Конструктор оружия, которое может перезаряжаться. Инициализирует переменные
        /// </summary>
        public ReloadingGun() {
            bulletsCount = GetBulletsInMagazine();
            state = GunState.READY;
            id = ObjectID.RandomID;
        }


        /// <summary>
        ///     Ссылка на MotionController игрока, который держит оружие
        /// </summary>
        protected MotionController player;
        /// <summary>
        ///     Обрабатывает событие, когда игрок подбирает оружие
        /// </summary>
        /// <param name="player">Игрок, подобравший оружие</param>
        public void OnPickedUp(GameObject player) {
            this.player = player.GetComponent<MotionController>();
            if (bulletsCount == 0) {
                SetReloadMagazine();
            }            
        }

        /// <summary>
        ///     Проверяет, остались ли патроны или магазины в оружии
        /// </summary>
        /// <returns>true, если не осталось ни патронов, ни магазинов. Иначе возвращает false</returns>
        public bool IsEmpty() => bulletsCount == 0 && magazinesCount == 0;

        /// <summary>
        ///     Создает на игрвом поле данное оружие
        /// </summary>
        private void Spawn() {
            Vector3 dir;
            var rig = player.GetComponent<Rigidbody>();
            dir = rig.velocity;
            if (dir.sqrMagnitude < 0.01f)
                dir = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);//player.transform.forward;
            //}

            id = ObjectID.RandomID;
            this.position = player.transform.position - dir.normalized * 2  + Vector3.up;
            CommandsHandler.gameRoom.RunSimpleCommand(this as ICommand, MessageFlags.IMPORTANT);
        }
        
        /// <summary>
        ///     Обрабатывает выбрасывание игроком оружия
        /// </summary>
        public void OnDropped() {
         /*   if (!IsEmpty()) // just destroy it
            {
                Spawn();
            }*/
 
            this.player = null;
          /*  
            // drop reloading state
            if (state == GunState.RELOADING_MAGAZINE)
                state = GunState.READY;*/
        }

        /// <summary>
        ///     Время, которое осталось до перезарядки патрона или магазина
        /// </summary>
        private float needTime = 0;
        
        /// <summary>
        ///     Обновляет состояние оружия. Перезаряжает патроны в оружии
        /// </summary>
        /// <param name="dt">Время, которое прошло с последнего обновления состояния</param>
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
                    bulletsCount = GetBulletsInMagazine();
                    magazinesCount--;
                }
            }
        }

        /// <summary>
        ///     Выпускает пулю из оружия
        /// </summary>
        protected abstract void DoShoot();
        
        
        /// <summary>
        ///     Производит выстрел из оружия
        /// </summary>
        /// <exception cref="InvalidOperationException">Вызывает исключение, если в оружии не было заряжено патрона</exception>
        public void Shoot() {
            if (state == GunState.READY) {
                DoShoot();
                bulletsCount--;
                SetReloadBullet();
          
                if (bulletsCount > 0) {
                    SetReloadBullet();
                } else if (magazinesCount > 0) {
                    SetReloadMagazine();
                } else {
                    //throw  new Exception("werwerwererw");
                    player.GetComponent<ActionController>().SetNothing();
                }
            } else {
                throw new InvalidOperationException("Cannot shoot without bullet");
            }
        }

        /// <summary>
        ///     Устанавливает состояние на перезарядку патрона
        /// </summary>
        private void SetReloadBullet() {
            state = GunState.RELOADING_BULLET;
            needTime = GetReloadTime();
        }
        
        /// <summary>
        ///     Устанавливает состояние на перезарядку магазина
        /// </summary>
        public void SetReloadMagazine() {
            if (magazinesCount == 0) return;
            bulletsCount = 0;
            state = GunState.RELOADING_MAGAZINE;
            needTime = GetFullReloadTime();
        }
    }
}