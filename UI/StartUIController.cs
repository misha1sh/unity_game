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
    /// <summary>
    ///     Компонента для управления стартовым интерфейсом в игре
    /// </summary>
    public class StartUIController : MonoBehaviour {
        /// <summary>
        ///     Поле для ввода ника
        /// </summary>
        public TMP_InputField nameInput;

        /// <summary>
        ///     Текстовый элемент с информацией о текущем матче
        /// </summary>
        public TextMeshProUGUI matchInfoText;
        
        /// <summary>
        ///     Панель с интерфейсом для ввода ника и кнопкой Play
        /// </summary>
        public GameObject JoinUI;
        /// <summary>
        ///     Панель для интерфейса с информацией о чате
        /// </summary>
        public GameObject MatchUI;
        /// <summary>
        ///     Ввёл ли пользователь ник
        /// </summary>
        public static bool specificName = false;
        
        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        public void Awake() {
            if (specificName)
                nameInput.text = PlayersManager.mainPlayer.name;
            JoinUI.SetActive(true);
            MatchUI.SetActive(false);
        }

        /// <summary>
        ///     Устанавливает обработчики событий
        /// </summary>
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

        /// <summary>
        ///     Обрабатывает нажатие кнопки играть
        /// </summary>
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
       
        /// <summary>
        ///     Переключает на главный интерфейс
        /// </summary>
        private void OnDestroy() {
            MainUIController.mainui.gameObject.SetActive(true);
        }
        
        /// <summary>
        ///     При нажатии клавиши enter переходит к поиску матча
        /// </summary>
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                OnPlayClicked();
            }
        }
    }
}