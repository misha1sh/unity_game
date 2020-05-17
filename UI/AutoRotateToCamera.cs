using UnityEngine;

/// <summary>
///     Компонента для автоматического поворота интерфейса к камере
/// </summary>
public class AutoRotateToCamera : MonoBehaviour {

    /// <summary>
    ///     Поворачивает объект к камере. Автоматически вызывается Unity каждый кадр
    /// </summary>
    private void LateUpdate() {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
