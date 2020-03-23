using System;
using UnityEngine;

namespace Interpolation.Properties {
    [Serializable]
    public class TransformProperty : IGameObjectProperty<TransformProperty> {
        public Vector3 position;
        public Quaternion rotation;
        
        public TransformProperty() {}
        public TransformProperty Create() {
            return new TransformProperty();
        }

        public void CopyFrom(TransformProperty state) {
            position = state.position;
            rotation = state.rotation;
        }

        public void FromGameObject(GameObject gameObject) {
            position = gameObject.transform.position;
            rotation = gameObject.transform.rotation;
        }

        public void ApplyToObject(GameObject gameObject) {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
        }

        public void Interpolate(TransformProperty lastLastState, TransformProperty lastState, TransformProperty nextState, float coef) {
            position = InterpolationFunctions.InterpolatePosition(lastLastState.position, lastState.position, nextState.position, coef);
            rotation = InterpolationFunctions.InterpolateRotation(lastState.rotation, nextState.rotation, coef);
        }
    }
}