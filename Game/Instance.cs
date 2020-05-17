using System.Collections.Generic;
using CommandsSystem.Commands;
using Networking;
using UnityEngine;

namespace GameMode {
    /// <summary>
    ///     Класс для хранения и синхронизации информации о клиенте
    /// </summary>
    public class Instance {
        /// <summary>
        ///     id данного instance
        /// </summary>
        public int id;

        /// <summary>
        ///     Отображаемое имя у данного instance
        /// </summary>
        public string name;
        /// <summary>
        ///     Текущий загруженный игровой режим
        /// </summary>
        public int currentLoadedGamemodeNum;
        /// <summary>
        ///     Конструктор для сериализации
        /// </summary>
        public Instance() {}

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="id">id создаваемого instance</param>
        public Instance(int id) {
            this.id = id;
            this.currentLoadedGamemodeNum = -1;
            this.name = "Instance#" + id;
        }

        /// <summary>
        ///     Отправляет информацию об instance другим игрокам
        /// </summary>
        public void Send() {
            CommandsHandler.gameRoom.RunSimpleCommand(new AddOrChangeInstance(this), MessageFlags.IMPORTANT);
        }
    }
}