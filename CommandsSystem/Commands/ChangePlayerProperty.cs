using Interpolation;
using UnityEngine;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Класс для хранения и синхронизации состояния игрока
    /// </summary>
    public partial class ChangePlayerProperty {
        /// <summary>
        ///     Состояние игрока
        /// </summary>
        public PlayerProperty property;
        /// <summary>
        ///     Время, которое прошло с последнего изменения состояния
        /// </summary>
        public float deltaTime;
        
        /// <summary>
        ///     Применяет изменение состояния
        /// </summary>
        public void Run() {
            GameObject gameObject;
            if (!ObjectID.TryGetObject(property.id, out gameObject))
            {
                return;
            }

            var controller = gameObject.GetComponent<UnmanagedGameObject<PlayerProperty>>();
            if (controller is null) return;
            controller.SetStateAnimated(property, deltaTime);
#if DEBUG_INTERPOLATION              
            DebugExtension.DebugPoint(property.position, Color.red, 0.1f, 3);
#endif
        }
    }
}