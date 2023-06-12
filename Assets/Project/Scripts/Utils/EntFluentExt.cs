using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ME.BECS;
using ME.BECS.TransformAspect;
using Unity.Mathematics;
using UnityEngine;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;

public static class EntFluentExt
{
    public struct TRS
    {
        public TransformAspect aspect;

        public readonly ref float3 localPosition { [MI(256)] get => ref aspect.localPosition; }
        public readonly ref quaternion localRotation { [MI(256)] get => ref aspect.localRotation; }
        public readonly ref float3 localScale { [MI(256)] get =>  ref aspect.localScale; }

        public float3 position { [MI(256)] get => aspect.position; [MI(256)] set => aspect.position = value; }
        public quaternion rotation { [MI(256)] get => aspect.rotation; [MI(256)] set => aspect.rotation = value; }

        public float3 worldMatrixPosition { [MI(256)] get => aspect.GetWorldMatrixPosition(); }
        public quaternion worldMatrixRotation { [MI(256)] get => aspect.GetWorldMatrixRotation(); }
        public float3 worldMatrixScale { [MI(256)] get => aspect.GetWorldMatrixScale(); }

        [MI(256)]
        public TRS WorldMatrixPosition(out float3 pos)
        {
            pos = aspect.GetWorldMatrixPosition(); return this;
        }
        [MI(256)]
        public TRS WorldMatrixRotation(out quaternion rot)
        {
            rot = aspect.GetWorldMatrixRotation(); return this;
        }
        [MI(256)]
        public TRS WorldMatrixScale(out float3 scale)
        {
            scale = aspect.GetWorldMatrixPosition(); return this;
        }


        [MI(256)] public TRS LocalPosition(out float3 pos) 
        { 
            pos = aspect.readLocalPosition; return this; 
        }
        [MI(256)] public TRS LocalRotation(out quaternion rot) 
        { 
            rot = aspect.readLocalRotation; return this; 
        }  
        [MI(256)] public TRS LocalScale(out float3 scale) 
        {
            scale = aspect.readLocalScale; return this;
        }


        [MI(256)] public TRS LocalPosition(float3 pos)
        { 
            aspect.localPosition = pos; return this; 
        }
        [MI(256)] public TRS LocalRotation(quaternion rot) 
        { 
            aspect.localRotation = rot; return this; 
        }
        [MI(256)] public TRS LocalScale(float3 scale) 
        { 
            aspect.localScale = scale; return this; 
        }
        [MI(256)] public TRS LocalPositionRotation(float3 pos, quaternion rot) 
        { 
            aspect.localPosition = pos; 
            aspect.localRotation = rot; return this; 
        }
        [MI(256)] public TRS LocalPositionRotationScale(float3 pos, quaternion rot, float3 scale) 
        { 
            aspect.localPosition = pos; 
            aspect.localRotation = rot; 
            aspect.localScale = scale; return this; 
        }

        [MI(256)] public TRS LocalPositionRotation(out float3 pos, out quaternion rot) 
        { 
            pos = aspect.readLocalPosition; 
            rot = aspect.readLocalRotation; return this; 
        }
            
        [MI(256)] public TRS LocalPositionRotationScale(out float3 pos, out quaternion rot, out float3 scale) 
        { 
            pos = aspect.readLocalPosition; 
            rot = aspect.readLocalRotation; 
            scale = aspect.readLocalScale; return this; 
        }

        [MI(256)]
        public TRS Position(float3 pos)
        {
            aspect.position = pos; return this;
        }
        [MI(256)]
        public TRS Rotation(quaternion rot)
        {
            aspect.rotation = rot; return this;
        }
        [MI(256)]
        public TRS PositionRotation(float3 pos, quaternion rot)
        {
            aspect.position = pos;
            aspect.rotation = rot; return this;
        }

        [MI(256)]
        public TRS PositionRotation(out float3 pos, out quaternion rot)
        {
            pos = aspect.position;
            rot = aspect.rotation; return this;
        }

        [MI(256)]
        public TRS Position(out float3 pos)
        {
            pos = aspect.position; return this;
        }
        [MI(256)]
        public TRS Rotation(out quaternion rot)
        {
            rot = aspect.rotation; return this;
        }
    }

    [MI(256)]
    public static TRS Transform(this Ent ent) 
    { 
        var trs = default(TRS); 
        trs.aspect = ent.GetAspect<TransformAspect>(); 
        return trs;
    }
}
