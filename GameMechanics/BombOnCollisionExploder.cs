using System;
using UnityEngine;

namespace GameMechanics {
    /// <summary>
    ///     Компонента для взрыва бомбы при столкновении с объектом
    /// </summary>
    [RequireComponent(typeof(Bomb))]
    public class BombOnCollisionExploder : MonoBehaviour {
        /// <summary>
        ///     Настраивает физику, чтобы бомба не сталкивалась с персонажем, создавшем её
        /// </summary>
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
        
        /// <summary>
        ///     Взрывает бомбу. Автоматически вызывается Unity при столкновении с другим объектом
        /// </summary>
        /// <param name="other">Информация о столкновении</param>
        public void OnCollisionEnter(Collision other) {
            GetComponent<Bomb>().Explode();
        }
    }
}