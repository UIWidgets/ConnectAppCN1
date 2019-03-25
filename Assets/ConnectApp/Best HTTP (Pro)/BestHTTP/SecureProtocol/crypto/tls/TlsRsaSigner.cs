#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Encodings;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public class TlsRsaSigner
        :   AbstractTlsSigner
    {
        public override byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm,
            AsymmetricKeyParameter privateKey, byte[] hash)
        {
            ISigner signer = MakeSigner(algorithm, true, true,
                new ParametersWithRandom(privateKey, this.mContext.SecureRandom));
            signer.BlockUpdate(hash, 0, hash.Length);
            return signer.GenerateSignature();
        }

        public override bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes,
            AsymmetricKeyParameter publicKey, byte[] hash)
        {
            ISigner signer = MakeSigner(algorithm, true, false, publicKey);
            signer.BlockUpdate(hash, 0, hash.Length);
            return signer.VerifySignature(sigBytes);
        }

        public override ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey)
        {
            return MakeSigner(algorithm, false, true, new ParametersWithRandom(privateKey, this.mContext.SecureRandom));
        }

        public override ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey)
        {
            return MakeSigner(algorithm, false, false, publicKey);
        }

        public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
        {
            return publicKey is RsaKeyParameters && !publicKey.IsPrivate;
        }

        protected virtual ISigner MakeSigner(SignatureAndHashAlgorithm algorithm, bool raw, bool forSigning,
            ICipherParameters cp)
        {
            if ((algorithm != null) != TlsUtilities.IsTlsV12(mContext))
                throw new InvalidOperationException();
            if (algorithm != null && algorithm.Signature != SignatureAlgorithm.rsa)
                throw new InvalidOperationException();

            IDigest d;
            if (raw)
            {
                d = new NullDigest();
            }
            else if (algorithm == null)
            {
                d = new CombinedHash();
            }
            else
            {
                d = TlsUtilities.CreateHash(algorithm.Hash);
            }

            ISigner s;
            if (algorithm != null)
            {
                /*
                 * RFC 5246 4.7. In RSA signing, the opaque vector contains the signature generated
                 * using the RSASSA-PKCS1-v1_5 signature scheme defined in [PKCS1].
                 */
                s = new RsaDigestSigner(d, TlsUtilities.GetOidForHashAlgorithm(algorithm.Hash));
            }
            else
            {
                /*
                 * RFC 5246 4.7. Note that earlier versions of TLS used a different RSA signature scheme
                 * that did not include a DigestInfo encoding.
                 */
                s = new GenericSigner(CreateRsaImpl(), d);
            }
            s.Init(forSigning, cp);
            return s;
        }

        protected virtual IAsymmetricBlockCipher CreateRsaImpl()
        {
            /*
             * RFC 5246 7.4.7.1. Implementation note: It is now known that remote timing-based attacks
             * on TLS are possible, at least when the client and server are on the same LAN.
             * Accordingly, implementations that use static RSA keys MUST use RSA blinding or some other
             * anti-timing technique, as described in [TIMING].
             */
            return new Pkcs1Encoding(new RsaBlindedEngine());
        }
    }
}
#pragma warning restore
#endif
