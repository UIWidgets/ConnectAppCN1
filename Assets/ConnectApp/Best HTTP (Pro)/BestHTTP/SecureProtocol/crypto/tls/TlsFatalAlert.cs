#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public class TlsFatalAlert
        : TlsException
    {
        private readonly byte alertDescription;

        public TlsFatalAlert(byte alertDescription)
            : this(alertDescription, null)
        {
        }

        public TlsFatalAlert(byte alertDescription, Exception alertCause)
            : base(Tls.AlertDescription.GetText(alertDescription), alertCause)
        {
            this.alertDescription = alertDescription;
        }

        public virtual byte AlertDescription
        {
            get { return alertDescription; }
        }
    }
}
#pragma warning restore
#endif
