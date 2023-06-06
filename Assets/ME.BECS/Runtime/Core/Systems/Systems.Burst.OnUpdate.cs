namespace ME.BECS {

    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Burst;
    using UnityEngine.Scripting;
    using BURST = Unity.Burst.BurstCompileAttribute;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    internal static unsafe class BurstCompileOnUpdateNoBurst<T> where T : unmanaged, IUpdate {

        [Preserve]
        [AOT.MonoPInvokeCallbackAttribute(typeof(FunctionPointerDelegate))]
        public static void CallNoBurst(void* systemData, ref SystemContext context) {

            UnsafeUtility.CopyPtrToStructure(systemData, out T tempData);
            tempData.OnUpdate(ref context);
            UnsafeUtility.CopyStructureToPtr(ref tempData, systemData);

        }

        [INLINE(256)]
        [Preserve]
        public static void MakeMethod(Node* node) {

            BurstCompileMethods.MakeMethodNoBurst<T, FunctionPointerDelegate>(nameof(IAwake.OnAwake), node->dataAwake, CallNoBurst);
            
        }

    }

    [BURST(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
    internal static unsafe class BurstCompileOnUpdate<T> where T : unmanaged, IUpdate {
   
        [Preserve]
        [BURST(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        [AOT.MonoPInvokeCallbackAttribute(typeof(FunctionPointerDelegate))]
        private static void Call(void* systemData, ref SystemContext context) {

            UnsafeUtility.CopyPtrToStructure(systemData, out T tempData);
            tempData.OnUpdate(ref context);
            UnsafeUtility.CopyStructureToPtr(ref tempData, systemData);

        }
        
        [INLINE(256)]
        [Preserve]
        public static void MakeMethod(Node* node) {

            BurstCompileMethods.MakeMethod<T, FunctionPointerDelegate>(nameof(IUpdate.OnUpdate), node->dataUpdate, Call, BurstCompileOnUpdateNoBurst<T>.CallNoBurst);

        }
        
    }

}