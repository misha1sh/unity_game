using GameMode;
using UnityEngine;

namespace Character.HP {
    /// <summary>
    ///     Класс для работы с истониками урона
    /// </summary>
    public static class DamageSource {

        /// <summary>
        ///    Возвращает числовое значение, соотвествующее отсутствию источнику урона
        /// </summary>
        /// <returns>Числовое значение, соотвествующее отсутствию источнику урона</returns>
        public static int None() {
            return 0;
        }

        /// <summary>
        ///     Возвращает числовое значение, соотвествующее бесконечно сильному источнику урона
        /// </summary>
        /// <returns>Числовое значение, соотвествующее бесконечно сильному источнику урона</returns>
        public static int InstaKill() {
            return 1;
        }

        /// <summary>
        ///     Возвращает числовое значение, соотвествующее игроку, который является источником урона
        /// </summary>
        /// <param name="id">id игрока, являющегося источником урона</param>
        /// <returns>Числовое значение, соотвествующее игроку, который является источником урона</returns>
        public static int Player(int id) {
            return id;
        }

        /// <summary>
        ///     Возвращает числовое значение, соотвествующее источнику урона от игрока
        /// </summary>
        /// <param name="gameObject">Объект игрока, являющегося источником урона</param>
        /// <returns>Числовое значение, соотвествующее источнику урона от игрока</returns>
        public static int Player(GameObject gameObject) {
            return Player(ObjectID.GetID(gameObject));
        }

        /// <summary>
        ///     Возвращает числовое значение, соотвествующее источнику урона от бомбы
        /// </summary>
        /// <returns>Числовое значение, соотвествующее источнику урона от бомбы</returns>
        public static int Bomb() {
            return 2;
        }

        /// <summary>
        ///     Получает объект, от которого был получен урон
        /// </summary>
        /// <param name="damageSource">Числовое значение, соответствующее источнику урона</param>
        /// <returns>Объект, от которого был получен урон. null, если урон был получен не от объекта или этот объект уже не существует</returns>
        public static GameObject GetSourceGO(int damageSource) {
            GameObject res;
            if (ObjectID.TryGetObject(damageSource, out res))
                return res;
            return null;
        } 
    }
}