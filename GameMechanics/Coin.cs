using CommandsSystem.Commands;
using GameMode;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private float picked = -100;

    private void OnTriggerEnter(Collider other) {
        if (Time.time - picked < 5) return;
        
        if (other.CompareTag("Player")) {
            picked = Time.time;
            var command = new PickCoinCommand(ObjectID.GetID(other.gameObject), ObjectID.GetID(this.gameObject));
            
            sClient.commandsHandler.RunSimpleCommand(command, 1);
        }
    }

}

