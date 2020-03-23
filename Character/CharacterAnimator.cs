using System.Collections;
using System.Collections.Generic;
using Interpolation;
using UnityEngine;

namespace Character {


    
    
    
    




    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HandAnimationBlender))]
    public class CharacterAnimator : MonoBehaviour
    {
        private Animator animator;
        private HandAnimationBlender handAnimationBlender;

        
        void Start()
        {
            animator = GetComponent<Animator>();
            handAnimationBlender = GetComponent<HandAnimationBlender>();
        }


        public GameObjectState GetState() {
         /*   var state = new GameObjectState();
            state.id = ObjectID.GetID(gameObject); // TODO : cache
            state.position = transform.position;
            state.rotation = transform.rotation;*/
            return  new GameObjectState();
        }
        
        void Update()
        {
            animator.SetBool("push", false);

            
            
        }
    }
}
