using System;
using Character.Actions;
using UnityEngine;

namespace Character {
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(PushAction))]
    public class ActionController : MonoBehaviour {
        
        private CharacterAnimator animator;
        void Start() {
            animator = GetComponent<CharacterAnimator>();
            ActionType = ActionType.PUSH;
        }
        
        private ActionType _actionType;
        private IAction currentAction = null;
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
        }


        private bool _actionDoing = false;
        public bool DoAction {
            get { return _actionDoing; }
            set {
                if (value == _actionDoing) return;
                _actionDoing = value;
                if (value) {
                    if (currentAction != null) 
                        StartCoroutine(currentAction.OnStartDoing());
                } else {
                    if (currentAction != null) 
                        StartCoroutine(currentAction?.OnStopDoing());
                }
            }
        }

  

        void Update() {
            
        }
    }
}