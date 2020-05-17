using UnityEngine;

namespace Util2 {
    /// <summary>
    ///     Компонента, автоматически отключающая Renderer у объекта при старте
    /// </summary>
    public class AutoDisableRendererOnStart : MonoBehaviour {
        /// <summary>
        ///     Отключает Renderer
        /// </summary>
        void Start() {
            GetComponent<Renderer>().enabled = false;
        }
    }
}