using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectID: MonoBehaviour
{
    class ObjectData {
        private WeakReference<GameObject> weakReference;

        public GameObject GameObject {
            get {
                GameObject res;
                if (!weakReference.TryGetTarget(out res)) return null;
                return res;
            }
        }

        public int owner;
        public int creator;

        public ObjectData(GameObject gameObject, int owner, int creator) {
            this.weakReference = new WeakReference<GameObject>(gameObject);
            this.owner = owner;
            this.creator = creator;
        }
    }

    private static Dictionary<int, ObjectData> IDToObject = new Dictionary<int, ObjectData>();
    private static Dictionary<int, int> UnityIDtoObjectID = new Dictionary<int, int>();

    private static System.Random random = new System.Random();
    public static int RandomID => random.Next();
    
    public static void StoreObject(GameObject gameObject, int id, int owner, int creator)
    {
        Assert.IsFalse(IDToObject.ContainsKey(id), $"ID#{id} already exists");
        Assert.IsFalse(UnityIDtoObjectID.ContainsKey(gameObject.GetInstanceID()), $"InstanceID#{gameObject.GetInstanceID()} already exists");
        IDToObject.Add(id, new ObjectData(gameObject, owner, creator));
        UnityIDtoObjectID.Add(gameObject.GetInstanceID(), id);
    }

    public static void StoreOwnedObject(GameObject gameObject, int creator) {
        StoreObject(gameObject, RandomID, sClient.ID, creator);
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

    private static ObjectData GetObjectData(int id) {
        if (!IDToObject.ContainsKey(id))
        {
            throw new KeyNotFoundException($"Gameobject {id} not found in ObjectID");
        }
        return IDToObject[id];
    }
    

    private static ObjectData GetObjectData(GameObject gameObject) {
        return GetObjectData(GetID(gameObject));
    }
    
    
    
    public static int GetOwner(int id) {
        return GetObjectData(id).owner;
    }
    public static int GetOwner(GameObject gameObject) {
        return GetObjectData(gameObject).owner;
    }
    
    public static bool TryGetOwner(int id, out int owner) {
        ObjectData res;
        if (!IDToObject.TryGetValue(id, out res) || res == null) {
            owner = 0;
            return false;
        }

        owner = res.owner;
        return true;
    }


    
    public static bool IsOwned(int id) {
        return GetOwner(id) == sClient.ID;
    }
    public static bool IsOwned(GameObject gameObject) {
        return GetOwner(gameObject) == sClient.ID;
    }
    public static void SetOwner(int id, int owner) {
        IDToObject[id].owner = owner;
    }
    
    
    
    public static int GetCreator(int id) {
        return GetObjectData(id).creator;
    }
    public static int GetCreator(GameObject gameObject) {
        return GetObjectData(gameObject).creator;
    }
    public static bool TryGetCreator(int id, out int creator) {
        ObjectData res;
        if (!IDToObject.TryGetValue(id, out res) || res == null) {
            creator = 0;
            return false;
        }
        creator = res.creator;
        return true;
    }
    
 
    
    


 /*   public static bool TryIsOwned(int id) {
        int owner;
        if (!ObjectID.tr)
    }
    */

 

    public static void Clear() {
        IDToObject.Clear();
        UnityIDtoObjectID.Clear();
    }
}
