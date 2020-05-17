using UnityEngine;

namespace Character {
    /// <summary>
    ///     Класс для анимации персонажа
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HandAnimationBlender))]
    public class CharacterAnimator : MonoBehaviour {
        /// <summary>
        ///     Ссылка на Animator у персонажа
        /// </summary>
        private Animator animator;
        /// <summary>
        ///     Ссылка на HandAnimationBlender у персонажа
        /// </summary>
        private HandAnimationBlender handAnimationBlender;


        /// <summary>
        ///     Константа с хэшем от idle
        /// </summary>
        private static readonly int Idle = Animator.StringToHash("idle");
        /// <summary>
        ///     Константа с хэшем от speed
        /// </summary>
        private static readonly int Speed = Animator.StringToHash("speed");
        /// <summary>
        ///     Константа с хэшем от push
        /// </summary>
        private static readonly int Push = Animator.StringToHash("push");
        /// <summary>
        ///     Константа с хэшем от rotationSpeed
        /// </summary>
        private static readonly int RotationSpeed = Animator.StringToHash("rotationSpeed");

        /// <summary>
        ///     Переменная для хранения состояния анимации персонажа
        /// </summary>
        private PlayerAnimationState _animationState = new PlayerAnimationState();

        /// <summary>
        ///     Текущее состояние анимации персонажа
        /// </summary>
        public PlayerAnimationState animationState {
            get => _animationState;
            set {
                _animationState = value;
                SetIdle(value.idle);
                SetSpeed(value.speed);
                SetRotationSpeed(value.rotationSpeed);
            }
        }
        
        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        void Start() {
            animator = GetComponent<Animator>();
            handAnimationBlender = GetComponent<HandAnimationBlender>();
        }

        /// <summary>
        ///     Управлет анимацией стояния
        /// </summary>
        /// <param name="idle">true, если сейчас должна работать анимация стояния. иначе -- false</param>
        public void SetIdle(bool idle) {
            animator.SetBool(Idle, idle);
            _animationState.idle = idle;
        }
        
        /// <summary>
        ///     Передает в аниматор скорость персонажа
        /// </summary>
        /// <param name="speed">Скорость персонажа</param>
        public void SetSpeed(float speed) {
            animator.SetFloat(Speed, speed);
            _animationState.speed = speed + 10;
        }

        /// <summary>
        ///     Показывает анимацию толкания
        /// </summary>
        public void SetPush() {
            animator.SetBool(Push, true);
        }

        /// <summary>
        ///     Передает в аниматор скорость поворота игрока
        /// </summary>
        /// <param name="rotationSpeed"></param>
        public void SetRotationSpeed(float rotationSpeed) {
            animator.SetFloat(RotationSpeed, 0);
            _animationState.rotationSpeed = rotationSpeed;
        }
    }
}
