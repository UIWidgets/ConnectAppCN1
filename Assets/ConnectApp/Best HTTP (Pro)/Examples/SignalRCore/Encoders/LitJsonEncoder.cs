#if !BESTHTTP_DISABLE_SIGNALR_CORE && !BESTHTTP_DISABLE_WEBSOCKET
using System;

namespace BestHTTP.SignalRCore.Encoders
{
    public sealed class LitJsonEncoder : BestHTTP.SignalRCore.IEncoder
    {
        public string Name { get { return "json"; } }

        public LitJsonEncoder()
        {
            LitJson.JsonMapper.RegisterImporter<int, long>((input) => input);
            LitJson.JsonMapper.RegisterImporter<long, int>((input) => (int)input);
            LitJson.JsonMapper.RegisterImporter<double, int>((input) => (int)(input + 0.5));
            LitJson.JsonMapper.RegisterImporter<string, DateTime>((input) => Convert.ToDateTime((string)input).ToUniversalTime());
            LitJson.JsonMapper.RegisterImporter<double, float>((input) => (float)input);
            LitJson.JsonMapper.RegisterImporter<string, byte[]>((input) => Convert.FromBase64String(input));
        }

        public T DecodeAs<T>(string text)
        {
            return LitJson.JsonMapper.ToObject<T>(text);
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
            return LitJson.JsonMapper.ToJson(value);
        }

        public object ConvertTo(Type toType, object obj)
        {
            string json = LitJson.JsonMapper.ToJson(obj);
            return LitJson.JsonMapper.ToObject(toType, json);
        }
    }
}

#endif