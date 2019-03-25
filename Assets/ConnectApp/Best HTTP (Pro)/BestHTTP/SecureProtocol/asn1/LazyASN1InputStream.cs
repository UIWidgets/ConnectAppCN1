#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	public class LazyAsn1InputStream
		: Asn1InputStream
	{
		public LazyAsn1InputStream(
			byte[] input)
			: base(input)
		{
		}

		public LazyAsn1InputStream(
			Stream inputStream)
			: base(inputStream)
		{
		}

		internal override DerSequence CreateDerSequence(
			DefiniteLengthInputStream dIn)
		{
			return new LazyDerSequence(dIn.ToArray());
		}

		internal override DerSet CreateDerSet(
			DefiniteLengthInputStream dIn)
		{
			return new LazyDerSet(dIn.ToArray());
		}
	}
}
#pragma warning restore
#endif
