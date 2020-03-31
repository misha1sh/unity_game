using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;
using Util2;
using Physics = UnityEngine.Physics;

namespace Character.Guns {
    public static class ShootSystem {


        
        public static void DrawTracer(Vector3 start, Vector3 stop, float width = 0.1f) {
            Client.client.TrailRenderer.MoveFromTo(start, stop);
        }
        
        public static bool SimpleRaycast(Transform transform, Vector3 directionDelta, out RaycastHit raycastRes) {
            var position = transform.position + Vector3.up * 1.5f;
            var direction = transform.rotation * (Vector3.forward + directionDelta);

            /*RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(position, direction, drawDuration: 0.1f,
                hitColor: Color.red, noHitColor: Color.white, preview: PreviewCondition.Both);*/
            

            bool rres = Physics.Raycast(position, direction, out raycastRes);
            DrawTracer(transform.position, raycastRes.point);

            return rres;
        }

        private static RaycastHit _raycastHit;
        public static bool ShootWithDamage(GameObject gameObject, Vector3 directionDelta, float damage) {
            var raycastRes = SimpleRaycast(gameObject.transform, directionDelta,  out _raycastHit);
            if (raycastRes == false) return false;
            
            var other = _raycastHit.collider.gameObject;
            var hp = other.GetComponent<HPController>();

            if (hp == null) return false;

            hp.TakeDamage(damage, DamageSource.Player(gameObject)); 
            
            return true;
        }

        public static Vector3 RandomDelta(double sigma) {
            float x = (float) Client.client.random.NextGaussian(0, sigma);
            float y = (float) Client.client.random.NextGaussian(0, sigma);
            return new Vector3(x, y, 0);
        }
        
        /*public static bool ShootRandomWithDamage(GameObject gameObject, float randomWidth, float randomDeep,
            float damage) {
            float randompoi
        }*/
    }
}