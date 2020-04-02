using System;
using System.Collections;
using System.Collections.Generic;
using Interpolation;
using UnityEngine;

namespace Character {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HandAnimationBlender))]
    public class CharacterAnimator : MonoBehaviour {
        private Animator animator;
        private HandAnimationBlender handAnimationBlender;


    //    private Vector3 lastPosition;
        private static readonly int Idle = Animator.StringToHash("idle");
        private static readonly int Speed = Animator.StringToHash("speed");
        private static readonly int Push = Animator.StringToHash("push");
        private static readonly int RotationSpeed = Animator.StringToHash("rotationSpeed");

        private PlayerAnimationState animationState = new PlayerAnimationState();
        
        void Start() {
            animator = GetComponent<Animator>();
            handAnimationBlender = GetComponent<HandAnimationBlender>();
      //      lastPosition = transform.position;
        }


        /*  public GameObjectState GetState() {
              var state = new GameObjectState();
              state.id = ObjectID.GetID(gameObject); // TODO : cache
              state.position = transform.position;
              state.rotation = transform.rotation;
              return  new GameObjectState();
          }*/

        
        
        public void SetIdle(bool idle) {
            animator.SetBool(Idle, idle);
            animationState.idle = idle;
        }
        
        public void SetSpeed(float speed) {
            animator.SetFloat(Speed, speed);
            animationState.speed = speed + 10;
        }

        public void SetPush() {
            animator.SetBool(Push, true);
        }

        public void SetRotationSpeed(float rotationSpeed) {
            animator.SetFloat(RotationSpeed, 0);
            animationState.rotationSpeed = rotationSpeed;
        }

        public PlayerAnimationState GetAnimationState() {
            return animationState;
        }

        public void SetAnimationState(PlayerAnimationState state) {
            animationState = state;
            SetIdle(state.idle);
            SetSpeed(state.speed);
            SetRotationSpeed(state.rotationSpeed);
        }
        
        
     /*   void LateUpdate() {
            //   animator.SetBool("push", false);
            //var deltaPos = transform.position - lastPosition;


              float linearSpeed = deltaPos.magnitude / Time.deltaTime;
              animator.SetBool("idle", linearSpeed < 0.001f);
              animator.SetFloat("speed", linearSpeed);

            //lastPosition = transform.position;
        }*/
    }
}
