#if !BESTHTTP_DISABLE_SIGNALR_CORE && !BESTHTTP_DISABLE_WEBSOCKET
using System;

namespace BestHTTP.SignalRCore.Encoders
{
    /*public sealed class JsonDotNetEncoder : BestHTTP.SignalRCore.IEncoder
    {
        public string Name { get { return "json"; } }

        public object ConvertTo(Type toType, object obj)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, toType);
        }

        public T DecodeAs<T>(string text)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(text);
        }

        public T DecodeAs<T>(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] EncodeAsBinary<T>(T value)
        {
            throw new NotImplementedException();
        }

        public string EncodeAsText<T>(T value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
    }*/
}
#endif