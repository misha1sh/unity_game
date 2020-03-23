using UnityEngine;

namespace Interpolation.Properties {
    public interface IGameObjectProperty<T>
        where T : new() {
        T Create();
        void CopyFrom(T state);
        void FromGameObject(GameObject gameObject);
        void ApplyToObject(GameObject gameObject);

        void Interpolate(
            T lastLastState,
            T lastState,
            T nextState, 
            float coef);
    }
}