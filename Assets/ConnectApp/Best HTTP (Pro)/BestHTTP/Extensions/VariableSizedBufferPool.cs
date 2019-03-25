using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
    /// <summary>
    /// Private data struct that contains the size <-> byte arrays mapping. 
    /// </summary>
    struct BufferStore
    {
        /// <summary>
        /// Size/length of the arrays stored in the buffers.
        /// </summary>
        public readonly long Size;

        /// <summary>
        /// 
        /// </summary>
        public List<BufferDesc> buffers;

        public BufferStore(long size)
        {
            this.Size = size;
            this.buffers = new List<BufferDesc>();
        }

        /// <summary>
        /// Create a new store with its first byte[] to store.
        /// </summary>
        public BufferStore(long size, byte[] buffer)
            : this(size)
        {
            this.buffers.Add(new BufferDesc(buffer));
        }
    }

    struct BufferDesc
    {
        public static readonly BufferDesc Empty = new BufferDesc(null);

        /// <summary>
        /// The actual reference to the stored byte array.
        /// </summary>
        public byte[] buffer;

        /// <summary>
        /// When the buffer is put back to the pool. Based on this value the pool will calculate the age of the buffer.
        /// </summary>
        public DateTime released;

        public BufferDesc(byte[] buff)
        {
            this.buffer = buff;
            this.released = DateTime.UtcNow;
        }
    }

    public static class VariableSizedBufferPool
    {
        public static readonly byte[] NoData = new byte[0];

        /// <summary>
        /// Setting this property to false the pooling mechanism can be disabled.
        /// </summary>
        public static bool IsEnabled {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;

                // When set to non-enabled remove all stored entries
                if (!_isEnabled)
                    Clear();
            }
        }
        public static volatile bool _isEnabled = true;

        /// <summary>
        /// Buffer entries that released back to the pool and older than this value are moved when next maintenance is triggered.
        /// </summary>
        public static TimeSpan RemoveOlderThan = TimeSpan.FromSeconds(30);

        /// <summary>
        /// How often pool maintenance must run.
        /// </summary>
        public static TimeSpan RunMaintenanceEvery = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Minumum buffer size that the plugin will allocate when the requested size is smaller than this value, and canBeLarger is set to true.
        /// </summary>
        public static long MinBufferSize = 256;

        /// <summary>
        /// Maximum size of a buffer that the plugin will store.
        /// </summary>
        public static long MaxBufferSize = long.MaxValue;

        /// <summary>
        /// Maximum accomulated size of the stored buffers.
        /// </summary>
        public static long MaxPoolSize = 10 * 1024 * 1024;

        /// <summary>
        /// Whether to remove empty buffer stores from the free list.
        /// </summary>
        public static bool RemoveEmptyLists = true;

        /// <summary>
        /// If it set to true and a byte[] is released more than once it will log out an error.
        /// </summary>
        public static bool IsDoubleReleaseCheckEnabled = false;

        // It must be sorted by buffer size!
        private static List<BufferStore> FreeBuffers = new List<BufferStore>();
        private static DateTime lastMaintenance = DateTime.MinValue;

        // Statistics
        private static volatile int PoolSize = 0;
        private static volatile uint GetBuffers = 0;
        private static volatile uint ReleaseBuffers = 0;
        private static System.Text.StringBuilder statiscticsBuilder = new System.Text.StringBuilder();

        static VariableSizedBufferPool()
        {
#if UNITY_EDITOR
            IsDoubleReleaseCheckEnabled = true;
#else
            IsDoubleReleaseCheckEnabled = false;
#endif
        }

        /// <summary>
        /// Get byte[] from the pool. If canBeLarge is true, the returned buffer might be larger than the requested size.
        /// </summary>
        public static byte[] Get(long size, bool canBeLarger)
        {
            if (!_isEnabled)
                return new byte[size];

            // Return a fix reference for 0 length requests. Any resize call (even Array.Resize) creates a new reference
            //  so we are safe to expose it to multiple callers.
            if (size == 0)
                return VariableSizedBufferPool.NoData;

            lock (FreeBuffers)
            {
                if (FreeBuffers.Count == 0)
                    return new byte[size];

                BufferDesc bufferDesc = FindFreeBuffer(size, canBeLarger);
                if (bufferDesc.buffer == null)
                {
                    if (canBeLarger)
                    {
                        if (size < MinBufferSize)
                            size = MinBufferSize;
                        else if (!IsPowerOfTwo(size))
                            size = NextPowerOf2(size);
                    }

                    return new byte[size];
                }
                else
                    GetBuffers++;

                PoolSize -= bufferDesc.buffer.Length;

                return bufferDesc.buffer;
            }
        }

        /// <summary>
        /// Release a list of byte arrays back to the pool
        /// </summary>
        public static void Release(List<byte[]> buffers)
        {
            if (!_isEnabled || buffers == null || buffers.Count == 0)
                return;

            for (int i = 0; i < buffers.Count; ++i)
                Release(buffers[i]);
        }

        /// <summary>
        /// Release back a byte array to the pool.
        /// </summary>
        public static void Release(byte[] buffer)
        {
            if (!_isEnabled || buffer == null)
                return;

            int size = buffer.Length;

            if (size == 0 || size > MaxBufferSize)
                return;

            lock (FreeBuffers)
            {
                if (PoolSize + size > MaxPoolSize)
                    return;
                PoolSize += size;

                ReleaseBuffers++;

                AddFreeBuffer(buffer);
            }
        }

        /// <summary>
        /// Resize a byte array. It will release the old one to the pool, and the new one is from the pool too.
        /// </summary>
        public static byte[] Resize(ref byte[] buffer, int newSize, bool canBeLarger)
        {
            if (!_isEnabled)
            {
                Array.Resize<byte>(ref buffer, newSize);
                return buffer;
            }

            byte[] newBuf = VariableSizedBufferPool.Get(newSize, canBeLarger);
            Array.Copy(buffer, 0, newBuf, 0, Math.Min(newBuf.Length, buffer.Length));
            VariableSizedBufferPool.Release(buffer);
            return buffer = newBuf;
        }

        /// <summary>
        /// Get textual statistics about the buffer pool.
        /// </summary>
        public static string GetStatistics(bool showEmptyBuffers = true)
        {
            lock (FreeBuffers)
            {
                statiscticsBuilder.Length = 0;
                statiscticsBuilder.AppendFormat("Pooled array reused count: {0:N0}\n", GetBuffers);
                statiscticsBuilder.AppendFormat("Release call count: {0:N0}\n", ReleaseBuffers);
                statiscticsBuilder.AppendFormat("PoolSize: {0:N0}\n", PoolSize);
                statiscticsBuilder.AppendFormat("Buffers: {0}\n", FreeBuffers.Count);

                for (int i = 0; i < FreeBuffers.Count; ++i)
                {
                    BufferStore store = FreeBuffers[i];
                    List<BufferDesc> buffers = store.buffers;

                    if (showEmptyBuffers || buffers.Count > 0)
                        statiscticsBuilder.AppendFormat("- Size: {0:N0} Count: {1:N0}\n", store.Size, buffers.Count);
                }

                return statiscticsBuilder.ToString();
            }
        }

        /// <summary>
        /// Remove all stored entries instantly.
        /// </summary>
        public static void Clear()
        {
            lock (FreeBuffers)
            {
                FreeBuffers.Clear();
                PoolSize = 0;
            }
        }

        /// <summary>
        /// Internal function called by the plugin the remove old, non-used buffers.
        /// </summary>
        internal static void Maintain()
        {
            DateTime now = DateTime.UtcNow;
            if (!_isEnabled || lastMaintenance + RunMaintenanceEvery > now)
                return;
            lastMaintenance = now;

            DateTime olderThan = now - RemoveOlderThan;
            lock (FreeBuffers)
            {
                if (HTTPManager.Logger.Level == Logger.Loglevels.All)
                    HTTPManager.Logger.Information("VariableSizedBufferPool", "Before Maintain: " + GetStatistics());

                for (int i = 0; i < FreeBuffers.Count; ++i)
                {
                    BufferStore store = FreeBuffers[i];
                    List<BufferDesc> buffers = store.buffers;

                    for (int cv = buffers.Count - 1; cv >= 0; cv--)
                    {
                        BufferDesc desc = buffers[cv];

                        if (desc.released < olderThan)
                        {
                            // buffers stores available buffers ascending by age. So, when we find an old enough, we can
                            //  delete all entries in the [0..cv] range.

                            int removeCount = cv + 1;
                            buffers.RemoveRange(0, removeCount);
                            PoolSize -= (int)(removeCount * store.Size);
                            break;
                        }
                    }

                    if (RemoveEmptyLists && buffers.Count == 0)
                        FreeBuffers.RemoveAt(i--);
                }

                if (HTTPManager.Logger.Level == Logger.Loglevels.All)
                    HTTPManager.Logger.Information("VariableSizedBufferPool", "After Maintain: " + GetStatistics());
            }
        }

