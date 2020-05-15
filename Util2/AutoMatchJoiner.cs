using System;
using Events;
using Game;
using UnityEngine;

namespace Util2 {
    public class AutoMatchJoiner : MonoBehaviour {
        public static bool isRunning = false;
        public static bool sneedWaitOtherPlayers;
        
        public bool needWaitOtherPlayers = false;

        public void Awake() {
            isRunning = true;
            sneedWaitOtherPlayers = needWaitOtherPlayers;
        }

        public void Start() {
            EventsManager.handler.OnCurrentMatchChanged += (last, currentMatch) => {

                if (!needWaitOtherPlayers || currentMatch.players.Count >= currentMatch.maxPlayersCount) {
                    MatchesManager.SendStartGame();
                }

      
            };
            
            sClient.StartFindingMatch();

        }
    }
}