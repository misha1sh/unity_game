using UnityEngine;

namespace Interpolation {
    public class UnmanagedGameObject<T> : MonoBehaviour
        where T: GameObjectState, new() {
        private float timePerFrame = 1f / Client.NETWORK_FPS;

        
        private T lastlastState, lastState, state, nextState;
        private float lastMessageTime;

        void Start() {
            state = new T();
            state.FromGameObject(gameObject);
            
            lastlastState = lastState = nextState = state;
            lastMessageTime = 0;
        }
        


        private float P0P1InterpolationCoef = 1;
        private float P1P2InterpolationCoef = 1;
        public void SetStateAnimated(T state) {
  /*          if (animator is null)
            {
                animator = GetComponent<CharacterAnimator>();
            }*/
            Debug.Log("got state " + state);

            lastlastState = lastState;
            lastState.FromGameObject(gameObject);
            nextState = state;
            
            lastMessageTime = -1f;
            P0P1InterpolationCoef = P1P2InterpolationCoef;
            //  lastMessageTime = Time.realtimeSinceStartup;// + Time.deltaTime;            // TODO? skip one frame
        }
        

        public void SetStateInstant(T state) {
            //transform.position = state.position;
            //transform.rotation = state.rotation;

            //lastlastState.FromGameObject(); = nextState = state;
            lastlastState = state;
            
        }
        
    }
}