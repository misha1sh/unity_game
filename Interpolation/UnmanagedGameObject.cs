using System;
using Interpolation.Properties;
using UnityEngine;

namespace Interpolation {
    public class UnmanagedGameObject<T> : MonoBehaviour
        where T : IGameObjectProperty, new() {
        private float timePerFrame = 1f / Client.NETWORK_FPS;

        
        private IGameObjectProperty lastlastState, lastState, state, nextState;
        private float lastMessageTime;

        void Init() {
            state = new T();
            state.FromGameObject(gameObject);
            
            lastState = new T();
            lastState.FromGameObject(gameObject);
            
            lastlastState = new T();
            lastlastState.FromGameObject(gameObject);
            
            lastMessageTime = 0;
        }
        
        
        


        private float P0P1InterpolationCoef = 1;
        private float P1P2InterpolationCoef = 1;
        public void SetStateAnimated(T state) {
            if (lastlastState is null) Init();
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
        

        public void SetStateInstant(T newState) {
            //transform.position = state.position;
            //transform.rotation = state.rotation;

            //lastlastState.FromGameObject(); = nextState = state;
            lastlastState.CopyFrom(newState);
            lastState.CopyFrom(newState);
            nextState.CopyFrom(newState);
            state.CopyFrom(newState);
            
            newState.ApplyToObject(gameObject);
        }

        void Update() {
            if (lastlastState is null) Init();

            if (lastMessageTime < 0) lastMessageTime = Time.realtimeSinceStartup - 
                                                       Math.Min(Time.deltaTime, timePerFrame); // endTime: lastMessageTime + timePerFrame
            // (beginTime, endTime]

            var interpolationTime = Time.realtimeSinceStartup  - lastMessageTime;
            if (interpolationTime > timePerFrame) {
                var waitTime = Math.Round((interpolationTime - timePerFrame) * 1000);
                // TODO adaptive correct
                //     Debug.LogWarning($"Where is message? Waiting {waitTime} msec.");
                //  return;
            }


            
            var coef = interpolationTime / timePerFrame;
            
            P1P2InterpolationCoef = coef;

           
           state.Interpolate(lastlastState, lastState, nextState, coef);
           state.ApplyToObject(gameObject);
        }
    }
}