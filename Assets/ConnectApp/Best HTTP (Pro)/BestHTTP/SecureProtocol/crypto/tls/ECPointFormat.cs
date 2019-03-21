#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    /// <summary>
    /// RFC 4492 5.1.2
    /// </summary>
    public abstract class ECPointFormat
    {
        public const byte uncompressed = 0;
        public const byte ansiX962_compressed_prime = 1;
        public const byte ansiX962_compressed_char2 = 2;

        /*
         * reserved (248..255)
         */
    }
}
#pragma warning restore
#endif
