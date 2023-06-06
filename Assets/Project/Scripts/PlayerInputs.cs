using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using Unity.Mathematics;
using UnityEngine;

public struct CharacterInputsData : IComponent, IConfigComponent
{
    [NonSerialized] public float2 move;
    [NonSerialized] public float3 cursorPos;
    [NonSerialized] public bool shoot;
}

public class PlayerInputs : MonoBehaviour
{
    public GOEntity target;
    Vector3 cursorPoint;

    void Start()
    {
        PlayerInputsSystem.PlayerEnt = target.ent;
    }
    void Update() // can't directly set entity data inside mono Update loop, transfered to PlayerInputsSystem IUpdate instead
    {
        PlayerInputsSystem.PlayerEnt = target.ent;
        ref var input = ref PlayerInputsSystem.PlayerInput;

        input.move = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1);
        input.shoot = Input.GetMouseButton(0);

        var cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        var upPlane = new Plane(Vector3.up, Vector3.zero);
        upPlane.Raycast(cursorRay, out var cursorEnter);
        cursorPoint = cursorRay.GetPoint(cursorEnter);
        //if (Physics.Raycast(cursorRay, out var cursorHit))
        //    cursorPoint = cursorHit.point;

        input.cursorPos = cursorPoint;
    }
}
public struct PlayerInputsSystem : IUpdate
{
    public static Ent PlayerEnt;
    public static CharacterInputsData PlayerInput;

    public void OnUpdate(ref SystemContext context)
    {
        if (PlayerEnt.IsAlive())
        {
            ref var input = ref PlayerEnt.Get<CharacterInputsData>();
            input = PlayerInput;
        }
    }
}