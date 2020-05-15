namespace DefaultNamespace {
    public class AutoHideOnStart : UnityEngine.MonoBehaviour {
        public void Start() {
            gameObject.SetActive(false);
        }
    }
}