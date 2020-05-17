using UnityEngine;
using CharacterController = Character.CharacterController;

/// <summary>
///     Класс для игрока, управлемого искуственным интеллектом
/// </summary>
public class AIController : CharacterController {

    /// <summary>
    ///     Изменяет параметры игрока на основе ИИ. Автоматически вызывается Unity каждый кадр
    /// </summary>
    void Update()
    {
        if (false && Random.value < 0.01f) {
            var dir = new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1);
            motionController.TargetDirection = dir;
            var rot = Random.rotation * Vector3.forward;
            rot.y = 0;
            motionController.TargetRotation = rot;

            actionController.DoAction = Random.value < 0.2f;
        }
    }
}
