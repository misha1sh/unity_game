using System;
using System.Collections;
using System.Collections.Generic;
using CommandsSystem;
using CommandsSystem.Commands;
using Events;
using Game;
using GameMode;
using JsonRequest;
using Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util2;

/// <summary>
///     Главный класс, управляющий игрой, выбором матча
/// </summary>
public class sClient : MonoBehaviour {
    /// <summary>
    ///     Состояние
    /// </summary>
    public enum STATE {
        START_SCREEN,
        FIND_MATCH,
        IN_GAME
    }

    /// <summary>
    ///     Набирает ли пользователь сейчас тект
    /// </summary>
    public static bool isTyping = false;
    /// <summary>
    ///     Количество сообщений в секунду, отправляемых по сети для синхронизации
    /// </summary>
    public const int NETWORK_FPS = 20;

    /// <summary>
    ///     ID клиента
    /// </summary>
    public static int ID => InstanceManager.ID;
    /// <summary>
    ///     Генератор случайных чисел
    /// </summary>
    public static System.Random random = new System.Random();

    /// <summary>
    ///     Время, когда началась игра
    /// </summary>
    private static float gameStartTime;

    /// <summary>
    ///     Текущее состояния
    /// </summary>
    public static STATE state = STATE.START_SCREEN;
    
    /// <summary>
    ///     Переключает состояние на начало игры
    /// </summary>
    public static void SetGameStarted() {
        if (state != STATE.FIND_MATCH) 
            Debug.LogError("Called SetGameStarted but sClient state is " + state);
        gameStartTime = Time.time;
        state = STATE.IN_GAME;
        MatchesManager.SetMatchIsPlaying();
    }

    /// <summary>
    ///     Время, прошедшее с начала игры
    /// </summary>
    public static float GameTime => Time.time - gameStartTime;

    /// <summary>
    ///     Был ли инициализирован этот класс
    /// </summary>
    private static bool initialized = false;

    /// <summary>
    ///     Инициализирует переменные
    /// </summary>
    public static void Init() {
        if (initialized) return;
        initialized = true;
        InstanceManager.Init();
        PlayersManager.mainPlayer = new Player(sClient.ID, sClient.ID, 0);
    }

    /// <summary>
    ///     Сбрасывает значения переменных
    /// </summary>
    public static void Reset() {
        
        CommandsHandler.Reset();
        MatchesManager.Reset();
        PlayersManager.Reset();
        InstanceManager.Reset();
        GameManager.Reset();
        ObjectID.Clear();
        state = STATE.START_SCREEN;
        if (AutoMatchJoiner.isRunning) {
            sClient.LoadScene("empty_scene");
        } else {
            sClient.LoadScene("start_scene");
        }
    }

    /// <summary>
    ///     Переключает состояние в поиск матча
    /// </summary>
    public static void StartFindingMatch() {
        state = STATE.FIND_MATCH;
    }
    
    /// <summary>
    ///     Инициалилизирует переменные при запуске игры
    /// </summary>
    private void Awake() {
        Init();
    }
    

    /// <summary>
    ///     Обновляет состояние
    /// </summary>
    void Update() {
        if (isTyping)
            Input.ResetInputAxes();

        CommandsHandler.Update();
        RequestsManager.Update();
        switch (state) {
            case STATE.START_SCREEN:
                break;
            case STATE.FIND_MATCH:
                MatchesManager.Update();
                break;
            case STATE.IN_GAME:
                GameManager.Update();
                break;
        }
    }
    
    /// <summary>
    ///     Обрабатывает выход из приложения
    /// </summary>
    private void OnApplicationQuit() {
        CommandsHandler.Stop();
    }
    
    /// <summary>
    ///     Загружает сцену
    /// </summary>
    /// <param name="sceneName"></param>
    public static void LoadScene(string sceneName) {
        UberDebug.LogChannel("Client", "Loading scene " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
