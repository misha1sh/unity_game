using System;
using Character;
using Interpolation;
using Interpolation.Properties;
using UnityEngine;

namespace CommandsSystem.Commands {
    public partial class PlayerProperty : GameObjectProperty<PlayerProperty> {
        public int id;
        public Vector3 position;
        public Quaternion rotation;
        public PlayerAnimationState animationState;

        private CharacterAnimator characterAnimator;
        
        public override void CopyFrom(PlayerProperty state) {
            id = state.id;
            position = state.position;
            rotation = state.rotation;
        }

        public override void FromGameObject(GameObject gameObject) {
            id = ObjectID.GetID(gameObject);
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
        
        public override void Run()
        {
            var gameObject = ObjectID.GetObject(id);
            if (gameObject is null)
            {
                var spawnCommand = new SpawnPrefabCommand {
                    id = this.id, 
                    position = position, 
                    rotation = rotation, 
                    prefabName = "RobotGhost"
                };

      
                
                
                gameObject = Client.client.SpawnObject(spawnCommand);
            }

            var controller = gameObject.GetComponent<UnmanagedGameObject<PlayerProperty>>();
            if (controller is null) return;
            controller.SetStateAnimated(this);

            /*    GhostController controller = character.GetComponent<GhostController>();
                if (controller is null) return;
                controller.SetStateAnimated(state);
    */

            /// Assert. (!character.CompareTag("EntityPlayer")) return;
        }
    }
    
    
    

    
}