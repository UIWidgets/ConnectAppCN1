#if UNITY_WEBGL && !UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using BestHTTP.Authentication;
using BestHTTP.Extensions;

namespace BestHTTP
{
    delegate void OnWebGLRequestHandlerDelegate(int nativeId, int httpStatus, IntPtr pBuffer, int length, int zero);
    delegate void OnWebGLBufferDelegate(int nativeId, IntPtr pBuffer, int length);
    delegate void OnWebGLProgressDelegate(int nativeId, int downloaded, int total);
    delegate void OnWebGLErrorDelegate(int nativeId, string error);
    delegate void OnWebGLTimeoutDelegate(int nativeId);
    delegate void OnWebGLAbortedDelegate(int nativeId);

    internal sealed class WebGLConnection : ConnectionBase
    {
        static Dictionary<int, WebGLConnection> Connections = new Dictionary<int, WebGLConnection>(4);

        int NativeId;
        BufferPoolMemoryStream Stream;

        public WebGLConnection(string serverAddress)
            : base(serverAddress, false)
        {
            XHR_SetLoglevel((byte)HTTPManager.Logger.Level);
        }

        internal override void Abort(HTTPConnectionStates newState)
        {
            State = newState;

            switch (State)
            {
                case HTTPConnectionStates.TimedOut: TimedOutStart = DateTime.UtcNow; break;
            }

            XHR_Abort(this.NativeId);
        }

        protected override void ThreadFunc(object param /*null*/)
        {
            // XmlHttpRequest setup

            this.NativeId = XHR_Create(HTTPRequest.MethodNames[(byte)CurrentRequest.MethodType],
                                       CurrentRequest.CurrentUri.OriginalString,
                                       CurrentRequest.Credentials != null ? CurrentRequest.Credentials.UserName : null,
                                       CurrentRequest.Credentials != null ? CurrentRequest.Credentials.Password : null,
                                       CurrentRequest.WithCredentials ? 1 : 0);
            Connections.Add(NativeId, this);

            CurrentRequest.EnumerateHeaders((header, values) =>
                {
                    if (header != "Content-Length")
                        for (int i = 0; i < values.Count; ++i)
                            XHR_SetRequestHeader(NativeId, header, values[i]);
                }, /*callBeforeSendCallback:*/ true);

            byte[] body = CurrentRequest.GetEntityBody();

            XHR_SetResponseHandler(NativeId, WebGLConnection.OnResponse, WebGLConnection.OnError, WebGLConnection.OnTimeout, WebGLConnection.OnAborted);
            // Setting OnUploadProgress result in an addEventListener("progress", ...) call making the request non-simple.
            // https://forum.unity.com/threads/best-http-released.200006/page-49#post-3696220
            XHR_SetProgressHandler(NativeId, WebGLConnection.OnDownloadProgress, CurrentRequest.OnUploadProgress == null ? (OnWebGLProgressDelegate)null : WebGLConnection.OnUploadProgress);

            XHR_SetTimeout(NativeId, (uint)(CurrentRequest.ConnectTimeout.TotalMilliseconds + CurrentRequest.Timeout.TotalMilliseconds));

            XHR_Send(NativeId, body, body != null ? body.Length : 0);
        }

#region Callback Implementations

        void OnResponse(int httpStatus, byte[] buffer, int bufferLength)
        {
            try
            {
                using (var ms = new BufferPoolMemoryStream())
                {
                    Stream = ms;

                    XHR_GetStatusLine(NativeId, OnBufferCallback);
                    XHR_GetResponseHeaders(NativeId, OnBufferCallback);

                    if (buffer != null && bufferLength > 0)
                        ms.Write(buffer, 0, bufferLength);

                    ms.Seek(0L, SeekOrigin.Begin);

                    var internalBuffer = ms.GetBuffer();
                    string tmp = System.Text.Encoding.UTF8.GetString(internalBuffer);
                    HTTPManager.Logger.Information(this.NativeId + " OnResponse - full response ", tmp);

                    SupportedProtocols protocol = CurrentRequest.ProtocolHandler == SupportedProtocols.Unknown ? HTTPProtocolFactory.GetProtocolFromUri(CurrentRequest.CurrentUri) : CurrentRequest.ProtocolHandler;
                    CurrentRequest.Response = HTTPProtocolFactory.Get(protocol, CurrentRequest, ms, CurrentRequest.UseStreaming, false);

                    CurrentRequest.Response.Receive(buffer != null && bufferLength > 0 ? (int)bufferLength : -1, true);

                    if (CurrentRequest.IsCookiesEnabled)
                        BestHTTP.Cookies.CookieJar.Set(CurrentRequest.Response);
                }
            }
            catch (Exception e)
            {
                HTTPManager.Logger.Exception(this.NativeId + " WebGLConnection", "OnResponse", e);

                if (CurrentRequest != null)
                {
                    // Something gone bad, Response must be null!
                    CurrentRequest.Response = null;

                    switch (State)
                    {
                        case HTTPConnectionStates.AbortRequested:
                            CurrentRequest.State = HTTPRequestStates.Aborted;
                            break;
                        case HTTPConnectionStates.TimedOut:
                            CurrentRequest.State = HTTPRequestStates.TimedOut;
                            break;
                        default:
                            CurrentRequest.Exception = e;
                            CurrentRequest.State = HTTPRequestStates.Error;
                            break;
                    }
                }
            }
            finally
            {
                Connections.Remove(NativeId);

                Stream = null;

                if (CurrentRequest != null)
                    lock (HTTPManager.Locker)
                    {
                        State = HTTPConnectionStates.Closed;
                        if (CurrentRequest.State == HTTPRequestStates.Processing)
                        {
                            if (CurrentRequest.Response != null)
                                CurrentRequest.State = HTTPRequestStates.Finished;
                            else
                                CurrentRequest.State = HTTPRequestStates.Error;
                        }
                    }

                LastProcessTime = DateTime.UtcNow;

                if (OnConnectionRecycled != null)
                    RecycleNow();

                XHR_Release(NativeId);
            }
        }

