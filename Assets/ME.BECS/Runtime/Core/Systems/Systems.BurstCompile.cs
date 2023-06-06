namespace ME.BECS {

    using static Cuts;
    using System.Reflection;
    using Unity.Burst;
    using System.Runtime.InteropServices;
    using UnityEngine.Scripting;
    using BURST = Unity.Burst.BurstCompileAttribute;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    /// <summary>
    /// Use this to run method in system without burst even when whole system run with burst  
    /// </summary>
    public class WithoutBurstAttribute : System.Attribute {}
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void FunctionPointerDelegate(void* systemData, ref SystemContext context);

    internal unsafe struct BurstCompileMethod {

        private static MethodInfo awakeMethodCache;
        private static MethodInfo awakeMethod {
            get {
                if (awakeMethodCache == null) {
                    awakeMethodCache = typeof(BurstCompileMethod).GetMethod(nameof(BurstCompileMethod.MakeAwake)); 
                }
                return awakeMethodCache;
            }
        }

        private static MethodInfo updateMethodCache;
        private static MethodInfo updateMethod {
            get {
                if (updateMethodCache == null) {
                    updateMethodCache = typeof(BurstCompileMethod).GetMethod(nameof(BurstCompileMethod.MakeUpdate)); 
                }
                return updateMethodCache;
            }
        }

        private static MethodInfo destroyMethodCache;
        private static MethodInfo destroyMethod {
            get {
                if (destroyMethodCache == null) {
                    destroyMethodCache = typeof(BurstCompileMethod).GetMethod(nameof(BurstCompileMethod.MakeDestroy)); 
                }
                return destroyMethodCache;
            }
        }
        
        private static object[] parameters = new object[1];

        [Preserve]
        public static void MakeAwake<T>(System.IntPtr node) where T : unmanaged, IAwake {
            
            BurstCompileOnAwake<T>.MakeMethod((Node*)node);
            
        }

        [Preserve]
        public static void MakeUpdate<T>(System.IntPtr node) where T : unmanaged, IUpdate {
            
            BurstCompileOnUpdate<T>.MakeMethod((Node*)node);
            
        }

        [Preserve]
        public static void MakeDestroy<T>(System.IntPtr node) where T : unmanaged, IDestroy {
            
            BurstCompileOnDestroy<T>.MakeMethod((Node*)node);
            
        }

        private static void MakeMethod_INTERNAL<T>(Node* node, MethodInfo methodInfo) where T : unmanaged, ISystem {
            var gMethod = methodInfo.MakeGenericMethod(typeof(T));
            parameters[0] = (System.IntPtr)node;
            gMethod.Invoke(null, parameters);
        }

        public static Method MakeMethod<T>(Node* node) where T : unmanaged, ISystem {
            
            var method = Method.Undefined;
            
            if (typeof(IAwake).IsAssignableFrom(typeof(T))) {
                method = Method.Awake;
                var methodPtr = _make(new Node.Data());
                node->SetMethod(method, methodPtr);
                BurstCompileMethod.MakeMethod_INTERNAL<T>(node, awakeMethod);
            }
            
            if (typeof(IUpdate).IsAssignableFrom(typeof(T))) {
                method = Method.Update;
                var methodPtr = _make(new Node.Data());
                node->SetMethod(method, methodPtr);
                BurstCompileMethod.MakeMethod_INTERNAL<T>(node, updateMethod);
            }
            
            if (typeof(IDestroy).IsAssignableFrom(typeof(T))) {
                method = Method.Destroy;
                var methodPtr = _make(new Node.Data());
                node->SetMethod(method, methodPtr);
                BurstCompileMethod.MakeMethod_INTERNAL<T>(node, destroyMethod);
            }
            
            return method;
        }

    }

    internal static unsafe class BurstCompileMethods {

        [INLINE(256)]
        [Preserve]
        public static void MakeMethod<T, TDelegate>(string name, Node.Data* method, TDelegate callBurst, TDelegate callNoBurst) where TDelegate : System.Delegate {

            var methodInfo = typeof(T).GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var attr = typeof(T).GetCustomAttribute(typeof(BURST));
            if (attr != null && methodInfo.GetCustomAttribute<WithoutBurstAttribute>() == null) {

                method->methodPtr = (void*)BurstCompiler.CompileFunctionPointer(callBurst).Value;
                return;

            }
            
            // no burst
            var handle = GCHandle.Alloc(callNoBurst);
            method->methodHandle = handle;
            method->methodPtr = (void*)Marshal.GetFunctionPointerForDelegate(callNoBurst);
            
        }

        [INLINE(256)]
        [Preserve]
        public static void MakeMethodNoBurst<T, TDelegate>(string name, Node.Data* method, TDelegate callNoBurst) where TDelegate : System.Delegate {

            var handle = GCHandle.Alloc(callNoBurst);
            method->methodHandle = handle;
            method->methodPtr = (void*)Marshal.GetFunctionPointerForDelegate(callNoBurst);
            
        }

    }
    
}