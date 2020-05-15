using System;
using Character.Actions;
using Character.Guns;
using UnityEngine;

namespace Character {


    
    
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(PushAction))]
    [RequireComponent(typeof(ShootPistolAction))]
    [RequireComponent(typeof(ShootSemiautoAction))]
    public class ActionController : MonoBehaviour {
        
        private CharacterAnimator animator;
        void Start() {
            animator = GetComponent<CharacterAnimator>();
           //    SetAction<ShootPistolAction>(action => action.gun = new BombGun());
         //      SetAction<ShootSemiautoAction>(action => action.gun = new SemiautoGun());
         //    SetAction<ShootPistolAction>(action => action.gun = new ShotGun());
          //  SetAction<ShootPistolAction>(action => action.gun = new Pistol());
            //SetAction<PushAction>(action => { });
        }

        private IAction currentAction = null;
        public Vector3 Target; // mouse position in world coordinates
        
        private void StopCurrent() {
            if (currentAction != null) {
                DoAction = false;
                (currentAction as MonoBehaviour).enabled = false;
            }
        }

        
        public void SetAction<T>(System.Action<T> setup)
            where T: MonoBehaviour, IAction {
            StopCurrent();
            var action = gameObject.GetComponent<T>();

            setup(action);
            
            action.enabled = true;
            currentAction = action;
        }
        
 /*       public void SetPushAction() {
            SetAction<PushAction>();
        }

        public void SetPistolAction(Pistol pistol) {
            StopCurrent();
            var shootPistolAction = gameObject.GetComponent<ShootPistolAction>();
            shootPistolAction.gun = pistol;

            shootPistolAction.enabled = true;
            currentAction = shootPistolAction;
        }*/

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

    /*   private void OnDrawGizmos() {
            //Gizmos.DrawSphere(Target, 0.1f);
            DebugExtension.DebugPoint(Target, Color.magenta, 3, 10);
        }*/
    }
}