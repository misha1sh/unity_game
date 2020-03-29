using System.Collections;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;
using Physics = UnityEngine.Physics;

namespace Character.Actions {
    public abstract class ShootAction : MonoBehaviour, IAction {
        private CharacterAnimator animator;

        protected bool SimpleRaycast(Vector3 directionDelta, out RaycastHit raycastRes) {
            var position = transform.position + Vector3.up * 1.5f;
            var direction = transform.rotation * Vector3.forward + directionDelta;

            RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(position, direction, drawDuration: 10,
                hitColor: Color.red, noHitColor: Color.white, preview: PreviewCondition.Both);

            return Physics.Raycast(position, direction, out raycastRes);
        }

        private RaycastHit _raycastHit;
        public bool ShootWithDamage(Vector3 directionDelta, float damage) {
            var raycastRes = SimpleRaycast(directionDelta,  out _raycastHit);
            if (raycastRes == false) return false;
            
            var other = _raycastHit.collider.gameObject;
            var hp = other.GetComponent<HPController>();

            if (hp == null) return false;

            hp.TakeDamage(damage, DamageSource.Player(gameObject)); 
            
            return true;
        }
        
        void Start() {
            animator = gameObject.GetComponent<CharacterAnimator>();
        }


        public abstract void OnStartDoing();

        public abstract void OnStopDoing();
    }
}