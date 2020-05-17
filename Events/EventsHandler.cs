using Character.Guns;
using Game;
using GameMode;
using UnityEngine;

namespace Events {
    /// <summary>
    ///     Класс для обработки событий
    /// </summary>
    public class EventsHandler {
        /// <summary>
        ///     Делегат для события об изменении параметра персонажа
        /// </summary>
        /// <param name="player">Персонаж</param>
        /// <param name="parameter">Изменившийся параметр</param>
        /// <typeparam name="T">Тип параметра</typeparam>
        public delegate void PlayerObjParameterChanged<T>(GameObject player, T parameter);

        
        /// <summary>
        ///     Событие изменения числа патронов в оружии персонажа
        /// </summary>
        public PlayerObjParameterChanged<int> OnPlayerBulletsCountChanged = delegate { };
        /// <summary>
        ///     Событие изменения числа магазинов у персонажа
        /// </summary>
        public PlayerObjParameterChanged<int> OnPlayerMagazinesCountChanged = delegate { };
        /// <summary>
        ///     Событие, когда персонаж подбирает оружие
        /// </summary>
        public PlayerObjParameterChanged<IGun> OnPlayerPickedUpGun = delegate { };
        /// <summary>
        ///     Событие, когда персонаж теряет оружие
        /// </summary>
        public PlayerObjParameterChanged<IGun> OnPlayerDroppedGun = delegate { };

        /// <summary>
        ///     Событие, когда персонаж подбирает монетку
        /// </summary>
        public PlayerObjParameterChanged<GameObject> OnPlayerPickedUpCoin = delegate { };
        
        
        
        /// <summary>
        ///     Делегат события об изменении параметра игрока
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <param name="parameter">Изменившийся параметр</param>
        /// <typeparam name="T">Тип параметра</typeparam>
        public delegate void PlayerParameterChanged<T>(Player player, T parameter);

        /// <summary>
        ///     Событие, когда игрок поу
        /// </summary>
        public PlayerParameterChanged<int> OnPlayerScoreChanged = delegate { };

        /// <summary>
        ///     Делегат для события смерти объекта
        /// </summary>
        /// <param name="go">Умерший объект</param>
        /// <param name="killSource">Источник урона</param>
        public delegate void ObjectDead(GameObject go, int killSource);
        /// <summary>
        ///     Событие, когда объект умирает
        /// </summary>
        public ObjectDead OnObjectDead = delegate { };

        /// <summary>
        ///     Делегат для события о получении урона объектом
        /// </summary>
        /// <param name="go">Объект, получивший урон</param>
        /// <param name="delta">Изменение здоровья объекта</param>
        /// <param name="damageSource">Источник урона</param>
        public delegate void ObjectGotDamage(GameObject go, float delta, int damageSource);
        /// <summary>
        ///     Событие о получении урона объектом
        /// </summary>
        public ObjectGotDamage OnObjectChangedHP = delegate { };


        /// <summary>
        ///     Делегат для события об изменении информации о текущем матче
        /// </summary>
        /// <param name="last">Старая информация о матче</param>
        /// <param name="current">Новая информация о матче</param>
        public delegate void CurrentMatchChanged(MatchInfo last, MatchInfo current);
        /// <summary>
        ///     Событие об изменении информации о текущем матче
        /// </summary>
        public CurrentMatchChanged OnCurrentMatchChanged = delegate { };

        /// <summary>
        ///     Делегат для события о появлении персонажа на игровом поле
        /// </summary>
        /// <param name="gameObject">Персонаж</param>
        /// <param name="player">Игрок</param>
        public delegate void SpawnedPlayer(GameObject gameObject, Player player);
        /// <summary>
        ///     Событие о появлении персонажа на игровом поле
        /// </summary>
        public SpawnedPlayer OnSpawnedPlayer = delegate { };
    }
}
