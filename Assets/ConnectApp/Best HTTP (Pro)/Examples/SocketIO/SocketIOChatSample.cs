#if !BESTHTTP_DISABLE_SOCKETIO

using System;
using System.Collections.Generic;

using UnityEngine;
using BestHTTP.SocketIO;

namespace BestHTTP.Examples
{
    public sealed class SocketIOChatSample : MonoBehaviour
    {
        private readonly TimeSpan TYPING_TIMER_LENGTH = TimeSpan.FromMilliseconds(700);

        private enum ChatStates
        {
            Login,
            Chat
        }

        #region Fields

        /// <summary>
        /// The Socket.IO manager instance.
        /// </summary>
        private SocketManager Manager;

        /// <summary>
        /// Current state of the chat demo.
        /// </summary>
        private ChatStates State;

        /// <summary>
        /// The selected nickname
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// Currently typing message
        /// </summary>
        private string message = string.Empty;

        /// <summary>
        /// Sent and received messages.
        /// </summary>
        private string chatLog = string.Empty;

        /// <summary>
        /// Position of the scroller
        /// </summary>
        private Vector2 scrollPos;

        /// <summary>
        /// True if the user is currently typing
        /// </summary>
        private bool typing;

        /// <summary>
        /// When the message changed.
        /// </summary>
        private DateTime lastTypingTime = DateTime.MinValue;

        /// <summary>
        /// Users that typing.
        /// </summary>
        private List<string> typingUsers = new List<string>();

        #endregion

        #region Unity Events

        void Start()
        {
            // The current state is Login
            State = ChatStates.Login;

            // Change an option to show how it should be done
            SocketOptions options = new SocketOptions();
            options.AutoConnect = false;
            options.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket;

            // Create the Socket.IO manager
            Manager = new SocketManager(new Uri("https://socket-io-chat.now.sh/socket.io/"), options);

            // Set up custom chat events
            Manager.Socket.On("login", OnLogin);
            Manager.Socket.On("new message", OnNewMessage);
            Manager.Socket.On("user joined", OnUserJoined);
            Manager.Socket.On("user left", OnUserLeft);
            Manager.Socket.On("typing", OnTyping);
            Manager.Socket.On("stop typing", OnStopTyping);

            // The argument will be an Error object.
            Manager.Socket.On(SocketIOEventTypes.Error, (socket, packet, args) => Debug.LogError(string.Format("Error: {0}", args[0].ToString())));
            // We set SocketOptions' AutoConnect to false, so we have to call it manually.
            Manager.Open();
        }

        void OnDestroy()
        {
            // Leaving this sample, close the socket
            Manager.Close();
        }

        void Update()
        {
            // Go back to the demo selector
            if (Input.GetKeyDown(KeyCode.Escape))
                SampleSelector.SelectedSample.DestroyUnityObject();

            // Stop typing if some time passed without typing
            if (typing)
            {
                var typingTimer = DateTime.UtcNow;
                var timeDiff = typingTimer - lastTypingTime;
                if (timeDiff >= TYPING_TIMER_LENGTH)
                {
                    Manager.Socket.Emit("stop typing");
                    typing = false;
                }
            }
        }

        void OnGUI()
        {
            switch (State)
            {
                case ChatStates.Login: DrawLoginScreen(); break;
                case ChatStates.Chat: DrawChatScreen(); break;
            }
        }

        #endregion

        #region Chat Logic

