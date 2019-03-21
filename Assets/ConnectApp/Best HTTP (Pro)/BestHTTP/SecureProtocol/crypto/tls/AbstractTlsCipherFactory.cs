#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public class AbstractTlsCipherFactory
        :   TlsCipherFactory
    {
        /// <exception cref="IOException"></exception>
        public virtual TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm)
        {
            throw new TlsFatalAlert(AlertDescription.internal_error);
        }
    }
}
#pragma warning restore
#endif
