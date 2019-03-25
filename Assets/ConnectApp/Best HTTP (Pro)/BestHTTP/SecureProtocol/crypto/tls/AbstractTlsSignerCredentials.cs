#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public abstract class AbstractTlsSignerCredentials
        : AbstractTlsCredentials, TlsSignerCredentials
    {
        /// <exception cref="IOException"></exception>
        public abstract byte[] GenerateCertificateSignature(byte[] hash);

        public virtual SignatureAndHashAlgorithm SignatureAndHashAlgorithm
        {
            get
            {
                throw new InvalidOperationException("TlsSignerCredentials implementation does not support (D)TLS 1.2+");
            }
        }
    }
}
#pragma warning restore
#endif
