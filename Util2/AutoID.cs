using UnityEngine;

namespace Util2 {
    public class AutoID : MonoBehaviour {
        public int ID;

        public void Reset() {
            ID = ObjectID.RandomID;
        }

        public void Awake() {
            ObjectID.StoreObject(gameObject, ID, 0);
        }

        public void Update() {
            Destroy(this);
        }
    }
}