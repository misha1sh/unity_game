
using System;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Character.Guns;
using UnityEditor;
using CommandsSystem;
using CommandsSystem.Commands;
using GameDevWare.Serialization;
using Interpolation;
using Interpolation.Properties;
using Networking;

using Debug = UnityEngine.Debug;
using Object = System.Object;

public class Client : MonoBehaviour
{
    //#if UNITY_WEBGL// && !UNITY_EDITOR
    public const int NETWORK_FPS = 20;


    public testscript TrailRenderer;
    

    public GameObject player = null;
    public GameObject spawnBorder = null;
   

    public List<GameObject> prefabsList = new List<GameObject>();
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    private TrianglePolygon spawnPolygon;
    public static Client client { get; private set; }
    public GameObject mainPlayer;
    
    public int ID => ObjectID.GetID(player);


    public System.Random random = new System.Random();

    public CommandsHandler commandsHandler;


    private float gameStartTime;

    public void SetGameStarted() {
        gameStartTime = Time.time;
    }

    public float GameTime => Time.time - gameStartTime;


    private void OnEnable() {
        client = this;
        
    }


    void Start()
    {
        /*
        var ttest = new IGameObjectProperty<TransformProperty>[10];
        for (int i = 0; i < 5; i++) {
            ttest[i] = new TransformProperty();
            ttest[i].FromGameObject(gameObject);
        }
        
     /*   var bf = new BinaryFormatter();
        var ms = new MemoryStream();
        
        bf.Serialize(ms, ttest);
  /*      var bf = new JsonSerializer();
        var ms = new StringWriter();
        
        bf.Serialize(ms, ttest);
        
        var tr = new StringReader(ms.ToString());
        
        var tttest = bf.Deserialize(tr, ttest.GetType());
        Console.Write(tttest);*/
        /*
        var ms = new StringWriter();
        Json.Serialize(new Pistol(), ms);
        Debug.LogError(ms.ToString());
        */
        Debug.LogError("test");
        Assembly.Load("Assembly-CSharp").GetType("AIController");
        Debug.LogError(Type.GetType("AIController"));
        Debug.Log("CLIENT starting");
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < spawnBorder.transform.childCount; i++)
        {
            points.Add(spawnBorder.transform.GetChild(i).transform.position);
        }
        spawnPolygon = new TrianglePolygon(points);

        foreach (var prefab in prefabsList)
        {
            prefabs.Add(prefab.name, prefab);
            /*for (int i = 0; i < 100; i++)
            {*/
        
           // }


        }

        Assert.IsNotNull(player);
        //ObjectID.StoreObject(player, player.GetInstanceID());

        commandsHandler = new CommandsHandler(new WebSocketHandler());
        commandsHandler.Start();
        commandsHandler.RunUniqCommand(new StartGameCommand(), 1, 1);




    }

    


    struct CapsuleGizmos
    {
        public Vector3 pos1, pos2;
        public float radius;

        public CapsuleGizmos(Vector3 pos1, Vector3 pos2, float radius)
        {
            this.pos1 = pos1;
            this.pos2 = pos2;
            this.radius = radius;
        }
    }

    List<CapsuleGizmos> capsules = new List<CapsuleGizmos>();

    private int lastCointId = -1;
    void Update() {
        if (commandsHandler is null) return;
        commandsHandler.Update();
        foreach (var command in commandsHandler.GetCommands()) {
            if (command is ChangeGameObjectStateCommand<PlayerProperty>) {//command is ChangeGameObjectStateCommand) {
//                Debug.Log("Client got command: ChangeCharacterStateCommand " + (command as ChangeCharacterStateCommand).state.id);
            } else {
                Debug.Log("Client got command: " + command);
            }
            
            command.Run();
        }


        if (FindObjectsOfType<CoinPicker>().Length < 15 && Random.value < 0.05)
        {

            Vector3 pos = FindPlaceForSpawn(10, 1);
            commandsHandler.RunUniqCommand(new SpawnPrefabCommand("coin", pos, new Quaternion()), 1, lastCointId++);
        }

        //Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        /*  if (Random.value < 0.01) {
              Test1 a = new Test1();
              a.a = 123;
              a.c = "kek wer wqe rqw ewq ";
              MemoryStream s = new MemoryStream();
              MsgPack.Serialize(a, s);
              await _webSocket.Send(s.ToArray());
          }*/

    }

    public Vector3 FindPlaceForSpawn(float height, float radius)
    {
        Vector3 pos1, pos2;
        int iterCount = 0;
        do
        {
            pos1 = pos2 = spawnPolygon.RandomPoint();
            pos1.y = -3;
            pos2.y = height;
            Assert.IsTrue(iterCount++ < 100, $"Unable to find free place for object with height: {height:F2}, radius: {radius:F2}");
        } while ((Physics.OverlapCapsule(pos1, pos2, radius).Length > 1));
  //      capsules.Add(new CapsuleGizmos(pos1, pos2, radius));

        return pos2;
    }

    private void OnDrawGizmos()
    {
        foreach (var capsule in capsules)
        {
            DebugExtension.DrawCapsule(capsule.pos1, capsule.pos2, Color.blue, capsule.radius);
        }
    }
    

    

    public void RemoveObject(GameObject gameObject)
    {
        ObjectID.RemoveObject(gameObject);
        Destroy(gameObject);
    }

    public GameObject SpawnObject(SpawnPrefabCommand command)
    {
        var prefab = prefabs[command.prefabName];
        var gameObject = Instantiate(prefab, command.position, command.rotation);
        ObjectID.StoreObject(gameObject, command.id);
        Debug.Log($"Spawned {gameObject}({gameObject.GetInstanceID()}). id: {command.id}");
        return gameObject;
    }

    private void OnApplicationQuit() {
        commandsHandler?.Stop();
    }

//#endif
}