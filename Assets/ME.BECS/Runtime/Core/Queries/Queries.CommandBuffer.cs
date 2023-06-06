namespace ME.BECS {

    using static CutsPool;
    using Unity.Jobs;
    using Unity.Collections.LowLevel.Unsafe;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
    using BURST = Unity.Burst.BurstCompileAttribute;

    public readonly unsafe struct CommandBufferJobParallel {

        public readonly CommandBuffer* buffer;
        public readonly uint index;
        public uint Count => this.buffer->count;
        public readonly uint entId;
        public readonly ushort entGen;
        public Ent ent => new Ent(this.entId, this.entGen, this.buffer->worldId);

        [INLINE(256)]
        public CommandBufferJobParallel(CommandBuffer* buffer, uint index) {
            this.buffer = buffer;
            this.index = index;
            this.entId = this.buffer->entities[index];
            this.entGen = this.buffer->state->entities.GetGeneration(this.buffer->state, this.entId);
        }

        [INLINE(256)]
        public ref readonly T Read<T>() where T : unmanaged, IComponent {

            return ref this.buffer->Read<T>(this.entId, this.entGen);

        }

        [INLINE(256)]
        public ref T Get<T>() where T : unmanaged, IComponent {

            return ref this.buffer->Get<T>(this.entId, this.entGen);

        }

        [INLINE(256)]
        public bool Set<T>(in T data) where T : unmanaged, IComponent {

            return this.buffer->Set<T>(this.entId, this.entGen, in data);

        }

        [INLINE(256)]
        public bool Remove<T>() where T : unmanaged, IComponent {

            return this.buffer->Remove<T>(this.entId, this.entGen);

        }

        [INLINE(256)]
        public bool Has<T>() where T : unmanaged, IComponent {

            return this.buffer->Has<T>(this.entId, this.entGen);

        }

    }

    public readonly unsafe struct CommandBufferJobBatch {

        private readonly CommandBuffer* buffer;
        public readonly uint fromIndex;
        public readonly uint toIndex;
        public uint Count => this.buffer->count;

        [INLINE(256)]
        public CommandBufferJobBatch(CommandBuffer* buffer, uint fromIndex, uint toIndex) {
            this.buffer = buffer;
            this.fromIndex = fromIndex;
            this.toIndex = toIndex;
        }

        [INLINE(256)]
        public ref readonly T Read<T>(uint index) where T : unmanaged, IComponent {

            var entId = this.buffer->entities[index];
            return ref this.buffer->Read<T>(entId, this.buffer->state->entities.GetGeneration(this.buffer->state, entId));

        }

        [INLINE(256)]
        public ref T Get<T>(uint index) where T : unmanaged, IComponent {

            var entId = this.buffer->entities[index];
            return ref this.buffer->Get<T>(entId, this.buffer->state->entities.GetGeneration(this.buffer->state, entId));

        }

        [INLINE(256)]
        public bool Set<T>(uint index, in T data) where T : unmanaged, IComponent {
            
            var entId = this.buffer->entities[index];
            return this.buffer->Set<T>(entId, this.buffer->state->entities.GetGeneration(this.buffer->state, entId), in data);

        }

        [INLINE(256)]
        public bool Remove<T>(uint index) where T : unmanaged, IComponent {
            
            var entId = this.buffer->entities[index];
            return this.buffer->Remove<T>(entId, this.buffer->state->entities.GetGeneration(this.buffer->state, entId));

        }

        [INLINE(256)]
        public bool Has<T>(uint index) where T : unmanaged, IComponent {
            
            var entId = this.buffer->entities[index];
            return this.buffer->Has<T>(entId, this.buffer->state->entities.GetGeneration(this.buffer->state, entId));

        }

    }
    
    public readonly unsafe struct CommandBufferJob {

        private readonly uint entId;
        private readonly ushort entGen;
        public readonly CommandBuffer* buffer;
        public uint Count => this.buffer->count;
        public Ent ent => new Ent(this.entId, this.buffer->state, this.buffer->worldId);
        
        [INLINE(256)]
        public CommandBufferJob(in uint entId, ushort gen, CommandBuffer* buffer) {
            this.entId = entId;
            this.entGen = gen;
            this.buffer = buffer;
        }

        [INLINE(256)]
        public ref readonly T Read<T>() where T : unmanaged, IComponent {

            return ref this.buffer->Read<T>(this.entId, this.entGen);

        }

        [INLINE(256)]
        public ref T Get<T>() where T : unmanaged, IComponent {

            return ref this.buffer->Get<T>(this.entId, this.entGen);

        }

        [INLINE(256)]
        public bool Set<T>(in T data) where T : unmanaged, IComponent {

            return this.buffer->Set<T>(this.entId, this.entGen, in data);

        }

        [INLINE(256)]
        public bool Remove<T>() where T : unmanaged, IComponent {

            return this.buffer->Remove<T>(this.entId, this.entGen);

        }

        [INLINE(256)]
        public bool Has<T>() where T : unmanaged, IComponent {

            return this.buffer->Has<T>(this.entId, this.entGen);

        }

    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public unsafe struct CommandBuffer {

        // [!] For some reason ScheduleParallelForDeferArraySize needs ptr first
        // so that's why we need LayoutKind.Sequential and first void* must be here
        // the second must be uint count
        [NativeDisableUnsafePtrRestriction]
        public uint* entities;
        public uint count;
        
        [NativeDisableUnsafePtrRestriction]
        public State* state;
        public ushort worldId;

        public bool sync;

        [INLINE(256)]
        public void BeginForEachRange(uint fromIndex, uint toIndex) {
            
        }

        [INLINE(256)]
        public void EndForEachRange() {
            
        }

        [INLINE(256)]
        public void Dispose() {

            if (this.entities != null) _freeArray(this.entities, this.count);
            this = default;
            
        }

        [INLINE(256)]
        public ref readonly T Read<T>(uint entId, ushort gen) where T : unmanaged, IComponent {

            return ref this.state->components.Read<T>(this.state, entId, gen);

        }

        [INLINE(256)]
        public ref T Get<T>(uint entId, ushort gen) where T : unmanaged, IComponent {
            
            if (this.sync == false && this.Has<T>(entId, gen) == false) {
                E.THREAD_CHECK(nameof(this.Get));
            }
            E.IS_IN_TICK(this.state);
            JobUtils.Lock(ref this.state->components.lockIndex);
            ref var res = ref this.state->components.Get<T>(this.state, entId, gen);
            JobUtils.Unlock(ref this.state->components.lockIndex);
            return ref res;

        }

        [INLINE(256)]
        public bool Set<T>(uint entId, ushort gen, in T data) where T : unmanaged, IComponent {

            if (this.sync == false && this.Has<T>(entId, gen) == false) {
                E.THREAD_CHECK(nameof(this.Set));
                return false;
            }
            E.IS_IN_TICK(this.state);
            JobUtils.Lock(ref this.state->components.lockIndex);
            var res = this.state->components.SetUnknownType(this.state, StaticTypes<T>.typeId, StaticTypes<T>.groupId, entId, gen, in data);
            JobUtils.Unlock(ref this.state->components.lockIndex);
            return res;

        }

        [INLINE(256)]
        public bool Remove<T>(uint entId, ushort gen) where T : unmanaged, IComponent {

            if (this.sync == false) {
                E.THREAD_CHECK(nameof(this.Remove));
                return false;
            } else {
                E.IS_IN_TICK(this.state);
                JobUtils.Lock(ref this.state->components.lockIndex);
                var res = this.state->components.RemoveUnknownType(this.state, StaticTypes<T>.typeId, StaticTypes<T>.groupId, entId, gen);
                JobUtils.Unlock(ref this.state->components.lockIndex);
                return res;
            }

        }

        [INLINE(256)]
        public bool Has<T>(uint entId, ushort gen) where T : unmanaged, IComponent {

            return this.state->components.Has<T>(this.state, entId, gen);

        }

    }

}