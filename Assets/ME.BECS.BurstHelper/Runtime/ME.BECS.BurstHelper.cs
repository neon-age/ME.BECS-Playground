namespace ME.BECS {
    [UnityEngine.Scripting.PreserveAttribute]
    public static unsafe class AOTBurstHelper {
        [UnityEngine.Scripting.PreserveAttribute] 
        public static void AOT() {
            StaticSystemTypes<AISystem>.Validate();
BurstCompileOnUpdateNoBurst<AISystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<AISystem>(default);
StaticSystemTypes<PlayerInputsSystem>.Validate();
BurstCompileOnUpdateNoBurst<PlayerInputsSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<PlayerInputsSystem>(default);
StaticSystemTypes<EnemySpawnSystem>.Validate();
BurstCompileOnUpdateNoBurst<EnemySpawnSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<EnemySpawnSystem>(default);
StaticSystemTypes<LifetimeSystem>.Validate();
BurstCompileOnUpdateNoBurst<LifetimeSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<LifetimeSystem>(default);
StaticSystemTypes<KillZoneSystem>.Validate();
BurstCompileOnUpdateNoBurst<KillZoneSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<KillZoneSystem>(default);
StaticSystemTypes<GOTransformSystem>.Validate();
BurstCompileOnUpdate<GOTransformSystem>.MakeMethod(null);
BurstCompileOnDestroy<GOTransformSystem>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<GOTransformSystem>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<GOTransformSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<GOTransformSystem>(default);
BurstCompileMethod.MakeDestroy<GOTransformSystem>(default);
StaticSystemTypes<ManagedCommandsSystem>.Validate();
BurstCompileOnAwakeNoBurst<ManagedCommandsSystem>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ManagedCommandsSystem>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ManagedCommandsSystem>(default);
BurstCompileMethod.MakeUpdate<ManagedCommandsSystem>(default);
StaticSystemTypes<BulletSystem>.Validate();
BurstCompileOnUpdateNoBurst<BulletSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<BulletSystem>(default);
StaticSystemTypes<WeaponSystem>.Validate();
BurstCompileOnUpdateNoBurst<WeaponSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<WeaponSystem>(default);
StaticSystemTypes<CubeTestSystem>.Validate();
BurstCompileOnAwakeNoBurst<CubeTestSystem>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<CubeTestSystem>.MakeMethod(null);
BurstCompileMethod.MakeAwake<CubeTestSystem>(default);
BurstCompileMethod.MakeUpdate<CubeTestSystem>(default);
StaticSystemTypes<HealthSystem>.Validate();
BurstCompileOnUpdateNoBurst<HealthSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<HealthSystem>(default);
StaticSystemTypes<ParticlesEmitterSystem>.Validate();
BurstCompileOnUpdateNoBurst<ParticlesEmitterSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<ParticlesEmitterSystem>(default);
StaticSystemTypes<CharacterSystem>.Validate();
BurstCompileOnUpdateNoBurst<CharacterSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<CharacterSystem>(default);
StaticSystemTypes<GOSyncTransformToEntitySystem>.Validate();
BurstCompileOnUpdateNoBurst<GOSyncTransformToEntitySystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<GOSyncTransformToEntitySystem>(default);
StaticSystemTypes<ME.BECS.TransformAspect.TransformWorldMatrixUpdateSystem>.Validate();
BurstCompileOnUpdateNoBurst<ME.BECS.TransformAspect.TransformWorldMatrixUpdateSystem>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<ME.BECS.TransformAspect.TransformWorldMatrixUpdateSystem>(default);
StaticTypes<Character.Static>.AOT();
StaticTypes<KillZoneData>.AOT();
StaticTypes<Particles.State>.AOT();
StaticTypes<AI.Data>.AOT();
StaticTypes<Weapon.Data>.AOT();
StaticTypes<LifetimeData>.AOT();
StaticTypes<Bullet.State>.AOT();
StaticTypes<Bullet.Config>.AOT();
StaticTypes<Weapon.State>.AOT();
StaticTypes<Character.Inputs>.AOT();
StaticTypes<Bullet.Shared>.AOT();
StaticTypes<CubeTestSystem.Cube>.AOT();
StaticTypes<HealthData>.AOT();
StaticTypes<HitboxLinkData>.AOT();
StaticTypes<Character.State>.AOT();
StaticTypes<GOTransformSystem.TransformCache>.AOT();
StaticTypes<ME.BECS.Views.ViewComponent>.AOT();
StaticTypes<ME.BECS.Views.IsViewRequested>.AOT();
StaticTypes<ME.BECS.Views.MeshRendererComponent>.AOT();
StaticTypes<ME.BECS.Views.MeshFilterComponent>.AOT();
StaticTypes<ME.BECS.Views.EntityViewProviderTag>.AOT();
StaticTypes<ME.BECS.Views.DrawMeshProviderTag>.AOT();
StaticTypes<ME.BECS.UI.UIComponent>.AOT();
StaticTypes<ME.BECS.EntityConfigComponent>.AOT();
StaticTypes<ME.BECS.T1>.AOT();
StaticTypes<ME.BECS.T2>.AOT();
StaticTypes<ME.BECS.TransformAspect.WorldMatrixComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.LocalScaleComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.LocalRotationComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.ParentComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.ChildrenComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.LocalPositionComponent>.AOT();
StaticTypes<ME.BECS.TransformAspect.IsHierarchyDirtyComponent>.AOT();
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
            StaticSystemTypes<AISystem>.Validate();
StaticSystemTypes<PlayerInputsSystem>.Validate();
StaticSystemTypes<EnemySpawnSystem>.Validate();
StaticSystemTypes<LifetimeSystem>.Validate();
StaticSystemTypes<KillZoneSystem>.Validate();
StaticSystemTypes<GOTransformSystem>.Validate();
StaticSystemTypes<ManagedCommandsSystem>.Validate();
StaticSystemTypes<BulletSystem>.Validate();
StaticSystemTypes<WeaponSystem>.Validate();
StaticSystemTypes<CubeTestSystem>.Validate();
StaticSystemTypes<HealthSystem>.Validate();
StaticSystemTypes<ParticlesEmitterSystem>.Validate();
StaticSystemTypes<CharacterSystem>.Validate();
StaticSystemTypes<GOSyncTransformToEntitySystem>.Validate();
StaticSystemTypes<ME.BECS.TransformAspect.TransformWorldMatrixUpdateSystem>.Validate();
StaticTypes<ME.BECS.TransformAspect.LocalScaleComponent>.ApplyGroup(typeof(ME.BECS.TransformAspect.TransformComponentGroup));
StaticTypes<ME.BECS.TransformAspect.LocalPositionComponent>.ApplyGroup(typeof(ME.BECS.TransformAspect.TransformComponentGroup));
StaticTypes<ME.BECS.TransformAspect.ChildrenComponent>.ApplyGroup(typeof(ME.BECS.TransformAspect.TransformComponentGroup));
StaticTypes<ME.BECS.TransformAspect.IsHierarchyDirtyComponent>.ApplyGroup(typeof(ME.BECS.TransformAspect.TransformComponentGroup));
StaticTypes<ME.BECS.TransformAspect.LocalRotationComponent>.ApplyGroup(typeof(ME.BECS.TransformAspect.TransformComponentGroup));
StaticTypes<ME.BECS.TransformAspect.ParentComponent>.ApplyGroup(typeof(ME.BECS.TransformAspect.TransformComponentGroup));
StaticTypes<Character.Static>.Validate(isTag: false);
StaticTypes<KillZoneData>.Validate(isTag: false);
StaticTypes<Particles.State>.Validate(isTag: false);
StaticTypes<AI.Data>.Validate(isTag: false);
StaticTypes<Weapon.Data>.Validate(isTag: false);
StaticTypes<LifetimeData>.Validate(isTag: false);
StaticTypes<Bullet.State>.Validate(isTag: false);
StaticTypes<Bullet.Config>.Validate(isTag: false);
StaticTypes<Weapon.State>.Validate(isTag: false);
StaticTypes<Character.Inputs>.Validate(isTag: false);
StaticTypes<Bullet.Shared>.Validate(isTag: false);
StaticTypes<CubeTestSystem.Cube>.Validate(isTag: false);
StaticTypes<HealthData>.Validate(isTag: false);
StaticTypes<HitboxLinkData>.Validate(isTag: false);
StaticTypes<Character.State>.Validate(isTag: false);
StaticTypes<GOTransformSystem.TransformCache>.Validate(isTag: false);
StaticTypes<ME.BECS.Views.ViewComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.Views.IsViewRequested>.Validate(isTag: true);
StaticTypes<ME.BECS.Views.MeshRendererComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.Views.MeshFilterComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.Views.EntityViewProviderTag>.Validate(isTag: true);
StaticTypes<ME.BECS.Views.DrawMeshProviderTag>.Validate(isTag: true);
StaticTypes<ME.BECS.UI.UIComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.EntityConfigComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.T1>.Validate(isTag: false);
StaticTypes<ME.BECS.T2>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.WorldMatrixComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.LocalScaleComponent>.Validate(isTag: false);
StaticTypesDefaultValue<ME.BECS.TransformAspect.LocalScaleComponent>.value.Data = ME.BECS.TransformAspect.LocalScaleComponent.Default;
StaticTypes<ME.BECS.TransformAspect.LocalRotationComponent>.Validate(isTag: false);
StaticTypesDefaultValue<ME.BECS.TransformAspect.LocalRotationComponent>.value.Data = ME.BECS.TransformAspect.LocalRotationComponent.Default;
StaticTypes<ME.BECS.TransformAspect.ParentComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.ChildrenComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.LocalPositionComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.TransformAspect.IsHierarchyDirtyComponent>.Validate(isTag: false);
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
WorldStaticCallbacks.RegisterCallback<ME.BECS.Network.UnsafeNetworkModule.MethodsStorage>(NetworkLoad);
WorldStaticCallbacks.RegisterCallback<World>(AspectsConstruct);
        }
        public static void ViewsLoad(ref ME.BECS.Views.ViewsModuleData viewsModule) {
ME.BECS.Views.ViewsTypeInfo.RegisterType<BulletView>(new ME.BECS.Views.ViewTypeInfo() {
flags = (ME.BECS.Views.TypeFlags)5,
});
ME.BECS.Views.ViewsTypeInfo.RegisterType<SoundView>(new ME.BECS.Views.ViewTypeInfo() {
flags = (ME.BECS.Views.TypeFlags)4,
});
ME.BECS.Views.ViewsTypeInfo.RegisterType<ME.BECS.Views.DefaultView>(new ME.BECS.Views.ViewTypeInfo() {
flags = (ME.BECS.Views.TypeFlags)0,
});
}
public static void NetworkLoad(ref ME.BECS.Network.UnsafeNetworkModule.MethodsStorage methods) {

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