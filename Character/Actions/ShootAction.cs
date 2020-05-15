using Character.Guns;
using Events;
using GameMode;
using UnityEngine;

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
            if (GameManager.sceneReloaded) return;
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