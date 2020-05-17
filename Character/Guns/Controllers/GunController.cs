using CommandsSystem.Commands;
using Networking;
using UnityEngine;

namespace Character.Guns {
    /// <summary>
    ///     Класс для оружия, которое расположено на игровом поле и которое можно подобрать
    /// </summary>
    /// <typeparam name="T">Оружие</typeparam>
    public class GunController<T> : MonoBehaviour
        where T: IGun {
        public T gun;
        
        /// <summary>
        ///     Время, когда была последний раз проведена попытка подобрать оружие.
        ///     Нужно для предотвращения спама командами подобрать оружие
        /// </summary>
        private float picked = float.MaxValue;

        /// <summary>
        ///     Автоматически вызывается Unity при столкновении с другими объектами
        /// </summary>
        /// <param name="other">Другой объект</param>
        private void OnTriggerEnter(Collider other) {
            if (picked - Time.time < 5) return;
        
            if (other.CompareTag("Player")) {
                picked = Time.time;
                var command = new PickUpGunCommand(ObjectID.GetID(other.gameObject), ObjectID.GetID(this.gameObject));
            
                CommandsHandler.gameRoom.RunUniqCommand(command, UniqCodes.PICK_UP_GUN, command.gun, MessageFlags.IMPORTANT);
            }
        }
    }
}