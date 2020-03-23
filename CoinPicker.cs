using System;
using System.Collections;
using System.Collections.Generic;
using CommandsSystem.Commands;
using UnityEngine;

public class CoinPicker : MonoBehaviour
{
    private void Start()
    {
    }

    private float picked = float.MaxValue;

    private void OnTriggerEnter(Collider other) {
        if (picked - Time.time < 5) return;
        
        if (other.CompareTag("Player")) {
            picked = Time.time;
            var command = new PickCoinCommand(ObjectID.GetID(other.gameObject), ObjectID.GetID(this.gameObject));
            
            Client.client.commandsHandler.RunSimpleCommand(command);
        }
    }

}

