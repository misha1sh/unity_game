using Character;
using CommandsSystem.Commands;
using UnityEngine;

namespace Interpolation {
    public class ManagedGameObject : MonoBehaviour {

        private float lastSendState = -1;


        public GameObjectState properties;

        
        
        
        public void Start() {
            properties.FromGameObject(gameObject);
        }
        
        
        void Update() {
            if (Time.time - lastSendState > 1f / Client.NETWORK_FPS) {
//                Debug.Log("Sending coordianates " );
                properties.FromGameObject(gameObject);
                var command = properties.GetCommand();
                Client.client.commandsHandler.RunSimpleCommand(command);
                lastSendState = Time.time;
            }
        }
    }
    
}