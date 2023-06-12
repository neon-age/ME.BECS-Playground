using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GlobalGOHandle : ScriptableObject
{
    [System.Serializable]
    public struct ID 
    {
        public int id;

        public GameObject GetInstance()
        {
            return GlobalGO.GetInstanceByHandle(id);
        }

        public T GetInstance<T>() where T : Component
        {
            var go = GlobalGO.GetInstanceByHandle(id);
            return go.GetComponent<T>();
        }

        public static implicit operator GlobalGOHandle.ID (GlobalGOHandle handle) => new ID { id = handle.GetHashCode() };
    }

    public GameObject GetInstance()
    {
        return GlobalGO.GetInstanceByHandle(GetHashCode());
    } 
    
    public T GetInstance<T>() where T : Component
    {
        var go = GlobalGO.GetInstanceByHandle(GetHashCode());
        return go.GetComponent<T>();
    }
}
