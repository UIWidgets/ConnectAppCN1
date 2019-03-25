#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    /*
     * RFC 6520 3.
     */
    public abstract class HeartbeatMessageType
    {
        public const byte heartbeat_request = 1;
        public const byte heartbeat_response = 2;

        public static bool IsValid(byte heartbeatMessageType)
        {
            return heartbeatMessageType >= heartbeat_request && heartbeatMessageType <= heartbeat_response;
        }
    }
}
#pragma warning restore
#endif
