#if !BESTHTTP_DISABLE_SIGNALR_CORE && !BESTHTTP_DISABLE_WEBSOCKET
using BestHTTP.Futures;
using BestHTTP.SignalRCore.Authentication;
using BestHTTP.SignalRCore.Messages;
using System;
using System.Collections.Generic;

namespace BestHTTP.SignalRCore
{
    public sealed class HubOptions
    {
        /// <summary>
        /// When this is set to true, the plugin will skip the negotiation request if the PreferedTransport is WebSocket. Its default value is false.
        /// </summary>
        public bool SkipNegotiation { get; set; }

        /// <summary>
        /// The preferred transport to choose when more than one available. Its default value is TransportTypes.WebSocket.
        /// </summary>
        public TransportTypes PreferedTransport { get; set; }

        /// <summary>
        /// A ping message is only sent if the interval has elapsed without a message being sent. Its default value is 15 seconds.
        /// </summary>
        public TimeSpan PingInterval { get; set; }

        /// <summary>
        /// The maximum count of redirect negoitiation result that the plugin will follow. Its default value is 100.
        /// </summary>
        public int MaxRedirects { get; set; }

        public HubOptions()
        {
            this.SkipNegotiation = false;
            this.PreferedTransport = TransportTypes.WebSocket;
            this.PingInterval = TimeSpan.FromSeconds(15);
            this.MaxRedirects = 100;
        }
    }

    public sealed class HubConnection : BestHTTP.Extensions.IHeartbeat
    {
        public static readonly object[] EmptyArgs = new object[0];

        /// <summary>
        /// Uri  of the Hub endpoint
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Current state of this connection.
        /// </summary>
        public ConnectionStates State { get; private set; }

        /// <summary>
        /// Current, active ITransport instance.
        /// </summary>
        public ITransport Transport { get; private set; }

        /// <summary>
        /// The IProtocol implementation that will parse, encode and decode messages.
        /// </summary>
        public IProtocol Protocol { get; private set; }

        /// <summary>
        /// This event is called when the connection is redirected to a new uri.
        /// </summary>
        public event Action<HubConnection, Uri, Uri> OnRedirected;

        /// <summary>
        /// This event is called when successfully connected to the hub.
        /// </summary>
        public event Action<HubConnection> OnConnected;

        /// <summary>
        /// This event is called when an unexpected error happen and the connection is closed.
        /// </summary>
        public event Action<HubConnection, string> OnError;

        /// <summary>
        /// This event is called when the connection is gracefully terminated.
        /// </summary>
        public event Action<HubConnection> OnClosed;

        /// <summary>
        /// This event is called for every server-sent message. When returns false, no further processing of the message is done
        /// by the plugin.
        /// </summary>
        public event Func<HubConnection, Message, bool> OnMessage;

        /// <summary>
        /// An IAuthenticationProvider implementation that will be used to authenticate the connection.
        /// </summary>
        public IAuthenticationProvider AuthenticationProvider { get; set; }

