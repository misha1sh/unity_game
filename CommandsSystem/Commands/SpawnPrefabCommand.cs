using UnityEngine;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда для создания объекта на игровом поле
    /// </summary>
    public partial class SpawnPrefabCommand {
        /// <summary>
        ///     Название префаба, в которой нужно создать
        /// </summary>
        public string prefabName;
        /// <summary>
        ///     Позиция, в которой нужно создать объект
        /// </summary>
        public Vector3 position;
        /// <summary>
        ///     Поворот, на который должен быть развернут объект
        /// </summary>
        public Quaternion rotation;
        /// <summary>
        ///     Id объекта
        /// </summary>
        public int id;
        /// <summary>
        ///     Владелец объекта
        /// </summary>
        public int owner;
        /// <summary>
        ///     Игрок, создавший объект (если объект создан не игроком следует указать -1)
        /// </summary>
        public int creator;
        
        private static System.Random random = new System.Random();

        /// <summary>
        ///     Создает объект с заданными параметрами
        /// </summary>
        public void Run() {
            var go = Client.client.SpawnObject(this);
        }
        
    }
}