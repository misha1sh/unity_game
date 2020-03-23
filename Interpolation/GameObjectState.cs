using System;
using CommandsSystem;
using CommandsSystem.Commands;
using Interpolation.Properties;
using UnityEngine;

namespace Interpolation {

    [Serializable]
    public class GameObjectState : IGameObjectProperty<GameObjectState> {

        public int id;

        
        [SerializeField]
        public bool Transform;
        
        private TransformProperty transformProperty;
        
        
        public GameObjectState Create() {
            return new GameObjectState();
        }

        public void CopyFrom(GameObjectState state) {
            transformProperty.CopyFrom(state.transformProperty);
        }

        public void FromGameObject(GameObject gameObject) {
            throw new NotImplementedException();
        }

        public void ApplyToObject(GameObject gameObject) {
            throw new NotImplementedException();
        }

        public void Interpolate(GameObjectState lastLastState, GameObjectState lastState, GameObjectState nextState, float coef) {
            throw new NotImplementedException();
        }

        public ICommand GetCommand() {
            return null;
        }
    }
    
    
/*
    [Serializable]
    public class GameObjectState {
        public int id;
        public Vector3 position;
        public Quaternion rotation;
        public float time;


        public GameObjectState() {}


        public void CopyFrom(GameObjectState state) {
            id = state.id;
            position = state.position;
            rotation = state.rotation;
            time = state.time;
        }
  
        public int ID => id;

        public void FromGameObject(GameObject gameObject) {
            id = ObjectID.GetID(gameObject);
            position = gameObject.transform.position;
            rotation = gameObject.transform.rotation;
            time = Client.client.GameTime;
        }

        public void ApplyToObject(GameObject gameObject) {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
        }

        public ICommand GetCommand() {
            return new ChangeGameObjectStateCommand<GameObjectState>(this);
        }


        public void Interpolate(GameObjectState lastLastState, GameObjectState lastState,
            GameObjectState nextState, float coef) {
            position = InterpolationFunctions.InterpolatePosition(
                lastLastState.position, lastState.position, nextState.position,
                coef);

            rotation = InterpolationFunctions.InterpolateRotation(
                lastState.rotation, nextState.rotation,
                coef);

            time = InterpolationFunctions.InterpolateFloat(
                lastState.time, nextState.time,
                coef);
        }
    }*/
}