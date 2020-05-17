using UnityEngine;

namespace Character {
    /// <summary>
    ///     Компонента для следования камеры за игроком
    /// </summary>
    public class CameraFollower : MonoBehaviour {
        /// <summary>
        ///     Игрок, за которым должна следовать камера
        /// </summary>
        public GameObject character;

        /// <summary>
        ///     Высота относительно игрока, на которой должна располагаться камера
        /// </summary>
        public float yLevel = 0;

        /// <summary>
        ///     Перемещает камеру в позицию над игроком. Автоматически вызывается Unity каждый кадр
        /// </summary>
        void LateUpdate() {
            if (character == null) return;
            var position = character.transform.position;
            var vec = new Vector3(position.x, yLevel, position.z);
            transform.position = vec;
        }
    }
}
