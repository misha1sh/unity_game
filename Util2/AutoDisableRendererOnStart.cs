using UnityEngine;

namespace Util2 {
    public class AutoDisableRendererOnStart : MonoBehaviour {
        void Start() {
            GetComponent<Renderer>().enabled = false;
        }
    }
}