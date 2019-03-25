#if !BESTHTTP_DISABLE_SIGNALR_CORE && !BESTHTTP_DISABLE_WEBSOCKET
using System;

namespace BestHTTP.SignalRCore.Messages
{
    public enum MessageTypes : int
    {
        /// <summary>
        /// This is a made up message type, for easier handshake handling.
        /// </summary>
        Handshake  = 0,

        /// <summary>
        /// https://github.com/aspnet/SignalR/blob/dev/specs/HubProtocol.md#invocation-message-encoding
        /// </summary>
        Invocation = 1,

        /// <summary>
        /// https://github.com/aspnet/SignalR/blob/dev/specs/HubProtocol.md#streamitem-message-encoding
        /// </summary>
        StreamItem = 2,

        /// <summary>
        /// https://github.com/aspnet/SignalR/blob/dev/specs/HubProtocol.md#completion-message-encoding
        /// </summary>
        Completion = 3,

        /// <summary>
        /// https://github.com/aspnet/SignalR/blob/dev/specs/HubProtocol.md#streaminvocation-message-encoding
        /// </summary>
        StreamInvocation = 4,

        /// <summary>
        /// https://github.com/aspnet/SignalR/blob/dev/specs/HubProtocol.md#cancelinvocation-message-encoding
        /// </summary>
        CancelInvocation = 5,

        /// <summary>
        /// https://github.com/aspnet/SignalR/blob/dev/specs/HubProtocol.md#ping-message-encoding
        /// </summary>
        Ping = 6,

        /// <summary>
        /// https://github.com/aspnet/SignalR/blob/dev/specs/HubProtocol.md#close-message-encoding
        /// </summary>
        Close = 7
    }

    public struct Message
    {
        public MessageTypes type;
        public string invocationId;
        public bool nonblocking;
        public string target;
        public object[] arguments;
        public object item;
        public object result;
        public string error;

        public override string ToString()
        {
            switch (this.type)
            {
                case MessageTypes.Invocation:
                    return string.Format("[Invocation Id: {0}, Target: '{1}', Argument count: {2}]", this.invocationId, this.target, this.arguments != null ? this.arguments.Length : 0);
                case MessageTypes.StreamItem:
                    return string.Format("[StreamItem Id: {0}, Item: {1}]", this.invocationId, this.item.ToString());
                case MessageTypes.Completion:
                    return string.Format("[Completion Id: {0}, Result: {1}, Error: '{2}']", this.invocationId, this.result, this.error);
                case MessageTypes.StreamInvocation:
                    return string.Format("[StreamInvocation Id: {0}, Target: '{1}', Argument count: {2}]", this.invocationId, this.target, this.arguments != null ? this.arguments.Length : 0);
                case MessageTypes.CancelInvocation:
                    return string.Format("[CancelInvocation Id: {0}]", this.invocationId);
                case MessageTypes.Ping:
                    return "[Ping]";
                case MessageTypes.Close:
                    return string.IsNullOrEmpty(this.error) ? "[Close]" : string.Format("[Close {0}]", this.error);
                default:
                    return "Unknown message! Type: " + this.type;
            }
        }
    }
}
#endif