        void OnBuffer(byte[] buffer, int bufferLength)
        {
            if (Stream != null)
            {
                Stream.Write(buffer, 0, bufferLength);
                //Stream.Write(new byte[2] { HTTPResponse.CR, HTTPResponse.LF }, 0, 2);
                Stream.Write(HTTPRequest.EOL, 0, HTTPRequest.EOL.Length);
            }
        }

        void OnDownloadProgress(int down, int total)
        {
            CurrentRequest.Downloaded = down;
            CurrentRequest.DownloadLength = total;
            CurrentRequest.DownloadProgressChanged = true;
        }

        void OnUploadProgress(int up, int total)
        {
            CurrentRequest.Uploaded = up;
            CurrentRequest.UploadLength = total;
            CurrentRequest.UploadProgressChanged = true;
        }

        void OnError(string error)
        {
            HTTPManager.Logger.Information(this.NativeId + " WebGLConnection - OnError", error);

            Connections.Remove(NativeId);

            Stream = null;

            if (CurrentRequest != null)
                lock (HTTPManager.Locker)
                {
                    State = HTTPConnectionStates.Closed;
                    CurrentRequest.State = HTTPRequestStates.Error;
                    CurrentRequest.Exception = new Exception(error);
                }

            LastProcessTime = DateTime.UtcNow;

            if (OnConnectionRecycled != null)
                RecycleNow();

            XHR_Release(NativeId);
        }

        void OnTimeout()
        {
            HTTPManager.Logger.Information(this.NativeId + " WebGLConnection - OnResponse", string.Empty);

            Connections.Remove(NativeId);

            Stream = null;

            if (CurrentRequest != null)
                lock (HTTPManager.Locker)
                {
                    State = HTTPConnectionStates.Closed;
                    CurrentRequest.State = HTTPRequestStates.TimedOut;
                }

            LastProcessTime = DateTime.UtcNow;

            if (OnConnectionRecycled != null)
                RecycleNow();

            XHR_Release(NativeId);
        }

        void OnAborted()
        {
            HTTPManager.Logger.Information(this.NativeId + " WebGLConnection - OnAborted", string.Empty);

            Connections.Remove(NativeId);

            Stream = null;

            if (CurrentRequest != null)
                lock (HTTPManager.Locker)
                {
                    State = HTTPConnectionStates.Closed;
                    CurrentRequest.State = HTTPRequestStates.Aborted;
                }

            LastProcessTime = DateTime.UtcNow;

            if (OnConnectionRecycled != null)
                RecycleNow();

            XHR_Release(NativeId);
        }

#endregion

#region WebGL Static Callbacks

        [AOT.MonoPInvokeCallback(typeof(OnWebGLRequestHandlerDelegate))]
        static void OnResponse(int nativeId, int httpStatus, IntPtr pBuffer, int length, int err)
        {
            HTTPManager.Logger.Information("WebGLConnection - OnResponse", string.Format("{0} {1} {2} {3}", nativeId, httpStatus, length, err));

            WebGLConnection conn = null;
            if (!Connections.TryGetValue(nativeId, out conn))
            {
                HTTPManager.Logger.Error("WebGLConnection - OnResponse", "No WebGL connection found for nativeId: " + nativeId.ToString());
                return;
            }

            byte[] buffer = VariableSizedBufferPool.Get(length, true);

            // Copy data from the 'unmanaged' memory to managed memory. Buffer will be reclaimed by the GC.
            Marshal.Copy(pBuffer, buffer, 0, length);

            conn.OnResponse(httpStatus, buffer, length);

            VariableSizedBufferPool.Release(buffer);
        }