        /// <summary>
        /// Negotiation response sent by the server.
        /// </summary>
        public NegotiationResult NegotiationResult { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public HubOptions Options { get; private set; }

        /// <summary>
        /// How many times this connection is redirected.
        /// </summary>
        public int RedirectCount { get; private set; }

        /// <summary>
        /// This will be increment to add a unique id to every message the plugin will send.
        /// </summary>
        private long lastInvocationId = 0;

        /// <summary>
        ///  Store the callback for all sent message that expect a return value from the server. All sent message has
        ///  a unique invocationId that will be sent back from the server.
        /// </summary>
        private Dictionary<long, Action<Message>> invocations = new Dictionary<long, Action<Message>>();

        /// <summary>
        /// This is where we store the methodname => callback mapping.
        /// </summary>
        private Dictionary<string, Subscription> subscriptions = new Dictionary<string, Subscription>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// When we sent out the last message to the server.
        /// </summary>
        private DateTime lastMessageSent;

        public HubConnection(Uri hubUri, IProtocol protocol)
            : this(hubUri, protocol, new HubOptions())
        {
        }

        public HubConnection(Uri hubUri, IProtocol protocol, HubOptions options)
        {
            this.Uri = hubUri;
            this.State = ConnectionStates.Initial;
            this.Options = options;
            this.Protocol = protocol;
            this.Protocol.Connection = this;
            this.AuthenticationProvider = new DefaultAccessTokenAuthenticator(this);
        }

        public void StartConnect()
        {
            if (this.State != ConnectionStates.Initial && this.State != ConnectionStates.Redirected)
            {
                HTTPManager.Logger.Warning("HubConnection", "StartConnect - Expected Initial or Redirected state, got " + this.State.ToString());
                return;
            }

            HTTPManager.Logger.Verbose("HubConnection", "StartConnect");

            if (this.AuthenticationProvider != null && this.AuthenticationProvider.IsPreAuthRequired)
            {
                HTTPManager.Logger.Information("HubConnection", "StartConnect - Authenticating");
                SetState(ConnectionStates.Authenticating);

                this.AuthenticationProvider.OnAuthenticationSucceded += OnAuthenticationSucceded;
                this.AuthenticationProvider.OnAuthenticationFailed += OnAuthenticationFailed;

                // Start the authentication process
                this.AuthenticationProvider.StartAuthentication();
            }
            else
                StartNegotiation();
        }

        private void OnAuthenticationSucceded(IAuthenticationProvider provider)
        {
            HTTPManager.Logger.Verbose("HubConnection", "OnAuthenticationSucceded");

            this.AuthenticationProvider.OnAuthenticationSucceded -= OnAuthenticationSucceded;
            this.AuthenticationProvider.OnAuthenticationFailed -= OnAuthenticationFailed;

            StartNegotiation();
        }

        private void OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
        {
            HTTPManager.Logger.Error("HubConnection", "OnAuthenticationFailed: " + reason);

            this.AuthenticationProvider.OnAuthenticationSucceded -= OnAuthenticationSucceded;
            this.AuthenticationProvider.OnAuthenticationFailed -= OnAuthenticationFailed;

            SetState(ConnectionStates.Closed, reason);
        }

        private void StartNegotiation()
        {
            HTTPManager.Logger.Verbose("HubConnection", "StartNegotiation");

            if (this.State == ConnectionStates.CloseInitiated)
            {
                SetState(ConnectionStates.Closed);
                return;
            }

            if (this.Options.SkipNegotiation)
            {
                HTTPManager.Logger.Verbose("HubConnection", "Skipping negotiation");
                ConnectImpl();

                return;
            }

            SetState(ConnectionStates.Negotiating);

            // https://github.com/aspnet/SignalR/blob/dev/specs/TransportProtocols.md#post-endpoint-basenegotiate-request
            // Send out a negotiation request. While we could skip it and connect right with the websocket transport
            //  it might return with additional information that could be useful.

            UriBuilder builder = new UriBuilder(this.Uri);
            builder.Path += "/negotiate";

            var request = new HTTPRequest(builder.Uri, HTTPMethods.Post, OnNegotiationRequestFinished);
            if (this.AuthenticationProvider != null)
                this.AuthenticationProvider.PrepareRequest(request);

            request.Send();
        }
        
        private void ConnectImpl()
        {
            HTTPManager.Logger.Verbose("HubConnection", "ConnectImpl");

            switch (this.Options.PreferedTransport)
            {
                case TransportTypes.WebSocket:
                    if (this.NegotiationResult != null && !IsTransportSupported("WebSockets"))
                    {
                        SetState(ConnectionStates.Closed, "The 'WebSockets' transport isn't supported by the server!");
                        return;
                    }

                    this.Transport = new Transports.WebSocketTransport(this);
                    this.Transport.OnStateChanged += Transport_OnStateChanged;
                    break;

                default:
                    SetState(ConnectionStates.Closed, "Unsupportted transport: " + this.Options.PreferedTransport);
                    break;
            }

            this.Transport.StartConnect();
        }

        private bool IsTransportSupported(string transportName)
        {
            // https://github.com/aspnet/SignalR/blob/release/2.2/specs/TransportProtocols.md#post-endpoint-basenegotiate-request
            // If the negotiation response contains only the url and accessToken, no 'availableTransports' list is sent
            if (this.NegotiationResult.SupportedTransports == null)
                return true;

            for (int i = 0; i < this.NegotiationResult.SupportedTransports.Count; ++i)
                if (this.NegotiationResult.SupportedTransports[i].Name == transportName)
                    return true;

            return false;
        }

        private void OnNegotiationRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            if (this.State == ConnectionStates.CloseInitiated)
            {
                SetState(ConnectionStates.Closed);
                return;
            }

            string errorReason = null;

            switch (req.State)
            {
                // The request finished without any problem.
                case HTTPRequestStates.Finished:
                    if (resp.IsSuccess)
                    {
                        HTTPManager.Logger.Information("HubConnection", "Negotiation Request Finished Successfully! Response: " + resp.DataAsText);

                        // Parse negotiation
                        this.NegotiationResult = NegotiationResult.Parse(resp.DataAsText, out errorReason, this);

                        // TODO: check validity of the negotiation result:
                        //  If url and accessToken is present, the other two must be null.
                        //  https://github.com/aspnet/SignalR/blob/dev/specs/TransportProtocols.md#post-endpoint-basenegotiate-request

                        if (string.IsNullOrEmpty(errorReason))
                        {
                            if (this.NegotiationResult.Url != null)
                            {
                                this.SetState(ConnectionStates.Redirected);

                                if (++this.RedirectCount >= this.Options.MaxRedirects)
                                    errorReason = string.Format("MaxRedirects ({0:N0}) reached!", this.Options.MaxRedirects);
                                else
                                {
                                    var oldUri = this.Uri;
                                    this.Uri = this.NegotiationResult.Url;

                                    if (this.OnRedirected != null)
                                    {
                                        try
                                        {
                                            this.OnRedirected(this, oldUri, Uri);
                                        }
                                        catch (Exception ex)
                                        {
                                            HTTPManager.Logger.Exception("HubConnection", "OnNegotiationRequestFinished - OnRedirected", ex);
                                        }
                                    }

                                    StartConnect();
                                }
                            }
                            else
                                ConnectImpl();
                        }
                    }
                    else // Internal server error?
                        errorReason = string.Format("Negotiation Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                        resp.StatusCode,
                                                        resp.Message,
                                                        resp.DataAsText);
                    break;

                // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                case HTTPRequestStates.Error:
                    errorReason = "Negotiation Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
                    break;

                // The request aborted, initiated by the user.
                case HTTPRequestStates.Aborted:
                    errorReason = "Negotiation Request Aborted!";
                    break;

                // Connecting to the server is timed out.
                case HTTPRequestStates.ConnectionTimedOut:
                    errorReason = "Negotiation Request - Connection Timed Out!";
                    break;

                // The request didn't finished in the given time.
                case HTTPRequestStates.TimedOut:
                    errorReason = "Negotiation Request - Processing the request Timed Out!";
                    break;
            }

            if (errorReason != null)
                SetState(ConnectionStates.Closed, errorReason);
        }

