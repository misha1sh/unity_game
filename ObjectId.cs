using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectID: MonoBehaviour
{

    private static Dictionary<int, WeakReference> IDToObject = new Dictionary<int, WeakReference>();
    private static Dictionary<int, int> UnityIDtoObjectID = new Dictionary<int, int>();

    private static System.Random random = new System.Random();
    public static int RandomID => random.Next();
    
    public static void StoreObject(GameObject gameObject, int id)
    {
        Assert.IsFalse(IDToObject.ContainsKey(id), "ID already exists");
        Assert.IsFalse(UnityIDtoObjectID.ContainsKey(gameObject.GetInstanceID()), "InstanceID already exists");
        IDToObject.Add(id, new WeakReference(gameObject));
        UnityIDtoObjectID.Add(gameObject.GetInstanceID(), id);
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

    public static GameObject GetObject(int id)
    {
        if (!IDToObject.ContainsKey(id))
        {
            Debug.LogWarning($"Cannot find object with id: {id}");
            return null;
        }
        return (GameObject)IDToObject[id].Target;
    }


    public static void RemoveObject(GameObject gameObject) {
        int id;
        if (!TryGetID(gameObject, out id)) return;
        Assert.IsTrue(IDToObject.Remove(id));
        Assert.IsTrue(UnityIDtoObjectID.Remove(gameObject.GetInstanceID()));
    }

    public static string ToString() {
        var text = "";
        foreach (var id_obj in IDToObject) {
            var id = id_obj.Key;
            var go = id_obj.Value.Target as GameObject;
            var unityid = (go).GetInstanceID();
            text += $"{go.name}#{unityid} -- {id}\n";
        }

        return text;
    }
}
