using Character;
using Interpolation;
using Interpolation.Properties;
using UnityEngine;

namespace CommandsSystem.Commands {
    /// <summary>
    ///     Состояние персонажа, которое можно синхронизировать по сети
    /// </summary>
    public class PlayerProperty : GameObjectProperty<PlayerProperty> {
        /// <summary>
        ///     ID персонажа
        /// </summary>
        public int id;
        
        /// <summary>
        ///     Позиция персонажа
        /// </summary>
        public Vector3 position;
        /// <summary>
        ///     Поворот персонажа
        /// </summary>
        public Quaternion rotation;
        /// <summary>
        ///     Состояние анимации персонажа
        /// </summary>
        public PlayerAnimationState animationState;

        /// <summary>
        ///     Ссылка на CharacterAnimator персонажа
        /// </summary>
        private CharacterAnimator characterAnimator;
        
        /// <summary>
        ///     Копирует состояние из другого
        /// </summary>
        /// <param name="state">Состояние из которого нужно копировать</param>
        public override void CopyFrom(PlayerProperty state) {
            id = state.id;
            position = state.position;
            rotation = state.rotation;
            animationState.idle = state.animationState.idle;
            animationState.speed = state.animationState.speed;
            animationState.rotationSpeed = state.animationState.rotationSpeed;
        }

        /// <summary>
        ///     Получает состояние из объекта
        /// </summary>
        /// <param name="gameObject">Объект</param>
        public override void FromGameObject(GameObject gameObject) {
            id = ObjectID.GetID(gameObject);
            position = gameObject.transform.position;
            rotation = gameObject.transform.rotation; 
            if (characterAnimator is null) characterAnimator = gameObject.GetComponent<CharacterAnimator>();
            animationState = characterAnimator.animationState;
        }

        /// <summary>
        ///     Применяет состояние к объекту
        /// </summary>
        /// <param name="gameObject">Объект</param>
        public override void ApplyToObject(GameObject gameObject) {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            if (characterAnimator is null) characterAnimator = gameObject.GetComponent<CharacterAnimator>();
            characterAnimator.animationState = animationState;
        }

        /// <summary>
        ///     Создаёт команду для отправки состояния другим клиентам
        /// </summary>
        /// <param name="deltaTime">Время, прошедшее с прошлой отправки</param>
        /// <returns>Команду</returns>
        public override ICommand CreateChangedCommand(float deltaTime) {
            return new ChangePlayerProperty(this, deltaTime);
        }
        

        /// <summary>
        ///     Интерполирует состояние между другими
        /// </summary>
        /// <param name="lastLastState">Предпредыдущее состояние</param>
        /// <param name="lastState">Предыдущее состояние</param>
        /// <param name="nextState">Следующее состояние</param>
        /// <param name="coef">Коэффициент интерполяции между состояниями (от 0 до 1)</param>
        public override void Interpolate(PlayerProperty lastLastState, PlayerProperty lastState, PlayerProperty nextState, float coef) {
            position = InterpolationFunctions.InterpolatePosition(lastLastState.position, lastState.position, nextState.position, coef);
            rotation = InterpolationFunctions.InterpolateRotation(lastState.rotation, nextState.rotation, coef);
            animationState =
                InterpolationFunctions.InterpolatePlayerAnimationState(lastState.animationState,
                    nextState.animationState, coef);
        }
        
       
    }
    
    
    

    
}