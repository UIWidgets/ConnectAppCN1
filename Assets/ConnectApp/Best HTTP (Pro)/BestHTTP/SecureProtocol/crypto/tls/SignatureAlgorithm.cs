#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    /**
     * RFC 5246 7.4.1.4.1 (in RFC 2246, there were no specific values assigned)
     */
    public abstract class SignatureAlgorithm
    {
        public const byte anonymous = 0;
        public const byte rsa = 1;
        public const byte dsa = 2;
        public const byte ecdsa = 3;
    }
}
#pragma warning restore
#endif
