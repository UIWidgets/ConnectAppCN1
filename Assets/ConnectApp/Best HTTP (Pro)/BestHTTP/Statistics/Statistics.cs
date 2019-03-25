using System;

namespace BestHTTP.Statistics
{
    [Flags]
    public enum StatisticsQueryFlags : byte
    {
        /// <summary>
        /// Connection based statistics will be returned as the result of the query.
        /// </summary>
        Connections = 0x01,

        /// <summary>
        /// Caching based statistics will be returned as the result of the query.
        /// </summary>
        Cache = 0x02,

        /// <summary>
        /// Cookie based statistics will be returned as the result of the query.
        /// </summary>
        Cookies = 0x04,

        /// <summary>
        /// All statistics will be returned as the result of the query.
        /// </summary>
        All = 0xFF,
    }

    public struct GeneralStatistics
    {
        public StatisticsQueryFlags QueryFlags;

        #region Connection Statistics

        /// <summary>
        /// Number of HTTPConnection instances
        /// </summary>
        public int Connections;

        /// <summary>
        /// Number of active connections. These connections are currently processing a request.
        /// </summary>
        public int ActiveConnections;

        /// <summary>
        /// Number of free connections. These connections are finished with there requests and waiting for another request or to recycle.
        /// </summary>
        public int FreeConnections;

        /// <summary>
        /// Number of recycled connections. These connections will be removed as soon as possible.
        /// </summary>
        public int RecycledConnections;

        /// <summary>
        /// Number of requests that are waiting in the queue for a free connection.
        /// </summary>
        public int RequestsInQueue;

        #endregion

        #region Cache Statistics

        /// <summary>
        /// Number of cached responses.
        /// </summary>
        public int CacheEntityCount;

        /// <summary>
        /// Sum size of the cached responses in bytes.
        /// </summary>
        public ulong CacheSize;

        #endregion

        #region Cookie Statistics

        /// <summary>
        /// Number of cookies in the Cookie Jar.
        /// </summary>
        public int CookieCount;

        /// <summary>
        /// Sum size of the stored cookies in bytes.
        /// </summary>
        public uint CookieJarSize;

        #endregion
    }
}