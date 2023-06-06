namespace ME.BECS {

    [System.Serializable]
    public struct StateProperties {

        [UnityEngine.MinAttribute(1)]
        [UnityEngine.Tooltip("Resize internal storage and fill pools with entities by default. Set up this value to fit max entities count of your world.")]
        public uint entitiesCapacity;
        [UnityEngine.MinAttribute(1)]
        [UnityEngine.Tooltip("Resize components storage per component type.")]
        public uint storageCapacity;
        [UnityEngine.MinAttribute(1)]
        [UnityEngine.Tooltip("Resize archetypes storage. Set up this value to max variations of unique components on entities.")]
        public uint archetypesCapacity;
        [UnityEngine.MinAttribute(0)]
        [UnityEngine.Tooltip("Resize static queries storage. Set up this value to fit max variations of unique static queries.")]
        public uint queriesCapacity;
        [UnityEngine.MinAttribute(0)]
        [UnityEngine.Tooltip("Resize shared components storage. Set up this value to fit max shared components count.")]
        public uint sharedComponentsCapacity;

    }

    [System.Serializable]
    public struct AllocatorProperties {

        [UnityEngine.MinAttribute(MemoryAllocator.MIN_ZONE_SIZE)]
        [UnityEngine.Tooltip("Memory Allocator default size, but it will be resized on demand. Min size is <i>{MIN_ZONE_SIZE_IN_KB} KB</i>. This size is used when new allocator zone created, so be sure you have took right size (Default value for this field 1MB).")]
        public uint sizeInBytesCapacity;

    }

    [System.Serializable]
    public struct WorldProperties {
        
        public static WorldProperties Default => new WorldProperties() {
            stateProperties = new StateProperties() {
                entitiesCapacity = 10000u,
                archetypesCapacity = 100u,
                storageCapacity = 1u,
                queriesCapacity = 100u,
                sharedComponentsCapacity = 10u,
            },
            allocatorProperties = new AllocatorProperties() {
                sizeInBytesCapacity = 1024 * 1024, // 1MB
            },
        };

        [UnityEngine.Tooltip("World's name (Useful in editor only).")]
        public Unity.Collections.FixedString64Bytes name;
        public StateProperties stateProperties;
        public AllocatorProperties allocatorProperties;

    }

}