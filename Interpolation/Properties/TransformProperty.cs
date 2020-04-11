namespace Interpolation.Properties {
   /* [Serializable]
    public class TransformProperty : GameObjectProperty<TransformProperty> {
        public Vector3 position;
        public Quaternion rotation;
        

        public override void CopyFrom(TransformProperty state) {
            position = state.position;
            rotation = state.rotation;
        }

        public override void FromGameObject(GameObject gameObject) {
            position = gameObject.transform.position;
            rotation = gameObject.transform.rotation;
        }

        public override void ApplyToObject(GameObject gameObject) {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
        }

        public override void Interpolate(TransformProperty lastLastState, TransformProperty lastState, TransformProperty nextState, float coef) {
            position = InterpolationFunctions.InterpolatePosition(lastLastState.position, lastState.position, nextState.position, coef);
            rotation = InterpolationFunctions.InterpolateRotation(lastState.rotation, nextState.rotation, coef);
        }
    }*/
}