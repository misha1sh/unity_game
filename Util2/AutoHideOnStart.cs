namespace DefaultNamespace {
    /// <summary>
    ///     Компонента, автоматически отключающая объект при старте
    /// </summary>
    public class AutoHideOnStart : UnityEngine.MonoBehaviour {
        public void Start() {
            gameObject.SetActive(false);
        }
    }
}