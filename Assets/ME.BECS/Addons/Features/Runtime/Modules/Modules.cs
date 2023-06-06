namespace ME.BECS {
    
    using Unity.Jobs;

    public abstract class Module : UnityEngine.ScriptableObject {

        protected internal WorldProperties worldProperties;
        
        public abstract void OnAwake(ref World world);
        public abstract JobHandle OnStart(ref World world, JobHandle dependsOn);
        public abstract JobHandle OnUpdate(JobHandle dependsOn);
        public abstract void OnDestroy();

    }

}