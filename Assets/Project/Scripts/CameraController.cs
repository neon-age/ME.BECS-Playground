using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float3 worldOffset;

    void Start()
    {
    }
    void FixedUpdate()
    {
        var playerEnt = PlayerInputsSystem.PlayerEnt;
        var playerPos = playerEnt.Read<LocalPositionComponent>();

        transform.position = playerPos.value + worldOffset;
    }
}
