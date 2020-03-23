using UnityEngine;
using UnityEngine.Assertions;

namespace Character {
    public class HandAnimationBlender : MonoBehaviour {

        public float blendSpeed = 1.5f;
    
        private bool blendStart = false;
        private bool blendStop = false;
        private float blendCoeff = 0.1f;
        private int layerIndex;
    
        private Animator animator;
    
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            layerIndex = animator.GetLayerIndex("hands");
            Assert.AreNotEqual(layerIndex, -1);
        }

        // Update is called once per frame
        void Update()
        {
            if (blendStart) {
                blendCoeff += blendSpeed * Time.deltaTime;
                if (blendCoeff >= 1) {
                    blendStart = false;
                    blendCoeff = 1;
                }
            }

            if (blendStop) {
                blendCoeff -= blendSpeed * Time.deltaTime;
                if (blendCoeff <= 0) {
                    blendStop = false;
                    blendCoeff = 0.1f;
                }
            }
        
            animator.SetLayerWeight(layerIndex, blendCoeff);
        }

        public void StartHandAnimation() {
            blendStart = true;
            blendStop = false;
        }

        public void StopHandAnimation() {
            blendStart = false;
            blendStop = true;
        }
    }
}