        /// <summary>
        /// Called from an OnGUI event to draw the Login Screen.
        /// </summary>
        void DrawLoginScreen()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
                {
                    GUILayout.BeginVertical();
                    GUILayout.FlexibleSpace();

                    GUIHelper.DrawCenteredText("What's your nickname?");
                    userName = GUILayout.TextField(userName);

                    if (GUILayout.Button("Join"))
                        SetUserName();

                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                });
        }

        /// <summary>
        /// Called from an OnGUI event to draw the Chat Screen.
        /// </summary>
        void DrawChatScreen()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
                {
                    GUILayout.BeginVertical();
                    scrollPos = GUILayout.BeginScrollView(scrollPos);
                    GUILayout.Label(chatLog, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    GUILayout.EndScrollView();

                    string typing = string.Empty;

                    if (typingUsers.Count > 0)
                    {
                        typing += string.Format("{0}", typingUsers[0]);

                        for (int i = 1; i < typingUsers.Count; ++i)
                            typing += string.Format(", {0}", typingUsers[i]);

                        if (typingUsers.Count == 1)
                            typing += " is typing!";
                        else
                            typing += " are typing!";
                    }

                    GUILayout.Label(typing);

                    GUILayout.Label("Type here:");

                    GUILayout.BeginHorizontal();
                    message = GUILayout.TextField(message);

                    if (GUILayout.Button("Send", GUILayout.MaxWidth(100)))
                        SendMessage();
                    GUILayout.EndHorizontal();

                    if (GUI.changed)
                        UpdateTyping();

                    GUILayout.EndVertical();
                });
        }

        void SetUserName()
        {
            if (string.IsNullOrEmpty(userName))
                return;

            State = ChatStates.Chat;

            Manager.Socket.Emit("add user", userName);
        }

        void SendMessage()
        {
            if (string.IsNullOrEmpty(message))
                return;

            Manager.Socket.Emit("new message", message);

            chatLog += string.Format("{0}: {1}\n", userName, message);
            message = string.Empty;
        }

        void UpdateTyping()
        {
            if (!typing)
            {
                typing = true;
                Manager.Socket.Emit("typing");
            }

            lastTypingTime = DateTime.UtcNow;
        }

        void addParticipantsMessage(Dictionary<string, object> data)
        {
            int numUsers = Convert.ToInt32(data["numUsers"]);

            if (numUsers == 1)
                chatLog += "there's 1 participant\n";
            else
                chatLog += "there are " + numUsers + " participants\n";
        }

        void addChatMessage(Dictionary<string, object> data)
        {
            var username = data["username"] as string;
            var msg = data["message"] as string;

            chatLog += string.Format("{0}: {1}\n", username, msg);
        }

        void AddChatTyping(Dictionary<string, object> data)
        {
            var username = data["username"] as string;

            typingUsers.Add(username);
        }

        void RemoveChatTyping(Dictionary<string, object> data)
        {
            var username = data["username"] as string;

            int idx = typingUsers.FindIndex((name) => name.Equals(username));
            if (idx != -1)
                typingUsers.RemoveAt(idx);
        }

        #endregion

        #region Custom SocketIO Events

        void OnLogin(Socket socket, Packet packet, params object[] args)
        {
            chatLog = "Welcome to Socket.IO Chat — \n";

            addParticipantsMessage(args[0] as Dictionary<string, object>);
        }

        void OnNewMessage(Socket socket, Packet packet, params object[] args)
        {
            addChatMessage(args[0] as Dictionary<string, object>);
        }

        void OnUserJoined(Socket socket, Packet packet, params object[] args)
        {
            var data = args[0] as Dictionary<string, object>;

            var username = data["username"] as string;

            chatLog += string.Format("{0} joined\n", username);

            addParticipantsMessage(data);
        }

        void OnUserLeft(Socket socket, Packet packet, params object[] args)
        {
            var data = args[0] as Dictionary<string, object>;

            var username = data["username"] as string;

            chatLog += string.Format("{0} left\n", username);

            addParticipantsMessage(data);
        }

        void OnTyping(Socket socket, Packet packet, params object[] args)
        {
            AddChatTyping(args[0] as Dictionary<string, object>);
        }

        void OnStopTyping(Socket socket, Packet packet, params object[] args)
        {
            RemoveChatTyping(args[0] as Dictionary<string, object>);
        }

        #endregion
    }
}

#endif