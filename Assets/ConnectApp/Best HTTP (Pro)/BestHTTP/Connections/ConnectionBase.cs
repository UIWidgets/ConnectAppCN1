using System;
using System.Threading;

#if NETFX_CORE
    using System.Threading.Tasks;

    //Disable CD4014: Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
    #pragma warning disable 4014

    //Disable warning CS1998: This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    #pragma warning disable 1998
#endif

namespace BestHTTP
{
    internal delegate void HTTPConnectionRecycledDelegate(ConnectionBase conn);

    internal abstract class ConnectionBase : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// The address of the server that this connection is bound to.
        /// </summary>
        public string ServerAddress { get; protected set; }

        /// <summary>
        /// The state of this connection.
        /// </summary>
        public HTTPConnectionStates State { get; protected set; }

        /// <summary>
        /// It's true if this connection is available to process a HTTPRequest.
        /// </summary>
        public bool IsFree { get { return State == HTTPConnectionStates.Initial || State == HTTPConnectionStates.Free; } }

        /// <summary>
        /// Returns true if it's an active connection.
        /// </summary>
        public bool IsActive { get { return State > HTTPConnectionStates.Initial && State < HTTPConnectionStates.Free; } }

        /// <summary>
        /// If the State is HTTPConnectionStates.Processing, then it holds a HTTPRequest instance. Otherwise it's null.
        /// </summary>
        public HTTPRequest CurrentRequest { get; protected set; }

        public virtual bool IsRemovable { get { return IsFree && (DateTime.UtcNow - LastProcessTime) > HTTPManager.MaxConnectionIdleTime; } }

        /// <summary>
        /// When we start to process the current request. It's set after the connection is established.
        /// </summary>
        public DateTime StartTime { get; protected set; }

        /// <summary>
        /// When this connection timed out.
        /// </summary>
        public DateTime TimedOutStart { get; protected set; }

#if !BESTHTTP_DISABLE_PROXY
        public bool HasProxy { get { return this.CurrentRequest != null && this.CurrentRequest.Proxy != null; } }
#endif

        public Uri LastProcessedUri { get; protected set; }

        #endregion

        #region Protected Fields

        protected DateTime LastProcessTime;
        protected HTTPConnectionRecycledDelegate OnConnectionRecycled = null;

        #endregion

        #region Privates

        private bool IsThreaded;

        #endregion

        public ConnectionBase(string serverAddress)
            :this(serverAddress, true)
        {}

        public ConnectionBase(string serverAddress, bool threaded)
        {
            this.ServerAddress = serverAddress;
            this.State = HTTPConnectionStates.Initial;
            this.LastProcessTime = DateTime.UtcNow;
            this.IsThreaded = threaded;
        }

        internal abstract void Abort(HTTPConnectionStates hTTPConnectionStates);

        internal void Process(HTTPRequest request)
        {
            if (State == HTTPConnectionStates.Processing)
                throw new Exception("Connection already processing a request!");

            StartTime = DateTime.MaxValue;
            State = HTTPConnectionStates.Processing;

            CurrentRequest = request;

            if (IsThreaded)
            {
#if NETFX_CORE
#pragma warning disable 4014
                Windows.System.Threading.ThreadPool.RunAsync(ThreadFunc);
#pragma warning restore 4014
#else
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadFunc));
                //new Thread(ThreadFunc)
                //    .Start();
#endif
            }
            else
                ThreadFunc(null);
        }

        protected virtual
#if NETFX_CORE
            async
#endif
            void ThreadFunc(object param)
        {

        }

        internal void HandleProgressCallback()
        {
            if (CurrentRequest.OnProgress != null && CurrentRequest.DownloadProgressChanged)
            {
                try
                {
                    CurrentRequest.OnProgress(CurrentRequest, CurrentRequest.Downloaded, CurrentRequest.DownloadLength);
                }
                catch (Exception ex)
                {
                    HTTPManager.Logger.Exception("ConnectionBase", "HandleProgressCallback - OnProgress", ex);
                }

                CurrentRequest.DownloadProgressChanged = false;
            }

            if (CurrentRequest.OnUploadProgress != null && CurrentRequest.UploadProgressChanged)
            {
                try
                {
                    CurrentRequest.OnUploadProgress(CurrentRequest, CurrentRequest.Uploaded, CurrentRequest.UploadLength);
                }
                catch (Exception ex)
                {
                    HTTPManager.Logger.Exception("ConnectionBase", "HandleProgressCallback - OnUploadProgress", ex);
                }
                CurrentRequest.UploadProgressChanged = false;
            }
        }

        internal void HandleCallback()
        {
            try
            {
                HandleProgressCallback();

                if (State == HTTPConnectionStates.Upgraded)
                {
                    if (CurrentRequest != null && CurrentRequest.Response != null && CurrentRequest.Response.IsUpgraded)
                        CurrentRequest.UpgradeCallback();
                    State = HTTPConnectionStates.WaitForProtocolShutdown;
                }
                else
                    CurrentRequest.CallCallback();
            }
            catch (Exception ex)
            {
                HTTPManager.Logger.Exception("ConnectionBase", "HandleCallback", ex);
            }
        }

        internal void Recycle(HTTPConnectionRecycledDelegate onConnectionRecycled)
        {
            OnConnectionRecycled = onConnectionRecycled;
            if (!(State > HTTPConnectionStates.Initial && State < HTTPConnectionStates.WaitForProtocolShutdown) || State == HTTPConnectionStates.Redirected)
                RecycleNow();
        }

        protected void RecycleNow()
        {
            if (State == HTTPConnectionStates.TimedOut ||
                State == HTTPConnectionStates.Closed)
                LastProcessTime = DateTime.MinValue;

            State = HTTPConnectionStates.Free;
            if (CurrentRequest != null)
                CurrentRequest.Dispose();
            CurrentRequest = null;

            if (OnConnectionRecycled != null)
            {
                OnConnectionRecycled(this);
                OnConnectionRecycled = null;
            }
        }

        #region Dispose Pattern

        protected bool IsDisposed { get; private set; }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            IsDisposed = true;
        }

        ~ConnectionBase()
        {
            Dispose(false);
        }

        #endregion
    }
}