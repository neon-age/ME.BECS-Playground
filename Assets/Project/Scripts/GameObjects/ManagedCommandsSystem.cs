using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

public static class ManagedCommandsExt 
{
    public static void RegisterAction(this Ent ent, Action action)
    {
        var world = ent.World;
        ref var system = ref world.GetSystem<ManagedCommandsSystem>();

        var buffer = ManagedCommandsSystem.ManagedBuffers[system.id];
        buffer.commands.Add(new ManagedCommandsSystem.Buffer.Command 
        {
            action = action 
        });
    }
    public static void RegisterInitOnBeginTick(this IBeginTickInit initObj, Ent ent, object userData = null)
    {
        var world = ent.World;
        ref var system = ref world.GetSystem<ManagedCommandsSystem>();

        var buffer = ManagedCommandsSystem.ManagedBuffers[system.id];
        buffer.commands.Add(new ManagedCommandsSystem.Buffer.Command 
        {
            ent = ent,
            initObj = initObj 
        });
    }
    public static void RegisterInitOnBeginTick(this IBeginTickInit initObj, object userData = null)
    {
        var ent = (initObj as Component).GetEntity();
        RegisterInitOnBeginTick(initObj, ent, userData);
    }
}

public struct ManagedCommandsSystem : IAwake, IUpdate
{
    internal class Buffer 
    {
        public struct Command 
        {
            public Action action;
            public IBeginTickInit initObj;
            public Ent ent;
            public object userData;
        }
        public System.Collections.Generic.List<Command> commands = new(100);
    }
    internal static Dictionary<int, Buffer> ManagedBuffers = new();

    internal static int GlobalId;
    internal int id;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnLoad()
    {
        GlobalId = 0;
        ManagedBuffers.Clear();
    }

    public void OnAwake(ref SystemContext context)
    {
        id = ++GlobalId;
        ManagedBuffers.Add(id, new Buffer());
    }

    public void OnUpdate(ref SystemContext context)
    {
        var buffer = ManagedBuffers[id];

        var count = buffer.commands.Count;
        for (int i = 0; i < count; i++)
        {
            var command = buffer.commands[i]; 
            try
            {
                if (command.action != null)
                {
                    command.action.Invoke();
                }
                if (command.initObj != null)
                {
                    command.initObj.OnBeginTickInit(command.ent, command.userData);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        buffer.commands.Clear();
    }
}
