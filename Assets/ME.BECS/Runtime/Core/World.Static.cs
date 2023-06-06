namespace ME.BECS {

    using static Cuts;
    using Internal;
    using BURST = Unity.Burst.BurstCompileAttribute;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    namespace Internal {

        public unsafe struct Array<T> where T : unmanaged {

            public uint Length;
            [Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestrictionAttribute]
            internal T* ptr;

            public ref T Get(int index) {
                E.RANGE(index, 0, this.Length);
                return ref *(this.ptr + index);
            }

            public ref T Get(uint index) {
                E.RANGE(index, 0, this.Length);
                return ref *(this.ptr + index);
            }

            public void Resize(uint length) {

                _resizeArray(ref this.ptr, ref this.Length, length);
                
            }

            public void Dispose() {
                _free(this.ptr);
                this = default;
            }

        }


        public unsafe struct ListUShort {

            public struct Node {

                public ushort data;
                public Node* next;

            }

            public Node* root;
            public uint Count;

            public bool isCreated => this.root != null;

            public ushort[] ToArray() {

                var result = new ushort[this.Count];
                var i = 0;
                var node = this.root;
                while (node != null) {
                    var n = node;
                    result[i++] = n->data;
                    node = node->next;
                }

                return result;

            }

            [INLINE(256)]
            public void Add(ushort value) {

                var node = _make(new Node() { data = value });
                node->next = this.root;
                this.root = node;
                ++this.Count;

            }

            [INLINE(256)]
            public ushort Pop() {

                var root = this.root;
                var val = this.root->data;
                this.root = this.root->next;
                _free(root);
                --this.Count;
                return val;

            }

            [INLINE(256)]
            public bool Remove(ushort value) {

                Node* prevNode = null;
                var node = this.root;
                while (node != null) {
                    if (node->data == value) {
                        if (prevNode == null) {
                            this.root = node->next;
                        } else {
                            prevNode->next = node->next;
                        }

                        _free(node);
                        --this.Count;
                        return true;
                    }

                    prevNode = node;
                    node = node->next;
                }

                return false;

            }

            [INLINE(256)]
            public void Clear() {

                var node = this.root;
                while (node != null) {
                    var n = node;
                    node = node->next;
                    _free(n);
                }

                this.root = null;
                this.Count = 0u;

            }

            [INLINE(256)]
            public void Dispose() {
                this.Clear();
                this = default;
            }

        }

    }

    public struct WorldHeader {

        public World world;
        public Unity.Collections.FixedString64Bytes name;

    }
    
    public struct WorldsStorage {

        private static readonly Unity.Burst.SharedStatic<Array<WorldHeader>> worldsArrBurst = Unity.Burst.SharedStatic<Array<WorldHeader>>.GetOrCreatePartiallyUnsafeWithHashCode<WorldsStorage>(TAlign<Array<WorldHeader>>.align, 10003);
        internal static ref Array<WorldHeader> worlds => ref worldsArrBurst.Data;
        
    }

    public struct WorldsIdStorage {

        private static readonly Unity.Burst.SharedStatic<ListUShort> worldIdsBurst = Unity.Burst.SharedStatic<ListUShort>.GetOrCreatePartiallyUnsafeWithHashCode<WorldsIdStorage>(TAlign<ListUShort>.align, 10001);
        internal static ref ListUShort worldIds => ref worldIdsBurst.Data;

    }
    
    public unsafe struct Worlds {
        
        internal sealed class Destructor {
            ~Destructor() {
                Worlds.CleanUp();
            }
        }

        internal static readonly Destructor staticDestructor = new Destructor();

        private static readonly Unity.Burst.SharedStatic<ushort> worldsCounterBurst = Unity.Burst.SharedStatic<ushort>.GetOrCreate<Worlds>();
        private static ref ushort counter => ref worldsCounterBurst.Data;

        [INLINE(256)]
        public static Array<WorldHeader> GetWorlds() {
            return WorldsStorage.worlds;
        }

        [INLINE(256)]
        public static bool IsAlive(uint id) {

            if (id >= WorldsStorage.worlds.Length) return false;
            return WorldsStorage.worlds.Get(id).world.state != null;

        }
        
        [INLINE(256)]
        public static ref readonly World GetWorld(ushort id) {

            var worldsStorage = WorldsStorage.worlds;
            if (id >= worldsStorage.Length) return ref StaticTypes<World>.defaultValue;

            return ref worldsStorage.Get(id).world;

        }
        
        [INLINE(256)]
        internal static ushort GetNextWorldId() {

            ushort id = 0;
            ref var worldIds = ref WorldsIdStorage.worldIds;
            if (worldIds.Count > 0) {
                id = worldIds.Pop();
            } else {
                id = ++counter;
            }

            return id;

        }

        [INLINE(256)]
        internal static void ReleaseWorldId(ushort worldId) {

            ref var worldIds = ref WorldsIdStorage.worldIds;
            worldIds.Add(worldId);

        }

        [INLINE(256)]
        public static Unity.Collections.FixedString64Bytes GetWorldName(ushort worldId) {
            
            ref var worldsStorage = ref WorldsStorage.worlds;
            if (worldId >= worldsStorage.Length) return default;
            return worldsStorage.Get(worldId).name;

        }

        [INLINE(256)]
        internal static void AddWorld(ref World world, ushort worldId = 0, Unity.Collections.FixedString64Bytes name = default, bool raiseCallback = true) {

            ref var worldsStorage = ref WorldsStorage.worlds;
            if (worldId == 0u) worldId = Worlds.GetNextWorldId();
            world.id = worldId;
            
            if (worldId >= worldsStorage.Length) {
                worldsStorage.Resize((worldId + 1u) * 2u);
            }

            if (name.IsEmpty == true) {
                name = $"World #{world.id}";
            } else {
                name = $"{name.ToString()} (World #{world.id})";
            }
            
            worldsStorage.Get(worldId) = new WorldHeader() {
                world = world,
                name = name,
            };
            
            if (raiseCallback == true) WorldStaticCallbacks.RaiseCallback(ref world);

        }

        [INLINE(256)]
        internal static void ReleaseWorld(in World world) {

            Worlds.ReleaseWorldId(world.id);
            ref var worldsStorage = ref WorldsStorage.worlds;
            worldsStorage.Get(world.id) = default;

        }

        [INLINE(256)]
        internal static void ResetWorldsCounter() {

            counter = 0;
            WorldsIdStorage.worldIds.Clear();
            
        }

        [INLINE(256)]
        internal static void CleanUp() {

            WorldsIdStorage.worldIds.Dispose();
            WorldsStorage.worlds.Dispose();

        }

    }

}