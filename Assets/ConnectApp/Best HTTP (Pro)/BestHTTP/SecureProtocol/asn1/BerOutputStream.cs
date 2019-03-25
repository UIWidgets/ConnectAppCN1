#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// TODO Make Obsolete in favour of Asn1OutputStream?
    public class BerOutputStream
        : DerOutputStream
    {
        public BerOutputStream(Stream os) : base(os)
        {
        }

		[Obsolete("Use version taking an Asn1Encodable arg instead")]
        public override void WriteObject(
            object    obj)
        {
            if (obj == null)
            {
                WriteNull();
            }
            else if (obj is Asn1Object)
            {
                ((Asn1Object)obj).Encode(this);
            }
            else if (obj is Asn1Encodable)
            {
                ((Asn1Encodable)obj).ToAsn1Object().Encode(this);
            }
            else
            {
                throw new IOException("object not BerEncodable");
            }
        }
    }
}
#pragma warning restore
#endif
