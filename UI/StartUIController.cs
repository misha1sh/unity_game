using System;
using System.Text;
using CommandsSystem.Commands;
using Events;
using Game;
using GameMode;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class StartUIController : MonoBehaviour {
        public TMP_InputField nameInput;

        public TextMeshProUGUI matchInfoText;
        
        public GameObject JoinUI;
        public GameObject MatchUI;
        
        public static bool specificName = false;
        
        public void Awake() {
            if (specificName)
                nameInput.text = PlayersManager.mainPlayer.name;
            JoinUI.SetActive(true);
            MatchUI.SetActive(false);
        }

        private void Start() {
            MainUIController.mainui.gameObject.SetActive(false);

            EventsManager.handler.OnCurrentMatchChanged += (last, currentMatch) => {

                var text = new StringBuilder();
                text.AppendLine(currentMatch.name);
                text.AppendLine($"Waiting players {currentMatch.players.Count}/{currentMatch.maxPlayersCount}:");
                foreach (var player in currentMatch.players) {
                    string color = player == PlayersManager.mainPlayer.name ? "green" : "red";
                    text.AppendLine($"<color={color}> -{player}</color>");
                }
                matchInfoText.SetText(text.ToString());
                
                
                
                if (currentMatch.players.Count >= currentMatch.maxPlayersCount) {
                    MatchesManager.SendStartGame();
                }

      
            };
        }

        public void OnPlayClicked() {
            if (nameInput.text != "") {
                PlayersManager.mainPlayer.name = nameInput.text;
                specificName = true;
            }

            sClient.StartFindingMatch();
            JoinUI.SetActive(false);
            MatchUI.SetActive(true);

            matchInfoText.text = "Finding matches...";
        }
       
        private void OnDestroy() {
            MainUIController.mainui.gameObject.SetActive(true);
        }
    }
}