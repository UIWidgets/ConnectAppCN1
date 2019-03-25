namespace BestHTTP
{
    /// <summary>
    /// Possible states of a Http Connection.
    /// The ideal lifecycle of a connection that has KeepAlive is the following: Initial => [Processing => WaitForRecycle => Free] => Closed.
    /// </summary>
    internal enum HTTPConnectionStates
    {
        /// <summary>
        /// This Connection instance is just created.
        /// </summary>
        Initial,

        /// <summary>
        /// This Connection is processing a request
        /// </summary>
        Processing,

        /// <summary>
        /// The request redirected.
        /// </summary>
        Redirected,

        /// <summary>
        /// The connection is upgraded from http.
        /// </summary>
        Upgraded,

        /// <summary>
        /// Wait for the upgraded protocol to shut down.
        /// </summary>
        WaitForProtocolShutdown,

        /// <summary>
        /// The Connection is finished processing the request, it's waiting now to deliver it's result.
        /// </summary>
        WaitForRecycle,

        /// <summary>
        /// The request result's delivered, it's now up to processing again.
        /// </summary>
        Free,

        /// <summary>
        /// A request from outside of the plugin to abort the connection.
        /// </summary>
        AbortRequested,

        /// <summary>
        /// The request is not finished in the given time.
        /// </summary>
        TimedOut,

        /// <summary>
        /// If it's not a KeepAlive connection, or something happend, then we close this connection and remove from the pool.
        /// </summary>
        Closed
    }
}