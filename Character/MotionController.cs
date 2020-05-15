using System.Collections.Generic;
using Character.HP;
using UnityEngine;

namespace Character {

    

//    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterAnimator))]
    public class MotionController : MonoBehaviour {
//        private CharacterController characterController;
        private new Rigidbody rigidbody;
        private CapsuleCollider capsuleCollider;
        private CharacterAnimator animator;
        
        public float moveForce = 4000;
        public float speed = 6.0f;
        public float rotationSpeed = 700.0f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public float maxGoAngle = 50.0f;


        

        void Start() {


            //            characterController = GetComponent<CharacterController>();
            rigidbody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            animator = GetComponent<CharacterAnimator>();
            
            
        }

        private List<GameObject> groundCollisions = new List<GameObject>();

        private void DeGround(GameObject gameObject) {
            if (!groundCollisions.Remove(gameObject)) return;
            isGrounded = groundCollisions.Count != 0;
            //            Debug.Log((isGrounded ? "still grounded" : "lose ground ") + gameObject.name);
        }

        private void OnTriggerStay(Collider other) {
            if (!groundCollisions.Contains(other.gameObject)) {
                groundCollisions.Add(other.gameObject);
                isGrounded = true;
            }
        }

        private void OnTriggerExit(Collider other) {
           // Debug.LogError(other.name);
            DeGround(other.gameObject);
        }
        private bool isGrounded = true;
        

        /// <summary>
        ///     from -1 to 1
        /// </summary>
        public Vector3 TargetDirection { get; set; }
        public Vector3 TargetRotation { get; set; }

      /*  private void FixedUpdate() {
         
        }
*/
        void FixedUpdate() {
            for (int i = 0; i < groundCollisions.Count; i++) {
                if (!groundCollisions[i]) {
                    DeGround(groundCollisions[i]);
                    break; // TODO
                }
            }

            if (transform.position.y < -15) {
                gameObject.GetComponent<HPController>().TakeDamage(100000, DamageSource.InstaKill(), true);
            }

            

           

            if (isGrounded) {
                var targetSpeed = speed * TargetDirection;
                var bodySpeed = rigidbody.velocity;

                var vec = bodySpeed - targetSpeed;
                vec.y = 0;
                if (vec.magnitude > 1) {
                    vec.Normalize();
                    if (rigidbody.velocity.sqrMagnitude > 5f) {// && //Mathf.Abs(rigidbody.velocity.sqrMagnitude - speed * speed) > 0.5f)
                        rigidbody.velocity = targetSpeed;                      
                    } else {
                        rigidbody.AddForce(-vec * moveForce);
                        Debug.Log("addforce");  
                    }
                       

                   /* var angle = Vector3.SignedAngle(vec, Vector3.forward, Vector3.up);
                    Debug.Log(angle);
                    animator.SetFloat("rotationSpeed", angle);*/

                   /*  var angle = Vector3.SignedAngle(vec, Vector3.forward, Vector3.up);
                     Debug.Log(angle / Math.Abs(angle));
                     
                     transform.Rotate(0, 1, 0);*/
                   // transform.rotation = Quaternion.LookRotation(TargetDirection);
                } else {
                    animator.SetRotationSpeed(0);
                }
            }

            /*if (TargetRotation != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(TargetRotation);*/
            if (TargetDirection != Vector3.zero)
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.LookRotation(TargetDirection),
                    rotationSpeed * Time.deltaTime);


            float linearSpeed = TargetDirection.magnitude;
            animator.SetIdle(linearSpeed == 0.0f);
            animator.SetSpeed(linearSpeed);





            //            rigidbody.AddForce(moveDirection);

            /* float curRotationSpeed = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
             animator.SetFloat("rotationSpeed", Input.GetAxis("Horizontal"));

             transform.Rotate(0, curRotationSpeed, 0);*/


            /*   int layerMask = 1 << 8;

               layerMask = ~layerMask;

               RaycastHit hit;
               Vector3 pos = transform.position + capsuleCollider.center;
               Vector3 rot = transform.rotation * Vector3.forward;
               if (Physics.Raycast(pos, rot, out hit, Mathf.Infinity, layerMask)) {
                   Debug.DrawRay(pos, rot * hit.distance, Color.yellow);
               } else {
                   Debug.DrawRay(pos, rot * 1000, Color.white);
               }*/





        }

        private void OnDrawGizmos() {

            if (!Application.isPlaying) return;

            var pos = transform.position + capsuleCollider.center;
            DebugExtension.DebugCircle(pos,
                Color.green, radius: 0.1f, depthTest: false);

            DebugExtension.DebugCircle(transform.position + Vector3.up * 2,
                isGrounded ? Color.red : Color.blue, radius: 0.1f);
            
            pos.y += 0.5f;

            DebugExtension.DebugArrow(pos, TargetDirection * 1f, Color.blue);
            DebugExtension.DebugArrow(pos, TargetRotation.normalized / 2, Color.red);

        }


    }
}




/*    


    private void OnTriggerStay(Collider collider) {
        var v = new Vector3(0, capsuleCollider.radius, 0) + transform.position;
        Vector3 p;
        if (collider is BoxCollider ||
            collider is SphereCollider ||
            collider is CapsuleCollider ||
            (collider is MeshCollider meshCollider &&
             meshCollider.convex)) {
            p = collider.ClosestPoint(v);
        }
        collider.
    }*/

    /*    private void OnCollisionStay(Collision other) {
            var v = new Vector3(0, capsuleCollider.radius, 0) + transform.position;
            Vector3 p;
            if (other.collider is BoxCollider ||
                other.collider is SphereCollider ||
                other.collider is CapsuleCollider ||
                (other.collider is MeshCollider meshCollider &&
                 meshCollider.convex)) {
                p = other.collider.ClosestPoint(v);
            } else {
                p = other.GetContact(0).point;
            }
            var downVec = p - v; //- (transform.position);//other.ClosestPoint(transform.position));
            CalcGroundedPoint(downVec, other.gameObject);
//             RaycastHit res;
//             if (!Physics.Raycast(v, Vector3.down, out res)) return;

        }
*/
    /*    private void CalcGroundedPoint(Vector3 downVec, GameObject gameObject) {

             var angle = Vector3.Angle(downVec, Vector3.down);
             

             if (angle < maxGoAngle) {
                 if (groundCollisions.Contains(gameObject)) return;
                 Debug.Log("gr " + downVec + " " + angle);
                 groundCollisions.Add(gameObject);
                 isGrounded = true;
                 Debug.Log("Grounded " + gameObject.name);
             } else {
                 Debug.Log("de " + downVec + " " + angle);

                 DeGround(gameObject);
             }
        }

         */
