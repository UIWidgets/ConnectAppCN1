#if !BESTHTTP_DISABLE_SIGNALR

using System;

using UnityEngine;
using BestHTTP.SignalR;

#if !BESTHTTP_DISABLE_COOKIES && (!UNITY_WEBGL || UNITY_EDITOR)
    using BestHTTP.Cookies;
#endif

namespace BestHTTP.Examples
{
    public sealed class ConnectionAPISample : MonoBehaviour
    {
        readonly Uri URI = new Uri(GUIHelper.BaseURL + "/raw-connection/");

        /// <summary>
        /// Possible message types that the client can send to the server
        /// </summary>
        enum MessageTypes
        {
            Send,               // 0
            Broadcast,          // 1
            Join,               // 2
            PrivateMessage,     // 3
            AddToGroup,         // 4
            RemoveFromGroup,    // 5
            SendToGroup,        // 6
            BroadcastExceptMe,  // 7
        }

        #region Private Fields

        /// <summary>
        /// Reference to the SignalR Connection
        /// </summary>
        Connection signalRConnection;

        // Input strings
        string ToEveryBodyText = string.Empty;
        string ToMeText = string.Empty;
        string PrivateMessageText = string.Empty;
        string PrivateMessageUserOrGroupName = string.Empty;

        GUIMessageList messages = new GUIMessageList();

        #endregion

        #region Unity Events

        void Start()
        {
#if !BESTHTTP_DISABLE_COOKIES && (!UNITY_WEBGL || UNITY_EDITOR)
            // Set a "user" cookie if we previously used the 'Enter Name' button.
            // The server will set this username to the new connection.
            if (PlayerPrefs.HasKey("userName"))
                CookieJar.Set(URI, new Cookie("user", PlayerPrefs.GetString("userName")));
#endif

            signalRConnection = new Connection(URI);

            // to serialize the Message class, set a more advanced json encoder
            signalRConnection.JsonEncoder = new BestHTTP.SignalR.JsonEncoders.LitJsonEncoder();

            // set up event handlers
            signalRConnection.OnStateChanged += signalRConnection_OnStateChanged;
            signalRConnection.OnNonHubMessage += signalRConnection_OnGeneralMessage;

            // Start to connect to the server.
            signalRConnection.Open();
        }

        /// <summary>
        /// Draw the gui.
        /// Get input strings.
        /// Handle function calls.
        /// </summary>
        void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
            {
                GUILayout.BeginVertical();

            #region To Everybody
            GUILayout.Label("To Everybody");

                GUILayout.BeginHorizontal();

                ToEveryBodyText = GUILayout.TextField(ToEveryBodyText, GUILayout.MinWidth(100));

                if (GUILayout.Button("Broadcast"))
                    Broadcast(ToEveryBodyText);

                if (GUILayout.Button("Broadcast (All Except Me)"))
                    BroadcastExceptMe(ToEveryBodyText);

                if (GUILayout.Button("Enter Name"))
                    EnterName(ToEveryBodyText);

                if (GUILayout.Button("Join Group"))
                    JoinGroup(ToEveryBodyText);

                if (GUILayout.Button("Leave Group"))
                    LeaveGroup(ToEveryBodyText);

                GUILayout.EndHorizontal();
            #endregion

            #region To Me
            GUILayout.Label("To Me");

                GUILayout.BeginHorizontal();

                ToMeText = GUILayout.TextField(ToMeText, GUILayout.MinWidth(100));

                if (GUILayout.Button("Send to me"))
                    SendToMe(ToMeText);

                GUILayout.EndHorizontal();
            #endregion

            #region Private Message
            GUILayout.Label("Private Message");

                GUILayout.BeginHorizontal();

                GUILayout.Label("Message:");
                PrivateMessageText = GUILayout.TextField(PrivateMessageText, GUILayout.MinWidth(100));

                GUILayout.Label("User or Group name:");
                PrivateMessageUserOrGroupName = GUILayout.TextField(PrivateMessageUserOrGroupName, GUILayout.MinWidth(100));

                if (GUILayout.Button("Send to user"))
                    SendToUser(PrivateMessageUserOrGroupName, PrivateMessageText);

                if (GUILayout.Button("Send to group"))
                    SendToGroup(PrivateMessageUserOrGroupName, PrivateMessageText);

                GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(20);

                if (signalRConnection.State == ConnectionStates.Closed)
                {
                    if (GUILayout.Button("Start Connection"))
                        signalRConnection.Open();
                }
                else if (GUILayout.Button("Stop Connection"))
                    signalRConnection.Close();

                GUILayout.Space(20);

            // Draw the messages
            GUILayout.Label("Messages");

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                messages.Draw(Screen.width - 20, 0);
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            });
        }

        void OnDestroy()
        {
            // Close the connection when the sample is closed
            signalRConnection.Close();
        }

        #endregion

        #region SignalR Events

        /// <summary>
        /// Handle non-hub messages
        /// </summary>
        void signalRConnection_OnGeneralMessage(Connection manager, object data)
        {
            // For now, just create a Json string from the sent data again
            string reencoded = BestHTTP.JSON.Json.Encode(data);

            // and display it
            messages.Add("[Server Message] " + reencoded);
        }

        void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
        {
            // display state changes
            messages.Add(string.Format("[State Change] {0} => {1}", oldState.ToString(), newState.ToString()));
        }

        #endregion

        #region To EveryBody Functions

        /// <summary>
        /// Broadcast a message to all connected clients
        /// </summary>
        private void Broadcast(string text)
        {
            signalRConnection.Send(new { Type = MessageTypes.Broadcast, Value = text });
        }

        /// <summary>
        /// Broadcast a message to all connected clients, except this client
        /// </summary>
        private void BroadcastExceptMe(string text)
        {
            signalRConnection.Send(new { Type = MessageTypes.BroadcastExceptMe, Value = text });
        }

        /// <summary>
        /// Set a name for this connection.
        /// </summary>
        private void EnterName(string name)
        {
            signalRConnection.Send(new { Type = MessageTypes.Join, Value = name });
        }

        /// <summary>
        /// Join to a group
        /// </summary>
        private void JoinGroup(string groupName)
        {
            signalRConnection.Send(new { Type = MessageTypes.AddToGroup, Value = groupName });
        }

        /// <summary>
        /// Leave a group
        /// </summary>
        private void LeaveGroup(string groupName)
        {
            signalRConnection.Send(new { Type = MessageTypes.RemoveFromGroup, Value = groupName });
        }

        #endregion

        #region To Me Functions

        /// <summary>
        /// Send a message to the very same client through the server
        /// </summary>
        void SendToMe(string text)
        {
            signalRConnection.Send(new { Type = MessageTypes.Send, Value = text });
        }

        #endregion

        #region Private Message Functions

        /// <summary>
        /// Send a private message to a user
        /// </summary>
        void SendToUser(string userOrGroupName, string text)
        {
            signalRConnection.Send(new { Type = MessageTypes.PrivateMessage, Value = string.Format("{0}|{1}", userOrGroupName, text) });
        }

        /// <summary>
        /// Send a message to a group
        /// </summary>
        void SendToGroup(string userOrGroupName, string text)
        {
            signalRConnection.Send(new { Type = MessageTypes.SendToGroup, Value = string.Format("{0}|{1}", userOrGroupName, text) });
        }

        #endregion
    }
}

#endif