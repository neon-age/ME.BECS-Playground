using UnityEngine;
using ME.BECS;
using ME.BECS.Network;

[DefaultExecutionOrder(-100000)]
public class ProjectInitializer : WorldInitializer
{
    public static int Tick;

    protected override void Awake()
    {
        Tick = 0;
        base.Awake();
    }

    void FixedUpdate()
    {
        base.LateUpdate();
        base.Update();
        Tick++;
    }

    protected override void LateUpdate()
    {
        // do nothing
    }
    protected override void Update()
    {
        // do nothing
    }
}