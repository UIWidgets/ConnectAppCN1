using System;
using System.Collections.Generic;

using UnityEngine;
using BestHTTP;
using BestHTTP.Statistics;
using BestHTTP.Examples;

namespace BestHTTP.Examples
{
    /// <summary>
    /// A class to describe an Example and store it's metadata.
    /// </summary>
    public sealed class SampleDescriptor
    {
        public bool IsLabel { get; set; }
        public Type Type { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public bool IsSelected { get; set; }
        public GameObject UnityObject { get; set; }
        public bool IsRunning { get { return UnityObject != null; } }

        public SampleDescriptor(Type type, string displayName, string description)
        {
            this.Type = type;
            this.DisplayName = displayName;
            this.Description = description;
        }

        public void CreateUnityObject()
        {
            if (UnityObject != null)
                return;

            UnityObject = new GameObject(DisplayName);
            UnityObject.AddComponent(Type);
        }

        public void DestroyUnityObject()
        {
            if (UnityObject != null)
            {
                UnityEngine.Object.Destroy(UnityObject);
                UnityObject = null;
            }
        }
    }

    public class SampleSelector : MonoBehaviour
    {
        public const int statisticsHeight = 160;

        List<SampleDescriptor> Samples = new List<SampleDescriptor>();
        public static SampleDescriptor SelectedSample;

        Vector2 scrollPos;

