#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	public class TlsNullCompression
		: TlsCompression
	{
		public virtual Stream Compress(Stream output)
		{
			return output;
		}

		public virtual Stream Decompress(Stream output)
		{
			return output;
		}
	}
}
#pragma warning restore
#endif
