#if !BESTHTTP_DISABLE_WEBSOCKET

using System;

using UnityEngine;

namespace BestHTTP.Examples
{
    public class WebSocketSample : MonoBehaviour
    {
        #region Private Fields

        /// <summary>
        /// The WebSocket address to connect
        /// </summary>
        string address = "wss://echo.websocket.org";

        /// <summary>
        /// Default text to send
        /// </summary>
        string msgToSend = "Hello World!";

        /// <summary>
        /// Debug text to draw on the gui
        /// </summary>
        string Text = string.Empty;

        /// <summary>
        /// Saved WebSocket instance
        /// </summary>
        WebSocket.WebSocket webSocket;

        /// <summary>
        /// GUI scroll position
        /// </summary>
        Vector2 scrollPos;

        #endregion

        #region Unity Events

        void OnDestroy()
        {
            if (webSocket != null)
            {
                webSocket.Close();
            }
        }

        void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
                {
                    scrollPos = GUILayout.BeginScrollView(scrollPos);
                    GUILayout.Label(Text);
                    GUILayout.EndScrollView();

                    GUILayout.Space(5);

                    GUILayout.FlexibleSpace();

                    address = GUILayout.TextField(address);

                    if (webSocket == null && GUILayout.Button("Open Web Socket"))
                    {
                        // Create the WebSocket instance
                        webSocket = new WebSocket.WebSocket(new Uri(address));

#if !UNITY_WEBGL
                    webSocket.StartPingThread = true;

#if !BESTHTTP_DISABLE_PROXY
                    if (HTTPManager.Proxy != null)
                        webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
#endif
#endif

                        // Subscribe to the WS events
                        webSocket.OnOpen += OnOpen;
                        webSocket.OnMessage += OnMessageReceived;
                        webSocket.OnClosed += OnClosed;
                        webSocket.OnError += OnError;

                        // Start connecting to the server
                        webSocket.Open();

                        Text += "Opening Web Socket...\n";
                    }

                    if (webSocket != null && webSocket.IsOpen)
                    {
                        GUILayout.Space(10);

                        GUILayout.BeginHorizontal();
                        msgToSend = GUILayout.TextField(msgToSend);

                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("Send", GUILayout.MaxWidth(70)))
                        {
                            Text += "Sending message...\n";

                            // Send message to the server
                            webSocket.Send(msgToSend);
                        }

                        GUILayout.Space(10);

                        if (GUILayout.Button("Close"))
                        {
                            // Close the connection
                            webSocket.Close(1000, "Bye!");
                        }
                    }
                });
        }

        #endregion

        #region WebSocket Event Handlers

        /// <summary>
        /// Called when the web socket is open, and we are ready to send and receive data
        /// </summary>
        void OnOpen(WebSocket.WebSocket ws)
        {
            Text += string.Format("-WebSocket Open!\n");
        }

        /// <summary>
        /// Called when we received a text message from the server
        /// </summary>
        void OnMessageReceived(WebSocket.WebSocket ws, string message)
        {
            Text += string.Format("-Message received: {0}\n", message);
        }

        /// <summary>
        /// Called when the web socket closed
        /// </summary>
        void OnClosed(WebSocket.WebSocket ws, UInt16 code, string message)
        {
            Text += string.Format("-WebSocket closed! Code: {0} Message: {1}\n", code, message);
            webSocket = null;
        }

        /// <summary>
        /// Called when an error occured on client side
        /// </summary>
        void OnError(WebSocket.WebSocket ws, Exception ex)
        {
            string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
            if (ws.InternalRequest.Response != null)
            {
                errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
            }
#endif

            Text += string.Format("-An error occured: {0}\n", (ex != null ? ex.Message : "Unknown Error " + errorMsg));

            webSocket = null;
        }

        #endregion
    }
}

#endif