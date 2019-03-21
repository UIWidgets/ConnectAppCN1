#if !BESTHTTP_DISABLE_CACHING

using System;
using System.Collections.Generic;
using System.Threading;

//
// Version 1: Initial release
// Version 2: Filenames are generated from an index.
//

namespace BestHTTP.Caching
{
    using BestHTTP.Extensions;
    using BestHTTP.PlatformSupport.FileSystem;

    public sealed class UriComparer : IEqualityComparer<Uri>
    {
        public bool Equals(Uri x, Uri y)
        {
            return Uri.Compare(x, y, UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0;
        }

        public int GetHashCode(Uri uri)
        {
            return uri.ToString().GetHashCode();
        }
    }


    public static class HTTPCacheService
    {
        #region Properties & Fields

        /// <summary>
        /// Library file-format versioning support
        /// </summary>
        private const int LibraryVersion = 2;

        public static bool IsSupported
        {
            get
            {
                if (IsSupportCheckDone)
                    return isSupported;

                try
                {
                    // If DirectoryExists throws an exception we will set IsSupprted to false

                    HTTPManager.IOService.DirectoryExists(HTTPManager.GetRootCacheFolder());
                    isSupported = true;
                }
                catch
                {
                    isSupported = false;

                    HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
                }
                finally
                {
                    IsSupportCheckDone = true;
                }

                return isSupported;
            }
        }
        private static bool isSupported;
        private static bool IsSupportCheckDone;

        private static Dictionary<Uri, HTTPCacheFileInfo> library;
        private static Dictionary<Uri, HTTPCacheFileInfo> Library { get { LoadLibrary(); return library; } }

        private static Dictionary<UInt64, HTTPCacheFileInfo> UsedIndexes = new Dictionary<ulong, HTTPCacheFileInfo>();

        internal static string CacheFolder { get; private set; }
        private static string LibraryPath { get; set; }

        private static bool InClearThread;
        private static bool InMaintainenceThread;

        /// <summary>
        /// Stores the index of the next stored entity. The entity's file name is generated from this index.
        /// </summary>
        private static UInt64 NextNameIDX;

        #endregion

        static HTTPCacheService()
        {
            NextNameIDX = 0x0001;
        }

        #region Common Functions

        internal static void CheckSetup()
        {
            if (!HTTPCacheService.IsSupported)
                return;

            try
            {
                SetupCacheFolder();
                LoadLibrary();
            }
            catch
            { }
        }

        internal static void SetupCacheFolder()
        {
            if (!HTTPCacheService.IsSupported)
                return;

            try
            {
                if (string.IsNullOrEmpty(CacheFolder) || string.IsNullOrEmpty(LibraryPath))
                {
                    CacheFolder = System.IO.Path.Combine(HTTPManager.GetRootCacheFolder(), "HTTPCache");
                    if (!HTTPManager.IOService.DirectoryExists(CacheFolder))
                        HTTPManager.IOService.DirectoryCreate(CacheFolder);

                    LibraryPath = System.IO.Path.Combine(HTTPManager.GetRootCacheFolder(), "Library");
                }
            }
            catch
            {
                isSupported = false;

                HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
            }
        }

        internal static UInt64 GetNameIdx()
        {
            lock(Library)
            {
                UInt64 result = NextNameIDX;

                do
                {
                    NextNameIDX = ++NextNameIDX % UInt64.MaxValue;
                } while (UsedIndexes.ContainsKey(NextNameIDX));

                return result;
            }
        }

        internal static bool HasEntity(Uri uri)
        {
            if (!IsSupported)
                return false;

            lock (Library)
                return Library.ContainsKey(uri);
        }

        public static bool DeleteEntity(Uri uri, bool removeFromLibrary = true)
        {
            if (!IsSupported)
                return false;

            object uriLocker = HTTPCacheFileLock.Acquire(uri);

            // Just use lock now: http://forum.unity3d.com/threads/4-6-ios-64-bit-beta.290551/page-6#post-1937033

            // To avoid a dead-lock we try acquire the lock on this uri only for a little time.
            // If we can't acquire it, its better to just return without risking a deadlock.
            //if (Monitor.TryEnter(uriLocker, TimeSpan.FromSeconds(0.5f)))
            lock(uriLocker)
            {
                try
                {
                    lock (Library)
                    {
                        HTTPCacheFileInfo info;
                        bool inStats = Library.TryGetValue(uri, out info);
                        if (inStats)
                            info.Delete();

                        if (inStats && removeFromLibrary)
                        {
                            Library.Remove(uri);
                            UsedIndexes.Remove(info.MappedNameIDX);
                        }

                        return true;
                    }
                }
                finally
                {
                    //Monitor.Exit(uriLocker);
                }
            }

            //return false;
        }

