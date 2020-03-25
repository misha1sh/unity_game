using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.Assertions;


public static class Vector2Extension {
    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
         
        float tx = v.x;
        float ty = v.y;
 
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}


public class PlayerController : MonoBehaviour
{
    
    public MotionController target;
    public Camera camera;

    void Start()
    {
        Assert.IsNotNull(target);
        Assert.IsNotNull(camera);
    }

    
    
    void Update() {
       /* Vector2 vec = new Vector2( Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        var vec3 = new Vector3(vec.x, 0, vec.y);
        vec3 = camera.transform.rotation * vec3;
        vec3.y = 0;
        target.CurrentSpeed = vec.magnitude;
        if (vec3 != Vector3.zero)
            target.gameObject.transform.rotation = Quaternion.LookRotation(vec3);
        //        target.CurrentRotationSpeed = Vector2.Angle(vec, Vector2.right);*/

       Vector2 vec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
       var len = 1;
       
       var vec3 = new Vector3(vec.x, 0, vec.y);
       vec3 = camera.transform.rotation * vec3;
       vec3.y = 0;
       vec3 = vec3.normalized * len;
       
       target.TargetDirection = vec3;

       var pos = camera.ScreenToViewportPoint(Input.mousePosition);
       pos.x -= 0.5f;
       pos.y -= 0.5f;
       pos.z = pos.y;
       pos.y = 0;

       pos = camera.transform.rotation * pos;
       pos.y = 0;
       target.TargetRotation = pos;


       target.Action1 = Input.GetMouseButtonDown(0);
//       var rot = Input.mousePosition;
//       target.TargetRotation = 
    }
}
