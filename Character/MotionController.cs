using CommandsSystem.Commands;
using System;
using System.Collections.Generic;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.PlayerLoop;
using Physics = UnityEngine.Physics;
using Random = UnityEngine.Random;

namespace Character {



//    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterAnimator))]
    public class MotionController : MonoBehaviour {
//        private CharacterController characterController;
        private Rigidbody rigidbody;
        private CapsuleCollider capsuleCollider;
        private Animator animator;
        
        public float moveForce = 1500;
        public float speed = 6.0f;
        public float rotationSpeed = 150.0f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public float maxGoAngle = 50.0f;


        void Start() {
            ObjectID.StoreObject(gameObject, Client.client.random.Next());


            //            characterController = GetComponent<CharacterController>();
            rigidbody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            animator = GetComponent<Animator>();
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
            DeGround(other.gameObject);
        }
        private bool isGrounded = true;
        

        /// <summary>
        ///     from -1 to 1
        /// </summary>
        public Vector3 TargetDirection { get; set; }
        public Vector3 TargetRotation { get; set; }
        
        public bool Action1 { get; set; }

        private void FixedUpdate()
        {
            // TODO: ffix

        
            
        }


        void Update() {



            animator.SetBool("push", false);

            if (isGrounded) {
                var targetSpeed = speed * TargetDirection;
                var bodySpeed = rigidbody.velocity;

                var vec = bodySpeed - targetSpeed;
                vec.y = 0;
                if (vec.magnitude > 1) {
                    vec.Normalize();
                    rigidbody.AddForce(-vec * moveForce);

                   /* var angle = Vector3.SignedAngle(vec, Vector3.forward, Vector3.up);
                    Debug.Log(angle);
                    animator.SetFloat("rotationSpeed", angle);*/
                   
                 /*  var angle = Vector3.SignedAngle(vec, Vector3.forward, Vector3.up);
                   Debug.Log(angle / Math.Abs(angle));
                   
                   transform.Rotate(0, 1, 0);*/
                // transform.rotation = Quaternion.LookRotation(TargetDirection);
                } else {
                    animator.SetFloat("rotationSpeed", 0);
                }
            }

            if (TargetRotation != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(TargetRotation);

            
            if (Input.GetMouseButtonDown(0)) {
                animator.SetBool("push", true);
            }

            float linearSpeed = TargetDirection.magnitude;
            animator.SetBool("idle", linearSpeed == 0f);
            animator.SetFloat("speed", linearSpeed);






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


        public GameObject pushCollider;
        public void pushEnd() {
            var center = pushCollider.transform.position;
            var scale = pushCollider.transform.localScale;
            var rotation = pushCollider.transform.rotation;

            var delta = rotation * Vector3.up;
            delta.Scale(scale);
            
            var radius = scale.x * transform.lossyScale.x / 2;

            delta -= rotation * (radius * Vector3.up);
            
            var start = center - delta;
            var stop = center + delta;
 
         //   DebugExtension.DebugCapsule(start, stop, Color.red, radius, 1);

         var f = /*Physics.CapsuleCastAll(start, stop, radius, Vector3.forward);//*/
             RotaryHeart.Lib.PhysicsExtension.Physics.OverlapCapsule(start, stop, radius, PreviewCondition.Both, drawDuration: 1);
         foreach (var v in f) {
             Debug.LogError(v.gameObject.name);
             var rig = v.gameObject.GetComponent<Rigidbody>();
             if (rig != null) {
                 rig.AddForce(rotation * Vector3.up * 500);
             }
         }
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
