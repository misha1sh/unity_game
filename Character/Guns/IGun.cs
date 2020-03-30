using UnityEngine;

namespace Character.Guns {
    public interface IGun {
        GunState state { get; }
        void OnPickedUp(GameObject player);
        void OnDropped();
    }
}