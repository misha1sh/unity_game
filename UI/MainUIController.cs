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

/// <summary>
///     Компонента, управляющая главным интерфейсом в игре
/// </summary>
public class MainUIController : MonoBehaviour {
    /// <summary>
    ///     Переменная для хранения ссылки на MainUIController
    /// </summary>
    private static MainUIController _mainui;
    /// <summary>
    ///     Был ли создан главный интерфейс
    /// </summary>
    private static bool spawned = false;
    /// <summary>
    ///     Ссылка на MainUIController. Автоматически создаёт интфрейс, если его нет на сцене
    /// </summary>
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

    /// <summary>
    ///     Элемент интерфейса с картинкой текущего оружия
    /// </summary>
    public Image gunImage;

    /// <summary>
    ///     Картинки оружия
    /// </summary>
    public Sprite pistolSprite, shotgunSprite, semiautoSprite, grenadeLauncherSprite;

    /// <summary>
    ///     Элемент интерфейса, показывающий оставшееся количество патронов
    /// </summary>
    public MultiImagePanel bulletsPanel;
    /// <summary>
    ///     Элемент интерфейса, показывающий оставшееся количество магазинов
    /// </summary>
    public MultiImagePanel magazinesPanel;

    /// <summary>
    ///     Панель интфрейса, показывающая информацию об оружии
    /// </summary>
    public GameObject gunsPanel;
    
    /// <summary>
    ///     Текстовый элемент интерфейса, показывающий очки игроков в текущей игре
    /// </summary>
    public TextMeshProUGUI scoreText;
    /// <summary>
    ///     Текстовый элемент интерфейса, показывающий задачу в текущей игре
    /// </summary>
    public TextMeshProUGUI taskText;
    /// <summary>
    ///     Текстовый элемент интерфейса, показывающий время до конца текущей игры
    /// </summary>
    public TextMeshProUGUI timerText;

    /// <summary>
    ///     Панель интерфейса, показывающая результаты игры
    /// </summary>
    public GameObject totalScorePanel;
    /// <summary>
    ///     Текст с результатми игры
    /// </summary>
    public TextMeshProUGUI totalScoreText;
    /// <summary>
    ///     Кнопка выхода из игры
    /// </summary>
    public Button exitButton;

    /// <summary>
    ///     Панель с чатом
    /// </summary>
    public GameObject chatPanel;
    /// <summary>
    ///     Поле для ввода сообщения в чат
    /// </summary>
    public TMP_InputField chatInput;
    /// <summary>
    ///     Текстовый элемент интфрейса, показывающий сообщения в чате
    /// </summary>
    public TextMeshProUGUI chatText;
    
    /// <summary>
    ///     Список с сообщениями в чате
    /// </summary>
    public List<string> chatMessages = new List<string>();

    /// <summary>
    ///     Возвращает цвет, в который нужно раскрашивать ник данного игрока
    /// </summary>
    /// <param name="player">Игрок</param>
    /// <returns>Строка с названием цвета</returns>
    public string ColorForPlayer(Player player) {
        return PlayersManager.IsMainPlayer(player) ? "green" : "red";
    }
    
    /// <summary>
    ///     Перерисовывает текст с очками игроков
    /// </summary>
    private void RedrawScore() {
        var text = new StringBuilder();
        text.AppendLine("<size=130%><align=center>Score</align></size>");
        foreach (var player in PlayersManager.players) {
            text.AppendLine($"<color={ColorForPlayer(player)}> {player.name}    <pos=65%>{player.score}</color>");
        }

        scoreText.text = text.ToString();
    }

    
    /// <summary>
    ///     Текст для результатов игры, в который нужно подставить оставшееся время
    /// </summary>
    private string totalScoreTextUnformatted = "{}";

    /// <summary>
    ///     Добавляет StringBuilder таблицу с резльтатми игры
    /// </summary>
    /// <param name="text">StringBuilder</param>
    private void AddScoreTable(StringBuilder text) {
        text.AppendLine("<size=110%>Player <pos=23%>Score in last game <pos=65%>Total score</size>");
        text.AppendLine();
        foreach (var player in PlayersManager.playersSortedByScore) {
            text.AppendLine($"<color={ColorForPlayer(player)}> {player.name}<pos=23%> {player.score} " +
                            $"<pos=65%>{player.totalScore}(+{PlayersManager.playersCount - player.placeInLastGame + 1})</color>");
        }
    }
    
    /// <summary>
    ///     Показывает таблицу с результатами игры
    /// </summary>
    /// <param name="gamesRemaining">Количество игр, которое осталось сыграть</param>
    /// <param name="timeRemaining">Время, через которое начнётся следующий игровой режим</param>
    public void ShowTotalScore(int gamesRemaining, int timeRemaining) {
        totalScorePanel.SetActive(true);
        exitButton.gameObject.SetActive(false);
        
        var text = new StringBuilder();
        text.AppendLine($"<size=130%>  Games Remaining: {gamesRemaining}");
        text.AppendLine("  Time to next game: {0}</size>");
        text.AppendLine();
        text.AppendLine();
        AddScoreTable(text);

        totalScoreTextUnformatted = text.ToString();
        
        SetTotalScoreTimeRemaining(timeRemaining);
    }

