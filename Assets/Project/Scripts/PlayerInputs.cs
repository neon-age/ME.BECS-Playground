using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using Unity.Mathematics;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public struct Data : IComponent
    {
        public float2 move;
        public float3 cursorPos;
        public bool shoot;
    }
    public GOEntity target;

    public static PlayerInputs Instance;
    internal Data input;

    void Start()
    {
        Instance = this;
    }
    void Update()
    {
        input.move = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.shoot = Input.GetMouseButton(0);

        var cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var upPlane = new Plane(Vector3.up, Vector3.zero);

        upPlane.Raycast(cursorRay, out var cursorEnter);
        input.cursorPos = cursorRay.GetPoint(cursorEnter);
    }
}
public struct PlayerInputsSystem : IUpdate
{
    public void OnUpdate(ref SystemContext context)
    {
        var ent = PlayerInputs.Instance.target.ent;
        ref var input = ref ent.Get<PlayerInputs.Data>();

        input = PlayerInputs.Instance.input;
    }
}