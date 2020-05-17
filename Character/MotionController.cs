using System.Collections.Generic;
using Character.HP;
using UnityEngine;

namespace Character {
    
    /// <summary>
    ///     Компонента для передвижения персонажа по игровому полю
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterAnimator))]
    public class MotionController : MonoBehaviour {
        /// <summary>
        ///     Ссылка на Rigidbody персонажа
        /// </summary>
        private new Rigidbody rigidbody;
        /// <summary>
        ///     Ссылка на CapsuleCollider персонажа
        /// </summary>
        private CapsuleCollider capsuleCollider;
        /// <summary>
        ///     Ссылка на CharacterAnimator персонажа
        /// </summary>
        private CharacterAnimator animator;
        
        /// <summary>
        ///     Сила, которая изменяет скорость персонажа
        /// </summary>
        public float moveForce = 4000;
        /// <summary>
        ///     Максимальная скорость персонажа
        /// </summary>
        public float speed = 6.0f;
        /// <summary>
        ///     Скорость поворота персонажа
        /// </summary>
        public float rotationSpeed = 700.0f;
        

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        void Start() {
            rigidbody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            animator = GetComponent<CharacterAnimator>();
        }

        private List<GameObject> groundCollisions = new List<GameObject>();

        /// <summary>
        ///     Провряет, что персонаж всё еще стоит на земле.
        /// </summary>
        /// <param name="gameObject">Объект, на котором персонаж прекратил стоять</param>
        private void DeGround(GameObject gameObject) {
            if (!groundCollisions.Remove(gameObject)) return;
            isGrounded = groundCollisions.Count != 0;
        }

        /// <summary>
        ///     Сообщает, что персонаж всё ещё стоит на данном коллайдере. Автоматически вызывается Unity
        /// </summary>
        /// <param name="other">Коллайдер</param>
        private void OnTriggerStay(Collider other) {
            if (!groundCollisions.Contains(other.gameObject)) {
                groundCollisions.Add(other.gameObject);
                isGrounded = true;
            }
        }

        /// <summary>
        ///     Сообщает, что персонаж больше не стоит на данном коллайдере . Автоматически вызывается Unity
        /// </summary>
        /// <param name="other">Коллайдер</param>
        private void OnTriggerExit(Collider other) {
            DeGround(other.gameObject);
        }
        
        /// <summary>
        ///     Находится ли персонаж на земле
        /// </summary>
        private bool isGrounded = true;
        
        /// <summary>
        ///     Направление, в котором должен двигаться персонаж
        /// </summary>
        public Vector3 TargetDirection { get; set; }
        /// <summary>
        ///     Направление, в котором смотрит персонаж
        /// </summary>
        public Vector3 TargetRotation { get; set; }

      
        /// <summary>
        ///     Производит передвижение персонажа. Автоматически вызывается Unity при каждой обработке физики
        /// </summary>
        void FixedUpdate() {
            for (int i = 0; i < groundCollisions.Count; i++) {
                if (!groundCollisions[i]) {
                    DeGround(groundCollisions[i]);
                    break;
                }
            }

            if (transform.position.y < -15) {
                gameObject.GetComponent<HPController>().TakeDamage(100000, DamageSource.InstaKill(), true);
            }

            if (isGrounded) {
                var targetSpeed = speed * TargetDirection;
                var bodySpeed = rigidbody.velocity;

                var vec = bodySpeed - targetSpeed;
                vec.y = 0;
                if (vec.magnitude > 1) {
                    vec.Normalize();
                    if (rigidbody.velocity.sqrMagnitude > 5f) {// && //Mathf.Abs(rigidbody.velocity.sqrMagnitude - speed * speed) > 0.5f)
                        rigidbody.velocity = targetSpeed;                      
                    } else {
                        rigidbody.AddForce(-vec * moveForce);
                        Debug.Log("addforce");  
                    }
                } else {
                    animator.SetRotationSpeed(0);
                }
            }

            if (TargetDirection != Vector3.zero)
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.LookRotation(TargetDirection),
                    rotationSpeed * Time.deltaTime);


            float linearSpeed = TargetDirection.magnitude;
            animator.SetIdle(linearSpeed == 0.0f);
            animator.SetSpeed(linearSpeed);
        }

        /// <summary>
        ///     Отрисовывает отладочную информацию о состоянии игрока. Автоматически вызывается средой Unity
        /// </summary>
        private void OnDrawGizmos() {

            if (!Application.isPlaying) return;

            var pos = transform.position + capsuleCollider.center;
            DebugExtension.DebugCircle(pos,
                Color.green, radius: 0.1f, depthTest: false);

            DebugExtension.DebugCircle(transform.position + Vector3.up * 2,
                isGrounded ? Color.red : Color.blue, radius: 0.1f);
            
            pos.y += 0.5f;

            DebugExtension.DebugArrow(pos, TargetDirection * 1f, Color.blue);
            DebugExtension.DebugArrow(pos, TargetRotation.normalized / 2, Color.red);
        }
    }
}

