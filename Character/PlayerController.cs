using System;
using UnityEngine;
using CharacterController = Character.CharacterController;

/// <summary>
///     Класс с дополнительными функциями для Vector2
/// </summary>
public static class Vector2Extension {
    /// <summary>
    ///     Поворачиает вектор
    /// </summary>
    /// <param name="v">Вектор</param>
    /// <param name="degrees">Угол в градусах</param>
    /// <returns>Повернутый вектор</returns>
    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
         
        float tx = v.x;
        float ty = v.y;
 
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}

/// <summary>
///     Компонента для персонажа, управляемого человеком
/// </summary>
public class PlayerController : CharacterController {
    /// <summary>
    ///     Переменная для внутреннего использования (нужна для уменьшения нагрузки на сборщик мусора)
    /// </summary>
    private Plane plane;

    /// <summary>
    ///     Инициализирует переменные
    /// </summary>
    protected override void Start()
    {
        base.Start();
        plane = new Plane(Vector3.up, 0);
        transform.parent = null;
    }

    /// <summary>
    ///     Управляет персонажем в соответвии с положением мышки и нажатыми клавишами. Автоматически вызывается Unity каждый кадр
    /// </summary>
    void Update() {
        if (sClient.isTyping)
           Input.ResetInputAxes();

        Vector2 vec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
       var len = 1;
       
       var vec3 = new Vector3(vec.x, 0, vec.y);
       vec3 = Camera.main.transform.rotation * vec3;
       vec3.y = 0;
       vec3 = vec3.normalized * len;
       
       motionController.TargetDirection = vec3;

       var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       var target_pos = target.transform.position + Vector3.up * 1.5f;
       plane.SetNormalAndPosition(Vector3.up, target_pos);

       float distance;
       plane.Raycast(ray, out distance);
       var pos = ray.GetPoint(distance);
       motionController.TargetRotation = pos - target_pos;
       
       
       plane.SetNormalAndPosition(Vector3.up, target.transform.position);
       plane.Raycast(ray, out distance);
       actionController.Target = ray.GetPoint(distance);

       actionController.DoAction = Input.GetMouseButton(0);
    }
}
