using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character {
    
    [RequireComponent(typeof(CharacterAnimator))]
    public class GhostController : MonoBehaviour {


        private CharacterAnimator animator;

        void Start() {
            animator = GetComponent<CharacterAnimator>();
            

        }





          


        
       // float lastUpdateTime
        void Update() {
         /*   if (lastMessageTime < 0) lastMessageTime = Time.realtimeSinceStartup - 
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

           
            var rot = Quaternion.Lerp(lastState.rotation, nextState.rotation, coef);
            transform.position = pos;
            transform.rotation = rot;*/
        }
    }
}