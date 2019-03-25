#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    internal class DtlsEpoch
    {
        private readonly DtlsReplayWindow mReplayWindow = new DtlsReplayWindow();

        private readonly int mEpoch;
        private readonly TlsCipher mCipher;

        private long mSequenceNumber = 0;

        internal DtlsEpoch(int epoch, TlsCipher cipher)
        {
            if (epoch < 0)
                throw new ArgumentException("must be >= 0", "epoch");
            if (cipher == null)
                throw new ArgumentNullException("cipher");

            this.mEpoch = epoch;
            this.mCipher = cipher;
        }

        internal long AllocateSequenceNumber()
        {
            // TODO Check for overflow
            return mSequenceNumber++;
        }

        internal TlsCipher Cipher
        {
            get { return mCipher; }
        }

        internal int Epoch
        {
            get { return mEpoch; }
        }

        internal DtlsReplayWindow ReplayWindow
        {
            get { return mReplayWindow; }
        }

        internal long SequenceNumber
        {
            get { return mSequenceNumber; }
        }
    }
}
#pragma warning restore
#endif
