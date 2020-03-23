using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.Assertions;

public class AIController : MonoBehaviour {

    public MotionController target;
    
    void Start()
    {}

    void Update()
    {
        if (Random.value < 0.01f) {
            var dir = new Vector3(Random.value * 2 - 1, 0, Random.value * 2- 1);
            target.TargetDirection = dir;
            var rot = Random.rotation * Vector3.forward;
            rot.y = 0;
            target.TargetRotation = rot;
        }
    }
}
