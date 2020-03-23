using UnityEngine;

namespace Character {
    public class CameraFollower : MonoBehaviour {

        public GameObject character;

        public float yLevel = 0;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void LateUpdate() {
            var position = character.transform.position;
            var vec = new Vector3(position.x, yLevel, position.z);
            transform.position = vec;
//            var rot = gameObject.transform.eulerAngles;
            // rot.y = character.transform.rotation.eulerAngles.y;
//            gameObject.transform.eulerAngles = rot;
            // gameObject.transform.Rotate(0.0f, 1.0f, 0.0f);
        }
    }
}
