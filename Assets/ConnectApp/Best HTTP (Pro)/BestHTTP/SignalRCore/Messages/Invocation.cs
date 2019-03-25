#if !BESTHTTP_DISABLE_SIGNALR_CORE && !BESTHTTP_DISABLE_WEBSOCKET
using System;

namespace BestHTTP.SignalRCore.Messages
{
    public struct InvocationMessage
    {
        public MessageTypes type;
        public string invocationId;
        public bool nonblocking;
        public string target;
        public object[] arguments;
    }

    public struct CancelInvocationMessage
    {
        public MessageTypes type { get { return MessageTypes.CancelInvocation; } }
        public string invocationId;
    }

    public struct PingMessage
    {
        public MessageTypes type { get { return MessageTypes.Ping; } }
    }
}
#endif