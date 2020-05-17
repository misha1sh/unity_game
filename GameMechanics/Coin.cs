using CommandsSystem.Commands;
using GameMode;
using Networking;
using UnityEngine;

/// <summary>
///     Класс для монетки
/// </summary>
public class Coin : MonoBehaviour {
    /// <summary>
    ///     Время, когда последний раз была отправлена команда подобрать монетку на сервер
    /// </summary>
    private float picked = -100;

    /// <summary>
    ///     Подбирает монетку при столкновении с игроком. Автоматически вызывается Unity
    /// </summary>
    /// <param name="other">Коллайдер объекта, с которым столкнулась монетка</param>
    private void OnTriggerEnter(Collider other) {
        if (Time.time - picked < 5) return;
        
        if (other.CompareTag("Player")) {
            picked = Time.time;
            var command = new PickCoinCommand(ObjectID.GetID(other.gameObject), ObjectID.GetID(this.gameObject));
            
            CommandsHandler.gameModeRoom.RunSimpleCommand(command, MessageFlags.IMPORTANT);
        }
    }

}

