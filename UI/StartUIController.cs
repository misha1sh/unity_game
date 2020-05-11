using System;
using GameMode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class StartUIController : MonoBehaviour {
        public TMP_InputField nameInput;

        public static bool specificName = false;
        
        public void Awake() {
            if (specificName)
                nameInput.text = PlayersManager.mainPlayer.name;
        }

        private void Start() {
            MainUIController.mainui.gameObject.SetActive(false);
        }

        public void OnPlayClicked() {
            if (nameInput.text != "") {
                PlayersManager.mainPlayer.name = nameInput.text;
                specificName = true;
            }

            sClient.StartFindingMatch();
        }

        private void OnDestroy() {
            MainUIController.mainui.gameObject.SetActive(true);
        }
    }
}