
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ME.BECS;
using UnityEngine;

public static class GOEntityLookup
{
    static Dictionary<int, ulong> GoHashToEnt = new Dictionary<int, ulong>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnLoad()
    {
        GoHashToEnt.Clear();
    }
    
    [MethodImpl(256)]
    public static Ent GetEntity(this GameObject go)
    {
        var hash = go.GetHashCode();
        if (!GoHashToEnt.TryGetValue(hash, out var id))
        {
            var ent = Ent.New();
            GoHashToEnt.Add(hash, id = ent.ToULong());
        }
        return new Ent(id);
    }
    [MethodImpl(256)]
    public static Ent GetEntity(this Component component)
    {
        return GetEntity(component.gameObject);
    }
}