        public void StartClose()
        {
            HTTPManager.Logger.Verbose("HubConnection", "StartClose");

            SetState(ConnectionStates.CloseInitiated);

            if (this.Transport != null)
                this.Transport.StartClose();
        }

        public IFuture<StreamItemContainer<TResult>> Stream<TResult>(string target, params object[] args)
        {
            var future = new Future<StreamItemContainer<TResult>>();
            
            long id = InvokeImp(target,
                        args,
                        callback: (message) =>
                        {
                            switch (message.type)
                            {
                                // StreamItem message contains only one item.
                                case MessageTypes.StreamItem:
                                    {
                                        var container = future.value;

                                        if (container.IsCanceled)
                                            break;

                                        container.AddItem((TResult)this.Protocol.ConvertTo(typeof(TResult), message.item));

                                        // (re)assign the container to raise OnItem event
                                        future.AssignItem(container);
                                        break;
                                    }

                                case MessageTypes.Completion:
                                    {
                                        bool isSuccess = string.IsNullOrEmpty(message.error);
                                        if (isSuccess)
                                        {
                                            var container = future.value;

                                            // While completion message must not contain any result, this should be future-proof
                                            //if (!container.IsCanceled && message.Result != null)
                                            //{
                                            //    TResult[] results = (TResult[])this.Protocol.ConvertTo(typeof(TResult[]), message.Result);
                                            //
                                            //    container.AddItems(results);
                                            //}

                                            future.Assign(container);
                                        }
                                        else
                                            future.Fail(new Exception(message.error));
                                        break;
                                    }
                            }
                        }, 
                        isStreamingInvocation: true);

            future.BeginProcess(new StreamItemContainer<TResult>(id));

            return future;
        }

        public void CancelStream<T>(StreamItemContainer<T> container)
        {
            Message message = new Message {
                type = MessageTypes.CancelInvocation,
                invocationId = container.id.ToString()
            };

            container.IsCanceled = true;

            SendMessage(message);
        }

