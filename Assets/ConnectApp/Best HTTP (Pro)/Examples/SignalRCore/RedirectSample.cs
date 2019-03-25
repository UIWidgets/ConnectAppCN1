#if !BESTHTTP_DISABLE_SIGNALR_CORE

using BestHTTP;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using System;
using UnityEngine;

namespace BestHTTP.Examples
{
    /// <summary>
    /// This sample demonstrates redirection capabilities. The server will redirect a few times the client before
    /// routing it to the final endpoint.
    /// </summary>
    public sealed class RedirectSample : MonoBehaviour
    {
        // Server uri to connect to
        readonly Uri URI = new Uri(GUIHelper.BaseURL + "/redirect_sample");

        // Instance of the HubConnection
        public HubConnection hub;

        Vector2 scrollPos;
        public string uiText;

        void Start()
        {
            // Server side of this example can be found here:
            // https://github.com/Benedicht/BestHTTP_DemoSite/blob/master/BestHTTP_DemoSite/Hubs/

            // Crete the HubConnection
            hub = new HubConnection(URI, new JsonProtocol(new LitJsonEncoder()));
            hub.AuthenticationProvider = new RedirectLoggerAccessTokenAuthenticator(hub);

            // Subscribe to hub events
            hub.OnConnected += Hub_OnConnected;
            hub.OnError += Hub_OnError;
            hub.OnClosed += Hub_OnClosed;

            hub.OnMessage += Hub_OnMessage;

            hub.OnRedirected += Hub_Redirected;

            // And finally start to connect to the server
            hub.StartConnect();

            uiText = "StartConnect called\n";
        }

        void OnDestroy()
        {
            if (hub != null)
            {
                hub.StartClose();
            }
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

        private void Hub_Redirected(HubConnection hub, Uri oldUri, Uri newUri)
        {
            uiText += string.Format("Hub connection redirected to '<color=green>{0}</color>'!\n", hub.Uri);
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

    public sealed class RedirectLoggerAccessTokenAuthenticator : IAuthenticationProvider
    {
        /// <summary>
        /// No pre-auth step required for this type of authentication
        /// </summary>
        public bool IsPreAuthRequired { get { return false; } }

#pragma warning disable 0067
        /// <summary>
        /// Not used event as IsPreAuthRequired is false
        /// </summary>
        public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

        /// <summary>
        /// Not used event as IsPreAuthRequired is false
        /// </summary>
        public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

#pragma warning restore 0067

        private HubConnection _connection;

        public RedirectLoggerAccessTokenAuthenticator(HubConnection connection)
        {
            this._connection = connection;
        }

        /// <summary>
        /// Not used as IsPreAuthRequired is false
        /// </summary>
        public void StartAuthentication()
        { }

        /// <summary>
        /// Prepares the request by adding two headers to it
        /// </summary>
        public void PrepareRequest(BestHTTP.HTTPRequest request)
        {
            request.SetHeader("x-redirect-count", _connection.RedirectCount.ToString());

            if (HTTPProtocolFactory.GetProtocolFromUri(request.CurrentUri) == SupportedProtocols.HTTP)
                request.Uri = PrepareUri(request.Uri);
        }

        public Uri PrepareUri(Uri uri)
        {
            if (this._connection.NegotiationResult != null && !string.IsNullOrEmpty(this._connection.NegotiationResult.AccessToken))
            {
                string query = string.IsNullOrEmpty(uri.Query) ? "?" : uri.Query + "&";
                UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, query + "access_token=" + this._connection.NegotiationResult.AccessToken);
                return uriBuilder.Uri;
            }
            else
                return uri;
        }
    }
}

#endif