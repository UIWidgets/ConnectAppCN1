using System;

namespace ConnectApp.utils {
    public class Snowflake {
        public static string CreateNonce() {
            var sinceEpoch = DateTime.UtcNow.ToUniversalTime().Subtract(
                                 new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                             ).TotalMilliseconds + 1;
            var nonce = Math.Floor(sinceEpoch).ToString();
            while (nonce.Length < 16) nonce = "0" + nonce;
            return nonce;
        }
    }
}