        public IFuture<TResult> Invoke<TResult>(string target, params object[] args)
        {
            Future<TResult> future = new Future<TResult>();

            InvokeImp(target,
                args,
                (message) =>
                    {
                        bool isSuccess = string.IsNullOrEmpty(message.error);
                        if (isSuccess)
                            future.Assign((TResult)this.Protocol.ConvertTo(typeof(TResult), message.result));
                        else
                            future.Fail(new Exception(message.error));
                    });

            return future;
        }

        public IFuture<bool> Send(string target, params object[] args)
        {
            Future<bool> future = new Future<bool>();

            InvokeImp(target,
                args,
                (message) =>
                    {
                        bool isSuccess = string.IsNullOrEmpty(message.error);
                        if (isSuccess)
                            future.Assign(true);
                        else
                            future.Fail(new Exception(message.error));
                    });

            return future;
        }

        private long InvokeImp(string target, object[] args, Action<Message> callback, bool isStreamingInvocation = false)
        {
            if (this.State != ConnectionStates.Connected)
                return -1;

            long invocationId = System.Threading.Interlocked.Increment(ref this.lastInvocationId);
            var message = new Message
            {
                type = isStreamingInvocation ? MessageTypes.StreamInvocation : MessageTypes.Invocation,
                invocationId = invocationId.ToString(),
                target = target,
                arguments = args,
                nonblocking = callback == null,
            };

            SendMessage(message);

            if (callback != null)
                this.invocations.Add(invocationId, callback);

            return invocationId;
        }

        private void SendMessage(Message message)
        {
            byte[] encoded = this.Protocol.EncodeMessage(message);
            this.Transport.Send(encoded);

            this.lastMessageSent = DateTime.UtcNow;
        }

        public void On(string methodName, Action callback)
        {
            On(methodName, null, (args) => callback());
        }

        public void On<T1>(string methodName, Action<T1> callback)
        {
            On(methodName, new Type[] { typeof(T1) }, (args) => callback((T1)args[0]));
        }

        public void On<T1, T2>(string methodName, Action<T1, T2> callback)
        {
            On(methodName,
                new Type[] { typeof(T1), typeof(T2) },
                (args) => callback((T1)args[0], (T2)args[1]));
        }

        public void On<T1, T2, T3>(string methodName, Action<T1, T2, T3> callback)
        {
            On(methodName,
                new Type[] { typeof(T1), typeof(T2), typeof(T3) },
                (args) => callback((T1)args[0], (T2)args[1], (T3)args[2]));
        }

