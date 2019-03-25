#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.Collections;

using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm
{
    public class Tables1kGcmExponentiator
        : IGcmExponentiator
    {
        // A lookup table of the power-of-two powers of 'x'
        // - lookupPowX2[i] = x^(2^i)
        private IList lookupPowX2;

        public void Init(byte[] x)
        {
            uint[] y = GcmUtilities.AsUints(x);
            if (lookupPowX2 != null && Arrays.AreEqual(y, (uint[])lookupPowX2[0]))
                return;

            lookupPowX2 = BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Platform.CreateArrayList(8);
            lookupPowX2.Add(y);
        }

        public void ExponentiateX(long pow, byte[] output)
        {
            uint[] y = GcmUtilities.OneAsUints();
            int bit = 0;
            while (pow > 0)
            {
                if ((pow & 1L) != 0)
                {
                    EnsureAvailable(bit);
                    GcmUtilities.Multiply(y, (uint[])lookupPowX2[bit]);
                }
                ++bit;
                pow >>= 1;
            }

            GcmUtilities.AsBytes(y, output);
        }

        private void EnsureAvailable(int bit)
        {
            int count = lookupPowX2.Count;
            if (count <= bit)
            {
                uint[] tmp = (uint[])lookupPowX2[count - 1];
                do
                {
                    tmp = Arrays.Clone(tmp);
                    GcmUtilities.Multiply(tmp, tmp);
                    lookupPowX2.Add(tmp);
                }
                while (++count <= bit);
            }
        }
    }
}
#pragma warning restore
#endif
