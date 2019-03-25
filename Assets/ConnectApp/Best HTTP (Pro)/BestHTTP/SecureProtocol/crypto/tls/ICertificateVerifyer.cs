#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Crypto.Tls
{
	/// <remarks>
	/// This should be implemented by any class which can find out, if a given
	/// certificate chain is being accepted by an client.
	/// </remarks>
	public interface ICertificateVerifyer
	{
		/// <param name="certs">The certs, which are part of the chain.</param>
        /// <param name="targetUri"></param>
		/// <returns>True, if the chain is accepted, false otherwise</returns>
		bool IsValid(Uri targetUri, X509CertificateStructure[] certs);
	}
}

#endif
