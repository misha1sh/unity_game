using Character.Guns;
using GameMode;
using UnityEngine;

public class EventsHandler {
    public delegate void PlayerObjParameterChanged<T>(GameObject player, T parameter);

    public PlayerObjParameterChanged<int> OnPlayerBulletsCountChanged = delegate { };
    public PlayerObjParameterChanged<int> OnPlayerMagazinesCountChanged = delegate { };
    public PlayerObjParameterChanged<IGun> OnPlayerPickedUpGun = delegate {  };
    public PlayerObjParameterChanged<IGun> OnPlayerDroppedGun = delegate {  };
    
    public PlayerObjParameterChanged<GameObject> OnPlayerPickedUpCoin = delegate {  };

    public delegate void PlayerParameterChanged<T>(Player player, T parameter);
    
    public PlayerParameterChanged<int> OnPlayerScoreChanged = delegate {  };
    /*   public void (GameObject player, int bulletsCount) {
           
       }
   
       public void HandlePlayerMagazinesCountChanged(GameObject player, int bulletsCount) {
           
       }*/
}
