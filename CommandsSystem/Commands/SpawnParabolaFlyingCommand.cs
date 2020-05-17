using GameMechanics;
using UnityEngine;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда для создания объекта с компонентой ParabolaFlyingObject
    /// </summary>
    public partial class SpawnParabolaFlyingCommand {
        /// <summary>
        ///     Команда для создания объекта
        /// </summary>
        public SpawnPrefabCommand command;
        /// <summary>
        ///     Среднее положение объекта
        /// </summary>
        public Vector3 medium;
        /// <summary>
        ///     Конечное положение объекта
        /// </summary>
        public Vector3 target;
        /// <summary>
        ///     Время полёта объекта
        /// </summary>
        public float totalTime;

        /// <summary>
        ///     Создает на игровом поле объект с хаданными парамаетрами
        /// </summary>
        public void Run()  {
            var go = Client.client.SpawnObject(command);
            var flying = go.AddComponent<ParabolaFlyingObject>();
            flying.start = command.position;
            flying.medium = medium;
            flying.stop = target;
            flying.totalTime = totalTime;
        }
    }
}