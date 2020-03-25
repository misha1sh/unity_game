using System;
using Character;
using UnityEngine;

namespace Interpolation.Properties {
    [Serializable]
    public class PlayerProperty : GameObjectProperty<PlayerProperty> {
        public Vector3 position;
        public Quaternion rotation;

        public PlayerAnimationState animationState;

        [NonSerialized]
        private CharacterAnimator characterAnimator;
        
        public override void CopyFrom(PlayerProperty state) {
            position = state.position;
            rotation = state.rotation;
        }

        public override void FromGameObject2(GameObject gameObject) {
            position = gameObject.transform.position;
            rotation = gameObject.transform.rotation;
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