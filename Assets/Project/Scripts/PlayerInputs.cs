using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public struct Data : IComponent
    {
        public Vector2 move;
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
        input.move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.shoot = Input.GetMouseButton(0);
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