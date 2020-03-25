using System;
using CommandsSystem;
using CommandsSystem.Commands;
using UnityEngine;

namespace Interpolation.Properties {
    
    [Serializable]
    public abstract class GameObjectProperty<T> : IGameObjectProperty  
        where T : GameObjectProperty<T>, new() { 

        public abstract void CopyFrom(T state);

        public int ID { get; set; }

        public void CopyFrom(IGameObjectProperty state) {
            CopyFrom(state as T);
            ID = state.ID;
        }
        
        public abstract void Interpolate(
            T lastLastState,
            T lastState,
            T nextState, 
            float coef);
        public void Interpolate(IGameObjectProperty lastLastState, IGameObjectProperty lastState, IGameObjectProperty nextState,
            float coef) {
            Interpolate(lastLastState as T, lastState as T, nextState as T, coef);
        }

        public ICommand GetCommand() {
            var command = new ChangeGameObjectStateCommand<T>();
            command.state = (T)this;
            return command;
        }


        public void FromGameObject(GameObject gameObject) {
            ID = ObjectID.GetID(gameObject);
            FromGameObject2(gameObject);
        }
        public abstract void FromGameObject2(GameObject gameObject);
        public abstract void ApplyToObject(GameObject gameObject);
    
        
    }
}