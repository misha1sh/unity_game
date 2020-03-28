using System.Collections;
using UnityEngine;

namespace Character.Actions {
    public abstract class ShootAction : MonoBehaviour, IAction {
        private CharacterAnimator animator;

        private RaycastHit _raycastRes;
        protected RaycastHit SimpleRaycast(Vector3 directionDelta) {
            var position = transform.position;
            var direction = transform.rotation * Vector3.forward + directionDelta;

            RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(position, direction, drawDuration: 0.3f,
                hitColor: Color.red, noHitColor: Color.white);

            Physics.Raycast(position, direction, out _raycastRes);
            return _raycastRes;
        }        
        
        //public void ShootWithDamage()
        
        void Start() {
            animator = gameObject.GetComponent<CharacterAnimator>();
        }
        
        
        public IEnumerator OnStartDoing() {
            throw new System.NotImplementedException();
        }

        public IEnumerator OnStopDoing() {
            throw new System.NotImplementedException();
        }
    }
}