        void Awake()
        {
            Application.runInBackground = true;
            HTTPManager.Logger.Level = BestHTTP.Logger.Loglevels.All;

#if UNITY_SAMSUNGTV
        SamsungTV.touchPadMode = SamsungTV.TouchPadMode.Mouse;

        // Create a red 'cursor' to see where we are pointing to
        Texture2D tex = new Texture2D(8, 8, TextureFormat.RGB24, false);
        for (int i = 0; i < tex.width; ++i)
            for (int cv = 0; cv < tex.height; ++cv)
                tex.SetPixel(i, cv, Color.red);
        tex.Apply(false, true);
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
#endif

            Samples.Add(new SampleDescriptor(null, "HTTP Samples", string.Empty) { IsLabel = true });

            Samples.Add(new SampleDescriptor(typeof(TextureDownloadSample), "Texture Download", "With HTTPManager.MaxConnectionPerServer you can control how many requests can be processed per server parallel.\n\nFeatures demoed in this example:\n-Parallel requests to the same server\n-Controlling the parallelization\n-Automatic Caching\n-Create a Texture2D from the downloaded data"));
            Samples.Add(new SampleDescriptor(typeof(AssetBundleSample), "AssetBundle Download", "A small example that shows a possible way to download an AssetBundle and load a resource from it.\n\nFeatures demoed in this example:\n-Using HTTPRequest without a callback\n-Using HTTPRequest in a Coroutine\n-Loading an AssetBundle from the downloaded bytes\n-Automatic Caching"));
#if !UNITY_WEBGL || UNITY_EDITOR
            Samples.Add(new SampleDescriptor(typeof(LargeFileDownloadSample), "Large File Download", "This example demonstrates how you can download a (large) file and continue the download after the connection is aborted.\n\nFeatures demoed in this example:\n-Setting up a streamed download\n-How to access the downloaded data while the download is in progress\n-Setting the HTTPRequest's StreamFragmentSize to controll the frequency and size of the fragments\n-How to use the SetRangeHeader to continue a previously disconnected download\n-How to disable the local, automatic caching"));
#endif

#if !BESTHTTP_DISABLE_WEBSOCKET
            Samples.Add(new SampleDescriptor(null, "WebSocket Samples", string.Empty) { IsLabel = true });
            Samples.Add(new SampleDescriptor(typeof(WebSocketSample), "Echo", "A WebSocket demonstration that connects to a WebSocket echo service.\n\nFeatures demoed in this example:\n-Basic usage of the WebSocket class"));
#endif

#if !BESTHTTP_DISABLE_SOCKETIO
            Samples.Add(new SampleDescriptor(null, "Socket.IO Samples", string.Empty) { IsLabel = true });
            Samples.Add(new SampleDescriptor(typeof(SocketIOChatSample), "Chat", "This example uses the Socket.IO implementation to connect to the official Chat demo server(http://chat.socket.io/).\n\nFeatures demoed in this example:\n-Instantiating and setting up a SocketManager to connect to a Socket.IO server\n-Changing SocketOptions property\n-Subscribing to Socket.IO events\n-Sending custom events to the server"));
#if !UNITY_WEBGL || UNITY_EDITOR
            Samples.Add(new SampleDescriptor(typeof(SocketIOWePlaySample), "WePlay", "This example uses the Socket.IO implementation to connect to the official WePlay demo server(http://weplay.io/).\n\nFeatures demoed in this example:\n-Instantiating and setting up a SocketManager to connect to a Socket.IO server\n-Subscribing to Socket.IO events\n-Receiving binary data\n-How to load a texture from the received binary data\n-How to disable payload decoding for fine tune for some speed\n-Sending custom events to the server"));
#endif
#endif

#if !BESTHTTP_DISABLE_SIGNALR_CORE
            Samples.Add(new SampleDescriptor(null, "SignalR Core Samples", string.Empty) { IsLabel = true });
            Samples.Add(new SampleDescriptor(typeof(TestHubExample), "Hub Sample", "This sample demonstrates most of the functionalities of the SignalR protocol:\n-How to set up HubConnection to connect to the server\n-Subscribing to server-callable function\n-Calling client-callable function on the server\n-Calling and handling streaming\n"));
            Samples.Add(new SampleDescriptor(typeof(HubWithAuthorizationSample), "Hub Authentication Sample", "This sample demonstrates the default access token authentication. The server sends a JWT token to the client with a new url. The client will connect to that new url and sends the JWT token.\n"));
            Samples.Add(new SampleDescriptor(typeof(HubWithPreAuthorizationSample), "Hub Pre-Authentication Sample", "This sample demonstrates manual authentication.\n"));
            Samples.Add(new SampleDescriptor(typeof(RedirectSample), "Hub Redirect Sample", "This sample demonstrates how the plugin handles redirection through the SignalR Core negotiation data.\n"));
#endif

#if (!BESTHTTP_DISABLE_SIGNALR && !UNITY_WEBGL) || UNITY_EDITOR
            Samples.Add(new SampleDescriptor(null, "SignalR Samples", string.Empty) { IsLabel = true });
            Samples.Add(new SampleDescriptor(typeof(SimpleStreamingSample), "Simple Streaming", "A very simple example of a background thread that broadcasts the server time to all connected clients every two seconds.\n\nFeatures demoed in this example:\n-Subscribing and handling non-hub messages"));
            Samples.Add(new SampleDescriptor(typeof(ConnectionAPISample), "Connection API", "Demonstrates all features of the lower-level connection API including starting and stopping, sending and receiving messages, and managing groups.\n\nFeatures demoed in this example:\n-Instantiating and setting up a SignalR Connection to connect to a SignalR server\n-Changing the default Json encoder\n-Subscribing to state changes\n-Receiving and handling of non-hub messages\n-Sending non-hub messages\n-Managing groups"));
            Samples.Add(new SampleDescriptor(typeof(ConnectionStatusSample), "Connection Status", "Demonstrates how to handle the events that are raised when connections connect, reconnect and disconnect from the Hub API.\n\nFeatures demoed in this example:\n-Connecting to a Hub\n-Setting up a callback for Hub events\n-Handling server-sent method call requests\n-Calling a Hub-method on the server-side\n-Opening and closing the SignalR Connection"));
            Samples.Add(new SampleDescriptor(typeof(DemoHubSample), "Demo Hub", "A contrived example that exploits every feature of the Hub API.\n\nFeatures demoed in this example:\n-Creating and using wrapper Hub classes to encapsulate hub functions and events\n-Handling long running server-side functions by handling progress messages\n-Groups\n-Handling server-side functions with return value\n-Handling server-side functions throwing Exceptions\n-Calling server-side functions with complex type parameters\n-Calling server-side functions with array parameters\n-Calling overloaded server-side functions\n-Changing Hub states\n-Receiving and handling hub state changes\n-Calling server-side functions implemented in VB .NET"));
#if !UNITY_WEBGL || UNITY_EDITOR
            Samples.Add(new SampleDescriptor(typeof(AuthenticationSample), "Authentication", "Demonstrates how to use the authorization features of the Hub API to restrict certain Hubs and methods to specific users.\n\nFeatures demoed in this example:\n-Creating and using wrapper Hub classes to encapsulate hub functions and events\n-Create and use a Header-based authenticator to access protected APIs\n-SignalR over HTTPS"));
#endif
#endif

#if !BESTHTTP_DISABLE_CACHING
            Samples.Add(new SampleDescriptor(null, "Plugin Samples", string.Empty) { IsLabel = true });
            Samples.Add(new SampleDescriptor(typeof(CacheMaintenanceSample), "Cache Maintenance", "With this demo you can see how you can use the HTTPCacheService's BeginMaintainence function to delete too old cached entities and keep the cache size under a specified value.\n\nFeatures demoed in this example:\n-How to set up a HTTPCacheMaintananceParams\n-How to call the BeginMaintainence function"));
#endif

            SelectedSample = Samples[1];
        }

        void Update()
        {
            GUIHelper.ClientArea = new Rect(0, SampleSelector.statisticsHeight + 5, Screen.width, Screen.height - SampleSelector.statisticsHeight - 50);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SelectedSample != null && SelectedSample.IsRunning)
                    SelectedSample.DestroyUnityObject();
                else
                    Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (SelectedSample != null && !SelectedSample.IsRunning)
                    SelectedSample.CreateUnityObject();
            }
        }

