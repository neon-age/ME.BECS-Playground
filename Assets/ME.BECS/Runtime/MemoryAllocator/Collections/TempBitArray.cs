namespace ME.BECS {

    using static CutsPool;
    using Unity.Collections.LowLevel.Unsafe;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(TempBitArrayDebugView))]
    public unsafe struct TempBitArray {

        private const int BITS_IN_ULONG = sizeof(ulong) * 8;

        [NativeDisableUnsafePtrRestriction]
        public readonly ulong* ptr;
        public uint Length;
        internal readonly Unity.Collections.Allocator allocator;

        public bool isCreated => this.ptr != null;

        [INLINE(256)]
        public TempBitArray(uint length, ClearOptions clearOptions = ClearOptions.ClearMemory, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.TempJob) {

            var sizeInBytes = Bitwise.AlignULongBits(length);
            this.allocator = allocator;
            this.ptr = (ulong*)UnsafeUtility.Malloc(sizeInBytes, TAlign<ulong>.alignInt, this.allocator);
            this.Length = length;

            if (clearOptions == ClearOptions.ClearMemory) {
                UnsafeUtility.MemClear(this.ptr, sizeInBytes);
            }
        }

        [INLINE(256)]
        public TempBitArray(in MemoryAllocator allocator, in BitArray bitmap, Unity.Collections.Allocator unityAllocator) {

            var newArr = new TempBitArray(bitmap.Length, ClearOptions.UninitializedMemory, unityAllocator);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, bitmap.ptr);
            UnsafeUtility.MemCpy(newArr.ptr, ptr, Bitwise.AlignULongBits(bitmap.Length));
            this = newArr;
            
        }

        [INLINE(256)]
        public void Resize(uint newLength, Unity.Collections.Allocator allocator) {

            if (newLength > this.Length) {
                var newArr = new TempBitArray(newLength, ClearOptions.ClearMemory, allocator);
                UnsafeUtility.MemCpy(newArr.ptr, this.ptr, Bitwise.AlignULongBits(this.Length));
                UnsafeUtility.Free(this.ptr, this.allocator);
                this = newArr;
            }
            
        }

        /// <summary>
        /// Sets all the bits in the bitmap to the specified value.
        /// </summary>
        /// <param name="value">The value to set each bit to.</param>
        /// <returns>The instance of the modified bitmap.</returns>
        [INLINE(256)]
        public void SetAllBits(bool value) {
            var len = Bitwise.GetLength(this.Length);
            var setValue = value ? ulong.MaxValue : ulong.MinValue;
            for (var index = 0; index < len; index++) {
                this.ptr[index] = setValue;
            }
        }

        /// <summary>
        /// Gets the value of the bit at the specified index.
        /// </summary>
        /// <param name="index">The index of the bit.</param>
        /// <returns>The value of the bit at the specified index.</returns>
        [INLINE(256)]
        public readonly bool IsSet(int index) {
            E.RANGE(index, 0, this.Length);
            return (this.ptr[index / TempBitArray.BITS_IN_ULONG] & (0x1ul << (index % TempBitArray.BITS_IN_ULONG))) > 0;
        }

        /// <summary>
        /// Sets the value of the bit at the specified index to the specified value.
        /// </summary>
        /// <param name="index">The index of the bit to set.</param>
        /// <param name="value">The value to set the bit to.</param>
        /// <returns>The instance of the modified bitmap.</returns>
        [INLINE(256)]
        public void Set(int index, bool value) {
            E.RANGE(index, 0, this.Length);
            if (value == true) {
                this.ptr[index / TempBitArray.BITS_IN_ULONG] |= 0x1ul << (index % TempBitArray.BITS_IN_ULONG);
            } else {
                this.ptr[index / TempBitArray.BITS_IN_ULONG] &= ~(0x1ul << (index % TempBitArray.BITS_IN_ULONG));
            }
        }

        /// <summary>
        /// Takes the union of this bitmap and the specified bitmap and stores the result in this
        /// instance.
        /// </summary>
        /// <param name="bitmap">The bitmap to union with this instance.</param>
        /// <returns>A reference to this instance.</returns>
        [INLINE(256)]
        public void Union(TempBitArray bitmap) {
            if (bitmap.Length == 0) return;
            this.Resize(bitmap.Length > this.Length ? bitmap.Length : this.Length, this.allocator);
            E.RANGE(bitmap.Length - 1u, 0u, this.Length);
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                this.ptr[index] |= bitmap.ptr[index];
            }
        }

        [INLINE(256)]
        public void Union(in MemoryAllocator allocator, BitArray bitmap) {
            if (bitmap.Length == 0) return;
            this.Resize(bitmap.Length > this.Length ? bitmap.Length : this.Length, this.allocator);
            E.RANGE(bitmap.Length - 1u, 0u, this.Length);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, bitmap.ptr);
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                this.ptr[index] |= ptr[index];
            }
        }

        /// <summary>
        /// Takes the intersection of this bitmap and the specified bitmap and stores the result in
        /// this instance.
        /// </summary>
        /// <param name="bitmap">The bitmap to intersect with this instance.</param>
        /// <returns>A reference to this instance.</returns>
        [INLINE(256)]
        public void Intersect(TempBitArray bitmap) {
            if (bitmap.Length == 0) {
                this.SetAllBits(false);
                return;
            }
            E.RANGE(bitmap.Length - 1u, 0u, this.Length);
            var ptr = bitmap.ptr;
            var len = Bitwise.GetLength(this.Length);
            var bLen = Bitwise.GetLength(bitmap.Length);
            for (var index = 0; index < len; ++index) {
                var v = 0UL;
                if (index < bLen) v = ptr[index];
                this.ptr[index] &= v;
            }
        }

        [INLINE(256)]
        public void Intersect(in MemoryAllocator allocator, in BitArray bitmap) {
            if (bitmap.Length == 0) {
                this.SetAllBits(false);
                return;
            }
            E.RANGE(bitmap.Length - 1u, 0u, this.Length);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, bitmap.ptr);
            var len = Bitwise.GetLength(this.Length);
            var bLen = Bitwise.GetLength(bitmap.Length);
            for (var index = 0; index < len; ++index) {
                var v = 0UL;
                if (index < bLen) v = ptr[index];
                this.ptr[index] &= v;
            }
        }

        [INLINE(256)]
        public void Remove(TempBitArray bitmap) {
            if (bitmap.Length == 0) return;
            this.Resize(bitmap.Length > this.Length ? bitmap.Length : this.Length, this.allocator);
            E.RANGE(bitmap.Length - 1u, 0u, this.Length);
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                this.ptr[index] &= ~bitmap.ptr[index];
            }
        }

        [INLINE(256)]
        public void Remove(in MemoryAllocator allocator, in BitArray bitmap) {
            if (bitmap.Length == 0) return;
            E.RANGE(bitmap.Length - 1u, 0u, this.Length);
            var ptr = (ulong*)MemoryAllocatorExt.GetUnsafePtr(in allocator, bitmap.ptr);
            var len = Bitwise.GetMinLength(bitmap.Length, this.Length);
            for (var index = 0; index < len; ++index) {
                this.ptr[index] &= ~ptr[index];
            }
        }

        /// <summary>
        /// Inverts all the bits in this bitmap.
        /// </summary>
        /// <returns>A reference to this instance.</returns>
        [INLINE(256)]
        public void Invert() {
            var len = Bitwise.GetLength(this.Length);
            for (var index = 0; index < len; ++index) {
                this.ptr[index] = ~this.ptr[index];
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
        public void SetRange(int start, int end, bool value) {
            if (start == end) {
                this.Set(start, value);
                return;
            }

            var startBucket = start / TempBitArray.BITS_IN_ULONG;
            var startOffset = start % TempBitArray.BITS_IN_ULONG;
            var endBucket = end / TempBitArray.BITS_IN_ULONG;
            var endOffset = end % TempBitArray.BITS_IN_ULONG;

            if (value) {
                this.ptr[startBucket] |= ulong.MaxValue << startOffset;
            } else {
                this.ptr[startBucket] &= ~(ulong.MaxValue << startOffset);
            }

            for (var bucketIndex = startBucket + 1; bucketIndex < endBucket; bucketIndex++) {
                this.ptr[bucketIndex] = value ? ulong.MaxValue : ulong.MinValue;
            }

            if (value) {
                this.ptr[endBucket] |= ulong.MaxValue >> (TempBitArray.BITS_IN_ULONG - endOffset - 1);
            } else {
                this.ptr[endBucket] &= ~(ulong.MaxValue >> (TempBitArray.BITS_IN_ULONG - endOffset - 1));
            }
        }

        [INLINE(256)]
        public void Clear() {

            this.SetAllBits(false);

        }

        [INLINE(256)]
        public void Dispose() {

            UnsafeUtility.Free(this.ptr, this.allocator);
            this = default;

        }

        [INLINE(256)]
        public UnsafeList<uint> GetTrueBitsTemp() {

            var trueBits = new UnsafeList<uint>((int)this.Length, Unity.Collections.Allocator.Temp);
            for (var i = 0; i < this.Length; ++i) {
                var val = this.ptr[i / TempBitArray.BITS_IN_ULONG];
                if ((val & (0x1ul << (i % TempBitArray.BITS_IN_ULONG))) > 0) {
                    trueBits.Add((uint)i);
                }
            }
            
            return trueBits;
        }

        [INLINE(256)]
        public UIntListHash GetTrueBitsTemp(ref MemoryAllocator allocator) {

            var trueBits = new UIntListHash(ref allocator, this.Length);
            for (var i = 0; i < this.Length; ++i) {
                var val = this.ptr[i / TempBitArray.BITS_IN_ULONG];
                if ((val & (0x1ul << (i % TempBitArray.BITS_IN_ULONG))) > 0) {
                    trueBits.Add(ref allocator, (uint)i);
                }
            }
            
            return trueBits;
        }

        public uint GetReservedSizeInBytes() {
            return Bitwise.AlignULongBits(this.Length);
        }

    }

    internal sealed class TempBitArrayDebugView {

        private TempBitArray Data;

        public TempBitArrayDebugView(TempBitArray data) {
            this.Data = data;
        }

        public bool[] Bits {
            get {
                var array = new bool[this.Data.Length];
                for (var i = 0; i < this.Data.Length; ++i) {
                    array[i] = this.Data.IsSet(i);
                }

                return array;
            }
        }

        public int[] BitIndexes {
            get {
                var array = new System.Collections.Generic.List<int>((int)this.Data.Length);
                for (var i = 0; i < this.Data.Length; ++i) {
                    if (this.Data.IsSet(i) == true) array.Add(i);
                }

                return array.ToArray();
            }
        }

    }

}