        internal static bool IsCachedEntityExpiresInTheFuture(HTTPRequest request)
        {
            if (!IsSupported)
                return false;

            HTTPCacheFileInfo info;
            lock (Library)
                if (Library.TryGetValue(request.CurrentUri, out info))
                    return info.WillExpireInTheFuture();

            return false;
        }

        /// <summary>
        /// Utility function to set the cache control headers according to the spec.: http://www.w3.org/Protocols/rfc2616/rfc2616-sec13.html#sec13.3.4
        /// </summary>
        /// <param name="request"></param>
        internal static void SetHeaders(HTTPRequest request)
        {
            if (!IsSupported)
                return;

            request.RemoveHeader("If-None-Match");
            request.RemoveHeader("If-Modified-Since");

            HTTPCacheFileInfo info;
            lock (Library)
                if (Library.TryGetValue(request.CurrentUri, out info))
                    info.SetUpRevalidationHeaders(request);
        }

        #endregion

        #region Get Functions

        internal static HTTPCacheFileInfo GetEntity(Uri uri)
        {
            if (!IsSupported)
                return null;
            HTTPCacheFileInfo info = null;
            lock (Library)
                Library.TryGetValue(uri, out info);
            return info;
        }

        internal static HTTPResponse GetFullResponse(HTTPRequest request)
        {
            if (!IsSupported)
                return null;

            HTTPCacheFileInfo info;
            lock (Library)
                if (Library.TryGetValue(request.CurrentUri, out info))
                    return info.ReadResponseTo(request);

            return null;
        }

        #endregion

        #region Storing

        /// <summary>
        /// Checks if the given response can be cached. http://www.w3.org/Protocols/rfc2616/rfc2616-sec13.html#sec13.4
        /// </summary>
        /// <returns>Returns true if cacheable, false otherwise.</returns>
        internal static bool IsCacheble(Uri uri, HTTPMethods method, HTTPResponse response)
        {
            if (!IsSupported)
                return false;

            if (method != HTTPMethods.Get)
                return false;

            if (response == null)
                return false;

            // https://www.w3.org/Protocols/rfc2616/rfc2616-sec13.html#sec13.12 - Cache Replacement
            // It MAY insert it into cache storage and MAY, if it meets all other requirements, use it to respond to any future requests that would previously have caused the old response to be returned.
            //if (response.StatusCode == 304)
            //    return false;

            if (response.StatusCode < 200 || response.StatusCode >= 400)
                return false;

            //http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.9.2
            var cacheControls = response.GetHeaderValues("cache-control");
            if (cacheControls != null)
            {
                if (cacheControls.Exists(headerValue => {
                                                            string value = headerValue.ToLower();
                                                            return value.Contains("no-store") || value.Contains("no-cache");
                                                        }))
                    return false;
            }

            var pragmas = response.GetHeaderValues("pragma");
            if (pragmas != null)
            {
                if (pragmas.Exists(headerValue => {
                                                      string value = headerValue.ToLower();
                                                      return value.Contains("no-store") || value.Contains("no-cache");
                                                  }))
                    return false;
            }

            // Responses with byte ranges not supported yet.
            var byteRanges = response.GetHeaderValues("content-range");
            if (byteRanges != null)
                return false;

            return true;
        }

        internal static HTTPCacheFileInfo Store(Uri uri, HTTPMethods method, HTTPResponse response)
        {
            if (response == null || response.Data == null || response.Data.Length == 0)
                return null;

            if (!IsSupported)
                return null;

            HTTPCacheFileInfo info = null;

            lock (Library)
            {
                if (!Library.TryGetValue(uri, out info))
                {
                    Library.Add(uri, info = new HTTPCacheFileInfo(uri));
                    UsedIndexes.Add(info.MappedNameIDX, info);
                }

                try
                {
                    info.Store(response);
                    if (HTTPManager.Logger.Level == Logger.Loglevels.All)
                        HTTPManager.Logger.Verbose("HTTPCacheService", string.Format("{0} - Saved to cache", uri.ToString()));
                }
                catch
                {
                    // If something happens while we write out the response, than we will delete it because it might be in an invalid state.
                    DeleteEntity(uri);

                    throw;
                }
            }

            return info;
        }

