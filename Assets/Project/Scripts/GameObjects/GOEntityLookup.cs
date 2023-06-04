
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ME.BECS;
using UnityEngine;

public static class GOEntityLookup
{
    public static Dictionary<Transform, ulong> TransformToEnt = new Dictionary<Transform, ulong>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnLoad()
    {
        TransformToEnt.Clear();
    }
    
    [MethodImpl(256)]
    public static Ent GetEntity(Transform trs)
    {
        if (!TransformToEnt.TryGetValue(trs, out var id))
        {
            var ent = Ent.New();
            TransformToEnt.Add(trs, id = ent.ToULong());
        }
        return new Ent(id);
    }
    [MethodImpl(256)]
    public static Ent GetEntity(this GameObject go)
    {
        return GetEntity(go.transform);
    }
    [MethodImpl(256)]
    public static Ent GetEntity(this Component component)
    {
        return GetEntity(component.gameObject);
    }

    [MethodImpl(256)]
    public static void RemoveEntity(Transform trs)
    {
        TransformToEnt.Remove(trs);
    }  
    [MethodImpl(256)]
    public static void RemoveEntity(GameObject go)
    {
        RemoveEntity(go.transform);
    } 
    [MethodImpl(256)]
    public static void RemoveEntity(Component component)
    {
        RemoveEntity(component.gameObject);
    }
}