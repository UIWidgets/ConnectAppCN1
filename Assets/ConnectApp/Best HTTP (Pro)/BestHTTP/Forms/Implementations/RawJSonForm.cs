// Contributed by Matt Senne from conjoinedcats.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BestHTTP.Forms
{
    public sealed class RawJsonForm : HTTPFormBase
    {
        private byte[] CachedData;

        /// <summary>
        /// Prepares the request to sending a form. It should set only the headers.
        /// </summary>
        public override void PrepareRequest(HTTPRequest request)
        {
            request.SetHeader("Content-Type", "application/json");
        }

        /// <summary>
        /// Prepares and returns with the form's raw data.
        /// </summary>
        public override byte[] GetData()
        {
            if (CachedData != null && !IsChanged)
                return CachedData;

            Dictionary<string, string> dict = Fields.ToDictionary(x => x.Name, x => x.Text);
            string json = JSON.Json.Encode(dict);

            IsChanged = false;
            return CachedData = Encoding.UTF8.GetBytes(json);
        }
    }
}