using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
///     Класс для хранения объектов по ID
/// </summary>
public class ObjectID: MonoBehaviour
{
    /// <summary>
    ///     Класс для хранения объекта с данными
    /// </summary>
    class ObjectData {
        /// <summary>
        ///     Переменная для хранения ссылки на объект
        /// </summary>
        private WeakReference<GameObject> weakReference;

        /// <summary>
        ///     Ссылка на объект
        /// </summary>
        public GameObject GameObject {
            get {
                GameObject res;
                if (!weakReference.TryGetTarget(out res)) return null;
                return res;
            }
        }

        /// <summary>
        ///     Владелец объекта
        /// </summary>
        public int owner;
        /// <summary>
        ///     Игрок, создавший объект
        /// </summary>
        public int creator;

        /// <summary>
        ///     Конструктор 
        /// </summary>
        /// <param name="gameObject">Объект</param>
        /// <param name="owner">Владелец</param>
        /// <param name="creator">Создатель</param>
        public ObjectData(GameObject gameObject, int owner, int creator) {
            this.weakReference = new WeakReference<GameObject>(gameObject);
            this.owner = owner;
            this.creator = creator;
        }
    }

    /// <summary>
    ///     Слоаврь объектов по ID
    /// </summary>
    private static Dictionary<int, ObjectData> IDToObject = new Dictionary<int, ObjectData>();
    /// <summary>
    ///     Словарь ID по UnityID
    /// </summary>
    private static Dictionary<int, int> UnityIDtoObjectID = new Dictionary<int, int>();

    /// <summary>
    ///     Генератор случайныъ чисел
    /// </summary>
    private static System.Random random = new System.Random();
    /// <summary>
    ///     Случаный ID
    /// </summary>
    public static int RandomID => random.Next();
    
    /// <summary>
    ///     Сохраняет объект
    /// </summary>
    /// <param name="gameObject">Объект</param>
    /// <param name="id">ID объекта</param>
    /// <param name="owner">Владелец объекта</param>
    /// <param name="creator">Создатель объекта</param>
    public static void StoreObject(GameObject gameObject, int id, int owner, int creator) {
        Assert.IsFalse(IDToObject.ContainsKey(id), $"ID#{id} already exists");
        Assert.IsFalse(UnityIDtoObjectID.ContainsKey(gameObject.GetInstanceID()), $"InstanceID#{gameObject.GetInstanceID()} already exists");
        IDToObject.Add(id, new ObjectData(gameObject, owner, creator));
        UnityIDtoObjectID.Add(gameObject.GetInstanceID(), id);
    }

    /// <summary>
    ///     Сохраняет объект, созданный локально
    /// </summary>
    /// <param name="gameObject">Объект</param>
    /// <param name="creator">Создатель</param>
    public static void StoreOwnedObject(GameObject gameObject, int creator) {
        StoreObject(gameObject, RandomID, sClient.ID, creator);
    }

    /// <summary>
    ///     Получает ID объекта
    /// </summary>
    /// <param name="gameObject">Объект</param>
    /// <returns>ID объекта</returns>
    public static int GetID(GameObject gameObject) {
        int result;
        if (!UnityIDtoObjectID.TryGetValue(gameObject.GetInstanceID(), out result)) {
            throw new KeyNotFoundException($"Gameobject {gameObject.name}#{gameObject.GetInstanceID()} not found in ObjectID");
        }
        return result;
    }

    /// <summary>
    ///     Пытается получить ID объекта
    /// </summary>
    /// <param name="gameObject">Объект</param>
    /// <param name="result">ID объекта</param>
    /// <returns>true, если данный объект суещствует. иначе false </returns>
    public static bool TryGetID(GameObject gameObject, out int result) {
        return UnityIDtoObjectID.TryGetValue(gameObject.GetInstanceID(), out result);
    }

    /// <summary>
    ///     Пытается получить объект по ID
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="gameObject">Объект</param>
    /// <returns>true, если объект был найден. иначе false</returns>
    public static bool TryGetObject(int id, out GameObject gameObject) {
        ObjectData go;
        var res = IDToObject.TryGetValue(id, out go);
        if (!res) {
            gameObject = null;
            return false;
        } else {
            gameObject = go.GameObject;
        }
        return gameObject != null;
    }
    
    /// <summary>
    ///     Получает объект по ID
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Объект</returns>
    public static GameObject GetObject(int id)
    {
        if (!IDToObject.ContainsKey(id))
        {
            Debug.LogWarning($"Cannot find object with id: {id}");
            return null;
        }
        return IDToObject[id].GameObject;
    }


