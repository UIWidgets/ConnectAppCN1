#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public interface TlsCipher
    {
        int GetPlaintextLimit(int ciphertextLimit);

        /// <exception cref="IOException"></exception>
        byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len);

        /// <exception cref="IOException"></exception>
        byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len);
    }
}
#pragma warning restore
#endif