        public void On<T1, T2, T3, T4>(string methodName, Action<T1, T2, T3, T4> callback)
        {
            On(methodName,
                new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) },
                (args) => callback((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]));
        }

        public void On(string methodName, Type[] paramTypes, Action<object[]> callback)
        {
            Subscription subscription = null;
            if (!this.subscriptions.TryGetValue(methodName, out subscription))
                this.subscriptions.Add(methodName, subscription = new Subscription());

            subscription.Add(paramTypes, callback);
        }

        internal void OnMessages(List<Message> messages)
        {
            for (int messageIdx = 0; messageIdx < messages.Count; ++messageIdx)
            {
                var message = messages[messageIdx];

                try
                {
                    if (this.OnMessage != null && !this.OnMessage(this, message))
                        return;
                }
                catch (Exception ex)
                {
                    HTTPManager.Logger.Exception("HubConnection", "Exception in OnMessage user code!", ex);
                }

                switch (message.type)
                {
                    case MessageTypes.Invocation:
                        {
                            Subscription subscribtion = null;
                            if (this.subscriptions.TryGetValue(message.target, out subscribtion))
                            {
                                for (int i = 0; i < subscribtion.callbacks.Count; ++i)
                                {
                                    var callbackDesc = subscribtion.callbacks[i];

                                    object[] realArgs = null;
                                    try
                                    {
                                        realArgs = this.Protocol.GetRealArguments(callbackDesc.ParamTypes, message.arguments);
                                    }
                                    catch (Exception ex)
                                    {
                                        HTTPManager.Logger.Exception("HubConnection", "OnMessages - Invocation - GetRealArguments", ex);
                                    }

                                    try
                                    {
                                        callbackDesc.Callback.Invoke(realArgs);
                                    }
                                    catch (Exception ex)
                                    {
                                        HTTPManager.Logger.Exception("HubConnection", "OnMessages - Invocation - Invoke", ex);
                                    }
                                }
                            }

                            break;
                        }

                    case MessageTypes.StreamItem:
                        {
                            long invocationId;
                            if (long.TryParse(message.invocationId, out invocationId))
                            {
                                Action<Message> callback;
                                if (this.invocations.TryGetValue(invocationId, out callback) && callback != null)
                                {
                                    try
                                    {
                                        callback(message);
                                    }
                                    catch (Exception ex)
                                    {
                                        HTTPManager.Logger.Exception("HubConnection", "OnMessages - StreamItem - callback", ex);
                                    }
                                }
                            }
                            break;
                        }

                    case MessageTypes.Completion:
                        {
                            long invocationId;
                            if (long.TryParse(message.invocationId, out invocationId))
                            {
                                Action<Message> callback;
                                if (this.invocations.TryGetValue(invocationId, out callback) && callback != null)
                                {
                                    try
                                    {
                                        callback(message);
                                    }
                                    catch (Exception ex)
                                    {
                                        HTTPManager.Logger.Exception("HubConnection", "OnMessages - Completion - callback", ex);
                                    }
                                }
                                this.invocations.Remove(invocationId);
                            }
                            break;
                        }

                    case MessageTypes.Close:
                        SetState(ConnectionStates.Closed, message.error);
                        break;
                }
            }
        }

        private void Transport_OnStateChanged(TransportStates oldState, TransportStates newState)
        {
            HTTPManager.Logger.Verbose("HubConnection", string.Format("Transport_OnStateChanged - oldState: {0} newState: {1}", oldState.ToString(), newState.ToString()));

            switch (newState)
            {
                case TransportStates.Connected:
                    SetState(ConnectionStates.Connected);
                    break;

                case TransportStates.Failed:
                    SetState(ConnectionStates.Closed, this.Transport.ErrorReason);
                    break;

                case TransportStates.Closed:
                    SetState(ConnectionStates.Closed);
                    break;
            }
        }

        private void SetState(ConnectionStates state, string errorReason = null)
        {
            if (string.IsNullOrEmpty(errorReason))
                HTTPManager.Logger.Information("HubConnection", "SetState - from State: '" + this.State.ToString() + "' to State: '" + state.ToString() + "'");
            else
                HTTPManager.Logger.Information("HubConnection", "SetState - from State: '" + this.State.ToString() + "' to State: '" + state.ToString() + "' errorReason: '" + errorReason + "'");

            if (this.State == state)
                return;

            this.State = state;

            switch (state)
            {
                case ConnectionStates.Initial:
                case ConnectionStates.Authenticating:
                case ConnectionStates.Negotiating:
                case ConnectionStates.CloseInitiated:
                    break;

                case ConnectionStates.Connected:
                    try
                    {
                        if (this.OnConnected != null)
                            this.OnConnected(this);
                    }
                    catch(Exception ex)
                    {
                        HTTPManager.Logger.Exception("HubConnection", "Exception in OnConnected user code!", ex);
                    }

                    HTTPManager.Heartbeats.Subscribe(this);
                    this.lastMessageSent = DateTime.UtcNow;
                    break;

                case ConnectionStates.Closed:
                    if (string.IsNullOrEmpty(errorReason))
                    {
                        if (this.OnClosed != null)
                        {
                            try
                            {
                                this.OnClosed(this);
                            }
                            catch(Exception ex)
                            {
                                HTTPManager.Logger.Exception("HubConnection", "Exception in OnClosed user code!", ex);
                            }
                        }
                    }
                    else
                    {
                        if (this.OnError != null)
                        {
                            try
                            {
                                this.OnError(this, errorReason);
                            }
                            catch(Exception ex)
                            {
                                HTTPManager.Logger.Exception("HubConnection", "Exception in OnError user code!", ex);
                            }
                        }
                    }
                    HTTPManager.Heartbeats.Unsubscribe(this);
                    break;
            }
        }

        void BestHTTP.Extensions.IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
        {
            switch (this.State)
            {
                case ConnectionStates.Connected:
                    if (this.Options.PingInterval != TimeSpan.Zero && DateTime.UtcNow - this.lastMessageSent >= this.Options.PingInterval)
                        SendMessage(new Message() { type = MessageTypes.Ping });
                    break;
            }
        }
    }
}

#endif