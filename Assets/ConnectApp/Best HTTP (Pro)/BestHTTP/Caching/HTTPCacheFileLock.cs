#if !BESTHTTP_DISABLE_CACHING

using System;
using System.Collections.Generic;

namespace BestHTTP.Caching
{
    sealed class HTTPCacheFileLock
    {
        private static Dictionary<Uri, object> FileLocks = new Dictionary<Uri, object>();
        private static object SyncRoot = new object();

        internal static object Acquire(Uri uri)
        {
            lock (SyncRoot)
            {
                object fileLock;
                if (!FileLocks.TryGetValue(uri, out fileLock))
                    FileLocks.Add(uri, fileLock = new object());

                return fileLock;
            }
        }

        internal static void Remove(Uri uri)
        {
            lock (SyncRoot)
            {
                if (FileLocks.ContainsKey(uri))
                    FileLocks.Remove(uri);
            }
        }

        internal static void Clear()
        {
            lock (SyncRoot)
            {
                FileLocks.Clear();
            }
        }
    }
}

#endif