using System;

namespace BestHTTP.SignalRCore.Authentication
{
    public sealed class DefaultAccessTokenAuthenticator : IAuthenticationProvider
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

        public DefaultAccessTokenAuthenticator(HubConnection connection)
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