        void OnGUI()
        {
            var stats = HTTPManager.GetGeneralStatistics(StatisticsQueryFlags.All);

            // Connection statistics
            GUIHelper.DrawArea(new Rect(0, 0, Screen.width / 3, statisticsHeight), false, () =>
                {
                // Header
                GUIHelper.DrawCenteredText("Connections");

                    GUILayout.Space(5);

                    GUIHelper.DrawRow("Sum:", stats.Connections.ToString());
                    GUIHelper.DrawRow("Active:", stats.ActiveConnections.ToString());
                    GUIHelper.DrawRow("Free:", stats.FreeConnections.ToString());
                    GUIHelper.DrawRow("Recycled:", stats.RecycledConnections.ToString());
                    GUIHelper.DrawRow("Requests in queue:", stats.RequestsInQueue.ToString());
                });

            // Cache statistics
            GUIHelper.DrawArea(new Rect(Screen.width / 3, 0, Screen.width / 3, statisticsHeight), false, () =>
                {
                    GUIHelper.DrawCenteredText("Cache");

#if !BESTHTTP_DISABLE_CACHING
                if (!BestHTTP.Caching.HTTPCacheService.IsSupported)
                    {
#endif
                    GUI.color = Color.yellow;
                        GUIHelper.DrawCenteredText("Disabled in WebPlayer, WebGL & Samsung Smart TV Builds!");
                        GUI.color = Color.white;
#if !BESTHTTP_DISABLE_CACHING
                }
                    else
                    {
                        GUILayout.Space(5);

                        GUIHelper.DrawRow("Cached entities:", stats.CacheEntityCount.ToString());
                        GUIHelper.DrawRow("Sum Size (bytes): ", stats.CacheSize.ToString("N0"));

                        GUILayout.BeginVertical();

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Clear Cache"))
                            BestHTTP.Caching.HTTPCacheService.BeginClear();

                        GUILayout.EndVertical();
                    }
#endif
            });

            // Cookie statistics
            GUIHelper.DrawArea(new Rect((Screen.width / 3) * 2, 0, Screen.width / 3, statisticsHeight), false, () =>
                {
                    GUIHelper.DrawCenteredText("Cookies");

#if !BESTHTTP_DISABLE_COOKIES
                if (!BestHTTP.Cookies.CookieJar.IsSavingSupported)
                {
#endif
                    GUI.color = Color.yellow;
                    GUIHelper.DrawCenteredText("Saving and loading from disk is disabled in WebPlayer, WebGL & Samsung Smart TV Builds!");
                    GUI.color = Color.white;
#if !BESTHTTP_DISABLE_COOKIES
                }
                else
                {
                    GUILayout.Space(5);

                    GUIHelper.DrawRow("Cookies:", stats.CookieCount.ToString());
                    GUIHelper.DrawRow("Estimated size (bytes):", stats.CookieJarSize.ToString("N0"));

                    GUILayout.BeginVertical();

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Clear Cookies"))
                        BestHTTP.Cookies.CookieJar.Clear();

                    GUILayout.EndVertical();
                }
#endif
            });

            if (SelectedSample == null || (SelectedSample != null && !SelectedSample.IsRunning))
            {
                // Draw the list of samples
                GUIHelper.DrawArea(new Rect(0, statisticsHeight + 5, SelectedSample == null ? Screen.width : Screen.width / 3, Screen.height - statisticsHeight - 5), false, () =>
                    {
                        scrollPos = GUILayout.BeginScrollView(scrollPos);
                        for (int i = 0; i < Samples.Count; ++i)
                            DrawSample(Samples[i]);
                        GUILayout.EndScrollView();
                    });

                if (SelectedSample != null)
                    DrawSampleDetails(SelectedSample);
            }
            else if (SelectedSample != null && SelectedSample.IsRunning)
            {
                GUILayout.BeginArea(new Rect(0, Screen.height - 50, Screen.width, 50), string.Empty);
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Back", GUILayout.MinWidth(100)))
                    SelectedSample.DestroyUnityObject();
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }

        private void DrawSample(SampleDescriptor sample)
        {
            if (sample.IsLabel)
            {
                GUILayout.Space(15);
                GUIHelper.DrawCenteredText(sample.DisplayName);
                GUILayout.Space(5);
            }
            else if (GUILayout.Button(sample.DisplayName))
            {
                sample.IsSelected = true;

                if (SelectedSample != null)
                    SelectedSample.IsSelected = false;

                SelectedSample = sample;
            }
        }

        private void DrawSampleDetails(SampleDescriptor sample)
        {
            Rect area = new Rect(Screen.width / 3, statisticsHeight + 5, (Screen.width / 3) * 2, Screen.height - statisticsHeight - 5);
            GUI.Box(area, string.Empty);

            GUILayout.BeginArea(area);
            GUILayout.BeginVertical();
            GUIHelper.DrawCenteredText(sample.DisplayName);
            GUILayout.Space(5);
            GUILayout.Label(sample.Description);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Start Sample"))
                sample.CreateUnityObject();

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}