using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundedChecker : MonoBehaviour {

    public float maxGoAngle = 50;
    
    
    public bool isGrounded { get; private set; }

    private List<GameObject> groundCollisions = new List<GameObject>();
/*
    private void OnTriggerEnter(Collider other) {
        Debug.Log();   
        
    }*/

   /* private void OnTriggerStay(Collider other) {
        var downVec = (transform.position - other.ClosestPoint(transform.position));
//            if (downVec.y == 0f) return;
//            downVec /= Math.Abs(downVec.y);
//            Debug.Log(downVec);
        var angle = Vector3.Angle(downVec, Vector3.down);
        Debug.Log(angle);
        if (angle < maxGoAngle) {
            groundCollisions.Add(other.gameObject);
            isGrounded = true;
            Debug.Log("Grounded " + other.gameObject.name);
        }
    }*/

    private void OnCollisionExit(Collision other) {
        groundCollisions.Remove(other.gameObject);
        isGrounded = groundCollisions.Count != 0;
        Debug.Log((isGrounded? "still grounded" : "lose ground ") + other.gameObject.name);

    }
}
