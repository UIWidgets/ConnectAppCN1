#if !BESTHTTP_DISABLE_SIGNALR_CORE && !BESTHTTP_DISABLE_WEBSOCKET
using System;
using System.Collections.Generic;

namespace BestHTTP.SignalRCore
{
    public enum TransportTypes
    {
        WebSocket
    }

    public enum TransferModes
    {
        Binary,
        Text
    }

    public enum TransportStates
    {
        Initial,
        Connecting,
        Connected,
        Closing,
        Failed,
        Closed
    }

    /// <summary>
    /// Possible states of a HubConnection
    /// </summary>
    public enum ConnectionStates
    {
        Initial,
        Authenticating,
        Negotiating,
        Redirected,
        Connected,
        CloseInitiated,
        Closed
    }

    public interface ITransport
    {
        TransferModes TransferMode { get; }
        TransportTypes TransportType { get; }
        TransportStates State { get; }

        string ErrorReason { get; }

        event Action<TransportStates, TransportStates> OnStateChanged;

        void StartConnect();
        void StartClose();

        void Send(byte[] msg);
    }

    public interface IEncoder
    {
        string Name { get; }

        string EncodeAsText<T>(T value);
        T DecodeAs<T>(string text);

        byte[] EncodeAsBinary<T>(T value);
        T DecodeAs<T>(byte[] data);

        object ConvertTo(Type toType, object obj);
    }

    public class StreamItemContainer<T>
    {
        public readonly long id;

        public List<T> Items { get; private set; }
        public T LastAdded { get; private set; }

        //public int newIdx;
        //public int newCount;

        public bool IsCanceled;

        public StreamItemContainer(long _id)
        {
            this.id = _id;
            this.Items = new List<T>();
        }

        public void AddItem(T item)
        {
            if (this.Items == null)
                this.Items = new List<T>();

            //this.newIdx = this.Items.Count;
            //this.newCount = 1;
            this.Items.Add(item);
            this.LastAdded = item;
        }

        //public void AddItems(T[] items)
        //{
        //    if (this.Items == null)
        //        this.Items = new List<T>();
        //
        //    this.newIdx = this.Items.Count;
        //    this.newCount = items.Length;
        //
        //    this.Items.AddRange(items);
        //}
    }

    struct CallbackDescriptor
    {
        public readonly Type[] ParamTypes;
        public readonly Action<object[]> Callback;
        public CallbackDescriptor(Type[] paramTypes, Action<object[]> callback)
        {
            this.ParamTypes = paramTypes;
            this.Callback = callback;
        }
    }

    internal sealed class Subscription
    {
        public List<CallbackDescriptor> callbacks = new List<CallbackDescriptor>(1);

        public void Add(Type[] paramTypes, Action<object[]> callback)
        {
            lock(callbacks)
                this.callbacks.Add(new CallbackDescriptor(paramTypes, callback));
        }

        public void Remove(Action<object[]> callback)
        {
            lock (callbacks)
            {
                int idx = -1;
                for (int i = 0; i < this.callbacks.Count && idx == -1; ++i)
                    if (this.callbacks[i].Callback == callback)
                        idx = i;

                if (idx != -1)
                    this.callbacks.RemoveAt(idx);
            }
        }
    }
}
#endif