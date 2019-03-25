#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	/**
	* a holding class for a byte array of data to be processed.
	*/
	public class CmsProcessableByteArray
		: CmsProcessable, CmsReadable
	{
		private readonly byte[] bytes;

        public CmsProcessableByteArray(byte[] bytes)
		{
			this.bytes = bytes;
		}

        public virtual Stream GetInputStream()
		{
			return new MemoryStream(bytes, false);
		}

        public virtual void Write(Stream zOut)
		{
			zOut.Write(bytes, 0, bytes.Length);
		}

        /// <returns>A clone of the byte array</returns>
        [Obsolete]
		public virtual object GetContent()
		{
			return bytes.Clone();
		}
	}
}
#pragma warning restore
#endif
