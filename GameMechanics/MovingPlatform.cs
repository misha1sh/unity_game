using CommandsSystem.Commands;
using GameMode;
using Networking;
using UnityEngine;
using Util2;

namespace GameMechanics {
    
    /// <summary>
    ///     Класс для подвижной платформы
    /// </summary>
    public class MovingPlatform : MonoBehaviour, IOwnedEventHandler  {
        /// <summary>
        ///     Позиция, между которой должна перемещаться платформа
        /// </summary>
        public Transform nextTransform;

        /// <summary>
        ///     Предыдущая позиция платформы
        /// </summary>
        private Vector3 lastPosition;
        /// <summary>
        ///     Следующая позиция
        /// </summary>
        private Vector3 nextPosition;
        
        /// <summary>
        ///     Скорость платформы
        /// </summary>
        public float speed = 10f;
        /// <summary>
        ///     Время, на которое платформа останавливается
        /// </summary>
        public float stayTime = 2;
        
        /// <summary>
        ///     Состояние платформы
        /// </summary>
        public int state = STAY_STATE;
        /// <summary>
        ///     Состояние платформы, при котором она двигается
        /// </summary>
        private const int MOVE_STATE = 0;
        /// <summary>
        ///     Состояние платформы, при котором она стоит на месте
        /// </summary>
        private const int STAY_STATE = 1;
        /// <summary>
        ///     Состояние плафтформы, при котором она ожидает, когда ей дадут команду передвигаться
        /// </summary>
        private const int WAITING_FOR_COMMAND = 2;
        
        /// <summary>
        ///     Направление, в котором двигается платформа
        /// </summary>
        public int direction = DIRECTION_LAST_TO_NEXT;
        /// <summary>
        ///     Направление от предыдушего к следующему
        /// </summary>
        private const int DIRECTION_LAST_TO_NEXT = 0;
        /// <summary>
        ///     Направление от следующего к предыдущему
        /// </summary>
        private const int DIRECTION_NEXT_TO_LAST = 1;
        
        /// <summary>
        ///     ID платформы
        /// </summary>
        private int id;

        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        private void Start() {
            lastPosition = transform.position;
            nextPosition = nextTransform.position;
            id = ObjectID.GetID(gameObject);
            CommandsHandler.gameModeRoom.RunSimpleCommand(new TakeOwnCommand(id, sClient.ID), 
                 MessageFlags.IMPORTANT);
        }

        /// <summary>
        ///     Обрабатывает событие, когда какой-либо объект попадает на платформу. Автоматически вызывается Unity при столкновении
        /// </summary>
        /// <param name="other">Информация о столкновении</param>
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.GetComponent<Rigidbody>() != null && other.transform.position.y > transform.position.y)
                other.transform.parent = transform;
        }

        /// <summary>
        ///     Обрабатывет событие, когда объект уходит с платформы. Автоматически вызывается Unity
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionExit(Collision other) {
            if (other.transform.parent == transform)
                other.transform.parent = null;
        }

        /// <summary>
        ///     Переключает плафторму в режим движения
        /// </summary>
        /// <param name="direction">Направление движения</param>
        public void SetMoveState(int direction) {
            if (direction != this.direction) {
                gUtil.Swap(ref lastPosition, ref nextPosition);
            }

            if (direction == this.direction || state == MOVE_STATE)
                transform.position = lastPosition;
            this.direction = direction;
            this.state = MOVE_STATE;
        }

        /// <summary>
        ///     Время, которое платформе осталось стоять на месте
        /// </summary>
        private float currentStayingTime = -100;
        
        /// <summary>
        ///     Обновляет состояние платформы и перемещает её
        /// </summary>
        public void Update() {
            if (id == 0) return;
            if (state == MOVE_STATE) {
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed*Time.deltaTime);
                if (transform.position == nextPosition) {
                    state = STAY_STATE;
                    currentStayingTime = stayTime;
                }
            } else if (state == STAY_STATE) {
                currentStayingTime -= Time.deltaTime;
                if (currentStayingTime < 0) {
                    if (ObjectID.IsOwned(id)) {
                        CommandsHandler.gameModeRoom.RunSimpleCommand(new SetPlatformStateCommand(id, 1 - direction),
                             MessageFlags.NONE);
                        currentStayingTime = 0.1f;
                    }

                    state = WAITING_FOR_COMMAND;
                }
            }
            
        }

        /// <summary>
        ///     Обрабатывает событие, когда у платформы появляется новый владелец
        /// </summary>
        /// <param name="owner">Новый владелец</param>
        public void HandleOwnTaken(int owner) {
            if (ObjectID.IsOwned(gameObject)) {
                if (state == WAITING_FOR_COMMAND)
                    state = STAY_STATE;
            }
        }
    }
}