        internal static System.IO.Stream PrepareStreamed(Uri uri, HTTPResponse response)
        {
            if (!IsSupported)
                return null;

            HTTPCacheFileInfo info;

            lock (Library)
            {
                if (!Library.TryGetValue(uri, out info))
                {
                    Library.Add(uri, info = new HTTPCacheFileInfo(uri));
                    UsedIndexes.Add(info.MappedNameIDX, info);
                }

                try
                {
                    return info.GetSaveStream(response);
                }
                catch
                {
                    // If something happens while we write out the response, than we will delete it because it might be in an invalid state.
                    DeleteEntity(uri);

                    throw;
                }
            }
        }

        #endregion

        #region Public Maintenance Functions

        /// <summary>
        /// Deletes all cache entity. Non blocking.
        /// <remarks>Call it only if there no requests currently processed, because cache entries can be deleted while a server sends back a 304 result, so there will be no data to read from the cache!</remarks>
        /// </summary>
        public static void BeginClear()
        {
            if (!IsSupported)
                return;

            if (InClearThread)
                return;
            InClearThread = true;

            SetupCacheFolder();

    #if !NETFX_CORE
            ThreadPool.QueueUserWorkItem(new WaitCallback((param) => ClearImpl(param)));
            //new Thread(ClearImpl).Start();
#else
#pragma warning disable 4014
            Windows.System.Threading.ThreadPool.RunAsync(ClearImpl);
#pragma warning restore 4014
#endif
        }

        private static void ClearImpl(object param)
        {
            if (!IsSupported)
                return;

            try
            {
                // GetFiles will return a string array that contains the files in the folder with the full path
                string[] cacheEntries = HTTPManager.IOService.GetFiles(CacheFolder);

                for (int i = 0; i < cacheEntries.Length; ++i)
                {
                    // We need a try-catch block because between the Directory.GetFiles call and the File.Delete calls a maintenance job, or other file operations can delete any file from the cache folder.
                    // So while there might be some problem with any file, we don't want to abort the whole for loop
                    try
                    {
                        HTTPManager.IOService.FileDelete(cacheEntries[i]);
                    }
                    catch
                    { }
                }
            }
            finally
            {
                UsedIndexes.Clear();
                library.Clear();
                NextNameIDX = 0x0001;

                SaveLibrary();
                InClearThread = false;
            }
        }

