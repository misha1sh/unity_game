using Character.Guns;
using Game;
using GameMode;
using UnityEngine;

namespace Events {
    public class EventsHandler {

        public PlayerObjParameterChanged<int> OnPlayerBulletsCountChanged = delegate { };
        public PlayerObjParameterChanged<int> OnPlayerMagazinesCountChanged = delegate { };
        public PlayerObjParameterChanged<IGun> OnPlayerPickedUpGun = delegate { };
        public PlayerObjParameterChanged<IGun> OnPlayerDroppedGun = delegate { };

        public PlayerObjParameterChanged<GameObject> OnPlayerPickedUpCoin = delegate { };

        public delegate void PlayerParameterChanged<T>(Player player, T parameter);

        public PlayerParameterChanged<int> OnPlayerScoreChanged = delegate { };


        public delegate void ObjectDead(GameObject go, int killSource);
        public ObjectDead OnObjectDead = delegate { };

        public delegate void ObjectGotDamage(GameObject go, float damage, int damageSource);
        public ObjectGotDamage OnObjectChangedHP = delegate { };



        public delegate void CurrentMatchChanged(MatchInfo last, MatchInfo current);
        public CurrentMatchChanged OnCurrentMatchChanged = delegate { };
        /*   public void (GameObject player, int bulletsCount) {
           
       }
   
       public void HandlePlayerMagazinesCountChanged(GameObject player, int bulletsCount) {
           
       }*/
    }
}
