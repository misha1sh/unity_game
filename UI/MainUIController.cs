using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Character.Guns;
using CommandsSystem.Commands;
using Events;
using GameMode;
using Networking;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class MainUIController : MonoBehaviour {
    private static MainUIController _mainui;
    private static bool spawned = false;
    public static MainUIController mainui {
        get {
            if (_mainui != null) return _mainui;
            
            _mainui = FindObjectOfType<MainUIController>();
            if (_mainui != null) return _mainui;
            if (spawned) return null; // means unity destroying scene
            
            var go = Client.client.SpawnPrefab("MainUI");
            _mainui = go.GetComponent<MainUIController>();

            return _mainui;
        }
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
    public Button exitButton;


    public GameObject chatPanel;
    public TMP_InputField chatInput;
    public TextMeshProUGUI chatText;
    
    public List<string> chatMessages = new List<string>();


    public string ColorForPlayer(Player player) {
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
        exitButton.gameObject.SetActive(false);
        
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

    public void ShowFinalResults() {
        totalScorePanel.SetActive(true);
        exitButton.gameObject.SetActive(true);
        var text = new StringBuilder();
        text.AppendLine($"<size=130%>  Game Finished");
        var winner = PlayersManager.playersSortedByTotalScore[0];
        text.AppendLine($"  Winner: <color={ColorForPlayer(winner)}>{winner.name}</color></size>");
        text.AppendLine();
        text.AppendLine();
        text.AppendLine("<size=115%>Player <pos=35%>Score <pos=65%>Total score</size>");
        
        foreach (var player in PlayersManager.playersSortedByTotalScore) {
            text.AppendLine($"<color={ColorForPlayer(player)}> {player.name}<pos=35%> {player.score} " +
                            $"<pos=65%>{player.totalScore}(+{PlayersManager.playersCount - player.placeInLastGame + 1})</color>");
        }

        totalScoreText.text = text.ToString();
    }

    public void ExitButtonClicked() {
        sClient.Reset();
    }


    public void SetupHandlers() {
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
    }

    private void Awake() {
        Object.DontDestroyOnLoad(gameObject);
        spawned = true;
        RedrawScore();
        RedrawChat();
        
    }


    public void SetTask(string text) {
        taskText.text = "<align=center><size=130%>Task</size></align>\n" + text;
    }


    public void StartTyping() {
        sClient.isTyping = true;
    }

    public void StopTyping() {
        sClient.isTyping = false;
    }

    public void SendToChat() {
        //chatText.text = chatInput.text;
        string message = chatInput.text;
        CommandsHandler.gameRoom.RunSimpleCommand(new CreateChatMessageCommand(PlayersManager.mainPlayer.id, message), 
            MessageFlags.NONE);
            
        chatInput.text = "";
        StopTyping();
        chatInput.DeactivateInputField(true); 
    }

    public void AddChatMessage(Player player, string message) {
        message = $"<color={ColorForPlayer(player)}>{player.name}: {message}</color>";
        chatMessages.Add(message);
        if (chatMessages.Count > 5)
            chatMessages.RemoveAt(0);
        
        RedrawChat();
        StartCoroutine(DeleteMessageAfterTime(7, message));
    }

    private IEnumerator DeleteMessageAfterTime(float time, string message) {
        yield return new WaitForSeconds(time);
        chatMessages.Remove(message);
        RedrawChat();
    }

    public void RedrawChat() {
        var text = new StringBuilder();
        foreach (var message in chatMessages) {
            text.AppendLine(message);
        }

        chatText.text = text.ToString();
    }
}
