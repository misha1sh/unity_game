using Character;
using CommandsSystem.Commands;
using Interpolation.Properties;
using UnityEngine;

namespace Interpolation {
    public class ManagedGameObject<T> : MonoBehaviour
    where T: IGameObjectProperty, new() {

        private float lastSendState = -1;


        public T property;




        public void Start() {
            ObjectID.StoreObject(gameObject, Client.client.random.Next());

            property = new T();
            property.FromGameObject(gameObject);
        }

        void Update() {
            if (Time.time - lastSendState > 1f / Client.NETWORK_FPS) {
//                Debug.Log("Sending coordianates " );
                property.FromGameObject(gameObject);
                var command = property.GetCommand();
                Client.client.commandsHandler.RunSimpleCommand(command);
                lastSendState = Time.time;
            }
        }
    }
    
}