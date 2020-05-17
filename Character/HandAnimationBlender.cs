using UnityEngine;
using UnityEngine.Assertions;

namespace Character {
    /// <summary>
    ///     Класс для анимации рук персонажа
    /// </summary>
    public class HandAnimationBlender : MonoBehaviour {
        /// <summary>
        ///     Скорость переключения между анимациями
        /// </summary>
        public float blendSpeed = 1.5f;
    
        /// <summary>
        ///     Началось ли производить переключение между анимациями
        /// </summary>
        private bool blendStart = false;
        /// <summary>
        ///     Началось ли производить обратное переключение между анимациями
        /// </summary>
        private bool blendStop = false;
        /// <summary>
        ///     Текущий коэффициент смешивания слоев анимации
        /// </summary>
        private float blendCoeff = 0.1f;
        /// <summary>
        ///     Индекс слоя с анимациями рук
        /// </summary>
        private int layerIndex;
    
        /// <summary>
        ///     Ссылка на Animator персонажа
        /// </summary>
        private Animator animator;
    
        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        void Start() {
            animator = GetComponent<Animator>();
            layerIndex = animator.GetLayerIndex("hands");
            Assert.AreNotEqual(layerIndex, -1);
        }

        /// <summary>
        ///     Обновляет анимацию рук (если нужно). Автоматически вызывается Unity каждый кадр
        /// </summary>
        void Update() {
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

        /// <summary>
        ///     Начинает анимацию рук. Автоматически вызывается Unity при запуске анимации
        /// </summary>
        public void StartHandAnimation() {
            blendStart = true;
            blendStop = false;
            animator.SetBool("push", false);
        }

        /// <summary>
        ///     Прекращает анимацию рук. Автоматически вызывается Unity по окончанию анимации
        /// </summary>
        public void StopHandAnimation() {
            blendStart = false;
            blendStop = true;
        }
    }
}
