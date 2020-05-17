using System.Collections.Generic;

namespace GameMode {
    /// <summary>
    ///     Класс для управления instance
    /// </summary>
    public static class InstanceManager {
        /// <summary>
        ///     Список instance в текущей игре
        /// </summary>
        public static List<Instance> instances = new List<Instance>();
        /// <summary>
        ///     Текущий instance
        /// </summary>
        public static Instance currentInstance;
        /// <summary>
        ///     Id текущего instance
        /// </summary>
        public static int ID => currentInstance.id;

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        public static void Init() {
            currentInstance = new Instance(ObjectID.RandomID);
            instances.Add(currentInstance);
        }

        /// <summary>
        ///     Сбрасывает состояние переменных
        /// </summary>
        public static void Reset() {
            instances.Clear();
            if (currentInstance != null) {
                instances.Add(currentInstance);
                currentInstance.currentLoadedGamemodeNum = -1;
            }
        }
    }
}