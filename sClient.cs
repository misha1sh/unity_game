using System;
using System.Collections.Generic;
using CommandsSystem;
using CommandsSystem.Commands;
using Game;
using GameMode;
using JsonRequest;
using Networking;
using UnityEngine;

public class sClient : MonoBehaviour {
    public enum STATE {
        FIND_MATCH,
        IN_GAME
    }
    
    
    public const int NETWORK_FPS = 20;

    public static int ID => InstanceManager.ID;
    public static System.Random random = new System.Random();

    public static int MatchmakingRoom = 1;
    public static int GameRoom = 137;
    public static int GameModeRoom;



    private static float gameStartTime;

    public static STATE state = STATE.FIND_MATCH;
    
    public static void SetGameStarted() {
        gameStartTime = Time.time;
        state = STATE.IN_GAME;
    }

    public static float GameTime => Time.time - gameStartTime;

    private static bool initialized = false;

    public static void Init() {
        if (initialized) return;
        initialized = true;
        InstanceManager.Init();
        CommandsHandler.Init();
        
   //     ID = random.Next();
    }

    private void Awake() {
        Init();
    }


    void Update() {
        CommandsHandler.Update();
        RequestsManager.Update();
        switch (state) {
            case STATE.FIND_MATCH:
                MatchesManager.Update();
                break;
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
    /*  public static sClient client { get; private set; }
      
  
      private void Init() {
          if (client == this) return;
          client = this;
      }*/
}
