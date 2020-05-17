using UnityEngine;

namespace Character.Guns {
    /// <summary>
    ///     Интерфейс оружия
    /// </summary>
    public interface IGun {
        /// <summary>
        ///     Состояние оружия
        /// </summary>
        GunState state { get; }
        /// <summary>
        ///     Обработчик. Вызывается при подборе игроком оружия
        /// </summary>
        /// <param name="player">Игрок, который подобрал оружие</param>
        void OnPickedUp(GameObject player);
        /// <summary>
        ///     Обработчик. Вызывается при потере игроком оружия
        /// </summary>
        void OnDropped();
    }
}