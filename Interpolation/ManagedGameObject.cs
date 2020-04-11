using System;
using CommandsSystem;
using CommandsSystem.Commands;
using Interpolation.Properties;
using UnityEngine;

namespace Interpolation {
    public class ManagedGameObject<T> : MonoBehaviour
    where T: IGameObjectProperty, new() {

        private float lastSendState = -1;


        public T property;

        protected virtual float updateTime => 1f / Client.NETWORK_FPS;


        public void Start() {
//            ObjectID.StoreOwnedObject(gameObject);

            property = new T();
            property.FromGameObject(gameObject);
        }
        

        void Update() {
            float curTime = Time.time;
            
            if (curTime - lastSendState > updateTime) {
//                Debug.Log("Sending coordianates " );
                property.FromGameObject(gameObject);
                ICommand command;
                if (property is PlayerProperty p) {
                    command = new ChangePlayerProperty(p, curTime - lastSendState);
                } else {
                    throw new Exception("Unknown property " + property.GetType());
                }
                // var command = property.GetCommand();
                Client.client.commandsHandler.RunSimpleCommand(command);
                lastSendState = curTime;
            }
        }
    }
    
}