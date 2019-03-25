#if !BESTHTTP_DISABLE_SIGNALR_CORE && !BESTHTTP_DISABLE_WEBSOCKET
using System;
using System.Collections.Generic;
using System.Text;

using BestHTTP.SignalRCore.Messages;

namespace BestHTTP.SignalRCore.Transports
{
    /// <summary>
    /// WebSockets transport implementation.
    /// https://github.com/aspnet/SignalR/blob/dev/specs/TransportProtocols.md#websockets-full-duplex
    /// </summary>
    internal sealed class WebSocketTransport : ITransport
    {
        public TransportTypes TransportType { get { return TransportTypes.WebSocket; } }
        public TransferModes TransferMode { get { return TransferModes.Binary; } }

        /// <summary>
        /// Current state of the transport. All changes will be propagated to the HubConnection through the onstateChanged event.
        /// </summary>
        public TransportStates State {
            get { return this._state; }
            private set
            {
                if (this._state != value)
                {
                    TransportStates oldState = this._state;
                    this._state = value;

                    if (this.OnStateChanged != null)
                        this.OnStateChanged(oldState, this._state);
                }
            }
        }
        private TransportStates _state;

        /// <summary>
        /// This will store the reason of failures so HubConnection can include it in its OnError event.
        /// </summary>
        public string ErrorReason { get; private set; }

        /// <summary>
        /// Called every time when the transport's <see cref="State"/> changed.
        /// </summary>
        public event Action<TransportStates, TransportStates> OnStateChanged;

        private WebSocket.WebSocket webSocket;
        private HubConnection connection;

        /// <summary>
        /// Cached list of parsed messages.
        /// </summary>
        private List<Message> messages = new List<Message>();

        public WebSocketTransport(HubConnection con)
        {
            this.connection = con;
            this.State = TransportStates.Initial;
        }

        void ITransport.StartConnect()
        {
            HTTPManager.Logger.Verbose("WebSocketTransport", "StartConnect");

            if (this.webSocket == null)
            {
                Uri uri = BuildUri(this.connection.Uri);

                // Also, if there's an authentication provider it can alter further our uri.
                if (this.connection.AuthenticationProvider != null)
                    uri = this.connection.AuthenticationProvider.PrepareUri(uri) ?? uri;

                HTTPManager.Logger.Verbose("WebSocketTransport", "StartConnect connecting to Uri: " + uri.ToString());

                this.webSocket = new WebSocket.WebSocket(uri);
            }

#if !UNITY_WEBGL || UNITY_EDITOR
            // prepare the internal http request
            if (this.connection.AuthenticationProvider != null)
                this.connection.AuthenticationProvider.PrepareRequest(webSocket.InternalRequest);
#endif
            this.webSocket.OnOpen += OnOpen;
            this.webSocket.OnMessage += OnMessage;
            this.webSocket.OnBinary += OnBinary;
            this.webSocket.OnErrorDesc += OnError;
            this.webSocket.OnClosed += OnClosed;

            this.webSocket.Open();
            
            this.State = TransportStates.Connecting;
        }
        
        void ITransport.Send(byte[] msg)
        {
            // To help debugging, in the editor when the plugin's loggging level is set to All, we will
            //  send all messages in textual form. This will help the readability when sent through a proxy.
#if UNITY_EDITOR
            if (HTTPManager.Logger.Level == Logger.Loglevels.All)
                this.webSocket.Send(System.Text.Encoding.UTF8.GetString(msg, 0, msg.Length));
            else
#endif
                this.webSocket.Send(msg);
        }

        // The websocket connection is open
        private void OnOpen(WebSocket.WebSocket webSocket)
        {
            HTTPManager.Logger.Verbose("WebSocketTransport", "OnOpen");

            // https://github.com/aspnet/SignalR/blob/dev/specs/HubProtocol.md#overview
            // When our websocket connection is open, send the 'negotiation' message to the server.

            string json = string.Format("{{'protocol':'{0}', 'version': 1}}", this.connection.Protocol.Encoder.Name);

            byte[] buffer = JsonProtocol.WithSeparator(json);

            (this as ITransport).Send(buffer);
        }

        private string ParseHandshakeResponse(string data)
        {
            // The handshake response is
            //  -an empty json object ('{}') if the handshake process is succesfull
            //  -otherwise it has one 'error' field

            Dictionary<string, object> response = BestHTTP.JSON.Json.Decode(data) as Dictionary<string, object>;

            if (response == null)
                return "Couldn't parse json data: " + data;

            object error;
            if (response.TryGetValue("error", out error))
                return error.ToString();

            return null;
        }

        private void HandleHandshakeResponse(string data)
        {
            this.ErrorReason = ParseHandshakeResponse(data);

            this.State = string.IsNullOrEmpty(this.ErrorReason) ? TransportStates.Connected : TransportStates.Failed;
        }

        private void OnMessage(WebSocket.WebSocket webSocket, string data)
        {
            if (this.State == TransportStates.Connecting)
            {
                HandleHandshakeResponse(data);

                return;
            }

            this.messages.Clear();
            try
            {
                this.connection.Protocol.ParseMessages(data, ref this.messages);

                this.connection.OnMessages(this.messages);
            }
            catch (Exception ex)
            {
                HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage(string)", ex);
            }
            finally
            {
                this.messages.Clear();
            }
        }

        private void OnBinary(WebSocket.WebSocket webSocket, byte[] data)
        {
            if (this.State == TransportStates.Connecting)
            {
                HandleHandshakeResponse(System.Text.Encoding.UTF8.GetString(data, 0, data.Length));

                return;
            }

            this.messages.Clear();
            try
            {
                this.connection.Protocol.ParseMessages(data, ref this.messages);

                this.connection.OnMessages(this.messages);
            }
            catch (Exception ex)
            {
                HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage(byte[])", ex);
            }
            finally
            {
                this.messages.Clear();
            }
        }

        private void OnError(WebSocket.WebSocket webSocket, string reason)
        {
            HTTPManager.Logger.Verbose("WebSocketTransport", "OnError: " + reason);

            this.ErrorReason = reason;
            this.State = TransportStates.Failed;
        }

        private void OnClosed(WebSocket.WebSocket webSocket, ushort code, string message)
        {
            HTTPManager.Logger.Verbose("WebSocketTransport", "OnClosed: " + code + " " + message);

            this.webSocket = null;

            this.State = TransportStates.Closed;
        }

        void ITransport.StartClose()
        {
            HTTPManager.Logger.Verbose("WebSocketTransport", "StartClose");

            if (this.webSocket != null)
            {
                this.State = TransportStates.Closing;
                this.webSocket.Close();
            }
            else
                this.State = TransportStates.Closed;
        }

        private Uri BuildUri(Uri baseUri)
        {
            if (this.connection.NegotiationResult == null)
                return baseUri;

            UriBuilder builder = new UriBuilder(baseUri);
            StringBuilder queryBuilder = new StringBuilder();

            queryBuilder.Append(baseUri.Query);
            if (!string.IsNullOrEmpty(this.connection.NegotiationResult.ConnectionId))
                queryBuilder.Append("&id=").Append(this.connection.NegotiationResult.ConnectionId);

            builder.Query = queryBuilder.ToString();

            return builder.Uri;
        }
    }
}
#endif