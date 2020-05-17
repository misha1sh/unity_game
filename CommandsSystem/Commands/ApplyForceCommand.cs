using UnityEngine;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Команда, сообщающая, что к игровому объекту нужно применить силу
    /// </summary>
    public partial class ApplyForceCommand {
        /// <summary>
        ///     Id игрового объекта, к которму нужно применить силу
        /// </summary>
        public int objectId;
        /// <summary>
        ///     Сила
        /// </summary>
        public Vector3 force;
        
        /// <summary>
        ///     Конструктор команды
        /// </summary>
        /// <param name="gameObject">Игровой объект, к которму нужно применить силу</param>
        /// <param name="force">Сила</param>
        public ApplyForceCommand(GameObject gameObject, Vector3 force) :
            this(ObjectID.GetID(gameObject), force) {}

        /// <summary>
        ///     Применяет силу, если объект обрабатывается на данном клиенте
        /// </summary>
        public void Run() {
            var gameObject = ObjectID.GetObject(objectId);
            if (gameObject == null) {
                Debug.LogError($"Not found gameobject#{objectId} for applying force");
                return;
            }
            var rigidBody = gameObject.GetComponent<Rigidbody>();
            if (rigidBody == null) return; // means we dont control this gameobject, so just skip it
            rigidBody.AddForce(force, ForceMode.Impulse);
        }
    }
}