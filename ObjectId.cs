using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectID: MonoBehaviour
{
    class Pair {
        private WeakReference<GameObject> weakReference;

        public GameObject GameObject {
            get {
                GameObject res;
                if (!weakReference.TryGetTarget(out res)) return null;
                return res;
            }
        }

        public int owner;

        public Pair(GameObject gameObject, int owner) {
            this.weakReference = new WeakReference<GameObject>(gameObject);
            this.owner = owner;
        }
    }

    private static Dictionary<int, Pair> IDToObject = new Dictionary<int, Pair>();
    private static Dictionary<int, int> UnityIDtoObjectID = new Dictionary<int, int>();

    private static System.Random random = new System.Random();
    public static int RandomID => random.Next();
    
    public static void StoreObject(GameObject gameObject, int id, int owner)
    {
        Assert.IsFalse(IDToObject.ContainsKey(id), $"ID#{id} already exists");
        Assert.IsFalse(UnityIDtoObjectID.ContainsKey(gameObject.GetInstanceID()), $"InstanceID#{gameObject.GetInstanceID()} already exists");
        IDToObject.Add(id, new Pair(gameObject, owner));
        UnityIDtoObjectID.Add(gameObject.GetInstanceID(), id);
    }

    public static void StoreOwnedObject(GameObject gameObject) {
        StoreObject(gameObject, RandomID, Client.client.ID);
    }

    public static int GetID(GameObject gameObject) {
        int result;
        if (!UnityIDtoObjectID.TryGetValue(gameObject.GetInstanceID(), out result)) {
            throw new KeyNotFoundException($"Gameobject {gameObject.name}#{gameObject.GetInstanceID()} not found in ObjectID");
        }
        return result;
    }

    public static bool TryGetID(GameObject gameObject, out int result) {
        return UnityIDtoObjectID.TryGetValue(gameObject.GetInstanceID(), out result);
    }

    public static bool TryGetObject(int id, out GameObject gameObject) {
        Pair go;
        var res = IDToObject.TryGetValue(id, out go);
        if (!res) {
            gameObject = null;
            return false;
        } else {
            gameObject = go.GameObject;
        }
        return gameObject != null;
    }
    
    public static GameObject GetObject(int id)
    {
        if (!IDToObject.ContainsKey(id))
        {
            Debug.LogWarning($"Cannot find object with id: {id}");
            return null;
        }
        return IDToObject[id].GameObject;
    }


    public static void RemoveObject(GameObject gameObject) {
        int id;
        if (!TryGetID(gameObject, out id)) return;
        Assert.IsTrue(IDToObject.Remove(id));
        Assert.IsTrue(UnityIDtoObjectID.Remove(gameObject.GetInstanceID()));
    }

    public static  string ToString() {
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

    public static int GetOwner(int id) {
        if (!IDToObject.ContainsKey(id))
        {
            throw new KeyNotFoundException($"Gameobject {id} not found in ObjectID");
        }
        return IDToObject[id].owner;
    }

    public static bool TryGetOwner(int id, out int owner) {
        Pair res;
        if (!IDToObject.TryGetValue(id, out res) || res == null) {
            owner = 0;
            return false;
        }

        owner = res.owner;
        return true;
    }

    public static int GetOwner(GameObject gameObject) {
        return GetOwner(GetID(gameObject));
    }

    public static bool IsOwned(int id) {
        return GetOwner(id) == Client.client.ID;
    }

    public static bool IsOwned(GameObject gameObject) {
        return GetOwner(gameObject) == Client.client.ID;
    }

    public static void SetOwner(int id, int owner) {
        IDToObject[id].owner = owner;
    }

    public static void Clear() {
        IDToObject.Clear();
        UnityIDtoObjectID.Clear();
    }
}
