using UnityEngine;

namespace CommandsSystem.Commands {
    public class ApplyForceCommand : Command<ApplyForceCommand> {
        public int objectId;

        public Vector3 force;
        
        public ApplyForceCommand() {}

        public ApplyForceCommand(GameObject gameObject, Vector3 force) {
            this.objectId = ObjectID.GetID(gameObject);
            this.force = force;
        }
        
        public override void Run() {
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