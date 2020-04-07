using System;
using Interpolation.Properties;
using UnityEngine;

namespace Interpolation {
    public class UnmanagedGameObject<T> : MonoBehaviour
        where T : IGameObjectProperty, new() {

        private class Data {
            public IGameObjectProperty s;
            public float timeSinceLast;

            public Data(IGameObjectProperty property, float timeSinceLast) {
                this.s = property;
                this.timeSinceLast = timeSinceLast;
            }
        }
        
        
        private float timePerFrame = 1f / Client.NETWORK_FPS;

        
        private Data lastlastState, lastState, nextState;
        private IGameObjectProperty state;
        private Data nextNextState; // apply it in next time
        private float lastMessageTime;

        void Init() {
            lastlastState = lastState = nextState = new Data(null, 1);
            state = new T();
            /*  state.s = new T();
              state.s.FromGameObject(gameObject);
              
              lastState = new T();
              lastState.s.FromGameObject(gameObject);
              
              lastlastState = new T();
              lastlastState.s.FromGameObject(gameObject);
  
              nextState = new T();
              nextState.FromGameObject(gameObject);
              
              lastMessageTime = 0;
  
              nextNextState = null;*/
        }
        
        
        


        private float P0P1InterpolationCoef = 1;
        private float P1P2InterpolationCoef = 1;

        private void SwitchToNextState() {
          /*  lastlastState.CopyFrom(lastState);
            lastState.FromGameObject(gameObject);
            nextState.CopyFrom(nextNextState);

            nextNextState = null;*/
            lastlastState = lastState;
            lastState = nextState;
            nextState = nextNextState;
            nextNextState = null;
            
            
            lastMessageTime = Time.realtimeSinceStartup;//-1f;
            P0P1InterpolationCoef = P1P2InterpolationCoef;
        }


        private float last_time = -1f;
        public void SetStateAnimated(T newState, float deltaSinceLast) {
            if (lastlastState is null) Init();
            nextNextState = new Data(newState, deltaSinceLast);
            Debug.LogWarning($"Time {(int)((Time.realtimeSinceStartup-last_time) * 1000)} msec. {(int)(deltaSinceLast*1000)}");
            last_time = Time.realtimeSinceStartup;
            /*          if (animator is null)
                      {
                          animator = GetComponent<CharacterAnimator>();
                      }*/
            //Debug.Log("got state " + state);

            /*   lastlastState.CopyFrom(lastState);
               lastState.FromGameObject(gameObject);
               nextState.CopyFrom(state);
               
               lastMessageTime = -1f;
               P0P1InterpolationCoef = P1P2InterpolationCoef;*/
            //  lastMessageTime = Time.realtimeSinceStartup;// + Time.deltaTime;            // TODO? skip one frame
        }
        
        
/*
        public void SetStateInstant(T newState) {
            if (lastlastState is null) Init();
            //transform.position = state.position;
            //transform.rotation = state.rotation;

            //lastlastState.FromGameObject(); = nextState = state;
            lastlastState.CopyFrom(newState);
            lastState.CopyFrom(newState);
            nextState.CopyFrom(newState);
            state.CopyFrom(newState);
            
            newState.ApplyToObject(gameObject);
        }
*/
        private void Animate(float delta) {
           /* if (lastMessageTime < 0) lastMessageTime = Time.realtimeSinceStartup;/* - 
                                                       Math.Min(Time.deltaTime, timePerFrame)*/; // endTime: lastMessageTime + timePerFrame
            // (beginTime, endTime]

           // var interpolationTime = Time.realtimeSinceStartup  - lastMessageTime;
            if (interpolationTime > nextState.timeSinceLast) {
                
                // TODO adaptive correct
                if (nextNextState == null) {
                    var waitTime = Math.Round((interpolationTime - nextState.timeSinceLast) * 1000);
                    Debug.LogWarning($"Where is message? Waiting {waitTime} msec.");
                    return;
                }
                SwitchToNextState();
                Animate();
                return;
            }


            
            var coef = interpolationTime / nextState.timeSinceLast;
            
            P1P2InterpolationCoef = coef;

           
            state.Interpolate(lastlastState.s, lastState.s, nextState.s, coef);
            state.ApplyToObject(gameObject);
        }

        void Update() {
            if (lastlastState is null) Init();
            Animate();
            
        }
    }
}