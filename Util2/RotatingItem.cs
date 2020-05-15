using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util2 {
    public class RotatingItem : UnityEngine.MonoBehaviour {
        public float rotationSpeed = 70;
        public float upDownSpeed = 3f;
        public float upDownAmplitude = 0.25f;
        private float phase;

        private float startingY;

        public void Start() {
            startingY = transform.position.y;
            phase  = Random.value * Mathf.PI * 2;
            transform.Rotate(0, Random.value * 2 * Mathf.PI, 0);
        }
        
        public void Update() {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            var pos = transform.localPosition;
            pos.y = upDownAmplitude * Mathf.Sin(upDownSpeed * Time.time + phase);
            transform.localPosition = pos;
        }
    }
}