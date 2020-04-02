using UnityEngine;

namespace CommandsSystem.Commands {
    public partial class ApplyForceCommand {
        public int objectId;
        public Vector3 force;


        public ApplyForceCommand(GameObject gameObject, Vector3 force) :
            this(ObjectID.GetID(gameObject), force) {}

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