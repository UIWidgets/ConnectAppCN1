#if !BESTHTTP_DISABLE_SIGNALR

using System;

using UnityEngine;
using BestHTTP.SignalR;

namespace BestHTTP.Examples
{
    public sealed class SimpleStreamingSample : MonoBehaviour
    {
        readonly Uri URI = new Uri(GUIHelper.BaseURL + "/streaming-connection");

        /// <summary>
        /// Reference to the SignalR Connection
        /// </summary>
        Connection signalRConnection;

        /// <summary>
        /// Helper GUI class to handle and display a string-list
        /// </summary>
        GUIMessageList messages = new GUIMessageList();

        #region Unity Events

        void Start()
        {
            // Create the SignalR connection
            signalRConnection = new Connection(URI);

            // set event handlers
            signalRConnection.OnNonHubMessage += signalRConnection_OnNonHubMessage;
            signalRConnection.OnStateChanged += signalRConnection_OnStateChanged;
            signalRConnection.OnError += signalRConnection_OnError;

            // Start connecting to the server
            signalRConnection.Open();
        }

        void OnDestroy()
        {
            // Close the connection when the sample is closed
            signalRConnection.Close();
        }

        void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
            {
                GUILayout.Label("Messages");

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                messages.Draw(Screen.width - 20, 0);
                GUILayout.EndHorizontal();
            });
        }

        #endregion

        #region SignalR Events

        /// <summary>
        /// Handle Server-sent messages
        /// </summary>
        void signalRConnection_OnNonHubMessage(Connection connection, object data)
        {
            messages.Add("[Server Message] " + data.ToString());
        }

        /// <summary>
        /// Display state changes
        /// </summary>
        void signalRConnection_OnStateChanged(Connection connection, ConnectionStates oldState, ConnectionStates newState)
        {
            messages.Add(string.Format("[State Change] {0} => {1}", oldState, newState));
        }

        /// <summary>
        /// Display errors.
        /// </summary>
        void signalRConnection_OnError(Connection connection, string error)
        {
            messages.Add("[Error] " + error);
        }

        #endregion
    }
}

#endif