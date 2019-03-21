#if !BESTHTTP_DISABLE_SOCKETIO

using System;
using System.Collections.Generic;

using UnityEngine;
using BestHTTP.SocketIO;

namespace BestHTTP.Examples
{
    public sealed class SocketIOWePlaySample : MonoBehaviour
    {
        /// <summary>
        /// Possible states of the game.
        /// </summary>
        enum States
        {
            Connecting,
            WaitForNick,
            Joined
        }

        /// <summary>
        /// Controls that the server understands as a parameter in the move event.
        /// </summary>
        private string[] controls = new string[] { "left", "right", "a", "b", "up", "down", "select", "start" };

        /// <summary>
        /// Ratio of the drawn GUI texture from the screen
        /// </summary>
        private const float ratio = 1.5f;

        /// <summary>
        /// How many messages to keep.
        /// </summary>
        private int MaxMessages = 50;

        /// <summary>
        /// Current state of the game.
        /// </summary>
        private States State;

        /// <summary>
        /// The root("/") Socket instance.
        /// </summary>
        private Socket Socket;

        /// <summary>
        /// The user-selected nickname.
        /// </summary>
        private string Nick = string.Empty;

        /// <summary>
        /// The message that the user want to send to the chat.
        /// </summary>
        private string messageToSend = string.Empty;

        /// <summary>
        /// How many user connected to the server.
        /// </summary>
        private int connections;

        /// <summary>
        /// Local and server sent messages.
        /// </summary>
        private List<string> messages = new List<string>();

        /// <summary>
        /// The chat scroll position.
        /// </summary>
        private Vector2 scrollPos;

        /// <summary>
        /// The decoded texture from the server sent binary data
        /// </summary>
        private Texture2D FrameTexture;

        #region Unity Events

        void Start()
        {
            // Change an option to show how it should be done
            SocketOptions options = new SocketOptions();
            options.AutoConnect = false;

            // Create the SocketManager instance
            var manager = new SocketManager(new Uri("http://io.weplay.io/socket.io/"), options);

            // Keep a reference to the root namespace
            Socket = manager.Socket;

            // Set up our event handlers.
            Socket.On(SocketIOEventTypes.Connect, OnConnected);
            Socket.On("joined", OnJoined);
            Socket.On("connections", OnConnections);
            Socket.On("join", OnJoin);
            Socket.On("move", OnMove);
            Socket.On("message", OnMessage);
            Socket.On("reload", OnReload);

            // Don't waste cpu cycles on decoding the payload, we are expecting only binary data with this event,
            //  and we can access it through the packet's Attachments property.
            Socket.On("frame", OnFrame, /*autoDecodePayload:*/ false);

            // Add error handler, so we can display it
            Socket.On(SocketIOEventTypes.Error, OnError);

            // We set SocketOptions' AutoConnect to false, so we have to call it manually.
            manager.Open();

            // We are connecting to the server.
            State = States.Connecting;
        }

        void OnDestroy()
        {
            // Leaving this sample, close the socket
            Socket.Manager.Close();
        }

        void Update()
        {
            // Go back to the demo selector
            if (Input.GetKeyDown(KeyCode.Escape))
                SampleSelector.SelectedSample.DestroyUnityObject();
        }

        void OnGUI()
        {
            switch (State)
            {
                case States.Connecting:
                    GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
                        {
                            GUILayout.BeginVertical();
                            GUILayout.FlexibleSpace();
                            GUIHelper.DrawCenteredText("Connecting to the server...");
                            GUILayout.FlexibleSpace();
                            GUILayout.EndVertical();
                        });
                    break;

                case States.WaitForNick:
                    GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
                        {
                            DrawLoginScreen();
                        });
                    break;

