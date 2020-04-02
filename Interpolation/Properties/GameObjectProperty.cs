using System;
using CommandsSystem;
using CommandsSystem.Commands;
using UnityEngine;

namespace Interpolation.Properties {
    
    [Serializable]
    public abstract class GameObjectProperty<T> : IGameObjectProperty, ICommand
        where T : GameObjectProperty<T>, new() { 

        public abstract void CopyFrom(T state);

        public void CopyFrom(IGameObjectProperty state) {
            CopyFrom(state as T);
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
            return this;
        }


        public abstract void FromGameObject(GameObject gameObject);
        public abstract void ApplyToObject(GameObject gameObject);


        public abstract void Run();
    }
}