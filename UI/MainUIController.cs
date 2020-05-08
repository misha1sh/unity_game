using System;
using System.Text;
using Character.Guns;
using GameMode;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class MainUIController : MonoBehaviour {
    public static MainUIController mainui;
    public void Awake() {
        mainui = this;
    }

    public Image gunImage;

    public Sprite pistolSprite;
    public Sprite shotgunSprite;
    public Sprite semiautoSprite;
    public Sprite grenadeLauncherSprite;

    public MultiImagePanel bulletsPanel;
    public MultiImagePanel magazinesPanel;

    public GameObject gunsPanel;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI taskText;


    
    private void RedrawScore() {
        var text = new StringBuilder();
        text.AppendLine("<size=130%>Score</size>");
        foreach (var player in PlayersManager.players) {
            if (PlayersManager.mainPlayer != null && player.id == PlayersManager.mainPlayer.id) {
                text.AppendLine($"<color=green> {player.name}    <pos=65%>{player.score}</color>");
            } else {
                text.AppendLine($"<color=red> {player.name}    <pos=65%>{player.score}</color>");
            }
        }

        scoreText.text = text.ToString();
    }
    

    private void Start() {

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
                case BombGun bombGun:
                    gunImage.sprite = grenadeLauncherSprite;
                    break;
                default:
                    throw new Exception("Unknown gun:" + gun);
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
            RedrawScore();
        };
        
        RedrawScore();
        
        Object.DontDestroyOnLoad(gameObject);
    }


    public void SetTask(string text) {
        taskText.text = "<align=center><size=130%>Task</size></align>\n" + text;
    }



}
