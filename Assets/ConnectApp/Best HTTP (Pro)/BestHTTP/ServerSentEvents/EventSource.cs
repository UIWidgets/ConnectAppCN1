#if !BESTHTTP_DISABLE_SERVERSENT_EVENTS

using System;
using System.Collections.Generic;

using BestHTTP.Extensions;

#if UNITY_WEBGL && !UNITY_EDITOR
    using System.Runtime.InteropServices;
#endif

namespace BestHTTP.ServerSentEvents
{
    /// <summary>
    /// Possible states of an EventSource object.
    /// </summary>
    public enum States
    {
        Initial,
        Connecting,
        Open,
        Retrying,
        Closing,
        Closed
    }

    public delegate void OnGeneralEventDelegate(EventSource eventSource);
    public delegate void OnMessageDelegate(EventSource eventSource, BestHTTP.ServerSentEvents.Message message);
    public delegate void OnErrorDelegate(EventSource eventSource, string error);
    public delegate bool OnRetryDelegate(EventSource eventSource);
    public delegate void OnEventDelegate(EventSource eventSource, BestHTTP.ServerSentEvents.Message message);
    public delegate void OnStateChangedDelegate(EventSource eventSource, States oldState, States newState);

#if UNITY_WEBGL && !UNITY_EDITOR

    delegate void OnWebGLEventSourceOpenDelegate(uint id);
    delegate void OnWebGLEventSourceMessageDelegate(uint id, string eventStr, string data, string eventId, int retry);
    delegate void OnWebGLEventSourceErrorDelegate(uint id, string reason);
#endif

