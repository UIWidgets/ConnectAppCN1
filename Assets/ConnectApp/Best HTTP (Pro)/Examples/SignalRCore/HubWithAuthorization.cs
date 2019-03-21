#if !BESTHTTP_DISABLE_SIGNALR_CORE

using BestHTTP.Examples;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using System;
using UnityEngine;

namespace BestHTTP.Examples
{
    /// <summary>
    /// A sample to demonstrate Bearer token authorization on the server. The client will connect to the /redirect route
    /// where it will receive the token and will receive the new url (/HubWithAuthorization) to connect to.
    /// HubWithAuthorization without the token would throw an error.
    /// </summary>
    public sealed class HubWithAuthorizationSample : MonoBehaviour
    {
        // Server uri to connect to
        readonly Uri URI = new Uri(GUIHelper.BaseURL + "/redirect");

        // Instance of the HubConnection
        HubConnection hub;

        Vector2 scrollPos;
        string uiText;

        void Start()
        {
            // Server side of this example can be found here:
            // https://github.com/Benedicht/BestHTTP_DemoSite/blob/master/BestHTTP_DemoSite/Hubs/

            // Crete the HubConnection
            hub = new HubConnection(URI, new JsonProtocol(new LitJsonEncoder()));

            // Subscribe to hub events
            hub.OnConnected += Hub_OnConnected;
            hub.OnError += Hub_OnError;
            hub.OnClosed += Hub_OnClosed;

            hub.OnMessage += Hub_OnMessage;

            // And finally start to connect to the server
            hub.StartConnect();

            uiText = "StartConnect called\n";
        }

        void OnDestroy()
        {
            if (hub != null)
                hub.StartClose();
        }

        // Draw the text stored in the 'uiText' field
        void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
                GUILayout.BeginVertical();

                GUILayout.Label(uiText);

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            });
        }

        /// <summary>
        /// This callback is called when the plugin is connected to the server successfully. Messages can be sent to the server after this point.
        /// </summary>
        private void Hub_OnConnected(HubConnection hub)
        {
            uiText += "Hub Connected\n";

            // Call a parameterless function. We expect a string return value.
            hub.Invoke<string>("Echo", "Message from the client")
                .OnSuccess(ret => uiText += string.Format(" 'Echo' returned: '{0}'\n", ret));
        }

        /// <summary>
        /// This callback is called for every hub message. If false is returned, the plugin will cancel any further processing of the message.
        /// </summary>
        private bool Hub_OnMessage(HubConnection hub, BestHTTP.SignalRCore.Messages.Message message)
        {
            //uiText += string.Format("( Message received: {0} )\n", message.ToString());

            return true;
        }

        /// <summary>
        /// This is called when the hub is closed after a StartClose() call.
        /// </summary>
        private void Hub_OnClosed(HubConnection hub)
        {
            uiText += "Hub Closed\n";
        }

        /// <summary>
        /// Called when an unrecoverable error happen. After this event the hub will not send or receive any messages.
        /// </summary>
        private void Hub_OnError(HubConnection hub, string error)
        {
            uiText += "Hub Error: " + error + "\n";
        }
    }
}

#endif