using System;
using System.Collections.Generic;
using Character.HP;
using CommandsSystem.Commands;
using Networking;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;
using UnityEngine.Jobs;
using Object = System.Object;

namespace GameMechanics {
    /// <summary>
    ///     Класс для бобмы
    /// </summary>
    public class Bomb : MonoBehaviour {
        /// <summary>
        ///     Урон от попадания
        /// </summary>
        public float damage;
        /// <summary>
        ///     Сила взрыва
        /// </summary>
        public float explosionForce = 400;
        /// <summary>
        ///     Сфера, внутри которой взрывается бобма
        /// </summary>
        public SphereCollider area;

        /// <summary>
        ///     Должна ли бомба наносить урон игроку, который её создал (false, если должна)
        /// </summary>
        public bool noDamageToCreator = false;

        /// <summary>
        ///     Радиус взрыва
        /// </summary>
        [NonSerialized]
        public float radius;

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        public void Start() {
            radius = transform.lossyScale.x * area.radius;
        }

        /// <summary>
        ///     Синхронно взрывает бомбу
        /// </summary>
        public void Explode() {
            int id = ObjectID.GetID(gameObject);
            CommandsHandler.gameModeRoom.RunUniqCommand(new ExplodeBombCommand(id), UniqCodes.EXPLODE_BOMB, id, MessageFlags.IMPORTANT);
        }
        
        /// <summary>
        ///     Взрывает бомбу локально
        /// </summary>
        public void RealExplode() {
            var creatorId = ObjectID.GetCreator(gameObject);

            // Physics.OverlapSphere(transform.position, radius);
           var colliders = RotaryHeart.Lib.PhysicsExtension.Physics.OverlapSphere(transform.position, radius, PreviewCondition.Both, 2,
               Color.red, Color.green);
           HashSet<HPController> gameObjects = new HashSet<HPController>();
           foreach (var collider in colliders) {

               if (ObjectID.TryGetID(collider.gameObject, out var iid)) {
                   Vector3 force = collider.transform.position - transform.position;
                   float len = force.magnitude;
                   force = force.normalized * explosionForce / Mathf.Sqrt(len);
                   CommandsHandler.gameModeRoom.RunSimpleCommand(new ApplyForceCommand(iid, force ), MessageFlags.NONE);
               }
               
               var hp = collider.GetComponent<HPController>();
               if (hp == null) continue;
               int hpid = ObjectID.GetID(hp.gameObject);
               if (ObjectID.IsOwned(hpid)) {
                   if (noDamageToCreator && (hpid == creatorId ||
                                             ObjectID.TryGetCreator(hpid, out var hpCreator) && hpCreator == creatorId))
                       continue;
                   gameObjects.Add(hp);
               }
           }

           foreach (var hp in gameObjects) {
               
               hp.TakeDamage(damage, DamageSource.Bomb(), true);
           }
        }
    }
}