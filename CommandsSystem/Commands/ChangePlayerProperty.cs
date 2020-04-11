using Interpolation;
using UnityEngine;

namespace CommandsSystem.Commands {
    public partial class ChangePlayerProperty {
        public PlayerProperty property;
        public float deltaTime;
        
        public void Run() {
            GameObject gameObject;
            if (!ObjectID.TryGetObject(property.id, out gameObject))
            {
                var spawnCommand = new SpawnPrefabCommand {
                    id = property.id, 
                    position = property.position, 
                    rotation = property.rotation, 
                    prefabName = "RobotGhost"
                };

      
                
                
                gameObject = Client.client.SpawnObject(spawnCommand);
            }

            var controller = gameObject.GetComponent<UnmanagedGameObject<PlayerProperty>>();
            if (controller is null) return;
            controller.SetStateAnimated(property, deltaTime);
#if DEBUG_INTERPOLATION              
            DebugExtension.DebugPoint(property.position, Color.red, 0.1f, 3);
#endif
            /*    GhostController controller = character.GetComponent<GhostController>();
                if (controller is null) return;
                controller.SetStateAnimated(state);
    */

            /// Assert. (!character.CompareTag("EntityPlayer")) return;
        }
    }
}