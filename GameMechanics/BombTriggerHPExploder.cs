using System;
using Character.HP;
using UnityEngine;

namespace GameMechanics {
    [RequireComponent(typeof(Bomb))]
    public class BombTriggerHPExploder : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            var hp = other.GetComponent<HPController>();
            if (hp != null) {
                GetComponent<Bomb>().Explode();
            }
        }
    }
}