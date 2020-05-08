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


    public GameObject totalScorePanel;
    public TextMeshProUGUI totalScoreText;


    
    private string ColorForPlayer(Player player) {
        return PlayersManager.IsMainPlayer(player) ? "green" : "red";
    }
    
    private void RedrawScore() {
        var text = new StringBuilder();
        text.AppendLine("<size=130%>Score</size>");
        foreach (var player in PlayersManager.players) {
            text.AppendLine($"<color={ColorForPlayer(player)}> {player.name}    <pos=65%>{player.score}</color>");
        }

        scoreText.text = text.ToString();
    }


    private string totalScoreTextUnformatted = "{}";
    public void ShowTotalScore(int gamesRemaining, int timeRemaining) {
        totalScorePanel.SetActive(true);
        var text = new StringBuilder();
        text.AppendLine($"<size=130%>  Games Remaining: {gamesRemaining}");
        text.AppendLine("  Time to next game: {0}</size>");
        text.AppendLine();
        text.AppendLine();
        text.AppendLine("<size=115%>Player <pos=35%>Score <pos=65%>Total score</size>");
        foreach (var player in PlayersManager.playersSortedByScore) {
            text.AppendLine($"<color={ColorForPlayer(player)}> {player.name}<pos=35%> {player.score} " +
                            $"<pos=65%>{player.totalScore}(+{PlayersManager.playersCount - player.placeInLastGame + 1})</color>");
        }

        totalScoreTextUnformatted = text.ToString();
        
        SetTotalScoreTimeRemaining(timeRemaining);
    }

    public void SetTotalScoreTimeRemaining(int time) {
        totalScoreText.text = String.Format(totalScoreTextUnformatted, time);
    }

    public void HideTotalScore() {
        totalScorePanel.SetActive(false);
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
