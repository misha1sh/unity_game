using System;
using Interpolation;
using UnityEngine;

namespace GameMechanics {
    public class ParabolaFlyingObject : MonoBehaviour {
        public Vector3 start;
        public Vector3 medium;
        public Vector3 stop;
        public float totalTime;

        public float startTime = -1f;

        public void Start() {
            startTime = Time.time;
        }

        private void OnCollisionEnter(Collision other) {
            Destroy(this);
        }

        public void Update() {
            float t = (Time.time - startTime) / totalTime;
            if (t > 1)
                t = 1;
            transform.position = InterpolationFunctions.BezierCurve(start, medium, stop, t);
          /*  if (t == 1) {
                Destroy(this);
            }*/
        }
    }
}