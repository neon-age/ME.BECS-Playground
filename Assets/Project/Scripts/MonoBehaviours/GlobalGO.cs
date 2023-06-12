using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS.Addons;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class GlobalGO : MonoBehaviour
{
    static Dictionary<int, GameObject> HandleToInstanceLookup = new();
    
    public GlobalGOHandle handle;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnLoad()
    {
        HandleToInstanceLookup.Clear();
    }

    void Awake()
    {
        HandleToInstanceLookup.Add(handle.GetHashCode(), gameObject);
    }

    public static GameObject GetInstanceByHandle(int id)
    {
        return HandleToInstanceLookup[id];
    }
}
