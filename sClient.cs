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

public class sClient : MonoBehaviour {
    public enum STATE {
        START_SCREEN,
        FIND_MATCH,
     //   WAITING_FOR_START,
        IN_GAME
    }


    public static bool isTyping = false;
    
    public const int NETWORK_FPS = 20;

    public static int ID => InstanceManager.ID;
    public static System.Random random = new System.Random();

    /*public static int MatchmakingRoom = 1;
    public static int GameRoom = 137;
    public static int GameModeRoom;
*/


    private static float gameStartTime;

    public static STATE state = STATE.START_SCREEN;
    
    
    
    public static void SetGameStarted() {
        if (state != STATE.FIND_MATCH) 
            Debug.LogError("Called SetGameStarted but sClient state is " + state);
        gameStartTime = Time.time;
        state = STATE.IN_GAME;
        MatchesManager.SetMatchIsPlaying();
    }

    public static float GameTime => Time.time - gameStartTime;

    private static bool initialized = false;

    public static void Init() {
        if (initialized) return;
        initialized = true;
        InstanceManager.Init();
        CommandsHandler.Init();
        PlayersManager.mainPlayer = new Player(sClient.ID, sClient.ID, 0);
   //     ID = random.Next();
    }

    public static void Reset() {
        
        CommandsHandler.Reset();
        MatchesManager.Reset();
        PlayersManager.Reset();
        InstanceManager.Reset();
        GameManager.Reset();
        state = STATE.START_SCREEN;
        if (AutoMatchJoiner.isRunning) {
            sClient.LoadScene("empty_scene");
        } else {
            sClient.LoadScene("start_scene");
        }
    }

    public static void StartFindingMatch() {
        state = STATE.FIND_MATCH;
    }

    public static void SetupHandlers() {
   /*     EventsManager.handler.OnCurrentMatchChanged += (last, current) => {
            if (state == STATE.FIND_MATCH && current.state == 1) {
                state = STATE.WAITING_FOR_START;
                CommandsHandler.gameRoom.RunUniqCommand(new StartGameCommand(123), UniqCodes.START_GAME, 0,
                    MessageFlags.NONE);
            }
        };*/
    }


    private void Awake() {
        Init();
    }

    /*private void Start() {
        Client.client.SpawnPrefab("empty_canvas");
    }
*/

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
       /*     case STATE.WAITING_FOR_START:
                break;*/
            case STATE.IN_GAME:
                GameManager.Update();
                break;
        }
        //GameManager.Update();
    }

  /*  private static void HandleCommands() {
        if (commandsHandler is null) return;
        webSocketHandler.Update();

        foreach (var command in commandsHandler.GetCommands()) {
            if (command is ChangePlayerProperty || command is DrawTargetedTracerCommand ||
                command is DrawPositionTracerCommand) { //command is ChangeGameObjectStateCommand) {
//                Debug.Log("Client got command: ChangeCharacterStateCommand " + (command as ChangeCharacterStateCommand).state.id);
            } else {
                Debug.Log("Client got command: " + command);
            }

            command.Run();
        }
    }*/

    private void OnApplicationQuit() {
        CommandsHandler.Stop();
    }
    
    
    public static void LoadScene(string sceneName) {
        UberDebug.LogChannel("Client", "Loading scene " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    /*  public static sClient client { get; private set; }
      
  
      private void Init() {
          if (client == this) return;
          client = this;
      }*/
}
