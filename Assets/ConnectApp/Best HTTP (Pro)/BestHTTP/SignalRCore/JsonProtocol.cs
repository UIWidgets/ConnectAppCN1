#if !BESTHTTP_DISABLE_SIGNALR_CORE && !BESTHTTP_DISABLE_WEBSOCKET

using BestHTTP.SignalRCore.Messages;
using System;
using System.Collections.Generic;

#if NETFX_CORE || NET_4_6
using System.Reflection;
#endif

namespace BestHTTP.SignalRCore
{
    public interface IProtocol
    {
        TransferModes Type { get; }
        IEncoder Encoder { get; }
        HubConnection Connection { get; set; }

        /// <summary>
        /// This function must parse textual representation of the messages into the list of Messages.
        /// </summary>
        void ParseMessages(string data, ref List<Message> messages);

        /// <summary>
        /// This function must parse binary representation of the messages into the list of Messages.
        /// </summary>
        void ParseMessages(byte[] data, ref List<Message> messages);

        /// <summary>
        /// This function must return the encoded representation of the given message.
        /// </summary>
        byte[] EncodeMessage(Message message);

        /// <summary>
        /// This function must convert all element in the arguments array to the corresponding type from the argTypes array.
        /// </summary>
        object[] GetRealArguments(Type[] argTypes, object[] arguments);

        /// <summary>
        /// Convert a value to the given type.
        /// </summary>
        object ConvertTo(Type toType, object obj);
    }

    public sealed class JsonProtocol : IProtocol
    {
        public const char Separator = (char)0x1E;

        public TransferModes Type { get { return TransferModes.Text; } }
        public IEncoder Encoder { get; private set; }
        public HubConnection Connection { get; set; }

        public JsonProtocol(IEncoder encoder)
        {
            if (encoder == null)
                throw new ArgumentNullException("encoder");
            if (encoder.Name != "json")
                throw new ArgumentException("Encoder must be a json encoder!");

            this.Encoder = encoder;
        }

        public void ParseMessages(string data, ref List<Message> messages)
        {
            int from = 0;
            int separatorIdx = data.IndexOf(JsonProtocol.Separator);
            if (separatorIdx == -1)
                throw new Exception("Missing separator!");

            while (separatorIdx != -1)
            {
                string sub = data.Substring(from, separatorIdx - from);

                var message = this.Encoder.DecodeAs<Message>(sub);

                messages.Add(message);

                from = separatorIdx + 1;
                separatorIdx = data.IndexOf(JsonProtocol.Separator, from);
            }
        }

        public void ParseMessages(byte[] data, ref List<Message> messages) { }
        
        public byte[] EncodeMessage(Message message)
        {
            string json = null;
            switch (message.type)
            {
                case MessageTypes.Invocation:
                case MessageTypes.StreamInvocation:
                    // While message contains all informations already, the spec states that no additional field are allowed in messages
                    //  So we are creating 'specialized' messages here to send to the server.
                    json = this.Encoder.EncodeAsText<InvocationMessage>(new InvocationMessage()
                    {
                        type = message.type,
                        invocationId = message.invocationId,
                        nonblocking = message.nonblocking,
                        target = message.target,
                        arguments = message.arguments
                    });
                    break;

                case MessageTypes.CancelInvocation:
                    json = this.Encoder.EncodeAsText<CancelInvocationMessage>(new CancelInvocationMessage()
                    {
                        invocationId = message.invocationId
                    });
                    break;

                case MessageTypes.Ping:
                    json = this.Encoder.EncodeAsText<PingMessage>(new PingMessage());
                    break;
            }

            return !string.IsNullOrEmpty(json) ? JsonProtocol.WithSeparator(json) : null;
        }

        public object[] GetRealArguments(Type[] argTypes, object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
                return null;

            if (argTypes.Length > arguments.Length)
                throw new Exception(string.Format("argType.Length({0}) < arguments.length({1})", argTypes.Length, arguments.Length));

            object[] realArgs = new object[arguments.Length];

            for (int i = 0; i < arguments.Length; ++i)
                realArgs[i] = ConvertTo(argTypes[i], arguments[i]);

            return realArgs;
        }

        public object ConvertTo(Type toType, object obj)
        {
            if (obj == null)
                return null;
#if NETFX_CORE //|| NET_4_6
            if (toType.GetTypeInfo().IsPrimitive || toType.GetTypeInfo().IsEnum)
#else
            if (toType.IsPrimitive || toType.IsEnum)
#endif
                return Convert.ChangeType(obj, toType);

            if (toType == typeof(string))
                return obj.ToString();

            return this.Encoder.ConvertTo(toType, obj);
        }

        /// <summary>
        /// Returns the given string parameter's bytes with the added separator(0x1E).
        /// </summary>
        public static byte[] WithSeparator(string str)
        {
            int len = System.Text.Encoding.UTF8.GetByteCount(str);

            byte[] buffer = new byte[len + 1];

            System.Text.Encoding.UTF8.GetBytes(str, 0, str.Length, buffer, 0);

            buffer[len] = 0x1e;

            return buffer;
        }
    }
}
#endif