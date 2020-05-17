using TMPro;
using UnityEngine;

namespace GameMode {
    /// <summary>
    ///     Компонента для хранения ссылки игрока внутри персонажа, которым он управляет
    /// </summary>
    public class PlayerStorage : MonoBehaviour {
        /// <summary>
        ///     Переменная для хранения ссылки на игрока
        /// </summary>
        private Player _player;

        /// <summary>
        ///     Панель, на которой должно отображаться имя игрока
        /// </summary>
        public TextMeshProUGUI namePanel;
        
        /// <summary>
        ///     Ссылка на игрока
        /// </summary>
        public Player Player {
            get => _player;
            set {
                _player = value;
                if (_player.id == PlayersManager.mainPlayer.id) {
                    namePanel.text = $"<color=green>{_player.name}</color>";
                } else {
                    namePanel.text = $"<color=red>{_player.name}</color>";
                }
            }
        }
    }
}