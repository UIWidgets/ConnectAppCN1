#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public abstract class ClientAuthenticationType
    {
        /*
         * RFC 5077 4
         */
        public const byte anonymous = 0;
        public const byte certificate_based = 1;
        public const byte psk = 2;
    }
}
#pragma warning restore
#endif
