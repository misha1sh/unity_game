using UnityEngine;

namespace Character {
    public class CharacterController : MonoBehaviour {
        public GameObject target;

        protected MotionController motionController;
        protected ActionController actionController;

        protected virtual void Start() {
            motionController = target.GetComponent<MotionController>();
            actionController = target.GetComponent<ActionController>();
        }
    }
}