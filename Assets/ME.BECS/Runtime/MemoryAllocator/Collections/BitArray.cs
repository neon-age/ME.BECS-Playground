namespace ME.BECS {
    
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
    using Unity.Collections.LowLevel.Unsafe;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(BitArrayDebugView))]
    public unsafe struct BitArray {

        private const int BITS_IN_ULONG = sizeof(ulong) * 8;

        public MemPtr ptr;
        public uint Length;
        [NativeDisableUnsafePtrRestriction]
        private ulong* cachedPtr;

        public bool isCreated => this.ptr.IsValid();

        [INLINE(256)]
        public BitArray(ref MemoryAllocator allocator, uint length, ClearOptions clearOptions = ClearOptions.ClearMemory) {

            var sizeInBytes = Bitwise.AlignULongBits(length);
            this.cachedPtr = null;
            this.ptr = MemoryAllocatorExt.Alloc(ref allocator, sizeInBytes, out var ptr);
            this.cachedPtr = (ulong*)ptr;
            this.Length = length;

            if (clearOptions == ClearOptions.ClearMemory) {
                allocator.MemClear(this.ptr, 0u, sizeInBytes);
            }
        }

        [INLINE(256)]
        public BitArray(ref MemoryAllocator allocator, BitArray source) {

            var sizeInBytes = Bitwise.AlignULongBits(source.Length);
            this.cachedPtr = null;
            this.ptr = MemoryAllocatorExt.Alloc(ref allocator, sizeInBytes, out var ptr);
            this.cachedPtr = (ulong*)ptr;
            this.Length = source.Length;
            var sourcePtr = MemoryAllocatorExt.GetUnsafePtr(in allocator, source.ptr);
            MemoryAllocatorExt.ValidateConsistency(allocator);
            UnsafeUtility.MemCpy(ptr, sourcePtr, sizeInBytes);
            MemoryAllocatorExt.ValidateConsistency(allocator);

        }

        [INLINE(256)]
        public bool ContainsAll(in MemoryAllocator allocator, BitArray other) {

            var len = Bitwise.GetMinLength(other.Length, this.Length);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var ptrOther = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in other.ptr);
            for (var index = 0; index < len; index++) {
                if ((ptr[index] & ptrOther[index]) != ptr[index]) return false;
            }

            return true;

        }

        [INLINE(256)]
        public bool ContainsAll(in MemoryAllocator allocator, BitArray other, TempBitArray otherAdd) {

            var len = Bitwise.GetMinLength(other.Length, this.Length);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var ptrOther = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in other.ptr);
            var ptrOtherAdd = otherAdd.ptr;
            for (var index = 0; index < len; index++) {
                var otherVal = ptrOther[index];
                if (index < otherAdd.Length) otherVal |= ptrOtherAdd[index];
                if ((ptr[index] & otherVal) != ptr[index]) return false;
            }

            return true;

        }

        [INLINE(256)]
        public bool ContainsAll(in MemoryAllocator allocator, TempBitArray other) {

            var len = Bitwise.GetMinLength(other.Length, this.Length);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var ptrOther = other.ptr;
            for (var index = 0; index < len; index++) {
                if ((ptr[index] & ptrOther[index]) != ptr[index]) return false;
            }

            return true;

        }

        [INLINE(256)]
        public bool NotContainsAll(in MemoryAllocator allocator, TempBitArray other) {

            var len = Bitwise.GetMinLength(other.Length, this.Length);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var ptrOther = other.ptr;
            for (var index = 0; index < len; index++) {
                if ((ptr[index] & ptrOther[index]) != 0UL) return false;
            }

            return true;

        }

        [INLINE(256)]
        public void Resize(ref MemoryAllocator allocator, uint newLength, ClearOptions clearOptions = ClearOptions.ClearMemory) {

            if (newLength > this.Length) {
                var newArr = new BitArray(ref allocator, newLength, clearOptions);
                if (this.isCreated == true) {
                    allocator.MemCopy(newArr.ptr, 0L, this.ptr, 0L, Bitwise.AlignULongBits(this.Length));
                    allocator.Free(this.ptr);
                }

                this = newArr;
            }
            
        }

        [INLINE(256)]
        public void BurstMode(in MemoryAllocator allocator, bool state) {
            if (state == true && this.isCreated == true) {
                this.cachedPtr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, this.ptr);
            } else {
                this.cachedPtr = default;
            }
        }

        /// <summary>
        /// Sets all the bits in the bitmap to the specified value.
        /// </summary>
        /// <param name="value">The value to set each bit to.</param>
        /// <returns>The instance of the modified bitmap.</returns>
        [INLINE(256)]
        public void SetAllBits(in MemoryAllocator allocator, bool value) {
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var len = Bitwise.GetLength(this.Length);
            var setValue = value ? ulong.MaxValue : ulong.MinValue;
            for (var index = 0; index < len; index++) {
                ptr[index] = setValue;
            }
        }

        /// <summary>
        /// Gets the value of the bit at the specified index.
        /// </summary>
        /// <param name="index">The index of the bit.</param>
        /// <returns>The value of the bit at the specified index.</returns>
        [INLINE(256)]
        public bool IsSet(in MemoryAllocator allocator, int index) {
            E.RANGE(index, 0, this.Length);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            return (ptr[index / BitArray.BITS_IN_ULONG] & (0x1ul << (index % BitArray.BITS_IN_ULONG))) > 0;
        }

        /// <summary>
        /// Sets the value of the bit at the specified index to the specified value.
        /// </summary>
        /// <param name="index">The index of the bit to set.</param>
        /// <param name="value">The value to set the bit to.</param>
        /// <returns>The instance of the modified bitmap.</returns>
        [INLINE(256)]
        public void Set(in MemoryAllocator allocator, int index, bool value) {
            E.RANGE(index, 0, this.Length);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            if (value == true) {
                ptr[index / BitArray.BITS_IN_ULONG] |= 0x1ul << (index % BitArray.BITS_IN_ULONG);
            } else {
                ptr[index / BitArray.BITS_IN_ULONG] &= ~(0x1ul << (index % BitArray.BITS_IN_ULONG));
            }
        }

        /// <summary>
        /// Takes the union of this bitmap and the specified bitmap and stores the result in this
        /// instance.
        /// </summary>
        /// <param name="bitmap">The bitmap to union with this instance.</param>
        /// <returns>A reference to this instance.</returns>
        [INLINE(256)]
        public void Union(in MemoryAllocator allocator, BitArray bitmap) {
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var otherPtr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in bitmap.ptr);
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                ptr[index] |= otherPtr[index];
            }
        }

        [INLINE(256)]
        public void Union(in MemoryAllocator allocator, TempBitArray bitmap) {
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var otherPtr = bitmap.ptr;
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                ptr[index] |= otherPtr[index];
            }
        }

        /// <summary>
        /// Takes the intersection of this bitmap and the specified bitmap and stores the result in
        /// this instance.
        /// </summary>
        /// <param name="bitmap">The bitmap to intersect with this instance.</param>
        /// <returns>A reference to this instance.</returns>
        [INLINE(256)]
        public void Intersect(in MemoryAllocator allocator, BitArray bitmap) {
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var otherPtr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in bitmap.ptr);
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                ptr[index] &= otherPtr[index];
            }
        }

        /*
        public void Intersect(in MemoryAllocator allocator, TempBitArray bitmap) {
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                ptr[index] &= bitmap.ptr[index];
            }
        }*/

        [INLINE(256)]
        public void Remove(in MemoryAllocator allocator, BitArray bitmap) {
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var otherPtr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in bitmap.ptr);
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                ptr[index] &= ~otherPtr[index];
            }
        }

        [INLINE(256)]
        public void Remove(in MemoryAllocator allocator, TempBitArray bitmap) {
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var otherPtr = bitmap.ptr;
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                ptr[index] &= ~otherPtr[index];
            }
        }

        /// <summary>
        /// Inverts all the bits in this bitmap.
        /// </summary>
        /// <returns>A reference to this instance.</returns>
        [INLINE(256)]
        public void Invert(in MemoryAllocator allocator) {
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var len = Bitwise.GetLength(this.Length);
            for (var index = 0; index < len; ++index) {
                ptr[index] = ~ptr[index];
            }
        }

        /// <summary>
        /// Sets a range of bits to the specified value.
        /// </summary>
        /// <param name="start">The index of the bit at the start of the range (inclusive).</param>
        /// <param name="end">The index of the bit at the end of the range (inclusive).</param>
        /// <param name="value">The value to set the bits to.</param>
        /// <returns>A reference to this instance.</returns>
        [INLINE(256)]
        public void SetRange(in MemoryAllocator allocator, int start, int end, bool value) {
            if (start == end) {
                this.Set(in allocator, start, value);
                return;
            }

            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, in this.ptr);
            var startBucket = start / BitArray.BITS_IN_ULONG;
            var startOffset = start % BitArray.BITS_IN_ULONG;
            var endBucket = end / BitArray.BITS_IN_ULONG;
            var endOffset = end % BitArray.BITS_IN_ULONG;

            if (value) {
                ptr[startBucket] |= ulong.MaxValue << startOffset;
            } else {
                ptr[startBucket] &= ~(ulong.MaxValue << startOffset);
            }

            for (var bucketIndex = startBucket + 1; bucketIndex < endBucket; bucketIndex++) {
                ptr[bucketIndex] = value ? ulong.MaxValue : ulong.MinValue;
            }

            if (value) {
                ptr[endBucket] |= ulong.MaxValue >> (BitArray.BITS_IN_ULONG - endOffset - 1);
            } else {
                ptr[endBucket] &= ~(ulong.MaxValue >> (BitArray.BITS_IN_ULONG - endOffset - 1));
            }
        }

        [INLINE(256)]
        public void Clear(in MemoryAllocator memoryAllocator) {

            this.SetAllBits(in memoryAllocator, false);

        }

        [INLINE(256)]
        public void Dispose(ref MemoryAllocator allocator) {

            allocator.Free(this.ptr);
            this = default;

        }

        public uint GetReservedSizeInBytes() {
            var sizeInBytes = Bitwise.AlignULongBits(this.Length);
            return sizeInBytes;
        }

    }

    internal sealed unsafe class BitArrayDebugView {

        private BitArray Data;

        public BitArrayDebugView(BitArray data) {
            this.Data = data;
        }

        public bool[] Bits {
            get {
                var allocator = Context.world.state->allocator;
                var array = new bool[this.Data.Length];
                for (var i = 0; i < this.Data.Length; ++i) {
                    array[i] = this.Data.IsSet(in allocator, i);
                }

                return array;
            }
        }

        public int[] BitIndexes {
            get {
                var allocator = Context.world.state->allocator;
                var array = new System.Collections.Generic.List<int>((int)this.Data.Length);
                for (var i = 0; i < this.Data.Length; ++i) {
                    if (this.Data.IsSet(in allocator, i) == true) array.Add(i);
                }

                return array.ToArray();
            }
        }

    }

}