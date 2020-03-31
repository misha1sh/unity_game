using System;
using System.Collections;
using Character.Guns;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;
using Physics = UnityEngine.Physics;

namespace Character.Actions {
    public abstract class ShootAction<T> : MonoBehaviour, IAction 
        where T: IGun {
        private CharacterAnimator animator;

        public T gun;

        void OnEnable() {
            if (gun != null) {
                gun.OnPickedUp(gameObject);
                EventsManager.handler.OnPlayerPickedUpGun(gameObject, gun);
            }
        }

        private void OnDisable() {
            if (gun != null) {
                gun.OnDropped();
                EventsManager.handler.OnPlayerDroppedGun(gameObject, gun);
            }
        }

       
        
        void Start() {
            animator = gameObject.GetComponent<CharacterAnimator>();
        }


        public abstract void OnStartDoing();

        public abstract void OnStopDoing();
    }
}