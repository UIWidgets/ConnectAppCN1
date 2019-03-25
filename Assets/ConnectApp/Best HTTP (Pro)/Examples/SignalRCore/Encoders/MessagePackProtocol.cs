#if !BESTHTTP_DISABLE_SIGNALR_CORE

using System;
using System.Collections.Generic;
using BestHTTP.SignalRCore.Messages;

namespace BestHTTP.SignalRCore.Encoders
{
    public sealed class MessagePackEncoder : BestHTTP.SignalRCore.IEncoder
    {
        public string Name { get { return "messagepack"; } }

        public object ConvertTo(Type toType, object obj)
        {
            throw new NotImplementedException();
        }

        public T DecodeAs<T>(string text)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }

    public sealed class MessagePackProtocol : BestHTTP.SignalRCore.IProtocol
    {
        public TransferModes Type { get { return TransferModes.Binary; } }
        public IEncoder Encoder { get; private set; }
        public HubConnection Connection { get; set; }

        public MessagePackProtocol()
        {
            this.Encoder = new MessagePackEncoder();
        }

        /// <summary>
        /// Convert a value to the given type.
        /// </summary>
        public object ConvertTo(Type toType, object obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function must return the encoded representation of the given message.
        /// </summary>
        public byte[] EncodeMessage(Message message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function must convert all element in the arguments array to the corresponding type from the argTypes array.
        /// </summary>
        public object[] GetRealArguments(Type[] argTypes, object[] arguments)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function must parse binary representation of the messages into the list of Messages.
        /// </summary>
        public void ParseMessages(string data, ref List<Message> messages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function must parse textual representation of the messages into the list of Messages.
        /// </summary>
        public void ParseMessages(byte[] data, ref List<Message> messages)
        {
            throw new NotImplementedException();
        }
    }
}

#endif