    /// <summary>
    ///     Изменяет оставшееся время в таблице с результатами игры
    /// </summary>
    /// <param name="time">Оставшееся количество времени</param>
    public void SetTotalScoreTimeRemaining(int time) {
        totalScoreText.text = String.Format(totalScoreTextUnformatted, time);
    }

    /// <summary>
    ///     Прекращает показ таблицы с результатми игры
    /// </summary>
    public void HideTotalScore() {
        totalScorePanel.SetActive(false);
    }

    /// <summary>
    ///     Показывает финальную таблицу с результатми игры
    /// </summary>
    public void ShowFinalResults() {
        totalScorePanel.SetActive(true);
        exitButton.gameObject.SetActive(true);
        var text = new StringBuilder();
        text.AppendLine($"<size=130%>  Game Finished");
        var winner = PlayersManager.playersSortedByTotalScore[0];
        text.AppendLine($"  Winner: <color={ColorForPlayer(winner)}>{winner.name}</color></size>");
        text.AppendLine();
        text.AppendLine();
        AddScoreTable(text);

        totalScoreText.text = text.ToString();
    }

    /// <summary>
    ///     Входит из матча. Вызывается Unity при нажатии на кнопку выхода
    /// </summary>
    public void ExitButtonClicked() {
        UberDebug.Log("Client", "exiting match");
        sClient.Reset();
    }


    /// <summary>
    ///     Устанавливает обработчики событий
    /// </summary>
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

        EventsManager.handler.OnPlayerDroppedGun += (player, gun) => {
            if (player != Client.client.mainPlayerObj) return;
            gunImage.enabled = false;
        };

        EventsManager.handler.OnPlayerScoreChanged += (_player, score) => {
            RedrawScore();
        };
    }

    /// <summary>
    ///     Инициализирует переменные и перерисовывает интерфейс
    /// </summary>
    private void Awake() {
        Object.DontDestroyOnLoad(gameObject);
        spawned = true;
        RedrawScore();
        RedrawChat();
    }


    /// <summary>
    ///     Устанавливает текст задачи
    /// </summary>
    /// <param name="text">Текст задачи</param>
    public void SetTask(string text) {
        taskText.text = "<align=center><size=130%>Task</size></align>\n" + text;
    }


    /// <summary>
    ///     Устанавливает состояние на набор текста
    /// </summary>
    public void StartTyping() {
        sClient.isTyping = true;
    }

    /// <summary>
    ///     Был ли в последнем кадре прекращён набор текста
    /// </summary>
    private bool recentlyStoppedTyping = false;
    /// <summary>
    ///     Выключает состояние набора текста
    /// </summary>
    public void StopTyping() {
        recentlyStoppedTyping = true;
        sClient.isTyping = false;
    }

    /// <summary>
    ///     Отправляет сообщение в чат
    /// </summary>
    public void SendToChat() {
        string message = chatInput.text;
        if (message != "")
            CommandsHandler.gameRoom.RunSimpleCommand(new CreateChatMessageCommand(PlayersManager.mainPlayer.id, message), 
                MessageFlags.NONE);
            
        chatInput.text = "";
        StopTyping();
        chatInput.DeactivateInputField(true); 
    }

    /// <summary>
    ///     Добавляет сообщение в чат
    /// </summary>
    /// <param name="player">Игрок, отправивший сообщение</param>
    /// <param name="message">Сообщение</param>
    public void AddChatMessage(Player player, string message) {
        message = $"<color={ColorForPlayer(player)}>{player.name}: {message}</color>";
        chatMessages.Add(message);
        if (chatMessages.Count > 4)
            chatMessages.RemoveAt(0);
        
        RedrawChat();
        StartCoroutine(DeleteMessageAfterTime(7, message));
    }

    /// <summary>
    ///     Корутина для удаления сообщения из чата через время
    /// </summary>
    /// <param name="time">Время, через которое нужно удалить сообщение</param>
    /// <param name="message">Сообщение</param>
    /// <returns>Корутину</returns>
    private IEnumerator DeleteMessageAfterTime(float time, string message) {
        yield return new WaitForSeconds(time);
        chatMessages.Remove(message);
        RedrawChat();
    }

    /// <summary>
    ///     Перерисовывает текст чата
    /// </summary>
    public void RedrawChat() {
        var text = new StringBuilder();
        foreach (var message in chatMessages) {
            text.AppendLine(message);
        }

        chatText.text = text.ToString();
    }

    /// <summary>
    ///     Время, которое сейчас показывается на таймере с обратным отсчётом
    /// </summary>
    private int lasttime = -1;
    /// <summary>
    ///     Устанавливает время на таймере с обратным отсчётом
    /// </summary>
    /// <param name="time">Время</param>
    public void SetTimerTime(int time) {
        if (lasttime == time) return;
        lasttime = time;

        int minutes = time / 60;
        int seconds = time % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    /// <summary>
    ///     Обновляет состояние интерфейса. Автоматически вызывается Unity каждый кадр
    /// </summary>
    private void Update() {
        if (recentlyStoppedTyping) {
            recentlyStoppedTyping = false;
        } else {
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !sClient.isTyping) {
                chatInput.ActivateInputField();
            }
        }
    }
}