    /// <summary>
    /// http://www.w3.org/TR/eventsource/
    /// </summary>
    public class EventSource
#if !UNITY_WEBGL || UNITY_EDITOR
        : IHeartbeat
#endif
    {
#region Public Properties

        /// <summary>
        /// Uri of the remote endpoint.
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Current state of the EventSource object.
        /// </summary>
        public States State
        {
            get
            {
                return _state;
            }
            private set
            {
                States oldState = _state;
                _state = value;

                if (OnStateChanged != null)
                {
                    try
                    {
                        OnStateChanged(this, oldState, _state);
                    }
                    catch(Exception ex)
                    {
                        HTTPManager.Logger.Exception("EventSource", "OnStateChanged", ex);
                    }
                }
            }
        }
        private States _state;

        /// <summary>
        /// Time to wait to do a reconnect attempt. Default to 2 sec. The server can overwrite this setting.
        /// </summary>
        public TimeSpan ReconnectionTime { get; set; }

        /// <summary>
        /// The last successfully received event's id.
        /// </summary>
        public string LastEventId { get; private set; }

#if !UNITY_WEBGL || UNITY_EDITOR
        /// <summary>
        /// The internal request object of the EventSource.
        /// </summary>
        public HTTPRequest InternalRequest { get; private set; }
#else
        public bool WithCredentials { get; set; }
#endif

#endregion

#region Public Events

        /// <summary>
        /// Called when successfully connected to the server.
        /// </summary>
        public event OnGeneralEventDelegate OnOpen;

        /// <summary>
        /// Called on every message received from the server.
        /// </summary>
        public event OnMessageDelegate OnMessage;

        /// <summary>
        /// Called when an error occurs.
        /// </summary>
        public event OnErrorDelegate OnError;

#if !UNITY_WEBGL || UNITY_EDITOR
        /// <summary>
        /// Called when the EventSource will try to do a retry attempt. If this function returns with false, it will cancel the attempt.
        /// </summary>
        public event OnRetryDelegate OnRetry;
#endif

        /// <summary>
        /// Called when the EventSource object closed.
        /// </summary>
        public event OnGeneralEventDelegate OnClosed;

        /// <summary>
        /// Called every time when the State property changed.
        /// </summary>
        public event OnStateChangedDelegate OnStateChanged;

#endregion

#region Privates

        /// <summary>
        /// A dictionary to store eventName => delegate mapping.
        /// </summary>
        private Dictionary<string, OnEventDelegate> EventTable;

#if !UNITY_WEBGL || UNITY_EDITOR
        /// <summary>
        /// Number of retry attempts made.
        /// </summary>
        private byte RetryCount;

        /// <summary>
        /// When we called the Retry function. We will delay the Open call from here.
        /// </summary>
        private DateTime RetryCalled;
#else
        private static Dictionary<uint, EventSource> EventSources = new Dictionary<uint, EventSource>();
        private uint Id;
#endif

#endregion

        public EventSource(Uri uri)
        {
            this.Uri = uri;
            this.ReconnectionTime = TimeSpan.FromMilliseconds(2000);

#if !UNITY_WEBGL || UNITY_EDITOR
            this.InternalRequest = new HTTPRequest(Uri, HTTPMethods.Get, true, true, OnRequestFinished);

            // Set headers
            this.InternalRequest.SetHeader("Accept", "text/event-stream");
            this.InternalRequest.SetHeader("Cache-Control", "no-cache");
            this.InternalRequest.SetHeader("Accept-Encoding", "identity");

            // Set protocol stuff
            this.InternalRequest.ProtocolHandler = SupportedProtocols.ServerSentEvents;
            this.InternalRequest.OnUpgraded = OnUpgraded;

            // Disable internal retry
            this.InternalRequest.DisableRetry = true;
#else
            if (!ES_IsSupported())
              throw new NotSupportedException("This browser isn't support the EventSource protocol!");

            this.Id = ES_Create(this.Uri.ToString(), WithCredentials, OnOpenCallback, OnMessageCallback, OnErrorCallback);

            EventSources.Add(this.Id, this);
#endif
        }

#region Public Functions

        /// <summary>
        /// Start to connect to the remote server.
        /// </summary>
        public void Open()
        {
            if (this.State != States.Initial &&
                this.State != States.Retrying &&
                this.State != States.Closed)
                return;

            this.State = States.Connecting;

#if !UNITY_WEBGL || UNITY_EDITOR
            if (!string.IsNullOrEmpty(this.LastEventId))
                this.InternalRequest.SetHeader("Last-Event-ID", this.LastEventId);

            this.InternalRequest.Send();
#endif
        }

        /// <summary>
        /// Start to close the connection.
        /// </summary>
        public void Close()
        {
            if (this.State == States.Closing ||
                this.State == States.Closed)
                return;

            this.State = States.Closing;
#if !UNITY_WEBGL || UNITY_EDITOR
            if (this.InternalRequest != null)
                this.InternalRequest.Abort();
            else
                this.State = States.Closed;
#else
            ES_Close(this.Id);

            SetClosed("Close");

            EventSources.Remove(this.Id);

            ES_Release(this.Id);
#endif
        }

        /// <summary>
        /// With this function an event handler can be subscribed for an event name.
        /// </summary>
        public void On(string eventName, OnEventDelegate action)
        {
            if (EventTable == null)
                EventTable = new Dictionary<string, OnEventDelegate>();

            EventTable[eventName] = action;
#if UNITY_WEBGL && !UNITY_EDITOR
            ES_AddEventHandler(this.Id, eventName);
#endif
        }

        /// <summary>
        /// With this function the event handler can be removed for the given event name.
        /// </summary>
        /// <param name="eventName"></param>
        public void Off(string eventName)
        {
            if (eventName == null || EventTable == null)
                return;

            EventTable.Remove(eventName);
        }

#endregion

#region Private Helper Functions

        private void CallOnError(string error, string msg)
        {
            if (OnError != null)
            {
                try
                {
                    OnError(this, error);
                }
                catch (Exception ex)
                {
                    HTTPManager.Logger.Exception("EventSource", msg + " - OnError", ex);
                }
            }
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        private bool CallOnRetry()
        {
            if (OnRetry != null)
            {
                try
                {
                    return OnRetry(this);
                }
                catch(Exception ex)
                {
                    HTTPManager.Logger.Exception("EventSource", "CallOnRetry", ex);
                }
            }

            return true;
        }
#endif

        private void SetClosed(string msg)
        {
            this.State = States.Closed;

            if (OnClosed != null)
            {
                try
                {
                    OnClosed(this);
                }
                catch (Exception ex)
                {
                    HTTPManager.Logger.Exception("EventSource", msg + " - OnClosed", ex);
                }
            }
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        private void Retry()
        {
            if (RetryCount > 0 ||
                !CallOnRetry())
            {
                SetClosed("Retry");
                return;
            }

            RetryCount++;
            RetryCalled = DateTime.UtcNow;

            HTTPManager.Heartbeats.Subscribe(this);

            this.State = States.Retrying;
        }
#endif

#endregion

#region HTTP Request Implementation
#if !UNITY_WEBGL || UNITY_EDITOR

        /// <summary>
        /// We are successfully upgraded to the EventSource protocol, we can start to receive and parse the incoming data.
        /// </summary>
        private void OnUpgraded(HTTPRequest originalRequest, HTTPResponse response)
        {
            EventSourceResponse esResponse = response as EventSourceResponse;

            if (esResponse == null)
            {
                CallOnError("Not an EventSourceResponse!", "OnUpgraded");
                return;
            }

            if (OnOpen != null)
            {
                try
                {
                    OnOpen(this);
                }
                catch (Exception ex)
                {
                    HTTPManager.Logger.Exception("EventSource", "OnOpen", ex);
                }
            }

            esResponse.OnMessage += OnMessageReceived;
            esResponse.StartReceive();

            this.RetryCount = 0;
            this.State = States.Open;
        }

        private void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            if (this.State == States.Closed)
                return;

            if (this.State == States.Closing ||
                req.State == HTTPRequestStates.Aborted)
            {
                SetClosed("OnRequestFinished");

                return;
            }

            string reason = string.Empty;

            // In some cases retry is prohibited
            bool canRetry = true;

            switch (req.State)
            {
                // The server sent all the data it's wanted.
                case HTTPRequestStates.Processing:
                    canRetry = !resp.HasHeader("content-length");
                    break;

                // The request finished without any problem.
                case HTTPRequestStates.Finished:
                    // HTTP 200 OK responses that have a Content-Type specifying an unsupported type, or that have no Content-Type at all, must cause the user agent to fail the connection.
                    if (resp.StatusCode == 200 && !resp.HasHeaderWithValue("content-type", "text/event-stream"))
                    {
                        reason = "No Content-Type header with value 'text/event-stream' present.";
                        canRetry = false;
                    }

                    // HTTP 500 Internal Server Error, 502 Bad Gateway, 503 Service Unavailable, and 504 Gateway Timeout responses, and any network error that prevents the connection
                    //  from being established in the first place (e.g. DNS errors), must cause the user agent to asynchronously reestablish the connection.
                    // Any other HTTP response code not listed here must cause the user agent to fail the connection.
                    if (canRetry &&
                        resp.StatusCode != 500 &&
                        resp.StatusCode != 502 &&
                        resp.StatusCode != 503 &&
                        resp.StatusCode != 504)
                    {
                        canRetry = false;

                        reason = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                        resp.StatusCode,
                                                        resp.Message,
                                                        resp.DataAsText);
                    }
                    break;

                // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                case HTTPRequestStates.Error:
                    reason = "Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
                    break;

                // The request aborted, initiated by the user.
                case HTTPRequestStates.Aborted:
                    // If the state is Closing, then it's a normal behaviour, and we close the EventSource
                    reason = "OnRequestFinished - Aborted without request. EventSource's State: " + this.State;
                    break;

                // Connecting to the server is timed out.
                case HTTPRequestStates.ConnectionTimedOut:
                    reason = "Connection Timed Out!";
                    break;

                // The request didn't finished in the given time.
                case HTTPRequestStates.TimedOut:
                    reason = "Processing the request Timed Out!";
                    break;
            }

            // If we are not closing the EventSource, then we will try to reconnect.
            if (this.State < States.Closing)
            {
                if (!string.IsNullOrEmpty(reason))
                    CallOnError(reason, "OnRequestFinished");

                if (canRetry)
                    Retry();
                else
                    SetClosed("OnRequestFinished");
            }
            else
                SetClosed("OnRequestFinished");
        }
#endif
#endregion

#region EventStreamResponse Event Handlers

        private void OnMessageReceived(
#if !UNITY_WEBGL || UNITY_EDITOR
            EventSourceResponse resp,
#endif
            BestHTTP.ServerSentEvents.Message message)
        {
            if (this.State >= States.Closing)
                return;

            // 1.) Set the last event ID string of the event source to value of the last event ID buffer.
            // The buffer does not get reset, so the last event ID string of the event source remains set to this value until the next time it is set by the server.
            // We check here only for null, because it can be a non-null but empty string.
            if (message.Id != null)
                this.LastEventId = message.Id;

            if (message.Retry.TotalMilliseconds > 0)
                this.ReconnectionTime = message.Retry;

            // 2.) If the data buffer is an empty string, set the data buffer and the event type buffer to the empty string and abort these steps.
            if (string.IsNullOrEmpty(message.Data))
                return;

            // 3.) If the data buffer's last character is a U+000A LINE FEED (LF) character, then remove the last character from the data buffer.
            // This step can be ignored. We constructed the string to be able to skip this step.

            if (OnMessage != null)
            {
                try
                {
                    OnMessage(this, message);
                }
                catch (Exception ex)
                {
                    HTTPManager.Logger.Exception("EventSource", "OnMessageReceived - OnMessage", ex);
                }
            }

            if (EventTable != null && !string.IsNullOrEmpty(message.Event))
            {
                OnEventDelegate action;
                if (EventTable.TryGetValue(message.Event, out action))
                {
                    if (action != null)
                    {
                        try
                        {
                            action(this, message);
                        }
                        catch(Exception ex)
                        {
                            HTTPManager.Logger.Exception("EventSource", "OnMessageReceived - action", ex);
                        }
                    }
                }
            }
        }

#endregion

#region IHeartbeat Implementation
#if !UNITY_WEBGL || UNITY_EDITOR

        void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
        {
            if (this.State != States.Retrying)
            {
                HTTPManager.Heartbeats.Unsubscribe(this);

                return;
            }

            if (DateTime.UtcNow - RetryCalled >= ReconnectionTime)
            {
                Open();

                if (this.State != States.Connecting)
                    SetClosed("OnHeartbeatUpdate");

                HTTPManager.Heartbeats.Unsubscribe(this);
            }
        }
#endif
#endregion

#region WebGL Static Callbacks
#if UNITY_WEBGL && !UNITY_EDITOR

        [AOT.MonoPInvokeCallback(typeof(OnWebGLEventSourceOpenDelegate))]
        static void OnOpenCallback(uint id)
        {
            EventSource es;
            if (EventSources.TryGetValue(id, out es))
            {
                if (es.OnOpen != null)
                {
                    try
                    {
                        es.OnOpen(es);
                    }
                    catch(Exception ex)
                    {
                        HTTPManager.Logger.Exception("EventSource", "OnOpen", ex);
                    }
                }

                es.State = States.Open;
            }
            else
                HTTPManager.Logger.Warning("EventSource", "OnOpenCallback - No EventSource found for id: " + id.ToString());
        }

        [AOT.MonoPInvokeCallback(typeof(OnWebGLEventSourceMessageDelegate))]
        static void OnMessageCallback(uint id, string eventStr, string data, string eventId, int retry)
        {
            EventSource es;
            if (EventSources.TryGetValue(id, out es))
            {
                var msg = new BestHTTP.ServerSentEvents.Message();
                msg.Id = eventId;
                msg.Data = data;
                msg.Event = eventStr;
                msg.Retry = TimeSpan.FromSeconds(retry);

                es.OnMessageReceived(msg);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnWebGLEventSourceErrorDelegate))]
        static void OnErrorCallback(uint id, string reason)
        {
            EventSource es;
            if (EventSources.TryGetValue(id, out es))
            {
                es.CallOnError(reason, "OnErrorCallback");
                es.SetClosed("OnError");

                EventSources.Remove(id);
            }

            try
            {
                ES_Release(id);
            }
            catch (Exception ex)
            {
                HTTPManager.Logger.Exception("EventSource", "ES_Release", ex);
            }
        }

#endif
#endregion

#region WebGL Interface
#if UNITY_WEBGL && !UNITY_EDITOR

        [DllImport("__Internal")]
        static extern bool ES_IsSupported();

        [DllImport("__Internal")]
        static extern uint ES_Create(string url, bool withCred, OnWebGLEventSourceOpenDelegate onOpen, OnWebGLEventSourceMessageDelegate onMessage, OnWebGLEventSourceErrorDelegate onError);

        [DllImport("__Internal")]
        static extern void ES_AddEventHandler(uint id, string eventName);

        [DllImport("__Internal")]
        static extern void ES_Close(uint id);

        [DllImport("__Internal")]
        static extern void ES_Release(uint id);

#endif
#endregion

    }
}

#endif