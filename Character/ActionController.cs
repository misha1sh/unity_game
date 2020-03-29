using System;
using System.Collections;
using Character.Actions;
using Character.Guns;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Character {
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(PushAction))]
    public class ActionController : MonoBehaviour {
        
        private CharacterAnimator animator;
        void Start() {
            animator = GetComponent<CharacterAnimator>();
            SetPistolAction(new Pistol());
           //SetPushAction();
        }

        private IAction currentAction = null;

        
        private void StopCurrent() {
            if (currentAction != null) {
                DoAction = false;
                Destroy(currentAction as MonoBehaviour);
            }
        }
        
        public void SetPushAction() {
            StopCurrent();
            var pushAction = gameObject.AddComponent<PushAction>();
            currentAction = pushAction;
        }

        public void SetPistolAction(Pistol pistol) {
            StopCurrent();
            var shootPistolAction = gameObject.AddComponent<ShootPistolAction>();
            shootPistolAction.Init(pistol);
            currentAction = shootPistolAction;
        }

        public void SetNothing() {
            StopCurrent();
            currentAction = null;
        }
   /*     private ActionType _actionType;
        public ActionType ActionType {
            get => _actionType;
            set {
                if (currentAction != null)
                    (currentAction as Behaviour).enabled = false;
    
                switch (value) {
                    case ActionType.PUSH:
                        currentAction = GetComponent<PushAction>();
                        (currentAction as Behaviour).enabled = true;
                        break;
                    default:
                        throw new ArgumentException($"Unknown action type: {value}");
                }
                _actionType = value;
            }
        }*/

        private bool _actionDoing = false;
        public bool DoAction {
            get => _actionDoing;
            set {
                if (value == _actionDoing) return;
                _actionDoing = value;
                if (value) {
                    currentAction?.OnStartDoing();
                } else {
                    currentAction?.OnStopDoing();
                }
            }
        }

  

        void Update() {
            
        }
    }
}