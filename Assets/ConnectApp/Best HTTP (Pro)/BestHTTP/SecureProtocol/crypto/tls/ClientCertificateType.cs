#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public abstract class ClientCertificateType
    {
        /*
         *  RFC 4346 7.4.4
         */
        public const byte rsa_sign = 1;
        public const byte dss_sign = 2;
        public const byte rsa_fixed_dh = 3;
        public const byte dss_fixed_dh = 4;
        public const byte rsa_ephemeral_dh_RESERVED = 5;
        public const byte dss_ephemeral_dh_RESERVED = 6;
        public const byte fortezza_dms_RESERVED = 20;

        /*
        * RFC 4492 5.5
        */
        public const byte ecdsa_sign = 64;
        public const byte rsa_fixed_ecdh = 65;
        public const byte ecdsa_fixed_ecdh = 66;
    }
}
#pragma warning restore
#endif
