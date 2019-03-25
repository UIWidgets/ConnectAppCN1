using System;
using System.Collections.Generic;

namespace BestHTTP.SignalRCore.Messages
{
    public sealed class SupportedTransport
    {
        /// <summary>
        /// Name of the transport.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Supported transfer formats of the transport.
        /// </summary>
        public List<string> SupportedFormats { get; private set; }

        internal SupportedTransport(string transportName, List<string> transferFormats)
        {
            this.Name = transportName;
            this.SupportedFormats = transferFormats;
        }
    }

    /// <summary>
    /// Negotiation result of the /negotiation request.
    /// <see cref="https://github.com/aspnet/SignalR/blob/dev/specs/TransportProtocols.md#post-endpoint-basenegotiate-request"/>
    /// </summary>
    public sealed class NegotiationResult
    {
        /// <summary>
        /// The connectionId which is required by the Long Polling and Server-Sent Events transports (in order to correlate sends and receives).
        /// </summary>
        public string ConnectionId { get; private set; }

        /// <summary>
        /// The availableTransports list which describes the transports the server supports. For each transport, the name of the transport (transport) is listed, as is a list of "transfer formats" supported by the transport (transferFormats)
        /// </summary>
        public List<SupportedTransport> SupportedTransports { get; private set; }

        /// <summary>
        /// The url which is the URL the client should connect to.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// The accessToken which is an optional bearer token for accessing the specified url.
        /// </summary>
        public string AccessToken { get; private set; }

        internal static NegotiationResult Parse(string json, out string error, HubConnection hub)
        {
            error = null;

            Dictionary<string, object> response = BestHTTP.JSON.Json.Decode(json) as Dictionary<string, object>;
            if (response == null)
            {
                error = "Json decoding failed!";
                return null;
            }

            try
            {
                NegotiationResult result = new NegotiationResult();
                object value;
                if (response.TryGetValue("connectionId", out value))
                    result.ConnectionId = value.ToString();

                if (response.TryGetValue("availableTransports", out value))
                {
                    List<object> transports = value as List<object>;
                    if (transports != null)
                    {
                        List<SupportedTransport> supportedTransports = new List<SupportedTransport>(transports.Count);

                        foreach (Dictionary<string, object> transport in transports)
                        {
                            string transportName = string.Empty;
                            List<string> transferModes = null;

                            if (transport.TryGetValue("transport", out value))
                                transportName = value.ToString();

                            if (transport.TryGetValue("transferFormats", out value))
                            {
                                List<object> transferFormats = value as List<object>;

                                if (transferFormats != null)
                                {
                                    transferModes = new List<string>(transferFormats.Count);
                                    foreach (var mode in transferFormats)
                                        transferModes.Add(mode.ToString());
                                }
                            }

                            supportedTransports.Add(new SupportedTransport(transportName, transferModes));
                        }

                        result.SupportedTransports = supportedTransports;
                    }
                }

                if (response.TryGetValue("url", out value))
                {
                    string uriStr = value.ToString();

                    Uri redirectUri;

                    // Here we will try to parse the received url. If TryCreate fails, we will throw an exception
                    //  as it should be able to successfully parse whole (absolute) urls (like "https://server:url/path")
                    //  and relative ones (like "/path").
                    if (!Uri.TryCreate(uriStr, UriKind.RelativeOrAbsolute, out redirectUri))
                        throw new Exception(string.Format("Couldn't parse url: '{0}'", uriStr));

                    // If received a relative url we will use the hub's current url to append the new path to it.
                    if (!redirectUri.IsAbsoluteUri)
                    {
                        Uri oldUri = hub.Uri;
                        var builder = new UriBuilder(oldUri);
                        builder.Path = uriStr;

                        redirectUri = builder.Uri;
                    }

                    result.Url = redirectUri;
                }

                if (response.TryGetValue("accessToken", out value))
                    result.AccessToken = value.ToString();
                else if (hub.NegotiationResult != null)
                    result.AccessToken = hub.NegotiationResult.AccessToken;

                return result;
            }
            catch (Exception ex)
            {
                error = "Error while parsing result: " + ex.Message + " StackTrace: " + ex.StackTrace;
                return null;
            }
        }
    }
}