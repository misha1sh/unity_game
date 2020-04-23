using CommandsSystem.Commands;
using Networking;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;

namespace Character.Actions {
    [RequireComponent(typeof(CharacterAnimator))]
    public class PushAction : MonoBehaviour, IAction {

        public GameObject pushCollider;
        public float force = 3300;

        
        private CharacterAnimator animator;

        void Start() {
            animator = gameObject.GetComponent<CharacterAnimator>();
        }

        public void OnStartDoing() {
            animator.SetPush();
            CommandsHandler.gameRoom.RunSimpleCommand(new PlayerPushCommand(ObjectID.GetID(gameObject)), MessageFlags.NONE);
        }

        public void pushEnd() {
            var center = pushCollider.transform.position;
            var scale = pushCollider.transform.localScale;
            var rotation = pushCollider.transform.rotation;

            var delta = rotation * Vector3.up * scale.y;
            //delta.Scale(scale);
            
            var radius = scale.x * transform.lossyScale.x / 2;

            delta -= rotation * (radius * Vector3.up);
            
            var start = center - delta;
            var stop = center + delta;
 
            //   DebugExtension.DebugCapsule(start, stop, Color.red, radius, 1);

            var f = /*Physics.CapsuleCastAll(start, stop, radius, Vector3.forward);//*/
                RotaryHeart.Lib.PhysicsExtension.Physics.OverlapCapsule(start, stop, radius, PreviewCondition.Both, drawDuration: 1);

            var force = rotation * Vector3.up * this.force;
            foreach (var v in f) {
                if (v.gameObject == gameObject) continue;
             
                if (v.gameObject.CompareTag("Unmanagable")) {
                    var command = new ApplyForceCommand(v.gameObject, force);
                    CommandsHandler.gameRoom.RunSimpleCommand(command, MessageFlags.NONE);
                } else {
                    var rig = v.gameObject.GetComponent<Rigidbody>();
                    if (rig != null) {
                        rig.AddForce(force, ForceMode.Impulse);
                    }
                }
                /*      Debug.LogError(v.gameObject.name);
                      var rig = v.gameObject.GetComponent<Rigidbody>();
                      if (rig != null) {
                          rig.AddForce(force);
                      }*/
            }
        }

        public void OnStopDoing() {
        }
    }
}