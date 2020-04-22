using System;
using System.Collections.Generic;
using CommandsSystem.Commands;
using GameMode;
using Networking;
using UnityEngine;

public class sClient : MonoBehaviour {
    public const int NETWORK_FPS = 20;
    
    public static int ID { get; private set; }
    public static System.Random random = new System.Random();
    public static ClientCommandsRoom commandsHandler;

    public static int MatchmakingRoom = 1;
    public static int GameRoom = 137;
    public static int GameModeRoom;



    private static float gameStartTime;

    public static void SetGameStarted() {
        gameStartTime = Time.time;
    }

    public static float GameTime => Time.time - gameStartTime;

    private static bool initialized = false;

    private static void Init() {
        if (initialized) return;
        initialized = true;
        webSocketHandler = new WebSocketHandler();
        webSocketHandler.Start();
        commandsHandler = new ClientCommandsRoom(137);
        commandsHandler.RunUniqCommand(new StartGameCommand(), 1, 1, 1);
        ID = random.Next();
    }

    private void Awake() {
        Init();
    }


    void Update() {
        GameManager.Update();
        HandleCommands();
    }

    private static void HandleCommands() {
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
    }

    private void OnApplicationQuit() {
        webSocketHandler?.Update();
    }
    /*  public static sClient client { get; private set; }
      
  
      private void Init() {
          if (client == this) return;
          client = this;
      }*/
}
