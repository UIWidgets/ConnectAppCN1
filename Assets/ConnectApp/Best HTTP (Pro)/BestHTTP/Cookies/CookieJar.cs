#if !BESTHTTP_DISABLE_COOKIES

using BestHTTP.PlatformSupport.FileSystem;
using System;
using System.Collections.Generic;

namespace BestHTTP.Cookies
{
    /// <summary>
    /// The Cookie Jar implementation based on RFC 6265(http://tools.ietf.org/html/rfc6265).
    /// </summary>
    public static class CookieJar
    {
        // Version of the cookie store. It may be used in a future version for maintaining compatibility.
        private const int Version = 1;

        /// <summary>
        /// Returns true if File apis are supported.
        /// </summary>
        public static bool IsSavingSupported
        {
            get
            {
#if !BESTHTTP_DISABLE_COOKIE_SAVE
                if (IsSupportCheckDone)
                    return _isSavingSupported;

                try
                {
                    HTTPManager.IOService.DirectoryExists(HTTPManager.GetRootCacheFolder());
                    _isSavingSupported = true;
                }
                catch
                {
                    _isSavingSupported = false;

                    HTTPManager.Logger.Warning("CookieJar", "Cookie saving and loading disabled!");
                }
                finally
                {
                    IsSupportCheckDone = true;
                }

                return _isSavingSupported;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// The plugin will delete cookies that are accessed this threshold ago. Its default value is 7 days.
        /// </summary>
        public static TimeSpan AccessThreshold = TimeSpan.FromDays(7);

#region Privates

        /// <summary>
        /// List of the Cookies
        /// </summary>
        private static List<Cookie> Cookies = new List<Cookie>();
        private static string CookieFolder { get; set; }
        private static string LibraryPath { get; set; }

        /// <summary>
        /// Synchronization object for thread safety.
        /// </summary>
        private static object Locker = new object();

#if !BESTHTTP_DISABLE_COOKIE_SAVE
        private static bool _isSavingSupported;
        private static bool IsSupportCheckDone;
#endif

        private static bool Loaded;
#endregion

#region Internal Functions

        internal static void SetupFolder()
        {
#if !BESTHTTP_DISABLE_COOKIE_SAVE
            if (!CookieJar.IsSavingSupported)
                return;

            try
            {
                if (string.IsNullOrEmpty(CookieFolder) || string.IsNullOrEmpty(LibraryPath))
                {
                    CookieFolder = System.IO.Path.Combine(HTTPManager.GetRootCacheFolder(), "Cookies");
                    LibraryPath = System.IO.Path.Combine(CookieFolder, "Library");
                }
            }
            catch
            { }
#endif
        }

        /// <summary>
        /// Will set or update all cookies from the response object.
        /// </summary>
        internal static void Set(HTTPResponse response)
        {
            if (response == null)
                return;

            lock(Locker)
            {
                try
                {
                    Maintain();

                    List<Cookie> newCookies = new List<Cookie>();
                    var setCookieHeaders = response.GetHeaderValues("set-cookie");

                    // No cookies. :'(
                    if (setCookieHeaders == null)
                        return;

                    foreach (var cookieHeader in setCookieHeaders)
                    {
                        try
                        {
                            Cookie cookie = Cookie.Parse(cookieHeader, response.baseRequest.CurrentUri);

                            if (cookie != null)
                            {
                                int idx;
                                var old = Find(cookie, out idx);

                                // if no value for the cookie or already expired then the server asked us to delete the cookie
                                bool expired = string.IsNullOrEmpty(cookie.Value) || !cookie.WillExpireInTheFuture();

                                if (!expired)
                                {
                                    // no old cookie, add it straight to the list
                                    if (old == null)
                                    {
                                        Cookies.Add(cookie);

                                        newCookies.Add(cookie);
                                    }
                                    else
                                    {
                                        // Update the creation-time of the newly created cookie to match the creation-time of the old-cookie.
                                        cookie.Date = old.Date;
                                        Cookies[idx] = cookie;

                                        newCookies.Add(cookie);
                                    }
                                }
                                else if (idx != -1) // delete the cookie
                                    Cookies.RemoveAt(idx);
                            }
                        }
                        catch
                        {
                            // Ignore cookie on error
                        }
                    }

                    response.Cookies = newCookies;
                }
                catch
                {}
            }
        }

        /// <summary>
        /// Deletes all expired or 'old' cookies, and will keep the sum size of cookies under the given size.
        /// </summary>
        internal static void Maintain()
        {
            // It's not the same as in the rfc:
            //  http://tools.ietf.org/html/rfc6265#section-5.3

            lock (Locker)
            {
                try
                {
                    uint size = 0;

                    for (int i = 0; i < Cookies.Count; )
                    {
                        var cookie = Cookies[i];

                        // Remove expired or not used cookies
                        if (!cookie.WillExpireInTheFuture() || (cookie.LastAccess + AccessThreshold) < DateTime.UtcNow)
                            Cookies.RemoveAt(i);
                        else
                        {
                            if (!cookie.IsSession)
                                size += cookie.GuessSize();
                            i++;
                        }
                    }

                    if (size > HTTPManager.CookieJarSize)
                    {
                        Cookies.Sort();

                        while (size > HTTPManager.CookieJarSize && Cookies.Count > 0)
                        {
                            var cookie = Cookies[0];
                            Cookies.RemoveAt(0);

                            size -= cookie.GuessSize();
                        }
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Saves the Cookie Jar to a file.
        /// </summary>
        /// <remarks>Not implemented under Unity WebPlayer</remarks>
        internal static void Persist()
        {
#if !BESTHTTP_DISABLE_COOKIE_SAVE
            if (!IsSavingSupported)
                return;

            lock (Locker)
            {
                if (!Loaded)
                    return;

                try
                {
                    // Delete any expired cookie
                    Maintain();

                    if (!HTTPManager.IOService.DirectoryExists(CookieFolder))
                        HTTPManager.IOService.DirectoryCreate(CookieFolder);

                    using (var fs = HTTPManager.IOService.CreateFileStream(LibraryPath, FileStreamModes.Create))
                    using (var bw = new System.IO.BinaryWriter(fs))
                    {
                        bw.Write(Version);

                        // Count how many non-session cookies we have
                        int count = 0;
                        foreach (var cookie in Cookies)
                            if (!cookie.IsSession)
                                count++;

                        bw.Write(count);

                        // Save only the persistable cookies
                        foreach (var cookie in Cookies)
                            if (!cookie.IsSession)
                                cookie.SaveTo(bw);
                    }
                }
                catch
                { }
            }
#endif
        }

        /// <summary>
        /// Load previously persisted cookie library from the file.
        /// </summary>
        internal static void Load()
        {
#if !BESTHTTP_DISABLE_COOKIE_SAVE
            if (!IsSavingSupported)
                return;

            lock (Locker)
            {
                if (Loaded)
                    return;

                SetupFolder();

                try
                {
                    Cookies.Clear();

                    if (!HTTPManager.IOService.DirectoryExists(CookieFolder))
                        HTTPManager.IOService.DirectoryCreate(CookieFolder);

                    if (!HTTPManager.IOService.FileExists(LibraryPath))
                        return;

                    using (var fs = HTTPManager.IOService.CreateFileStream(LibraryPath, FileStreamModes.Open))
                    using (var br = new System.IO.BinaryReader(fs))
                    {
                        /*int version = */br.ReadInt32();
                        int cookieCount = br.ReadInt32();

                        for (int i = 0; i < cookieCount; ++i)
                        {
                            Cookie cookie = new Cookie();
                            cookie.LoadFrom(br);

                            if (cookie.WillExpireInTheFuture())
                                Cookies.Add(cookie);
                        }
                    }
                }
                catch
                {
                    Cookies.Clear();
                }
                finally
                {
                    Loaded = true;
                }
            }
#endif
        }

#endregion

#region Public Functions

        /// <summary>
        /// Returns all Cookies that corresponds to the given Uri.
        /// </summary>
        public static List<Cookie> Get(Uri uri)
        {
            lock (Locker)
            {
                Load();

                List<Cookie> result = null;

                for (int i = 0; i < Cookies.Count; ++i)
                {
                    Cookie cookie = Cookies[i];
                    if (cookie.WillExpireInTheFuture() && uri.Host.IndexOf(cookie.Domain) != -1 && uri.AbsolutePath.StartsWith(cookie.Path))
                    {
                        if (result == null)
                            result = new List<Cookie>();

                        result.Add(cookie);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Will add a new, or overwrite an old cookie if already exists.
        /// </summary>
        public static void Set(Uri uri, Cookie cookie)
        {
            Set(cookie);
        }

        /// <summary>
        /// Will add a new, or overwrite an old cookie if already exists.
        /// </summary>
        public static void Set(Cookie cookie)
        {
            lock (Locker)
            {
                Load();

                int idx;
                Find(cookie, out idx);

                if (idx >= 0)
                    Cookies[idx] = cookie;
                else
                    Cookies.Add(cookie);
            }
        }

        public static List<Cookie> GetAll()
        {
            lock (Locker)
            {
                Load();

                return Cookies;
            }
        }

        /// <summary>
        /// Deletes all cookies from the Jar.
        /// </summary>
        public static void Clear()
        {
            lock (Locker)
            {
                Load();

                Cookies.Clear();
            }
        }

        /// <summary>
        /// Removes cookies that older than the given parameter.
        /// </summary>
        public static void Clear(TimeSpan olderThan)
        {
            lock (Locker)
            {
                Load();

                for (int i = 0; i < Cookies.Count; )
                {
                    var cookie = Cookies[i];

                    // Remove expired or not used cookies
                    if (!cookie.WillExpireInTheFuture() || (cookie.Date + olderThan) < DateTime.UtcNow)
                        Cookies.RemoveAt(i);
                    else
                        i++;
                }
            }
        }

        /// <summary>
        /// Removes cookies that matches to the given domain.
        /// </summary>
        public static void Clear(string domain)
        {
            lock (Locker)
            {
                Load();

                for (int i = 0; i < Cookies.Count; )
                {
                    var cookie = Cookies[i];

                    // Remove expired or not used cookies
                    if (!cookie.WillExpireInTheFuture() || cookie.Domain.IndexOf(domain) != -1)
                        Cookies.RemoveAt(i);
                    else
                        i++;
                }
            }
        }

        public static void Remove(Uri uri, string name)
        {
            lock(Locker)
            {
                Load();

                for (int i = 0; i < Cookies.Count; )
                {
                    var cookie = Cookies[i];

                    if (cookie.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && uri.Host.IndexOf(cookie.Domain) != -1)
                        Cookies.RemoveAt(i);
                    else
                        i++;
                }
            }
        }

#endregion

#region Private Helper Functions

        /// <summary>
        /// Find and return a Cookie and his index in the list.
        /// </summary>
        private static Cookie Find(Cookie cookie, out int idx)
        {
            for (int i = 0; i < Cookies.Count; ++i)
            {
                Cookie c = Cookies[i];

                if (c.Equals(cookie))
                {
                    idx = i;
                    return c;
                }
            }

            idx = -1;
            return null;
        }

#endregion
    }
}

#endif