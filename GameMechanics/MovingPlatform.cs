using System;
using UnityEngine;
using Util2;

namespace GameMechanics {
    
    
    public class MovingPlatform : MonoBehaviour {

        
        public Transform nextTransform;

        private Vector3 lastPosition;
        private Vector3 nextPosition;
        
        public float speed = 0.1f;
        public float stayTime = 2;
        
        public int state = STAY_STATE;

        private const int MOVE_STATE = 0;
        private const int STAY_STATE = 1;

        private Rigidbody rigidbody;
        private void Start() {
            lastPosition = transform.position;
            nextPosition = nextTransform.position;
            rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.GetComponent<Rigidbody>() != null && other.transform.position.y > transform.position.y)
                other.transform.parent = transform;
        }

        private void OnCollisionExit(Collision other) {
            if (other.transform.parent == transform)
                other.transform.parent = null;
        }

        private float currentStayingTime = -100;
        public void Update() {
            if (state == MOVE_STATE) {
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed);
                if (transform.position == nextPosition) {
                    state = STAY_STATE;
                    currentStayingTime = stayTime;
                }
              //  rigidbody.velocity = (((nextPosition - lastPosition).normalized  * speed ));
            } else if (state == STAY_STATE) {
                currentStayingTime -= Time.deltaTime;
                if (currentStayingTime < 0) {
                    gUtil.Swap(ref lastPosition, ref nextPosition);
                    state = MOVE_STATE;
                    // send start moving command
                }
            }
            
        }
    }
}