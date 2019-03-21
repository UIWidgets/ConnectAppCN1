#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    public class TlsFatalAlertReceived
        : TlsException
    {
        private readonly byte alertDescription;

        public TlsFatalAlertReceived(byte alertDescription)
            : base(Tls.AlertDescription.GetText(alertDescription), null)
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
