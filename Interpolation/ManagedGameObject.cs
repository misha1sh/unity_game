using System;
using CommandsSystem;
using CommandsSystem.Commands;
using Interpolation.Properties;
using Networking;
using UnityEngine;

namespace Interpolation {
    /// <summary>
    ///     Компонента для игрового объекта, управляемого из текущего клиента
    /// </summary>
    public class ManagedGameObject<T> : MonoBehaviour
    where T: IGameObjectProperty, new() {
        /// <summary>
        ///     Время, когда в последний раз был синхронищирован объект
        /// </summary>
        private float lastSendState = -1;
        
        /// <summary>
        ///     Свойство объекта
        /// </summary>
        public T property;

        /// <summary>
        ///     Период синхронизации состояния
        /// </summary>
        protected virtual float updateTime => 1f / sClient.NETWORK_FPS;


        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        public void Start() {
            property = new T();
            property.FromGameObject(gameObject);
        }
        
        /// <summary>
        ///     Отправляет состояние объекта, если нужно. Автоматически вызвается Unity каждый кадр
        /// </summary>
        void Update() {
            float curTime = Time.time;
            
            if (curTime - lastSendState > updateTime) {
//                Debug.Log("Sending coordianates " );
                property.FromGameObject(gameObject);
                ICommand command;
                command = property.CreateChangedCommand(curTime - lastSendState);
                // var command = property.GetCommand();
                CommandsHandler.gameRoom.RunSimpleCommand(command, MessageFlags.NONE);
                lastSendState = curTime;
            }
        }
    }
    
}