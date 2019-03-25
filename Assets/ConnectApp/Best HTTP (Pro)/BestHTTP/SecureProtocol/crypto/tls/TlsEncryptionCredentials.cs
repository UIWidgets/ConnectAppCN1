#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public interface TlsEncryptionCredentials
        :   TlsCredentials
    {
        /// <exception cref="IOException"></exception>
        byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
    }
}
#pragma warning restore
#endif
