using Character;
using System;
using Interpolation;
using UnityEngine;

namespace CommandsSystem.Commands {
    public class ChangeGameObjectStateCommand<T> : Command<ChangeGameObjectStateCommand<T>>
        where T: GameObjectState, new()
    {
        public override CommandType type() => CommandType.CreateGhostCommand; // rqwelmrpqwoer
        
        public T state;

        public ChangeGameObjectStateCommand(T state)
        {
            this.state = state;
            
        }
        
        public ChangeGameObjectStateCommand() {}


        public override void Run()
        {
            var gameObject = ObjectID.GetObject(state.id);
            if (gameObject is null)
            {
                var spawnCommand = new SpawnPrefabCommand {
                    id = state.id, 
                    //position = state.position, 
                    //rotation = state.rotation, 
                    prefabName = "PlayerGhost"
                };

      
                
                
                gameObject = Client.client.SpawnObject(spawnCommand);
            }

            var controller = gameObject.GetComponent<UnmanagedGameObject<T>>();
            if (controller is null) return;
            controller.SetStateAnimated(state);

            /*    GhostController controller = character.GetComponent<GhostController>();
                if (controller is null) return;
                controller.SetStateAnimated(state);
    */

            /// Assert. (!character.CompareTag("EntityPlayer")) return;
        }
    }
}