    /// <summary>
    ///     Удаляет объект
    /// </summary>
    /// <param name="gameObject">Объект</param>
    public static void RemoveObject(GameObject gameObject) {
        int id;
        if (!TryGetID(gameObject, out id)) return;
        Assert.IsTrue(IDToObject.Remove(id));
        Assert.IsTrue(UnityIDtoObjectID.Remove(gameObject.GetInstanceID()));
    }

    /// <summary>
    ///     Выводит информацию об объектах в строку
    /// </summary>
    /// <returns>Строку с информацией об объектах</returns>
    public new static string ToString() {
        var text = "";
        foreach (var id_obj in IDToObject) {
            var id = id_obj.Key;
            var pair = id_obj.Value;
            var go = pair.GameObject;
            var unityid = go.GetInstanceID();
            text += $"{go.name}#{unityid} -- {id} (owned {pair.owner})\n";
        }

        return text;
    }

    /// <summary>
    ///     Получает данные об объекте по ID
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>Данные об объекте</returns>
    private static ObjectData GetObjectData(int id) {
        if (!IDToObject.ContainsKey(id))
        {
            throw new KeyNotFoundException($"Gameobject {id} not found in ObjectID");
        }
        return IDToObject[id];
    }
    

    /// <summary>
    ///     Получает данные об объекте
    /// </summary>
    /// <param name="gameObject">Объект</param>
    /// <returns>Данные об объекте</returns>
    private static ObjectData GetObjectData(GameObject gameObject) {
        return GetObjectData(GetID(gameObject));
    }
    
    /// <summary>
    ///     Получает владельца объекта
    /// </summary>
    /// <param name="id">ID объекта</param>
    /// <returns>Владельца объекта</returns>
    public static int GetOwner(int id) {
        return GetObjectData(id).owner;
    }
    
    /// <summary>
    ///     Получает владельца объекта
    /// </summary>
    /// <param name="gameObject">Объект</param>
    /// <returns>Владельца объекта</returns>
    public static int GetOwner(GameObject gameObject) {
        return GetObjectData(gameObject).owner;
    }
    
    /// <summary>
    ///     Пытается получить владельца объекта
    /// </summary>
    /// <param name="id">ID объекта</param>
    /// <param name="owner">Владелец</param>
    /// <returns>true, если объект был найден. Иначе false</returns>
    public static bool TryGetOwner(int id, out int owner) {
        ObjectData res;
        if (!IDToObject.TryGetValue(id, out res) || res == null) {
            owner = 0;
            return false;
        }

        owner = res.owner;
        return true;
    }
    
    /// <summary>
    ///     Проверяет, владеет ли данный клиент объектом
    /// </summary>
    /// <param name="id">ID объекта</param>
    /// <returns>true, если владеет. Иначе false</returns>
    public static bool IsOwned(int id) {
        return GetOwner(id) == sClient.ID;
    }
    
    /// <summary>
    ///     Проверяет, владеет ли данный клиент объектом
    /// </summary>
    /// <param name="go">Объект</param>
    /// <returns>true, если владеет. Иначе false</returns>
    public static bool IsOwned(GameObject go) {
        return GetOwner(go) == sClient.ID;
    }
    
    /// <summary>
    ///     Изменяет владельца объекта
    /// </summary>
    /// <param name="id">ID объекта</param>
    /// <param name="owner">Новый владелец</param>
    public static void SetOwner(int id, int owner) {
        IDToObject[id].owner = owner;
    }
    
    
    /// <summary>
    ///     Получает создателя объекта
    /// </summary>
    /// <param name="id">ID объекта</param>
    /// <returns>ID создателя</returns>
    public static int GetCreator(int id) {
        return GetObjectData(id).creator;
    }
    
    /// <summary>
    ///     Получает создателя объекта
    /// </summary>
    /// <param name="gameObject">Объект</param>
    /// <returns>ID создателя</returns>
    public static int GetCreator(GameObject gameObject) {
        return GetObjectData(gameObject).creator;
    }
    
    /// <summary>
    ///     Пытается получить создателя объекта
    /// </summary>
    /// <param name="id">ID объекта</param>
    /// <param name="creator">Создатель объекта</param>
    /// <returns>true, если объект был найден. Иначе false</returns>
    public static bool TryGetCreator(int id, out int creator) {
        ObjectData res;
        if (!IDToObject.TryGetValue(id, out res) || res == null) {
            creator = 0;
            return false;
        }
        creator = res.creator;
        return true;
    }
    
 
    
    /// <summary>
    ///    Очищает значения переменных
    /// </summary>    
    public static void Clear() {
        IDToObject.Clear();
        UnityIDtoObjectID.Clear();
    }
}