        [AOT.MonoPInvokeCallback(typeof(OnWebGLBufferDelegate))]
        static void OnBufferCallback(int nativeId, IntPtr pBuffer, int length)
        {
            WebGLConnection conn = null;
            if (!Connections.TryGetValue(nativeId, out conn))
            {
                HTTPManager.Logger.Error("WebGLConnection - OnBufferCallback", "No WebGL connection found for nativeId: " + nativeId.ToString());
                return;
            }

            byte[] buffer = VariableSizedBufferPool.Get(length, true);

            // Copy data from the 'unmanaged' memory to managed memory. Buffer will be reclaimed by the GC.
            Marshal.Copy(pBuffer, buffer, 0, length);

            conn.OnBuffer(buffer, length);

            VariableSizedBufferPool.Release(buffer);
        }

        [AOT.MonoPInvokeCallback(typeof(OnWebGLProgressDelegate))]
        static void OnDownloadProgress(int nativeId, int downloaded, int total)
        {
            HTTPManager.Logger.Information(nativeId + " OnDownloadProgress", downloaded.ToString() + " / " + total.ToString());

            WebGLConnection conn = null;
            if (!Connections.TryGetValue(nativeId, out conn))
            {
                HTTPManager.Logger.Error("WebGLConnection - OnDownloadProgress", "No WebGL connection found for nativeId: " + nativeId.ToString());
                return;
            }

            conn.OnDownloadProgress(downloaded, total);
        }

        [AOT.MonoPInvokeCallback(typeof(OnWebGLProgressDelegate))]
        static void OnUploadProgress(int nativeId, int uploaded, int total)
        {
            HTTPManager.Logger.Information(nativeId + " OnUploadProgress", uploaded.ToString() + " / " + total.ToString());

            WebGLConnection conn = null;
            if (!Connections.TryGetValue(nativeId, out conn))
            {
                HTTPManager.Logger.Error("WebGLConnection - OnUploadProgress", "No WebGL connection found for nativeId: " + nativeId.ToString());
                return;
            }

            conn.OnUploadProgress(uploaded, total);
        }

        [AOT.MonoPInvokeCallback(typeof(OnWebGLErrorDelegate))]
        static void OnError(int nativeId, string error)
        {
            WebGLConnection conn = null;
            if (!Connections.TryGetValue(nativeId, out conn))
            {
                HTTPManager.Logger.Error("WebGLConnection - OnError", "No WebGL connection found for nativeId: " + nativeId.ToString() + " Error: " + error);
                return;
            }

            conn.OnError(error);
        }

        [AOT.MonoPInvokeCallback(typeof(OnWebGLTimeoutDelegate))]
        static void OnTimeout(int nativeId)
        {
            WebGLConnection conn = null;
            if (!Connections.TryGetValue(nativeId, out conn))
            {
                HTTPManager.Logger.Error("WebGLConnection - OnTimeout", "No WebGL connection found for nativeId: " + nativeId.ToString());
                return;
            }

            conn.OnTimeout();
        }

        [AOT.MonoPInvokeCallback(typeof(OnWebGLAbortedDelegate))]
        static void OnAborted(int nativeId)
        {
            WebGLConnection conn = null;
            if (!Connections.TryGetValue(nativeId, out conn))
            {
                HTTPManager.Logger.Error("WebGLConnection - OnAborted", "No WebGL connection found for nativeId: " + nativeId.ToString());
                return;
            }

            conn.OnAborted();
        }

#endregion

#region WebGL Interface

        [DllImport("__Internal")]
        private static extern int XHR_Create(string method, string url, string userName, string passwd, int withCredentials);

        /// <summary>
        /// Is an unsigned long representing the number of milliseconds a request can take before automatically being terminated. A value of 0 (which is the default) means there is no timeout.
        /// </summary>
        [DllImport("__Internal")]
        private static extern void XHR_SetTimeout(int nativeId, uint timeout);

        [DllImport("__Internal")]
        private static extern void XHR_SetRequestHeader(int nativeId, string header, string value);

        [DllImport("__Internal")]
        private static extern void XHR_SetResponseHandler(int nativeId, OnWebGLRequestHandlerDelegate onresponse, OnWebGLErrorDelegate onerror, OnWebGLTimeoutDelegate ontimeout, OnWebGLAbortedDelegate onabort);

        [DllImport("__Internal")]
        private static extern void XHR_SetProgressHandler(int nativeId, OnWebGLProgressDelegate onDownloadProgress, OnWebGLProgressDelegate onUploadProgress);

        [DllImport("__Internal")]
        private static extern void XHR_Send(int nativeId, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)] byte[] body, int length);

        [DllImport("__Internal")]
        private static extern void XHR_GetResponseHeaders(int nativeId, OnWebGLBufferDelegate callback);

        [DllImport("__Internal")]
        private static extern void XHR_GetStatusLine(int nativeId, OnWebGLBufferDelegate callback);

        [DllImport("__Internal")]
        private static extern void XHR_Abort(int nativeId);

        [DllImport("__Internal")]
        private static extern void XHR_Release(int nativeId);

        [DllImport("__Internal")]
        private static extern void XHR_SetLoglevel(int logLevel);

#endregion
    }
}

#endif