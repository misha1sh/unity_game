﻿using UnityEngine;

public class Startup
{
    static bool serializerRegistered = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
    }

#if UNITY_EDITOR


    [UnityEditor.InitializeOnLoadMethod]
    static void EditorInitialize()
    {
        Initialize();
    }

#endif
}