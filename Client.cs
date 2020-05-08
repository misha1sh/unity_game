
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Reflection;
using Character.Guns;
using CommandsSystem.Commands;
using GameMode;
using Networking;

using Debug = UnityEngine.Debug;

public class Client : MonoBehaviour
{
    //#if UNITY_WEBGL// && !UNITY_EDITOR
    public static Client client { get; private set; }


    public testscript TrailRenderer;
    

    public GameObject spawnBorder = null;
   


    public TrianglePolygon spawnPolygon;
    
    public GameObject mainPlayerObj;
    public GameObject cameraObj;




    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    public List<GameObject> prefabsList = new List<GameObject>();





    private void Awake() {
        client = this;

        sClient.Init();

        if (spawnBorder != null) {
            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < spawnBorder.transform.childCount; i++) {
                points.Add(spawnBorder.transform.GetChild(i).transform.position);
            }

            spawnPolygon = new TrianglePolygon(points);
        }

        foreach (var prefab in prefabsList) {
            prefabs.Add(prefab.name, prefab);
        }

        if (FindObjectsOfType<MainUIController>().Length == 0)
            Instantiate(prefabs["MainUI"]);
        //ObjectID.StoreObject(player, player.GetInstanceID());

        var c = new SpawnPrefabCommand("123123", Vector3.back, Quaternion.identity, 123, 4, 778);
        var f = c.Serialize();
        var d = SpawnPrefabCommand.Deserialize(f);

        Debug.LogError("test");
        Assembly.Load("Assembly-CSharp").GetType("AIController");
        Debug.LogError(Type.GetType("AIController"));
        Debug.Log("CLIENT starting");
    }

    



    public void RemoveObject(GameObject gameObject) {
     /*   if (gameObject.TryGetComponent<Rigidbody>(out var r)) {// need to fire ontriggerexitevent
            r.position = new Vector3(Random.Range(-100000, 100000),
                -10000,
                Random.Range(-100000, 100000));
        } else {
            gameObject.transform.position = new Vector3(Random.Range(-100000, 100000),
                -10000,
                Random.Range(-100000, 100000));
        }*/
        ObjectID.RemoveObject(gameObject);
       Destroy(gameObject);
    }

    public GameObject SpawnObject(SpawnPrefabCommand command)
    {
        if (!prefabs.ContainsKey(command.prefabName)) {
            throw new ArgumentException($"not found prefab '{command.prefabName}' in Client.prefabs");
        }
        GameObject prefab = prefabs[command.prefabName];
        var gameObject = Instantiate(prefab, command.position, command.rotation);
        ObjectID.StoreObject(gameObject, command.id, command.owner, command.creator);
        Debug.Log($"Spawned {gameObject}({gameObject.GetInstanceID()}). id: {command.id}");
        return gameObject;
    }



//#endif
}