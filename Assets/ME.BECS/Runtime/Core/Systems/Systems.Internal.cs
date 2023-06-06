namespace ME.BECS {

    using static Cuts;
    using Unity.Jobs;
    using BURST = Unity.Burst.BurstCompileAttribute;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    internal unsafe struct Queue {

        private const uint MIN_GROW = 4;
        private const uint GROW_FACTOR = 200;

        private SystemGroup.NodeData* arr;
        private uint arrLength;

        private uint head;
        private uint tail;
        public uint Count;

        [INLINE(256)]
        public void Clear() {

            this.head = 0u;
            this.tail = 0u;
            this.Count = 0u;

        }

        [INLINE(256)]
        public void Enqueue(Node* node) {

            if (this.Count == this.arrLength) {
                var newCapacity = (this.arrLength * GROW_FACTOR / 100u);
                if (newCapacity < this.arrLength + MIN_GROW) {
                    newCapacity = this.arrLength + MIN_GROW;
                }
                this.SetCapacity(newCapacity);
            }
    
            this.arr[this.tail].data = node;
            this.tail = (this.tail + 1u) % this.arrLength;
            ++this.Count;

        }

        [INLINE(256)]
        private void SetCapacity(uint capacity) {
                
            _resizeArray(ref this.arr, ref this.arrLength, capacity);
                
            this.head = 0u;
            this.tail = (this.Count == capacity) ? 0u : this.Count;
        }
            
        [INLINE(256)]
        public Node* Peek() => this.arr[this.head].data;
            
        [INLINE(256)]
        public Node* Dequeue() {

            --this.Count;
            var removed = this.arr[this.head].data;
            this.arr[this.head] = default;
            this.head = (this.head + 1u) % this.arrLength;
            return removed;

        }

        [INLINE(256)]
        public void Dispose() {

            if (this.arr != null) _free(this.arr);

        }

    }
    
    internal enum Method : byte {

        Undefined = 0,
        Awake,
        Update,
        Destroy,

    }

    internal unsafe struct Node {

        public struct Data {

            public System.Runtime.InteropServices.GCHandle methodHandle;
            public void* methodPtr;

            [INLINE(256)]
            public void Dispose() {

                if (this.methodHandle.IsAllocated == true) this.methodHandle.Free();
                this.methodPtr = null;

            }

        }

        internal uint id;
        internal bool isStarted;
        internal bool isPointer;
        
        internal SystemGroup* graph;

        internal SystemGroup.NodeData* parents;
        private uint parentCount;
        internal uint parentIndex;
        
        internal SystemGroup.NodeData* children;
        private uint childrenCount;
        internal uint childrenIndex;

        internal SystemGroup.NodeData* deps;
        private uint depsCount;
        internal uint depsIndex;

        public JobHandle dependsOn;

        public Unity.Collections.FixedString64Bytes name;
        public void* systemData;
        public uint systemTypeId;
        internal Data* dataAwake;
        internal Data* dataUpdate;
        internal Data* dataDestroy;

        [INLINE(256)]
        public bool AllParentsStarted(Method method) {

            for (uint i = 0; i < this.parentIndex; ++i) {

                if (this.parents[i].data->HasMethod(method) == true && this.parents[i].data->isStarted == false) return false;

            }

            return true;

        }
        
        [INLINE(256)]
        public bool HasMethod(Method method) {
            return this.GetMethod(method) != null;
        }
        
        [INLINE(256)]
        public Data* GetMethod(Method method) {
            Data* ptr = null;
            switch (method) {
                case Method.Awake:
                    ptr = this.dataAwake;
                    break;
                case Method.Update:
                    ptr = this.dataUpdate;
                    break;
                case Method.Destroy:
                    ptr = this.dataDestroy;
                    break;
            }

            return ptr;
        }

        [INLINE(256)]
        public void SetMethod(Method method, Data* ptr) {
            switch (method) {
                case Method.Awake:
                    this.dataAwake = ptr;
                    break;
                case Method.Update:
                    this.dataUpdate = ptr;
                    break;
                case Method.Destroy:
                    this.dataDestroy = ptr;
                    break;
            }
        }

        [INLINE(256)]
        public static Node* CreateMethods<T>(in T system) where T : unmanaged, ISystem {

            var node = Node.Create();
            node->systemData = _make(system);
            node->systemTypeId = StaticSystemTypes<T>.typeId;
            BurstCompileMethod.MakeMethod<T>(node);
            return node;

        }

        [INLINE(256)]
        public JobHandle GetJobHandle() {

            JobHandle handle = default;
            if (this.parentIndex == 1u) {
                handle = this.parents[0].data->dependsOn;
            } else if (this.parentIndex == 2u) {
                handle = JobHandle.CombineDependencies(this.parents[0].data->dependsOn, this.parents[1].data->dependsOn);
            } else if (this.parentIndex == 3u) {
                handle = JobHandle.CombineDependencies(this.parents[0].data->dependsOn, this.parents[1].data->dependsOn, this.parents[2].data->dependsOn);
            } else if (this.parentIndex > 3u) {
                using var list = new Unity.Collections.NativeList<JobHandle>((int)this.parentIndex, Unity.Collections.Allocator.Temp);
                for (uint i = 0; i < this.parentIndex; ++i) {
                    list.Add(this.parents[i].data->dependsOn);
                }
                handle = JobHandle.CombineDependencies(list);
            }

            if (this.dependsOn.IsCompleted == true) return handle;
            return JobHandle.CombineDependencies(handle, this.dependsOn);

        }

        [INLINE(256)]
        public static Node* Create(bool isPointer = false) => _make(new Node() { isPointer = isPointer });

        [INLINE(256)]
        public static Node* Create(SystemGroup graph) => _make(new Node() { graph = _make(graph) });

        [INLINE(256)]
        public void AddDependency(Node* node) {
            
            if (this.depsIndex >= this.depsCount) {
                var newLength = (this.depsIndex + 1u) * 2u;
                _resizeArray(ref this.deps, ref this.depsCount, newLength);
            }

            this.deps[this.depsIndex++] = new SystemGroup.NodeData() { data = node };
            
        }

        [INLINE(256)]
        public void AddChild(Node* node, Node* parent) {
            
            if (this.childrenIndex >= this.childrenCount) {
                var newLength = (this.childrenIndex + 1u) * 2u;
                _resizeArray(ref this.children, ref this.childrenCount, newLength);
            }

            this.children[this.childrenIndex++] = new SystemGroup.NodeData() { data = node };
            node->AddParent(parent);

        }

        [INLINE(256)]
        public void AddParent(Node* node) {
            
            if (this.parentIndex >= this.parentCount) {
                var newLength = (this.parentIndex + 1u) * 2u;
                _resizeArray(ref this.parents, ref this.parentCount, newLength);
            }

            this.parents[this.parentIndex++] = new SystemGroup.NodeData() { data = node };
            
        }

        [INLINE(256)]
        public void Dispose() {

            if (this.graph != null) {
                this.graph->Dispose();
                _free(this.graph);
            }

            if (this.dataAwake != null) {
                this.dataAwake->Dispose();
                _free(this.dataAwake);
            }

            if (this.dataUpdate != null) {
                this.dataUpdate->Dispose();
                _free(this.dataUpdate);
            }

            if (this.dataDestroy != null) {
                this.dataDestroy->Dispose();
                _free(this.dataDestroy);
            }

            if (this.systemData != null) {
                _free(ref this.systemData);
            }
            
            if (this.children != null) {
                _free(this.children);
            }

            if (this.parents != null) {
                _free(this.parents);
            }

            if (this.deps != null) {
                _free(this.deps);
            }

        }

    }

}