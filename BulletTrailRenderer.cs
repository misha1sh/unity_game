using UnityEngine;

/// <summary>
///     Компонента для отрисовки следа от пули
/// </summary>
public class BulletTrailRenderer : MonoBehaviour {

    /// <summary>
    ///     Является ли данная компонента главной/ от которой все копируются
    /// </summary>
    private bool main = true;
    /// <summary>
    ///     Скорость
    /// </summary>
    public float speed = 10;
    /// <summary>
    ///     Первая позиция
    /// </summary>
    public Vector3 v1;
    /// <summary>
    ///     Вторая позиция
    /// </summary>
    public Vector3 v2;

    /// <summary>
    ///     Создаёт след от пули
    /// </summary>
    /// <param name="v1">Стартовая позиция</param>
    /// <param name="v2">Конечная позиция</param>
    public void MoveFromTo(Vector3 v1, Vector3 v2) {
        
        var go = Instantiate(gameObject, v1, Quaternion.identity);
        go.GetComponent<BulletTrailRenderer>()._moveTo(v2);
    }

    /// <summary>
    ///     Устанавливает цель, к которой должен следовать данный след
    /// </summary>
    /// <param name="v2"></param>
    private void _moveTo(Vector3 v2) {
        this.v1 = transform.position;
        this.v2 = v2;
        main = false;
    }
    
    /// <summary>
    ///     Передвигает след от пули
    /// </summary>
    void Update() {
        if (main) return;
        transform.position = Vector3.MoveTowards(transform.position, v2, speed * Time.deltaTime);
        if (transform.position == v2) {
            Destroy(gameObject);
        }
    }
}
