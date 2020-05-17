
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

/// <summary>
///     Класс с функциями для работы с игровым полем
/// </summary>
public class Client : MonoBehaviour
{
    /// <summary>
    ///     Статическая ссылка на Client (синглтон)
    /// </summary>
    public static Client client { get; private set; }

    /// <summary>
    ///     Рендерер следов от пуль
    /// </summary>
    public BulletTrailRenderer bulletTrailRenderer;

    /// <summary>
    ///     Объект, хранящиц границу внутри которой можно создавать объекты
    /// </summary>
    public GameObject spawnBorder = null;
   
    
    /// <summary>
    ///     Граница игрового поля
    /// </summary>
    public TrianglePolygon spawnPolygon;
    
    /// <summary>
    ///    Главный персонаж 
    /// </summary>
    public GameObject mainPlayerObj;
    /// <summary>
    ///     Камера
    /// </summary>
    public GameObject cameraObj;
    
    /// <summary>
    ///     Слоаврь префабов, которые можно создавать на игровом поле
    /// </summary>
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    /// <summary>
    ///     Список префабов, которые можно создавать на игровом поле
    /// </summary>
    public List<GameObject> prefabsList = new List<GameObject>();




    /// <summary>
    ///     Инициализирует переменные
    /// </summary>
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


        var c = new SpawnPrefabCommand("123123", Vector3.back, Quaternion.identity, 123, 4, 778);
        var f = c.Serialize();
        var d = SpawnPrefabCommand.Deserialize(f);

        Debug.LogError("test");
        Assembly.Load("Assembly-CSharp").GetType("AIController");
        Debug.LogError(Type.GetType("AIController"));
        Debug.Log("CLIENT starting");
    }

    


    /// <summary>
    ///     Удаляет объект с игрового поля
    /// </summary>
    /// <param name="gameObject">Объект</param>
    public void RemoveObject(GameObject gameObject) {
        ObjectID.RemoveObject(gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    ///     Создаёт объект на игровом поле
    /// </summary>
    /// <param name="command">Команда для создание объекта</param>
    /// <returns>Созданный объект</returns>
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

    /// <summary>
    ///     Создаёт объект на игровом поле
    /// </summary>
    /// <param name="name">Название префаба</param>
    /// <param name="position">Положение объекта</param>
    /// <param name="rotation">Поворот объекта</param>
    /// <returns>Созданный объект</returns>
    public GameObject SpawnPrefab(string name, Vector3 position = new Vector3(),
        Quaternion rotation = new Quaternion()) {
        return Instantiate(prefabs[name], position, rotation);
    }
}