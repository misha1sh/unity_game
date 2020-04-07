using System;
using CommandsSystem.Commands;
using UnityEngine;
using Util2;

namespace GameMechanics {
    
    
    public class MovingPlatform : MonoBehaviour, IOwnedEventHandler  {

        
        public Transform nextTransform;

        private Vector3 lastPosition;
        private Vector3 nextPosition;
        
        public float speed = 10f;
        public float stayTime = 2;
        
        public int state = STAY_STATE;
        private const int MOVE_STATE = 0;
        private const int STAY_STATE = 1;
        private const int WAITING_FOR_COMMAND = 2;
        
        public int direction = DIRECTION_LAST_TO_NEXT;
        private const int DIRECTION_LAST_TO_NEXT = 0;
        private const int DIRECTION_NEXT_TO_LAST = 1;


        private int id;
        private Rigidbody rigidbody;
        private void Start() {
            lastPosition = transform.position;
            nextPosition = nextTransform.position;
            rigidbody = GetComponent<Rigidbody>();
            id = ObjectID.GetID(gameObject);
            Client.client.commandsHandler.RunSimpleCommand(new TakeOwnCommand(id, Client.client.ID));
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.GetComponent<Rigidbody>() != null && other.transform.position.y > transform.position.y)
                other.transform.parent = transform;
        }

        private void OnCollisionExit(Collision other) {
            if (other.transform.parent == transform)
                other.transform.parent = null;
        }

        public void SetMoveState(int direction) {
            if (direction != this.direction) {
                gUtil.Swap(ref lastPosition, ref nextPosition);
            }

            if (direction == this.direction || state == MOVE_STATE)
                transform.position = lastPosition;
            /*if (state == MOVE_STATE)
                transform.position = lastPosition;*/
            this.direction = direction;
            this.state = MOVE_STATE;
        }

        private float currentStayingTime = -100;
        public void Update() {
            if (state == MOVE_STATE) {
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed*Time.deltaTime);
                if (transform.position == nextPosition) {
                    state = STAY_STATE;
                    currentStayingTime = stayTime;
                }
                //  rigidbody.velocity = (((nextPosition - lastPosition).normalized  * speed ));
            } else if (state == STAY_STATE) {
                currentStayingTime -= Time.deltaTime;
                if (currentStayingTime < 0) {
                    if (ObjectID.IsOwned(id)) {
                        Client.client.commandsHandler.RunSimpleCommand(new StartMovePlatform(id, 1 - direction));
                    }

                    state = WAITING_FOR_COMMAND;
                    // gUtil.Swap(ref lastPosition, ref nextPosition);
                    //  state = MOVE_STATE;
                    //       Client.client.commandsHandler.RunSimpleCommand();
                }
            }
            
        }

        public void HandleOwnTaken(int owner) {
            if (ObjectID.IsOwned(gameObject)) {
                if (state == WAITING_FOR_COMMAND)
                    state = STAY_STATE;
            }
        }
    }
}