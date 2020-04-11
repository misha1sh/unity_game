using System.Text;
using Character.Guns;
using GameMode;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour {

    public Image gunImage;

    public Sprite pistolSprite;
    public Sprite shotgunSprite;
    public Sprite semiautoSprite;
    public Sprite grenadeLauncherSprite;

    public MultiImagePanel bulletsPanel;
    public MultiImagePanel magazinesPanel;

    public TextMeshProUGUI scoreText;



    private void OnEnable() {

        EventsManager.handler.OnPlayerBulletsCountChanged += (player, count) => {
            if (player != Client.client.mainPlayerObj) return;
            bulletsPanel.SetActiveImagesCount(count);
        };
        EventsManager.handler.OnPlayerMagazinesCountChanged += (player, count) => {
            if (player != Client.client.mainPlayerObj) return;
            magazinesPanel.SetActiveImagesCount(count);
        };
        EventsManager.handler.OnPlayerPickedUpGun += (player, gun) => {
            if (player != Client.client.mainPlayerObj) return;
            gunImage.enabled = true;
            switch (gun) {
                case Pistol pistol:
                    gunImage.sprite = pistolSprite;
                    break;
                case ShotGun shotGun:
                    gunImage.sprite = shotgunSprite;
                    break;
                case SemiautoGun semiautoGun:
                    gunImage.sprite = semiautoSprite;
                    break;
            }

            if (gun is ReloadingGun g) {
                bulletsPanel.SetMaxImagesCount(g.GetBulletsInMagazine());
                bulletsPanel.SetActiveImagesCount(g.bulletsCount);
                magazinesPanel.SetMaxImagesCount(5);
                magazinesPanel.SetActiveImagesCount(g.magazinesCount);
            }
        };

        EventsManager.handler.OnPlayerDroppedGun += (player, gun) => { gunImage.enabled = false; };

        EventsManager.handler.OnPlayerScoreChanged += (_player, score) => {
            var text = new StringBuilder();
            text.AppendLine("<size=130%>Score</size>");
            foreach (var player in PlayersManager.players) {
                if (Client.client.mainPlayer != null && player.id == Client.client.mainPlayer.id) {
                    text.AppendLine($"<color=green> {player.name}    <pos=65%>{player.score}</color>");
                } else {
                    text.AppendLine($"<color=red> {player.name}    <pos=65%>{player.score}</color>");
                }
            }

            scoreText.text = text.ToString();

        };
    }


}
