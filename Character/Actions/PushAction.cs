using CommandsSystem.Commands;
using Networking;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine;

namespace Character.Actions {
    /// <summary>
    ///     Действие персонажа, при котором он толкает предметы перед собой с определенной силой
    /// </summary>
    [RequireComponent(typeof(CharacterAnimator))]
    public class PushAction : MonoBehaviour, IAction {
        /// <summary>
        ///     Объект, внутри котрого нужно толькать предметы
        /// </summary>
        public GameObject pushCollider;
        /// <summary>
        ///     Сила с которой нужно толкать предметы
        /// </summary>
        public float force = 3300;
        
        /// <summary>
        ///     Ссылка на CharacterAnimator у игрока
        /// </summary>
        private CharacterAnimator animator;

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        void Start() {
            animator = gameObject.GetComponent<CharacterAnimator>();
        }

        /// <summary>
        ///     Начинает выполнять данное действие
        /// </summary>
        public void OnStartDoing() {
            animator.SetPush();
            CommandsHandler.gameRoom.RunSimpleCommand(new PlayerPushCommand(ObjectID.GetID(gameObject)), MessageFlags.NONE);
        }

        /// <summary>
        ///     Толкает предметы перед собой. Автоматически вызывается Unity в конце анимации рук персонажа
        /// </summary>
        public void pushEnd() {
            var center = pushCollider.transform.position;
            var scale = pushCollider.transform.localScale;
            var rotation = pushCollider.transform.rotation;

            var delta = rotation * Vector3.up * scale.y;
            //delta.Scale(scale);
            
            var radius = scale.x * transform.lossyScale.x / 2;

            delta -= rotation * (radius * Vector3.up);
            
            var start = center - delta;
            var stop = center + delta;
 
            //   DebugExtension.DebugCapsule(start, stop, Color.red, radius, 1);

            var f = 
                RotaryHeart.Lib.PhysicsExtension.Physics.OverlapCapsule(start, stop, radius, PreviewCondition.Both, drawDuration: 1);

            var force = rotation * Vector3.up * this.force;
            foreach (var v in f) {
                if (v.gameObject == gameObject) continue;
             
                if (v.gameObject.CompareTag("Unmanagable")) {
                    var command = new ApplyForceCommand(v.gameObject, force);
                    CommandsHandler.gameRoom.RunSimpleCommand(command, MessageFlags.NONE);
                } else {
                    var rig = v.gameObject.GetComponent<Rigidbody>();
                    if (rig != null) {
                        rig.AddForce(force, ForceMode.Impulse);
                    }
                }
            }
        }

        /// <summary>
        ///     Заканчивает выполнять данное действие
        /// </summary>
        public void OnStopDoing() { } 
    }
}