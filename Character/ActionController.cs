using System;
using Character.Actions;
using Character.Guns;
using UnityEngine;

namespace Character {


    
    /// <summary>
    ///     Компонента для обработки действий, которые может делать игрок
    /// </summary>
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(PushAction))]
    [RequireComponent(typeof(ShootPistolAction))]
    [RequireComponent(typeof(ShootSemiautoAction))]
    public class ActionController : MonoBehaviour {
        
        /// <summary>
        ///     Ссылка на CharacterAnimator игрока
        /// </summary>
        private CharacterAnimator animator;
        
        /// <summary>
        ///     Инициализирует переменны
        /// </summary>
        void Start() {
            animator = GetComponent<CharacterAnimator>();
        }

        /// <summary>
        ///     Текущее действие
        /// </summary>
        private IAction currentAction = null;
        /// <summary>
        ///     Координата, в которую направлен прицел игрока
        /// </summary>
        public Vector3 Target;
        
        /// <summary>
        ///    Прекращает выполнение текущего действия
        /// </summary>
        private void StopCurrent() {
            if (currentAction != null) {
                DoAction = false;
                (currentAction as MonoBehaviour).enabled = false;
            }
        }

        /// <summary>
        ///     Изменяет действие персонажа
        /// </summary>
        /// <param name="setup">Функция для инициализации нового действия</param>
        /// <typeparam name="T">Тип нового действия, должен быть MonoBehaivour и IAction</typeparam>
        public void SetAction<T>(System.Action<T> setup)
            where T: MonoBehaviour, IAction {
            StopCurrent();
            var action = gameObject.GetComponent<T>();

            setup(action);
            
            action.enabled = true;
            currentAction = action;
        }
        

        /// <summary>
        ///     Изменяет действие перонажа на пустое
        /// </summary>
        public void SetNothing() {
            StopCurrent();
            currentAction = null;
        }

        /// <summary>
        ///     Переменная для хранения DoAction
        /// </summary>
        private bool _actionDoing = false;
        
        /// <summary>
        ///     Автоматическая переменная, устанавливающая выполняет ли сейчас действие игрок
        /// </summary>
        public bool DoAction {
            get => _actionDoing;
            set {
                if (value == _actionDoing) return;
                _actionDoing = value;
                if (value) {
                    currentAction?.OnStartDoing();
                } else {
                    currentAction?.OnStopDoing();
                }
            }
        }
    }
}