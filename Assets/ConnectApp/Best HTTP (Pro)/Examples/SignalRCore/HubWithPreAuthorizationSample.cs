#if !BESTHTTP_DISABLE_SIGNALR_CORE

using BestHTTP;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using System;
using UnityEngine;

namespace BestHTTP.Examples
{
    public sealed class HubWithPreAuthorizationSample : MonoBehaviour
    {
        // Server uri to connect to
        readonly Uri URI = new Uri(GUIHelper.BaseURL + "/HubWithAuthorization");
        readonly Uri AuthURI = new Uri(GUIHelper.BaseURL + "/generateJwtToken");

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
            hub.AuthenticationProvider = new PreAuthAccessTokenAuthenticator(AuthURI);
            hub.AuthenticationProvider.OnAuthenticationSucceded += AuthenticationProvider_OnAuthenticationSucceded;
            hub.AuthenticationProvider.OnAuthenticationFailed += AuthenticationProvider_OnAuthenticationFailed;

            // Subscribe to hub events
            hub.OnConnected += Hub_OnConnected;
            hub.OnError += Hub_OnError;
            hub.OnClosed += Hub_OnClosed;

            hub.OnMessage += Hub_OnMessage;

            // And finally start to connect to the server
            hub.StartConnect();

            uiText = "StartConnect called\n";
        }

        private void AuthenticationProvider_OnAuthenticationSucceded(IAuthenticationProvider provider)
        {
            string str = string.Format("Pre-Authentication Succeded! Token: '{0}' \n", (hub.AuthenticationProvider as PreAuthAccessTokenAuthenticator).Token);
            Debug.Log(str);
            uiText += str;
        }

        private void AuthenticationProvider_OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
        {
            uiText += string.Format("Authentication Failed! Reason: '{0}'\n", reason);
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

    public sealed class PreAuthAccessTokenAuthenticator : IAuthenticationProvider
    {
        /// <summary>
        /// No pre-auth step required for this type of authentication
        /// </summary>
        public bool IsPreAuthRequired { get { return true; } }

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

        private Uri authenticationUri;

        public string Token { get; private set; }

        public PreAuthAccessTokenAuthenticator(Uri authUri)
        {
            this.authenticationUri = authUri;
        }

        public void StartAuthentication()
        {
            var request = new HTTPRequest(this.authenticationUri, OnAuthenticationRequestFinished);
            request.Send();
        }

        private void OnAuthenticationRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            switch (req.State)
            {
                // The request finished without any problem.
                case HTTPRequestStates.Finished:
                    if (resp.IsSuccess)
                    {
                        this.Token = resp.DataAsText;
                        if (this.OnAuthenticationSucceded != null)
                            this.OnAuthenticationSucceded(this);
                    }
                    else // Internal server error?
                        AuthenticationFailed(string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                        resp.StatusCode,
                                                        resp.Message,
                                                        resp.DataAsText));
                    break;

                // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                case HTTPRequestStates.Error:
                    AuthenticationFailed("Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
                    break;

                // The request aborted, initiated by the user.
                case HTTPRequestStates.Aborted:
                    AuthenticationFailed("Request Aborted!");
                    break;

                // Connecting to the server is timed out.
                case HTTPRequestStates.ConnectionTimedOut:
                    AuthenticationFailed("Connection Timed Out!");
                    break;

                // The request didn't finished in the given time.
                case HTTPRequestStates.TimedOut:
                    AuthenticationFailed("Processing the request Timed Out!");
                    break;
            }
        }

        private void AuthenticationFailed(string reason)
        {
            if (this.OnAuthenticationFailed != null)
                this.OnAuthenticationFailed(this, reason);
        }

        /// <summary>
        /// Prepares the request by adding two headers to it
        /// </summary>
        public void PrepareRequest(BestHTTP.HTTPRequest request)
        {
            if (HTTPProtocolFactory.GetProtocolFromUri(request.CurrentUri) == SupportedProtocols.HTTP)
                request.Uri = PrepareUri(request.Uri);
        }

        public Uri PrepareUri(Uri uri)
        {
            if (!string.IsNullOrEmpty(this.Token))
            {
                string query = string.IsNullOrEmpty(uri.Query) ? "?" : uri.Query + "&";
                UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, query + "access_token=" + this.Token);
                return uriBuilder.Uri;
            }
            else
                return uri;
        }
    }
}

#endif