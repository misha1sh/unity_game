using UnityEngine;

namespace Util2 {
    /// <summary>
    ///     Компонента, автоматически присваивающая объекту ID
    /// </summary>
    public class AutoID : MonoBehaviour {
        /// <summary>
        ///     ID объекта
        /// </summary>
        public int ID;

        /// <summary>
        ///     Генерирует случайный ID
        /// </summary>
        public void Reset() {
            ID = ObjectID.RandomID;
        }

        /// <summary>
        ///     Сохраняет ID объекта
        /// </summary>
        public void Awake() {
            ObjectID.StoreObject(gameObject, ID, 0, 0);
        }

        /// <summary>
        ///     Удаляет эту компоненту из объекта во время игры
        /// </summary>
        public void Update() {
            Destroy(this);
        }
    }
}