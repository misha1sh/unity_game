
using Character;
using Events;
using GameMode;
using UnityEngine;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда для создания на игровом поле персонажа
    /// </summary>
    public partial class SpawnPlayerCommand {
        /// <summary>
        ///     Базовая команда для создания объекта 
        /// </summary>
        public SpawnPrefabCommand command;
        
        /// <summary>
        ///     Id игрока, который будет управлять данным персонажем
        /// </summary>
        public int playerId;
  
        /// <summary>
        ///     Создает персонажа с заданными параметрами
        /// </summary>
        public void Run() {
            Player player = PlayersManager.GetPlayerById(playerId);

            GameObject go;
            if (command.owner == sClient.ID) {
                if (player.controllerType == 0) {
                    command.prefabName += "WithPlayer";
                    go = Client.client.SpawnObject(command);
                    Client.client.cameraObj.GetComponent<CameraFollower>().character = go;
                    Client.client.mainPlayerObj = go;
                } else {
                    command.prefabName += "WithAI";
                    go = Client.client.SpawnObject(command);
                }

            } else {
                command.prefabName += "Ghost";
                go = Client.client.SpawnObject(command);
            }

            go.GetComponent<PlayerStorage>().Player = player;

            EventsManager.handler.OnSpawnedPlayer(go, player);
            /*     if (ObjectID.IsOwned(go)) {
                     switch (controllerType) {
                         case 0:
                             
                             break;
                         case 1:
                             
                             break;
                     }
                 }
                */

        }
    }
}