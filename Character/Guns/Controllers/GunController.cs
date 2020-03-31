using UnityEngine;

namespace Character.Guns {
    public class GunController<T> : MonoBehaviour
        where T: IGun {
        public T gun;
    }
}