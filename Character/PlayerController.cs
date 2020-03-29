using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using CharacterController = Character.CharacterController;


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


public class PlayerController : CharacterController
{
    
    public Camera camera;
    private Plane plane;

    void Start()
    {
        base.Start();
        Assert.IsNotNull(camera);
        plane = new Plane(Vector3.up, 0);
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
       
       motionController.TargetDirection = vec3;

       var ray = camera.ScreenPointToRay(Input.mousePosition);

       var target_pos = target.transform.position + Vector3.up * 1.5f;
       plane.SetNormalAndPosition(Vector3.up, target_pos);

       float distance;
       plane.Raycast(ray, out distance);
       var pos = ray.GetPoint(distance);
       /*DebugExtension.DebugPoint(pos, Color.red, 3f);
       DebugExtension.DebugArrow(target_pos, pos - target_pos, Color.magenta);*/
       motionController.TargetRotation = pos - target_pos;
     
       /*  Vector3 tttest = Input.mousePosition;
       tttest.z = 10;
       var pos = camera.ScreenToWorldPoint(tttest);
    //   var rotation = camera.transform.rotation
//       var pos2 = ().//camera.transform.position + camera.transform.forward * 10;
       DebugExtension.DebugPoint(pos, Color.red, 3f);
  //     DebugExtension.DebugPoint(pos2, Color.magenta, 3f);

        var pos3 = Quaternion.Inverse()
       
       motionController.TargetRotation = pos - pos2;

/*
    
  */
        
        /*

       if (Input.GetMouseButton(0)) {
           GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = pos;
           
       }
       
       pos.x -= 0.5f;
       pos.y -= 0.5f;
       pos.z = pos.y;
       pos.y = 0;

       pos = camera.transform.rotation * pos;
       pos.y = 0;
       motionController.TargetRotation = pos;*/


       actionController.DoAction = Input.GetMouseButton(0);
//       var rot = Input.mousePosition;
//       target.TargetRotation = 
    }
}
