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

        protected virtual float updateTime => 1f / sClient.NETWORK_FPS;


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
                command = property.CreateChangedCommand(curTime - lastSendState);
                // var command = property.GetCommand();
                sClient.commandsHandler.RunSimpleCommand(command, 0);
                lastSendState = curTime;
            }
        }
    }
    
}