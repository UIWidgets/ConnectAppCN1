#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public interface TlsSigner
    {
        void Init(TlsContext context);

        byte[] GenerateRawSignature(AsymmetricKeyParameter privateKey, byte[] md5AndSha1);

        byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm,
            AsymmetricKeyParameter privateKey, byte[] hash);

        bool VerifyRawSignature(byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] md5AndSha1);

        bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes,
            AsymmetricKeyParameter publicKey, byte[] hash);

        ISigner CreateSigner(AsymmetricKeyParameter privateKey);

        ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey);

        ISigner CreateVerifyer(AsymmetricKeyParameter publicKey);

        ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey);

        bool IsValidPublicKey(AsymmetricKeyParameter publicKey);
    }
}
#pragma warning restore
#endif
