using System;
using Character.HP;
using UnityEngine;

namespace GameMechanics {
    /// <summary>
    ///     Компонента для взрыва бомбы при столкновении с объектом, имеющим здоровье
    /// </summary>
    [RequireComponent(typeof(Bomb))]
    public class BombTriggerHPExploder : MonoBehaviour {
        /// <summary>
        ///     Взрывает бомбу, если объект имеет здоровье. Автоматически вызывается Unity при столкновении
        /// </summary>
        /// <param name="other">Коллайдер объекта, с которым столкнулась бомба</param>
        private void OnTriggerEnter(Collider other) {
            var hp = other.GetComponent<HPController>();
            if (hp != null) {
                GetComponent<Bomb>().Explode();
            }
        }
    }
}