#region Private helper functions

        private static bool IsPowerOfTwo(long x)
        {
            return (x & (x - 1)) == 0;
        }

        private static long NextPowerOf2(long x)
        {
            long pow = 1;
            while (pow <= x)
                pow *= 2;
            return pow;
        }

        private static BufferDesc FindFreeBuffer(long size, bool canBeLarger)
        {
            for (int i = 0; i < FreeBuffers.Count; ++i)
            {
                BufferStore store = FreeBuffers[i];

                if (store.buffers.Count > 0 && (store.Size == size || (canBeLarger && store.Size > size)))
                {
                    // Getting the last one has two desired effect:
                    //  1.) RemoveAt should be quicker as it don't have to move all the remaining entries
                    //  2.) Old, non-used buffers will age. Getting a buffer and putting it back will not keep buffers fresh.

                    BufferDesc lastFree = store.buffers[store.buffers.Count - 1];
                    store.buffers.RemoveAt(store.buffers.Count - 1);

                    return lastFree;
                }
            }

            return BufferDesc.Empty;
        }

        private static void AddFreeBuffer(byte[] buffer)
        {
            int bufferLength = buffer.Length;
            for (int i = 0; i < FreeBuffers.Count; ++i)
            {
                BufferStore store = FreeBuffers[i];

                if (store.Size == bufferLength)
                {
                    // We highly assume here that every buffer will be released only once.
                    //  Checking for double-release would mean that we have to do another O(n) operation, where n is the
                    //  count of the store's elements.

                    if (IsDoubleReleaseCheckEnabled)
                        for (int cv = 0; cv < store.buffers.Count; ++cv)
                            if (store.buffers[cv].buffer == buffer)
                            {
                                HTTPManager.Logger.Error("VariableSizedBufferPool", "Buffer already added to the pool!");
                                return;
                            }

                    store.buffers.Add(new BufferDesc(buffer));
                    return;
                }

                if (store.Size > bufferLength)
                {
                    FreeBuffers.Insert(i, new BufferStore(bufferLength, buffer));
                    return;
                }
            }

            // When we reach this point, there's no same sized or larger BufferStore present, so we have to add a new one
            //  to the endo of our list.
            FreeBuffers.Add(new BufferStore(bufferLength, buffer));
        }

#endregion
    }
}