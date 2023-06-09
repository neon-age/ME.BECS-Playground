using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

public interface IBeginTickInit
{
    public struct Context
    {
        public Ent ent;
        public object userData;
    }
    void OnBeginTickInit(Context ctx);
}

public static class ManagedCommandsExt 
{
    public static void RegisterActionOnBeginTick(this Ent ent, Action<IBeginTickInit.Context> action, object userData = null)
    {
        var world = ent.World;
        ref var system = ref world.GetSystem<ManagedCommandsSystem>();

        var buffer = ManagedCommandsSystem.ManagedBuffers[system.id];
        buffer.commands.Add(new ManagedCommandsSystem.Buffer.Command 
        {
            action = action,
            userData = userData,
        });
    }
    public static void RegisterActionOnBeginTick(this IBeginTickInit initObj, Action<IBeginTickInit.Context> action, object userData = null)
    {
        var ent = (initObj as Component).GetEntity();
        RegisterActionOnBeginTick(ent, action, userData);
    }

    public static void RegisterInitOnBeginTick(this IBeginTickInit initObj, Ent ent, object userData = null)
    {
        var world = ent.World;
        ref var system = ref world.GetSystem<ManagedCommandsSystem>();

        var buffer = ManagedCommandsSystem.ManagedBuffers[system.id];
        buffer.commands.Add(new ManagedCommandsSystem.Buffer.Command 
        {
            ent = ent,
            initObj = initObj,
            userData = userData,
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
            public Action<IBeginTickInit.Context> action;
            public IBeginTickInit initObj;
            public Ent ent;
            public object userData;
        }
        public System.Collections.Generic.List<Command> commands = new(100);
    }
    internal static System.Collections.Generic.List<Buffer> ManagedBuffers = new();

    internal static int IdCounter;
    internal int id;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnLoad()
    {
        IdCounter = -1;
        ManagedBuffers.Clear();
    }

    public void OnAwake(ref SystemContext context)
    {
        id = ++IdCounter;
        ManagedBuffers.Add(new Buffer());
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
                var ctx = new IBeginTickInit.Context { ent = command.ent, userData = command.userData };
                if (command.action != null)
                {
                    command.action.Invoke(ctx);
                }
                if (command.initObj != null)
                {
                    command.initObj.OnBeginTickInit(ctx);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        buffer.commands.Clear();
    }
}
