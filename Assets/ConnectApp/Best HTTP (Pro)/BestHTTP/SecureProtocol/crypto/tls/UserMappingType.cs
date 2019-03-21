#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    /// <remarks>RFC 4681</remarks>
    public abstract class UserMappingType
    {
        /*
         * RFC 4681
         */
        public const byte upn_domain_hint = 64;
    }
}
#pragma warning restore
#endif
