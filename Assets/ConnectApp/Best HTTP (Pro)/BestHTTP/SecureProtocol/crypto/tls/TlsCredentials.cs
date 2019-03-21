#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	public interface TlsCredentials
	{
		Certificate Certificate { get; }
	}
}
#pragma warning restore
#endif
