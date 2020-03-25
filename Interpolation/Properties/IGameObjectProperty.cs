using CommandsSystem;
using UnityEngine;

namespace Interpolation.Properties {
    public interface IGameObjectProperty {
        int ID { get; }
        
        void CopyFrom(IGameObjectProperty state);
        void FromGameObject(GameObject gameObject);
        void ApplyToObject(GameObject gameObject);

        void Interpolate(
            IGameObjectProperty lastLastState,
            IGameObjectProperty lastState,
            IGameObjectProperty nextState, 
            float coef);

        ICommand GetCommand();

    }
}