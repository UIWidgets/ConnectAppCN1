#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	/**
	 * A BER Null object.
	 */
	public class BerNull
		: DerNull
	{
		public static new readonly BerNull Instance = new BerNull(0);

		[Obsolete("Use static Instance object")]
		public BerNull()
		{
		}

		private BerNull(int dummy) : base(dummy)
		{
		}

		internal override void Encode(
			DerOutputStream  derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(Asn1Tags.Null);
			}
			else
			{
				base.Encode(derOut);
			}
		}
	}
}
#pragma warning restore
#endif
