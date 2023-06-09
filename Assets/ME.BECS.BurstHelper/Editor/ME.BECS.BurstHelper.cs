namespace ME.BECS.Editor {
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
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestSystem2_2>.Validate();
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestSystem2_2>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestSystem2_2>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestSystem3>.Validate();
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestSystem3>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestSystem3>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_1>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_1>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_1>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_1>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_1>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_1>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_1>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>.Validate();
BurstCompileOnAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>.MakeMethod(null);
BurstCompileOnUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>.MakeMethod(null);
BurstCompileOnDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>.MakeMethod(null);
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_2>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_2>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_2>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_2>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_2>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_2>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_2>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestSystem2_1>.Validate();
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestSystem2_1>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestSystem2_1>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>.Validate();
BurstCompileOnAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>.MakeMethod(null);
BurstCompileOnUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>.MakeMethod(null);
BurstCompileOnDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>.MakeMethod(null);
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_1>.Validate();
BurstCompileOnUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_1>.MakeMethod(null);
BurstCompileOnDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_1>.MakeMethod(null);
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_1>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_1>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_1>.MakeMethod(null);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_1>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_1>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestSystem1>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestSystem1>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestSystem1>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Systems_Graph.TestSystem1>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestSystem1>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_3>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_3>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_3>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_3>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_3>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_3>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_3>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>.Validate();
BurstCompileOnAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>.MakeMethod(null);
BurstCompileOnUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>.MakeMethod(null);
BurstCompileOnDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>.MakeMethod(null);
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_4>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_4>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_4>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_4>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_4>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_4>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_4>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystemDefer1>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystemDefer1>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystemDefer1>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystemDefer1>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Queries_Static.TestSystemDefer1>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Queries_Static.TestSystemDefer1>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Queries_Static.TestSystemDefer1>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystem2>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystem2>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Queries_Static.TestSystem2>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystem3>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystem3>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Queries_Static.TestSystem3>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>.Validate();
BurstCompileOnAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>.MakeMethod(null);
BurstCompileOnUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>.MakeMethod(null);
BurstCompileOnDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>.MakeMethod(null);
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystem1>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystem1>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystem1>.MakeMethod(null);
BurstCompileOnDestroyNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystem1>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Queries_Static.TestSystem1>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Queries_Static.TestSystem1>(default);
BurstCompileMethod.MakeDestroy<ME.BECS.Tests.Tests_Queries_Static.TestSystem1>(default);
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystem4>.Validate();
BurstCompileOnAwakeNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystem4>.MakeMethod(null);
BurstCompileOnUpdateNoBurst<ME.BECS.Tests.Tests_Queries_Static.TestSystem4>.MakeMethod(null);
BurstCompileMethod.MakeAwake<ME.BECS.Tests.Tests_Queries_Static.TestSystem4>(default);
BurstCompileMethod.MakeUpdate<ME.BECS.Tests.Tests_Queries_Static.TestSystem4>(default);
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
StaticTypes<ME.BECS.Tests.Test5Component>.AOT();
StaticTypes<ME.BECS.Tests.Tests_EntityConfig.TestConfig1Component>.AOT();
StaticTypes<ME.BECS.Tests.Test1Component>.AOT();
StaticTypes<ME.BECS.Tests.Test2Component>.AOT();
StaticTypes<ME.BECS.Tests.Test4Component>.AOT();
StaticTypes<ME.BECS.Tests.TestComponent>.AOT();
StaticTypes<ME.BECS.Tests.Tests_EntityConfig.TestConfig2Component>.AOT();
StaticTypes<ME.BECS.Tests.Test3Component>.AOT();
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
StaticTypesShared<ME.BECS.Tests.Tests_Components_Shared.TestCustom1SharedComponent>.AOT();
StaticTypesShared<ME.BECS.Tests.Tests_Components_Shared.TestCustom2SharedComponent>.AOT();
StaticTypesShared<ME.BECS.Tests.Tests_EntityConfig.TestConfigShared1Component>.AOT();
StaticTypesShared<ME.BECS.Tests.Tests_Components_Shared.TestSharedComponent>.AOT();
StaticTypesStatic<ME.BECS.Tests.Tests_EntityConfig.TestConfig2StaticComponent>.AOT();
StaticTypesStatic<ME.BECS.Tests.Tests_EntityConfig.TestConfig1StaticComponent>.AOT();
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
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestSystem2_2>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestSystem3>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_1>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem3_1>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_2>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestSystem2_1>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_2>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_1>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestSystem1>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_3>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_3>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem1_4>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystemDefer1>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystem2>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystem3>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Systems_Graph.TestGraphSystem2_4>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystem1>.Validate();
StaticSystemTypes<ME.BECS.Tests.Tests_Queries_Static.TestSystem4>.Validate();
StaticSystemTypes<ME.BECS.TransformAspect.TransformWorldMatrixUpdateSystem>.Validate();
StaticTypes<ME.BECS.Tests.TestComponent>.ApplyGroup(typeof(ME.BECS.Tests.TestGroup));
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
StaticTypes<ME.BECS.Tests.Test5Component>.Validate(isTag: false);
StaticTypes<ME.BECS.Tests.Tests_EntityConfig.TestConfig1Component>.Validate(isTag: false);
StaticTypes<ME.BECS.Tests.Test1Component>.Validate(isTag: false);
StaticTypes<ME.BECS.Tests.Test2Component>.Validate(isTag: false);
StaticTypes<ME.BECS.Tests.Test4Component>.Validate(isTag: false);
StaticTypes<ME.BECS.Tests.TestComponent>.Validate(isTag: false);
StaticTypes<ME.BECS.Tests.Tests_EntityConfig.TestConfig2Component>.Validate(isTag: false);
StaticTypes<ME.BECS.Tests.Test3Component>.Validate(isTag: false);
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
StaticTypes<ME.BECS.Tests.Tests_Components_Shared.TestCustom1SharedComponent>.ValidateShared(isTag: false, hasCustomHash: true);
StaticTypes<ME.BECS.Tests.Tests_Components_Shared.TestCustom2SharedComponent>.ValidateShared(isTag: false, hasCustomHash: true);
StaticTypes<ME.BECS.Tests.Tests_EntityConfig.TestConfigShared1Component>.ValidateShared(isTag: false, hasCustomHash: false);
StaticTypes<ME.BECS.Tests.Tests_Components_Shared.TestSharedComponent>.ValidateShared(isTag: false, hasCustomHash: false);
StaticTypes<ME.BECS.Tests.Tests_EntityConfig.TestConfig2StaticComponent>.ValidateStatic(isTag: false);
StaticTypes<ME.BECS.Tests.Tests_EntityConfig.TestConfig1StaticComponent>.ValidateStatic(isTag: false);
AspectTypeInfo<ME.BECS.Tests.Tests_Aspects.TestAspect>.Validate();
AspectTypeInfo<ME.BECS.Tests.Aspect1>.Validate();
AspectTypeInfo<ME.BECS.Tests.Aspect1>.with.Resize(2);
AspectTypeInfo<ME.BECS.Tests.Aspect1>.with.Get(0) = StaticTypes<ME.BECS.Tests.Test1Component>.typeId;
AspectTypeInfo<ME.BECS.Tests.Aspect1>.with.Get(1) = StaticTypes<ME.BECS.Tests.Test2Component>.typeId;
AspectTypeInfo<ME.BECS.Tests.Aspect2>.Validate();
AspectTypeInfo<ME.BECS.Tests.Aspect2>.with.Resize(2);
AspectTypeInfo<ME.BECS.Tests.Aspect2>.with.Get(0) = StaticTypes<ME.BECS.Tests.Test3Component>.typeId;
AspectTypeInfo<ME.BECS.Tests.Aspect2>.with.Get(1) = StaticTypes<ME.BECS.Tests.Test4Component>.typeId;
AspectTypeInfo<ME.BECS.Tests.TestAspect>.Validate();
AspectTypeInfo<ME.BECS.Tests.TestAspect>.with.Resize(1);
AspectTypeInfo<ME.BECS.Tests.TestAspect>.with.Get(0) = StaticTypes<ME.BECS.Tests.TestComponent>.typeId;
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

}
public static void NetworkLoad(ref ME.BECS.Network.UnsafeNetworkModule.MethodsStorage methods) {

}
public static void AspectsConstruct(ref World world) {
{
ref var aspect = ref world.InitializeAspect<ME.BECS.Tests.Tests_Aspects.TestAspect>();
aspect.dataPtr = new ME.BECS.AspectDataPtr<ME.BECS.T1>(in world);
aspect.dataPtr1 = new ME.BECS.AspectDataPtr<ME.BECS.T2>(in world);
}
{
ref var aspect = ref world.InitializeAspect<ME.BECS.Tests.Aspect1>();
aspect.t1Value = new ME.BECS.AspectDataPtr<ME.BECS.Tests.Test1Component>(in world);
aspect.t2Value = new ME.BECS.AspectDataPtr<ME.BECS.Tests.Test2Component>(in world);
}
{
ref var aspect = ref world.InitializeAspect<ME.BECS.Tests.Aspect2>();
aspect.t1Value = new ME.BECS.AspectDataPtr<ME.BECS.Tests.Test3Component>(in world);
aspect.t2Value = new ME.BECS.AspectDataPtr<ME.BECS.Tests.Test4Component>(in world);
}
{
ref var aspect = ref world.InitializeAspect<ME.BECS.Tests.TestAspect>();
aspect.dataPtr1 = new ME.BECS.AspectDataPtr<ME.BECS.Tests.Test1Component>(in world);
aspect.dataPtr2 = new ME.BECS.AspectDataPtr<ME.BECS.Tests.Test2Component>(in world);
aspect.dataPtr3 = new ME.BECS.AspectDataPtr<ME.BECS.Tests.Test3Component>(in world);
aspect.dataPtr4 = new ME.BECS.AspectDataPtr<ME.BECS.Tests.Test4Component>(in world);
aspect.dataPtr5 = new ME.BECS.AspectDataPtr<ME.BECS.Tests.Test5Component>(in world);
aspect.dataPtr = new ME.BECS.AspectDataPtr<ME.BECS.Tests.TestComponent>(in world);
}
}
    }
}