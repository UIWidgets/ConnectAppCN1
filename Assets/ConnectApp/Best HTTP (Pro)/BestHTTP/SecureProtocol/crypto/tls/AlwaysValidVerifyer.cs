#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Crypto.Tls
{
	/// <remarks>
	/// A certificate verifyer, that will always return true.
	/// <pre>
	/// DO NOT USE THIS FILE UNLESS YOU KNOW EXACTLY WHAT YOU ARE DOING.
	/// </pre>
	/// </remarks>
	public class AlwaysValidVerifyer : ICertificateVerifyer
	{
		/// <summary>Return true.</summary>
		public bool IsValid(Uri targetUri, X509CertificateStructure[] certs)
		{
			return true;
		}
	}
}

#endif
