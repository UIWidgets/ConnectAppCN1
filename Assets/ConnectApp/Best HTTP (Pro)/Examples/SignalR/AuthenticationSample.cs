#if !BESTHTTP_DISABLE_SIGNALR

using System;
using System.Collections.Generic;

using UnityEngine;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using BestHTTP.SignalR.Authentication;

namespace BestHTTP.Examples
{
    public class AuthenticationSample : MonoBehaviour
    {
        readonly Uri URI = new Uri(GUIHelper.BaseURL + "/signalr");

        #region Private Fields

        /// <summary>
        /// Reference to the SignalR Connection
        /// </summary>
        Connection signalRConnection;

        string userName = string.Empty;
        string role = string.Empty;

        Vector2 scrollPos;

        #endregion

        #region Unity Events

        void Start()
        {
            // Create the SignalR connection, and pass the hubs that we want to connect to
            signalRConnection = new Connection(URI, new BaseHub("noauthhub", "Messages"),
                                                    new BaseHub("invokeauthhub", "Messages Invoked By Admin or Invoker"),
                                                    new BaseHub("authhub", "Messages Requiring Authentication to Send or Receive"),
                                                    new BaseHub("inheritauthhub", "Messages Requiring Authentication to Send or Receive Because of Inheritance"),
                                                    new BaseHub("incomingauthhub", "Messages Requiring Authentication to Send"),
                                                    new BaseHub("adminauthhub", "Messages Requiring Admin Membership to Send or Receive"),
                                                    new BaseHub("userandroleauthhub", "Messages Requiring Name to be \"User\" and Role to be \"Admin\" to Send or Receive"));

            // Set the authenticator if we have valid fields
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(role))
                signalRConnection.AuthenticationProvider = new HeaderAuthenticator(userName, role);

            // Set up event handler
            signalRConnection.OnConnected += signalRConnection_OnConnected;

            // Start to connect to the server.
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
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
                GUILayout.BeginVertical();

                if (signalRConnection.AuthenticationProvider == null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Username (Enter 'User'):");
                    userName = GUILayout.TextField(userName, GUILayout.MinWidth(100));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Roles (Enter 'Invoker' or 'Admin'):");
                    role = GUILayout.TextField(role, GUILayout.MinWidth(100));
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Log in"))
                        Restart();
                }

                for (int i = 0; i < signalRConnection.Hubs.Length; ++i)
                    (signalRConnection.Hubs[i] as BaseHub).Draw();

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            });
        }

        #endregion

        /// <summary>
        /// Called when we successfully connected to the server.
        /// </summary>
        void signalRConnection_OnConnected(Connection manager)
        {
            // call 'InvokedFromClient' on all hubs
            for (int i = 0; i < signalRConnection.Hubs.Length; ++i)
                (signalRConnection.Hubs[i] as BaseHub).InvokedFromClient();
        }

        /// <summary>
        /// Helper function to do a hard-restart to the server.
        /// </summary>
        void Restart()
        {
            // Clean up
            signalRConnection.OnConnected -= signalRConnection_OnConnected;

            // Close current connection
            signalRConnection.Close();
            signalRConnection = null;

            // start again, with authentication if we filled in all input fields
            Start();

        }
    }

    /// <summary>
    /// Hub implementation for the authentication demo. All hubs that we connect to has the same server and client side functions.
    /// </summary>
    class BaseHub : Hub
    {
        #region Private Fields

        /// <summary>
        /// Hub specific title
        /// </summary>
        private string Title;

        private GUIMessageList messages = new GUIMessageList();

        #endregion

        public BaseHub(string name, string title)
            : base(name)
        {
            this.Title = title;

            // Map the server-callable method names to the real functions.
            On("joined", Joined);
            On("rejoined", Rejoined);
            On("left", Left);
            On("invoked", Invoked);
        }

        #region Server Called Functions

        private void Joined(Hub hub, MethodCallMessage methodCall)
        {
            Dictionary<string, object> AuthInfo = methodCall.Arguments[2] as Dictionary<string, object>;
            messages.Add(string.Format("{0} joined at {1}\n\tIsAuthenticated: {2} IsAdmin: {3} UserName: {4}", methodCall.Arguments[0], methodCall.Arguments[1], AuthInfo["IsAuthenticated"], AuthInfo["IsAdmin"], AuthInfo["UserName"]));
        }

        private void Rejoined(Hub hub, MethodCallMessage methodCall)
        {
            messages.Add(string.Format("{0} reconnected at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
        }

        private void Left(Hub hub, MethodCallMessage methodCall)
        {
            messages.Add(string.Format("{0} left at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
        }

        private void Invoked(Hub hub, MethodCallMessage methodCall)
        {
            messages.Add(string.Format("{0} invoked hub method at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
        }

        #endregion

        #region Client callable function implementation

        public void InvokedFromClient()
        {
            base.Call("invokedFromClient", OnInvoked, OnInvokeFailed);
        }

        private void OnInvoked(Hub hub, ClientMessage originalMessage, ResultMessage result)
        {
            Debug.Log(hub.Name + " invokedFromClient success!");
        }

        /// <summary>
        /// This callback function will be called every time we try to access a protected API while we are using an non-authenticated connection.
        /// </summary>
        private void OnInvokeFailed(Hub hub, ClientMessage originalMessage, FailureMessage result)
        {
            Debug.LogWarning(hub.Name + " " + result.ErrorMessage);
        }

        #endregion

        public void Draw()
        {
            GUILayout.Label(this.Title);

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            messages.Draw(Screen.width - 20, 100);
            GUILayout.EndHorizontal();
        }
    }
}

#endif