using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS.Addons;
using UnityEngine;

public class GlobalPrefabInstance : MonoBehaviour
{
    public static Dictionary<GameObject, GameObject> PrefabToInstanceLookup = new();
    
    public GameObject prefab;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnLoad()
    {
        PrefabToInstanceLookup.Clear();
    }

    void Awake()
    {
        PrefabToInstanceLookup.Add(prefab, gameObject);
    }

    public static GameObject GetInstanceByPrefab(GameObject prefab)
    {
        return PrefabToInstanceLookup[prefab];
    }
    public static T GetInstanceByPrefab<T>(T prefab) where T : Component
    {
        var go = PrefabToInstanceLookup[prefab.gameObject];
        return go.GetComponent<T>();
    }
}
