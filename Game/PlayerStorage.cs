using TMPro;
using UnityEngine;

namespace GameMode {
    public class PlayerStorage : MonoBehaviour {
        private Player _player;

        public TextMeshProUGUI namePanel;
        
        public Player Player {
            get => _player;
            set {
                _player = value;
                if (_player.id == Client.client.mainPlayer.id) {
                    namePanel.text = $"<color=green>{_player.name}</color>";
                } else {
                    namePanel.text = $"<color=red>{_player.name}</color>";
                }
            }
        }
    }
}