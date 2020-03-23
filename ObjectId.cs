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


    public static void StoreObject(GameObject gameObject, int id)
    {
        Assert.IsFalse(IDToObject.ContainsKey(id), "ID already exists");
        Assert.IsFalse(UnityIDtoObjectID.ContainsKey(gameObject.GetInstanceID()), "InstanceID already exists");
        IDToObject.Add(id, new WeakReference(gameObject));
        UnityIDtoObjectID.Add(gameObject.GetInstanceID(), id);
    }

    public static int GetID(GameObject gameObject)
    {
        return UnityIDtoObjectID[gameObject.GetInstanceID()];
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


    public static void RemoveObject(GameObject gameObject)
    {
        int id = GetID(gameObject);
        Assert.IsTrue(IDToObject.Remove(id));
        Assert.IsTrue(UnityIDtoObjectID.Remove(gameObject.GetInstanceID()));
    }
}
