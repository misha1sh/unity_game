using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.Assertions;
using CharacterController = Character.CharacterController;

public class AIController : CharacterController {


    void Update()
    {
        if (Random.value < 0.01f) {
            var dir = new Vector3(Random.value * 2 - 1, 0, Random.value * 2- 1);
            motionController.TargetDirection = dir;
            var rot = Random.rotation * Vector3.forward;
            rot.y = 0;
            motionController.TargetRotation = rot;

            actionController.DoAction = Random.value < 0.2f;
        }
    }
}
