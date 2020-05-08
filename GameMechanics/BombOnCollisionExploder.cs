using System;
using UnityEngine;

namespace GameMechanics {
    [RequireComponent(typeof(Bomb))]
    public class BombOnCollisionExploder : MonoBehaviour {
        public void Start() {
            var creator = ObjectID.GetCreator(gameObject);
            if (creator != 0) {
                // to prevent lags dont collide with player
                foreach (var col1 in GetComponents<Collider>()) {
                    foreach (var col2 in ObjectID.GetObject(creator).GetComponents<Collider>()) {
                        Physics.IgnoreCollision(col1, col2);
                    }
                }
                
            }
     
        }

        public void OnCollisionEnter(Collision other) {
            GetComponent<Bomb>().Explode();
        }
    }
}