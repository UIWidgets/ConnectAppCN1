#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    /// <summary>An interface for verifying that Diffie-Hellman parameters are acceptable.</summary>
    public interface TlsDHVerifier
    {
        /// <summary>Verify that the given <c>DHParameters</c> are acceptable.</summary>
        /// <param name="dhParameters">The <c>DHParameters</c> to verify.</param>
        /// <returns>true if (and only if) the specified parameters are acceptable.</returns>
        bool Accept(DHParameters dhParameters);
    }
}
#pragma warning restore
#endif
