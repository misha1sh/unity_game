﻿using UnityEngine;

public class EventsHandler {
    public delegate void PlayerParameterChanged<T>(GameObject player, T parameter);

    public PlayerParameterChanged<int> OnPlayerBulletsCountChanged = delegate { };
    public PlayerParameterChanged<int> OnPlayerMagazinesCountChanged = delegate { };
    


    /*   public void (GameObject player, int bulletsCount) {
           
       }
   
       public void HandlePlayerMagazinesCountChanged(GameObject player, int bulletsCount) {
           
       }*/
}