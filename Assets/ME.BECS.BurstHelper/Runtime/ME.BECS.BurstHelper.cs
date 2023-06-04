namespace ME.BECS {
    [UnityEngine.Scripting.PreserveAttribute]
    public static unsafe class AOTBurstHelper {
        [UnityEngine.Scripting.PreserveAttribute] 
        public static void AOT() {
            StaticSystemTypes<BulletSystem>.Validate();
BurstCompileOnUpdateNoBurst<BulletSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<BulletSystem>(default);
StaticSystemTypes<WeaponSystem>.Validate();
BurstCompileOnUpdateNoBurst<WeaponSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<WeaponSystem>(default);
StaticSystemTypes<CharacterSystem>.Validate();
BurstCompileOnUpdateNoBurst<CharacterSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<CharacterSystem>(default);
StaticSystemTypes<PlayerInputsSystem>.Validate();
BurstCompileOnUpdateNoBurst<PlayerInputsSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<PlayerInputsSystem>(default);
StaticSystemTypes<GOTransformSystem>.Validate();
BurstCompileOnUpdate<GOTransformSystem>.MakeMethod(null);
BurstCompileOnDestroy<GOTransformSystem>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<GOTransformSystem>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<GOTransformSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<GOTransformSystem>(default);
BurstCompileMethod.MakeDestroy<GOTransformSystem>(default);
StaticSystemTypes<ME.BECS.TransformAspect.TransformWorldMatrixUpdateSystem>.Validate();
BurstCompileOnUpdateNoBurst<ME.BECS.TransformAspect.TransformWorldMatrixUpdateSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<ME.BECS.TransformAspect.TransformWorldMatrixUpdateSystem>(default);
StaticTypes<CharacterState>.AOT();
StaticTypes<WeaponData>.AOT();
StaticTypes<Character.Data>.AOT();
StaticTypes<PlayerInputs.Data>.AOT();
StaticTypes<BulletData>.AOT();
StaticTypes<GOTransform>.AOT();
StaticTypes<WeaponState>.AOT();
StaticTypes<ME.BECS.Views.ViewComponent>.AOT();
StaticTypes<ME.BECS.Views.IsViewRequested>.AOT();
StaticTypes<ME.BECS.Views.MeshRendererComponent>.AOT();
StaticTypes<ME.BECS.Views.MeshFilterComponent>.AOT();
StaticTypes<ME.BECS.Views.EntityViewProviderTag>.AOT();
StaticTypes<ME.BECS.Views.DrawMeshProviderTag>.AOT();
StaticTypes<ME.BECS.T1>.AOT();
StaticTypes<ME.BECS.T2>.AOT();
StaticTypes<ME.BECS.EntityConfigComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.WorldMatrixComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.LocalRotationComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.ParentComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.LocalPositionComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.IsHierarchyDirtyComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.LocalScaleComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.ChildrenComponent>.AOT();
        }
    }
        
    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadAttribute]
    #endif
    [UnityEngine.Scripting.PreserveAttribute]
    public static unsafe class StaticTypesInitializer {
        [UnityEngine.Scripting.PreserveAttribute] 
        static StaticTypesInitializer() { 
            Load();
        }
        [UnityEngine.Scripting.PreserveAttribute] 
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Load() {
            JobUtils.Initialize();
            StaticSystemTypes<BulletSystem>.Validate();
StaticSystemTypes<WeaponSystem>.Validate();
StaticSystemTypes<CharacterSystem>.Validate();
StaticSystemTypes<PlayerInputsSystem>.Validate();
StaticSystemTypes<GOTransformSystem>.Validate();
StaticSystemTypes<ME.BECS.TransformAspect.TransformWorldMatrixUpdateSystem>.Validate();
StaticTypes<ME.BECS.TransformAspect.ParentComponent>.ApplyGroup(10);
StaticTypes<ME.BECS.TransformAspect.LocalScaleComponent>.ApplyGroup(10);
StaticTypes<ME.BECS.TransformAspect.ChildrenComponent>.ApplyGroup(10);
StaticTypes<ME.BECS.TransformAspect.LocalPositionComponent>.ApplyGroup(10);
StaticTypes<ME.BECS.TransformAspect.IsHierarchyDirtyComponent>.ApplyGroup(10);
StaticTypes<ME.BECS.TransformAspect.LocalRotationComponent>.ApplyGroup(10);
StaticTypes<CharacterState>.Validate(isTag: false);
StaticTypes<WeaponData>.Validate(isTag: false);
StaticTypes<Character.Data>.Validate(isTag: false);
StaticTypes<PlayerInputs.Data>.Validate(isTag: false);
StaticTypes<BulletData>.Validate(isTag: false);
StaticTypes<GOTransform>.Validate(isTag: false);
StaticTypes<WeaponState>.Validate(isTag: false);
StaticTypes<ME.BECS.Views.ViewComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.Views.IsViewRequested>.Validate(isTag: true);
StaticTypes<ME.BECS.Views.MeshRendererComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.Views.MeshFilterComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.Views.EntityViewProviderTag>.Validate(isTag: true);
StaticTypes<ME.BECS.Views.DrawMeshProviderTag>.Validate(isTag: true);
StaticTypes<ME.BECS.T1>.Validate(isTag: false);
StaticTypes<ME.BECS.T2>.Validate(isTag: false);
StaticTypes<ME.BECS.EntityConfigComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.WorldMatrixComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.LocalRotationComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.LocalRotationComponent>.defaultValueBurst.Data = ME.BECS.TransformAspect.LocalRotationComponent.Default;
StaticTypes<ME.BECS.TransformAspect.ParentComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.LocalPositionComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.IsHierarchyDirtyComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.LocalScaleComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.LocalScaleComponent>.defaultValueBurst.Data = ME.BECS.TransformAspect.LocalScaleComponent.Default;
StaticTypes<ME.BECS.TransformAspect.ChildrenComponent>.Validate(isTag: false);
AspectTypeInfo<ME.BECS.TestAspect>.Validate();
AspectTypeInfo<ME.BECS.TransformAspect.TransformAspect>.Validate();
AspectTypeInfo<ME.BECS.TransformAspect.TransformAspect>.with.Resize(2);
AspectTypeInfo<ME.BECS.TransformAspect.TransformAspect>.with.Get(0) = StaticTypes<ME.BECS.TransformAspect.LocalPositionComponent>.typeId;
AspectTypeInfo<ME.BECS.TransformAspect.TransformAspect>.with.Get(1) = StaticTypes<ME.BECS.TransformAspect.LocalRotationComponent>.typeId;
        }
    }
    
    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadAttribute]
    #endif
    [UnityEngine.Scripting.PreserveAttribute]
    public static unsafe class StaticMethods {
        [UnityEngine.Scripting.PreserveAttribute] 
        static StaticMethods() {
            Load();
        }
        [UnityEngine.Scripting.PreserveAttribute] 
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Load() {
            WorldStaticCallbacks.RegisterCallback<ME.BECS.Views.ViewsModuleData>(ViewsLoad);
WorldStaticCallbacks.RegisterCallback<UnsafeNetworkModule.MethodsStorage>(NetworkLoad);
WorldStaticCallbacks.RegisterCallback<World>(AspectsConstruct);
        }
        public static void ViewsLoad(ref ME.BECS.Views.ViewsModuleData viewsModule) {
ME.BECS.Views.ViewsTypeInfo.RegisterType<ME.BECS.Views.DefaultView>(new ME.BECS.Views.ViewTypeInfo() {
flags = (ME.BECS.Views.TypeFlags)0,
});
}
public static void NetworkLoad(ref UnsafeNetworkModule.MethodsStorage methods) {
methods.Add(ME.BECS.UnsafeNetworkModule.TestNetMethod);
}
public static void AspectsConstruct(ref World world) {
{
ref var aspect = ref world.InitializeAspect<ME.BECS.TestAspect>();
aspect.t1Value = new ME.BECS.AspectDataPtr<ME.BECS.T1>(in world);
aspect.t2Value = new ME.BECS.AspectDataPtr<ME.BECS.T2>(in world);
}
{
ref var aspect = ref world.InitializeAspect<ME.BECS.TransformAspect.TransformAspect>();
aspect.childrenData = new ME.BECS.RefRO<ME.BECS.TransformAspect.ChildrenComponent>(in world);
aspect.parentData = new ME.BECS.RefRO<ME.BECS.TransformAspect.ParentComponent>(in world);
aspect.isDirty = new ME.BECS.RefRW<ME.BECS.TransformAspect.IsHierarchyDirtyComponent>(in world);
aspect.localPositionData = new ME.BECS.RefRW<ME.BECS.TransformAspect.LocalPositionComponent>(in world);
aspect.localRotationData = new ME.BECS.RefRW<ME.BECS.TransformAspect.LocalRotationComponent>(in world);
aspect.localScaleData = new ME.BECS.RefRW<ME.BECS.TransformAspect.LocalScaleComponent>(in world);
aspect.worldMatrixData = new ME.BECS.RefRW<ME.BECS.TransformAspect.WorldMatrixComponent>(in world);
}
}
    }
}