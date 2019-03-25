#if !BESTHTTP_DISABLE_SIGNALR

using System;

using UnityEngine;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;

namespace BestHTTP.Examples
{
    public sealed class ConnectionStatusSample : MonoBehaviour
    {
        readonly Uri URI = new Uri(GUIHelper.BaseURL + "/signalr");

        /// <summary>
        /// Reference to the SignalR Connection
        /// </summary>
        Connection signalRConnection;

        GUIMessageList messages = new GUIMessageList();

        #region Unity Events

        void Start()
        {
            // Connect to the StatusHub hub
            signalRConnection = new Connection(URI, "StatusHub");

            // General events
            signalRConnection.OnNonHubMessage += signalRConnection_OnNonHubMessage;
            signalRConnection.OnError += signalRConnection_OnError;
            signalRConnection.OnStateChanged += signalRConnection_OnStateChanged;

            // Set up a callback for Hub events
            signalRConnection["StatusHub"].OnMethodCall += statusHub_OnMethodCall;

            // Connect to the server
            signalRConnection.Open();
        }

        void OnDestroy()
        {
            // Close the connection when we are closing the sample
            signalRConnection.Close();
        }

        void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
            {
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("START") && signalRConnection.State != ConnectionStates.Connected)
                    signalRConnection.Open();

                if (GUILayout.Button("STOP") && signalRConnection.State == ConnectionStates.Connected)
                {
                    signalRConnection.Close();
                    messages.Clear();
                }

                if (GUILayout.Button("PING") && signalRConnection.State == ConnectionStates.Connected)
                {
                // Call a Hub-method on the server.
                signalRConnection["StatusHub"].Call("Ping");
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(20);

                GUILayout.Label("Connection Status Messages");

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                messages.Draw(Screen.width - 20, 0);
                GUILayout.EndHorizontal();
            });
        }

        #endregion

        #region SignalR Events

        /// <summary>
        /// Called on server-sent non-hub messages.
        /// </summary>
        void signalRConnection_OnNonHubMessage(Connection manager, object data)
        {
            messages.Add("[Server Message] " + data.ToString());
        }

        /// <summary>
        /// Called when the SignalR Connection's state changes.
        /// </summary>
        void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
        {
            messages.Add(string.Format("[State Change] {0} => {1}", oldState, newState));
        }

        /// <summary>
        /// Called when an error occures. The plugin may close the connection after this event.
        /// </summary>
        void signalRConnection_OnError(Connection manager, string error)
        {
            messages.Add("[Error] " + error);
        }

        /// <summary>
        /// Called when the "StatusHub" hub wants to call a method on this client.
        /// </summary>
        void statusHub_OnMethodCall(Hub hub, string method, params object[] args)
        {
            string id = args.Length > 0 ? args[0] as string : string.Empty;
            string when = args.Length > 1 ? args[1].ToString() : string.Empty;

            switch (method)
            {
                case "joined":
                    messages.Add(string.Format("[{0}] {1} joined at {2}", hub.Name, id, when));
                    break;

                case "rejoined":
                    messages.Add(string.Format("[{0}] {1} reconnected at {2}", hub.Name, id, when));
                    break;

                case "leave":
                    messages.Add(string.Format("[{0}] {1} leaved at {2}", hub.Name, id, when));
                    break;

                default: // pong
                    messages.Add(string.Format("[{0}] {1}", hub.Name, method));
                    break;
            }
        }

        #endregion
    }
}

#endif