        /// <summary>
        /// Deletes all expired cache entity.
        /// <remarks>Call it only if there no requests currently processed, because cache entries can be deleted while a server sends back a 304 result, so there will be no data to read from the cache!</remarks>
        /// </summary>
        public static void BeginMaintainence(HTTPCacheMaintananceParams maintananceParam)
        {
            if (maintananceParam == null)
                throw new ArgumentNullException("maintananceParams == null");

            if (!HTTPCacheService.IsSupported)
                return;

            if (InMaintainenceThread)
                return;

            InMaintainenceThread = true;

            SetupCacheFolder();

#if !NETFX_CORE
            ThreadPool.QueueUserWorkItem(new WaitCallback((param) =>
            //new Thread((param) =>
#else
#pragma warning disable 4014
            Windows.System.Threading.ThreadPool.RunAsync((param) =>
#pragma warning restore 4014
#endif
            {
                try
                    {
                        lock (Library)
                        {
                            // Delete cache entries older than the given time.
                            DateTime deleteOlderAccessed = DateTime.UtcNow - maintananceParam.DeleteOlder;
                            List<HTTPCacheFileInfo> removedEntities = new List<HTTPCacheFileInfo>();
                            foreach (var kvp in Library)
                                if (kvp.Value.LastAccess < deleteOlderAccessed)
                                {
                                    if (DeleteEntity(kvp.Key, false))
                                        removedEntities.Add(kvp.Value);
                                }

                            for (int i = 0; i < removedEntities.Count; ++i)
                            {
                                Library.Remove(removedEntities[i].Uri);
                                UsedIndexes.Remove(removedEntities[i].MappedNameIDX);
                            }
                            removedEntities.Clear();

                            ulong cacheSize = GetCacheSize();

                            // This step will delete all entries starting with the oldest LastAccess property while the cache size greater then the MaxCacheSize in the given param.
                            if (cacheSize > maintananceParam.MaxCacheSize)
                            {
                                List<HTTPCacheFileInfo> fileInfos = new List<HTTPCacheFileInfo>(library.Count);

                                foreach(var kvp in library)
                                    fileInfos.Add(kvp.Value);

                                fileInfos.Sort();

                                int idx = 0;
                                while (cacheSize >= maintananceParam.MaxCacheSize && idx < fileInfos.Count)
                                {
                                    try
                                    {
                                        var fi = fileInfos[idx];
                                        ulong length = (ulong)fi.BodyLength;

                                        DeleteEntity(fi.Uri);

                                        cacheSize -= length;
                                    }
                                    catch
                                    {}
                                    finally
                                    {
                                        ++idx;
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        SaveLibrary();
                        InMaintainenceThread = false;
                    }
                }
    #if !NETFX_CORE
                ));
    #else
                );
    #endif
        }

        public static int GetCacheEntityCount()
        {
            if (!HTTPCacheService.IsSupported)
                return 0;

            CheckSetup();

            lock(Library)
                return Library.Count;
        }

        public static ulong GetCacheSize()
        {
            ulong size = 0;

            if (!IsSupported)
                return size;

            CheckSetup();

            lock (Library)
                foreach (var kvp in Library)
                    if (kvp.Value.BodyLength > 0)
                        size += (ulong)kvp.Value.BodyLength;
            return size;
        }

        #endregion

        #region Cache Library Management

        private static void LoadLibrary()
        {
            // Already loaded?
            if (library != null)
                return;

            if (!IsSupported)
                return;

            library = new Dictionary<Uri, HTTPCacheFileInfo>(new UriComparer());

            if (!HTTPManager.IOService.FileExists(LibraryPath))
            {
                DeleteUnusedFiles();
                return;
            }

            try
            {
                int version;

                lock (library)
                {
                    using (var fs = HTTPManager.IOService.CreateFileStream(LibraryPath, FileStreamModes.Open))
                    using (var br = new System.IO.BinaryReader(fs))
                    {
                        version = br.ReadInt32();

                        if (version > 1)
                            NextNameIDX = br.ReadUInt64();

                        int statCount = br.ReadInt32();

                        for (int i = 0; i < statCount; ++i)
                        {
                            Uri uri = new Uri(br.ReadString());

                            var entity = new HTTPCacheFileInfo(uri, br, version);
                            if (entity.IsExists())
                            {
                                library.Add(uri, entity);

                                if (version > 1)
                                    UsedIndexes.Add(entity.MappedNameIDX, entity);
                            }
                        }
                    }
                }

                if (version == 1)
                    BeginClear();
                else
                    DeleteUnusedFiles();
            }
            catch
            {}
        }

        internal static void SaveLibrary()
        {
            if (library == null)
                return;

            if (!IsSupported)
                return;

            try
            {
                lock (Library)
                {
                    using (var fs = HTTPManager.IOService.CreateFileStream(LibraryPath, FileStreamModes.Create))
                    using (var bw = new System.IO.BinaryWriter(fs))
                    {
                        bw.Write(LibraryVersion);
                        bw.Write(NextNameIDX);

                        bw.Write(Library.Count);
                        foreach (var kvp in Library)
                        {
                            bw.Write(kvp.Key.ToString());

                            kvp.Value.SaveTo(bw);
                        }
                    }
                }
            }
            catch
            {}
        }


        internal static void SetBodyLength(Uri uri, int bodyLength)
        {
            if (!IsSupported)
                return;

            lock (Library)
            {
                HTTPCacheFileInfo fileInfo;
                if (Library.TryGetValue(uri, out fileInfo))
                    fileInfo.BodyLength = bodyLength;
                else
                {
                    Library.Add(uri, fileInfo = new HTTPCacheFileInfo(uri, DateTime.UtcNow, bodyLength));
                    UsedIndexes.Add(fileInfo.MappedNameIDX, fileInfo);
                }
            }
        }

        /// <summary>
        /// Deletes all files from the cache folder that isn't in the Library.
        /// </summary>
        private static void DeleteUnusedFiles()
        {
            if (!IsSupported)
                return;

            CheckSetup();

            // GetFiles will return a string array that contains the files in the folder with the full path
            string[] cacheEntries = HTTPManager.IOService.GetFiles(CacheFolder);

            for (int i = 0; i < cacheEntries.Length; ++i)
            {
                // We need a try-catch block because between the Directory.GetFiles call and the File.Delete calls a maintenance job, or other file operations can delete any file from the cache folder.
                // So while there might be some problem with any file, we don't want to abort the whole for loop
                try
                {
                    string filename = System.IO.Path.GetFileName(cacheEntries[i]);
                    UInt64 idx = 0;
                    bool deleteFile = false;
                    if (UInt64.TryParse(filename, System.Globalization.NumberStyles.AllowHexSpecifier, null, out idx))
                        lock (Library)
                            deleteFile = !UsedIndexes.ContainsKey(idx);
                    else
                        deleteFile = true;

                    if (deleteFile)
                        HTTPManager.IOService.FileDelete(cacheEntries[i]);
                }
                catch
                {}
            }
        }

        #endregion
    }
}

#endif