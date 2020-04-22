
using Character;
using GameMode;
using UnityEngine;

namespace CommandsSystem.Commands {
    public partial class SpawnPlayerCommand {
        public SpawnPrefabCommand command;

        public int playerId;
  
        
        
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