using System;
using Character;
using Interpolation;
using Interpolation.Properties;
using UnityEngine;

namespace CommandsSystem.Commands {
    public class PlayerProperty : GameObjectProperty<PlayerProperty> {
        public int id;


      //  public int parentTransform;
        public Vector3 position;
        public Quaternion rotation;
        public PlayerAnimationState animationState;

        private CharacterAnimator characterAnimator;
        
        public override void CopyFrom(PlayerProperty state) {
            id = state.id;
            position = state.position;
            rotation = state.rotation;
            animationState.idle = state.animationState.idle;
            animationState.speed = state.animationState.speed;
            animationState.rotationSpeed = state.animationState.rotationSpeed;
            //    parentTransform = state.parentTransform;
        }

        public override void FromGameObject(GameObject gameObject) {
            id = ObjectID.GetID(gameObject);
            position = gameObject.transform.position;
            rotation = gameObject.transform.rotation;
        /*   if (gameObject.transform.parent == null) {
                parentTransform = 0;
            } else {
                parentTransform = ObjectID.GetID()
            }*/
            if (characterAnimator is null) characterAnimator = gameObject.GetComponent<CharacterAnimator>();
            animationState = characterAnimator.GetAnimationState();
        }

        public override void ApplyToObject(GameObject gameObject) {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            if (characterAnimator is null) characterAnimator = gameObject.GetComponent<CharacterAnimator>();
            characterAnimator.SetAnimationState(animationState);
        }

        public override void Interpolate(PlayerProperty lastLastState, PlayerProperty lastState, PlayerProperty nextState, float coef) {
            position = InterpolationFunctions.InterpolatePosition(lastLastState.position, lastState.position, nextState.position, coef);
            rotation = InterpolationFunctions.InterpolateRotation(lastState.rotation, nextState.rotation, coef);
            animationState =
                InterpolationFunctions.InterpolatePlayerAnimationState(lastState.animationState,
                    nextState.animationState, coef);
        }
        
       
    }
    
    
    

    
}