                case States.Joined:
                    GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
                        {
                        // Draw Texture
                        if (FrameTexture != null)
                                GUILayout.Box(FrameTexture);

                            DrawControls();
                            DrawChat();
                        });
                    break;
            }
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Called from an OnGUI event to draw the Login Screen.
        /// </summary>
        void DrawLoginScreen()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUIHelper.DrawCenteredText("What's your nickname?");

            Nick = GUILayout.TextField(Nick);

            if (GUILayout.Button("Join"))
                Join();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        void DrawControls()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Controls:");

            for (int i = 0; i < controls.Length; ++i)
                if (GUILayout.Button(controls[i]))
                    Socket.Emit("move", controls[i]);

            GUILayout.Label(" Connections: " + connections);

            GUILayout.EndHorizontal();
        }

        void DrawChat(bool withInput = true)
        {
            GUILayout.BeginVertical();

            // Draw the messages
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
            for (int i = 0; i < messages.Count; ++i)
                GUILayout.Label(messages[i], GUILayout.MinWidth(Screen.width));
            GUILayout.EndScrollView();

            if (withInput)
            {
                GUILayout.Label("Your message: ");

                GUILayout.BeginHorizontal();

                messageToSend = GUILayout.TextField(messageToSend);

                if (GUILayout.Button("Send", GUILayout.MaxWidth(100)))
                    SendMessage();

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Add a message to the message log
        /// </summary>
        /// <param name="msg"></param>
        void AddMessage(string msg)
        {
            messages.Insert(0, msg);
            if (messages.Count > MaxMessages)
                messages.RemoveRange(MaxMessages, messages.Count - MaxMessages);
        }

        /// <summary>
        /// Send a chat message. The message must be in the messageToSend field.
        /// </summary>
        void SendMessage()
        {
            if (string.IsNullOrEmpty(messageToSend))
                return;

            Socket.Emit("message", messageToSend);
            AddMessage(string.Format("{0}: {1}", Nick, messageToSend));
            messageToSend = string.Empty;
        }

        /// <summary>
        /// Join to the game with the nickname stored in the Nick field.
        /// </summary>
        void Join()
        {
            PlayerPrefs.SetString("Nick", Nick);

            Socket.Emit("join", Nick);
        }

        /// <summary>
        /// Reload the game.
        /// </summary>
        void Reload()
        {
            FrameTexture = null;

            if (Socket != null)
            {
                Socket.Manager.Close();
                Socket = null;

                Start();
            }
        }

        #endregion

        #region SocketIO Events

        /// <summary>
        /// Socket connected event.
        /// </summary>
        private void OnConnected(Socket socket, Packet packet, params object[] args)
        {
            if (PlayerPrefs.HasKey("Nick"))
            {
                Nick = PlayerPrefs.GetString("Nick", "NickName");
                Join();
            }
            else
                State = States.WaitForNick;

            AddMessage("connected");
        }

        /// <summary>
        /// Local player joined after sending a 'join' event
        /// </summary>
        private void OnJoined(Socket socket, Packet packet, params object[] args)
        {
            State = States.Joined;
        }

        /// <summary>
        /// Server sent us a 'reload' event.
        /// </summary>
        private void OnReload(Socket socket, Packet packet, params object[] args)
        {
            Reload();
        }

        /// <summary>
        /// Someone wrote a message to the chat.
        /// </summary>
        private void OnMessage(Socket socket, Packet packet, params object[] args)
        {
            if (args.Length == 1)
                AddMessage(args[0] as string);
            else
                AddMessage(string.Format("{0}: {1}", args[1], args[0]));
        }

        /// <summary>
        /// Someone (including us) pressed a button.
        /// </summary>
        private void OnMove(Socket socket, Packet packet, params object[] args)
        {
            AddMessage(string.Format("{0} pressed {1}", args[1], args[0]));
        }

        /// <summary>
        /// Someone joined to the game
        /// </summary>
        private void OnJoin(Socket socket, Packet packet, params object[] args)
        {
            string loc = args.Length > 1 ? string.Format("({0})", args[1]) : string.Empty;

            AddMessage(string.Format("{0} joined {1}", args[0], loc));
        }

        /// <summary>
        /// How many players are connected to the game.
        /// </summary>
        private void OnConnections(Socket socket, Packet packet, params object[] args)
        {
            connections = Convert.ToInt32(args[0]);
        }

        /// <summary>
        /// The server sent us a new picture to draw the game.
        /// </summary>
        private void OnFrame(Socket socket, Packet packet, params object[] args)
        {
            if (State != States.Joined)
                return;

            if (FrameTexture == null)
            {
                FrameTexture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
                FrameTexture.filterMode = FilterMode.Point;
            }

            // Binary data usage case 1 - using directly the Attachments property:
            byte[] data = packet.Attachments[0];

            // Binary data usage case 2 - using the packet's ReconstructAttachmentAsIndex() function
            /*packet.ReconstructAttachmentAsIndex();
            args = packet.Decode(socket.Manager.Encoder);
            data = packet.Attachments[Convert.ToInt32(args[0])];*/

            // Binary data usage case 3 - using the packet's ReconstructAttachmentAsBase64() function
            /*packet.ReconstructAttachmentAsBase64();
            args = packet.Decode(socket.Manager.Encoder);
            data = Convert.FromBase64String(args[0] as string);*/

            // Load the server sent picture
            FrameTexture.LoadImage(data);
        }

        /// <summary>
        /// Called on local or remote error.
        /// </summary>
        private void OnError(Socket socket, Packet packet, params object[] args)
        {
            AddMessage(string.Format("--ERROR - {0}", args[0].ToString()));
        }

        #endregion
    }
}

#endif