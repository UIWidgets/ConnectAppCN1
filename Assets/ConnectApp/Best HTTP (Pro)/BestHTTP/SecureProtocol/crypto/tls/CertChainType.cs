#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    /*
     * RFC 3546 3.3.
     */
    public abstract class CertChainType
    {
        public const byte individual_certs = 0;
        public const byte pkipath = 1;

        public static bool IsValid(byte certChainType)
        {
            return certChainType >= individual_certs && certChainType <= pkipath;
        }
    }
}
#pragma warning restore
#endif
