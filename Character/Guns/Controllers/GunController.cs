using CommandsSystem.Commands;
using UnityEngine;

namespace Character.Guns {
    public class GunController<T> : MonoBehaviour
        where T: IGun {
        public T gun;
        
        
        private float picked = float.MaxValue;

        private void OnTriggerEnter(Collider other) {
            if (picked - Time.time < 5) return;
        
            if (other.CompareTag("Player")) {
                picked = Time.time;
                var command = new PickUpGunCommand(ObjectID.GetID(other.gameObject), ObjectID.GetID(this.gameObject));
            
                Client.client.commandsHandler.RunSimpleCommand(command);
            }
        }
    }
}