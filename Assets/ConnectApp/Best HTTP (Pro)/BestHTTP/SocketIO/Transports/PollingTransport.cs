#if !BESTHTTP_DISABLE_SOCKETIO

using System;
using System.Text;

namespace BestHTTP.SocketIO.Transports
{
    internal sealed class PollingTransport : ITransport
    {
        #region Public (ITransport) Properties

        public TransportTypes Type { get { return TransportTypes.Polling; } }
        public TransportStates State { get; private set; }
        public SocketManager Manager { get; private set; }
        public bool IsRequestInProgress { get { return LastRequest != null; } }
        public bool IsPollingInProgress { get { return PollRequest != null; } }

        #endregion

        #region Private Fields

        /// <summary>
        /// The last POST request we sent to the server.
        /// </summary>
        private HTTPRequest LastRequest;

        /// <summary>
        /// Last GET request we sent to the server.
        /// </summary>
        private HTTPRequest PollRequest;

        /// <summary>
        /// The last packet with expected binary attachments
        /// </summary>
        private Packet PacketWithAttachment;

        #endregion

        private enum PayloadTypes : byte
        {
            Text,
            Binary
        }

        public PollingTransport(SocketManager manager)
        {
            Manager = manager;
        }

        public void Open()
        {
            string format = "{0}?EIO={1}&transport=polling&t={2}-{3}{5}";
            if (Manager.Handshake != null)
                format += "&sid={4}";

            bool sendAdditionalQueryParams = !Manager.Options.QueryParamsOnlyForHandshake || (Manager.Options.QueryParamsOnlyForHandshake && Manager.Handshake == null);

            HTTPRequest request = new HTTPRequest(new Uri(string.Format(format,
                                                                        Manager.Uri.ToString(),
                                                                        SocketManager.MinProtocolVersion,
                                                                        Manager.Timestamp.ToString(),
                                                                        Manager.RequestCounter++.ToString(),
                                                                        Manager.Handshake != null ? Manager.Handshake.Sid : string.Empty,
                                                                        sendAdditionalQueryParams ? Manager.Options.BuildQueryParams() : string.Empty)),
                                                OnRequestFinished);

#if !BESTHTTP_DISABLE_CACHING
            // Don't even try to cache it
            request.DisableCache = true;
#endif

            request.DisableRetry = true;

            request.Send();

            State = TransportStates.Opening;
        }

        /// <summary>
        /// Closes the transport and cleans up resources.
        /// </summary>
        public void Close()
        {
            if (State == TransportStates.Closed)
                return;

            State = TransportStates.Closed;

            /*
            if (LastRequest != null)
                LastRequest.Abort();

            if (PollRequest != null)
                PollRequest.Abort();*/
        }

        #region Packet Sending Implementation

        private System.Collections.Generic.List<Packet> lonelyPacketList = new System.Collections.Generic.List<Packet>(1);
        public void Send(Packet packet)
        {
            try
            {
                lonelyPacketList.Add(packet);
                Send(lonelyPacketList);
            }
            finally
            {
                lonelyPacketList.Clear();
            }
        }

        public void Send(System.Collections.Generic.List<Packet> packets)
        {
            if (State != TransportStates.Opening && State != TransportStates.Open)
                return;

            if (IsRequestInProgress)
                throw new Exception("Sending packets are still in progress!");

            byte[] buffer = null;

            try
            {
                buffer = packets[0].EncodeBinary();

                for (int i = 1; i < packets.Count; ++i)
                {
                    byte[] tmpBuffer = packets[i].EncodeBinary();

                    Array.Resize(ref buffer, buffer.Length + tmpBuffer.Length);

                    Array.Copy(tmpBuffer, 0, buffer, buffer.Length - tmpBuffer.Length, tmpBuffer.Length);
                }

                packets.Clear();
            }
            catch (Exception ex)
            {
                (Manager as IManager).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);
                return;
            }

            LastRequest = new HTTPRequest(new Uri(string.Format("{0}?EIO={1}&transport=polling&t={2}-{3}&sid={4}{5}",
                                                                 Manager.Uri.ToString(),
                                                                 SocketManager.MinProtocolVersion,
                                                                 Manager.Timestamp.ToString(),
                                                                 Manager.RequestCounter++.ToString(),
                                                                 Manager.Handshake.Sid,
                                                                 !Manager.Options.QueryParamsOnlyForHandshake ? Manager.Options.BuildQueryParams() : string.Empty)),
                                          HTTPMethods.Post,
                                          OnRequestFinished);


#if !BESTHTTP_DISABLE_CACHING
            // Don't even try to cache it
            LastRequest.DisableCache = true;
#endif

            LastRequest.SetHeader("Content-Type", "application/octet-stream");
            LastRequest.RawData = buffer;

            LastRequest.Send();
        }

        private void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            // Clear out the LastRequest variable, so we can start sending out new packets
            LastRequest = null;

            if (State == TransportStates.Closed)
                return;

            string errorString = null;

            switch (req.State)
            {
                // The request finished without any problem.
                case HTTPRequestStates.Finished:
                    if (HTTPManager.Logger.Level <= BestHTTP.Logger.Loglevels.All)
                        HTTPManager.Logger.Verbose("PollingTransport", "OnRequestFinished: " + resp.DataAsText);

                    if (resp.IsSuccess)
                    {
                        // When we are sending data, the response is an 'ok' string
                        if (req.MethodType != HTTPMethods.Post)
                            ParseResponse(resp);
                    }
                    else
                        errorString = string.Format("Polling - Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}",
                                                        resp.StatusCode,
                                                        resp.Message,
                                                        resp.DataAsText,
                                                        req.CurrentUri);
                    break;

                // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                case HTTPRequestStates.Error:
                    errorString = (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
                    break;

                // The request aborted, initiated by the user.
                case HTTPRequestStates.Aborted:
                    errorString = string.Format("Polling - Request({0}) Aborted!", req.CurrentUri);
                    break;

                // Connecting to the server is timed out.
                case HTTPRequestStates.ConnectionTimedOut:
                    errorString = string.Format("Polling - Connection Timed Out! Uri: {0}", req.CurrentUri);
                    break;

                // The request didn't finished in the given time.
                case HTTPRequestStates.TimedOut:
                    errorString = string.Format("Polling - Processing the request({0}) Timed Out!", req.CurrentUri);
                    break;
            }

            if (!string.IsNullOrEmpty(errorString))
                (Manager as IManager).OnTransportError(this, errorString);
        }

        #endregion

        #region Polling Implementation

        public void Poll()
        {
            if (PollRequest != null || State == TransportStates.Paused)
                return;

            PollRequest = new HTTPRequest(new Uri(string.Format("{0}?EIO={1}&transport=polling&t={2}-{3}&sid={4}{5}",
                                                                Manager.Uri.ToString(),
                                                                SocketManager.MinProtocolVersion,
                                                                Manager.Timestamp.ToString(),
                                                                Manager.RequestCounter++.ToString(),
                                                                Manager.Handshake.Sid,
                                                                !Manager.Options.QueryParamsOnlyForHandshake ? Manager.Options.BuildQueryParams() : string.Empty)),
                                        HTTPMethods.Get,
                                        OnPollRequestFinished);

#if !BESTHTTP_DISABLE_CACHING
            // Don't even try to cache it
            PollRequest.DisableCache = true;
#endif

            PollRequest.DisableRetry = true;

            PollRequest.Send();
        }

        private void OnPollRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            // Clear the PollRequest variable, so we can start a new poll.
            PollRequest = null;

            if (State == TransportStates.Closed)
                return;

            string errorString = null;

            switch (req.State)
            {
                // The request finished without any problem.
                case HTTPRequestStates.Finished:

                    if (HTTPManager.Logger.Level <= BestHTTP.Logger.Loglevels.All)
                        HTTPManager.Logger.Verbose("PollingTransport", "OnPollRequestFinished: " + resp.DataAsText);

                    if (resp.IsSuccess)
                        ParseResponse(resp);
                    else
                        errorString = string.Format("Polling - Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}",
                                                            resp.StatusCode,
                                                            resp.Message,
                                                            resp.DataAsText,
                                                            req.CurrentUri);
                    break;

                // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                case HTTPRequestStates.Error:
                    errorString = req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception";
                    break;

                // The request aborted, initiated by the user.
                case HTTPRequestStates.Aborted:
                    errorString = string.Format("Polling - Request({0}) Aborted!", req.CurrentUri);
                    break;

                // Connecting to the server is timed out.
                case HTTPRequestStates.ConnectionTimedOut:
                    errorString = string.Format("Polling - Connection Timed Out! Uri: {0}", req.CurrentUri);
                    break;

                // The request didn't finished in the given time.
                case HTTPRequestStates.TimedOut:
                    errorString = string.Format("Polling - Processing the request({0}) Timed Out!", req.CurrentUri);
                    break;
            }

            if (!string.IsNullOrEmpty(errorString))
                (Manager as IManager).OnTransportError(this, errorString);
        }

        #endregion

        #region Packet Parsing and Handling

        /// <summary>
        /// Preprocessing and sending out packets to the manager.
        /// </summary>
        private void OnPacket(Packet packet)
        {
            if (packet.AttachmentCount != 0 && !packet.HasAllAttachment)
            {
                PacketWithAttachment = packet;
                return;
            }

            switch (packet.TransportEvent)
            {
                case TransportEventTypes.Open:
                    if (this.State != TransportStates.Opening)
                        HTTPManager.Logger.Warning("PollingTransport", "Received 'Open' packet while state is '" + State.ToString() + "'");
                    else
                        State = TransportStates.Open;
                    goto default;

                case TransportEventTypes.Message:
                  if (packet.SocketIOEvent == SocketIOEventTypes.Connect) //2:40
                    this.State = TransportStates.Open;
                  goto default;

                default:
                    (Manager as IManager).OnPacket(packet);
                    break;
            }
        }

        /// <summary>
        /// Will parse the response, and send out the parsed packets.
        /// </summary>
        private void ParseResponse(HTTPResponse resp)
        {
            try
            {
                if (resp != null && resp.Data != null && resp.Data.Length >= 1)
                {

// 1.x
//00000000  00 09 07 ff 30 7b 22 73 69 64 22 3a 22 6f 69 48       0{"sid":"oiH
//00000010  34 31 33 73 61 49 4e 52 53 67 37 41 4b 41 41 41   413saINRSg7AKAAA
//00000020  41 22 2c 22 75 70 67 72 61 64 65 73 22 3a 5b 22   A","upgrades":["
//00000030  77 65 62 73 6f 63 6b 65 74 22 5d 2c 22 70 69 6e   websocket"],"pin
//00000040  67 49 6e 74 65 72 76 61 6c 22 3a 32 35 30 30 30   gInterval":25000
//00000050  2c 22 70 69 6e 67 54 69 6d 65 6f 75 74 22 3a 36   ,"pingTimeout":6
//00000060  30 30 30 30 7d                                    0000}           

// 2.x
//00000000  39 37 3a 30 7b 22 73 69 64 22 3a 22 73 36 62 5a   97:0{"sid":"s6bZ
//00000010  6c 43 37 66 51 59 6b 4f 46 4f 62 35 41 41 41 41   lC7fQYkOFOb5AAAA
//00000020  22 2c 22 75 70 67 72 61 64 65 73 22 3a 5b 22 77   ","upgrades":["w
//00000030  65 62 73 6f 63 6b 65 74 22 5d 2c 22 70 69 6e 67   ebsocket"],"ping
//00000040  49 6e 74 65 72 76 61 6c 22 3a 32 35 30 30 30 2c   Interval":25000,
//00000050  22 70 69 6e 67 54 69 6d 65 6f 75 74 22 3a 36 30   "pingTimeout":60
//00000060  30 30 30 7d 32 3a 34 30                           000}2:40        

                    int idx = 0;

                    while (idx < resp.Data.Length)
                    {
                        PayloadTypes type = PayloadTypes.Text;
                        int length = 0;

                        if (resp.Data[idx] < '0') {
                          type = (PayloadTypes)resp.Data[idx++];

                          byte num = resp.Data[idx++];
                          while (num != 0xFF) {
                            length = (length * 10) + num;
                            num = resp.Data[idx++];
                          }
                        }
                        else {
                          byte next = resp.Data[idx++];
                          while (next != ':') {
                            length = (length * 10) + (next - '0');
                            next = resp.Data[idx++];
                          } 
                        }

                        Packet packet = null;
                        switch(type)
                        {
                            case PayloadTypes.Text:
                                packet = new Packet(Encoding.UTF8.GetString(resp.Data, idx, length));
                                break;
                            case PayloadTypes.Binary:
                                if (PacketWithAttachment != null)
                                {
                                    // First byte is the packet type. We can skip it, so we advance our idx and we also have
                                    // to decrease length
                                    idx++;
                                    length--;

                                    byte[] buffer = new byte[length];
                                    Array.Copy(resp.Data, idx, buffer, 0, length);

                                    PacketWithAttachment.AddAttachmentFromServer(buffer, true);

                                    if (PacketWithAttachment.HasAllAttachment)
                                    {
                                        packet = PacketWithAttachment;
                                        PacketWithAttachment = null;
                                    }
                                }
                                break;
                        } // switch

                        if (packet != null)
                        {
                            try
                            {
                                OnPacket(packet);
                            }
                            catch (Exception ex)
                            {
                                HTTPManager.Logger.Exception("PollingTransport", "ParseResponse - OnPacket", ex);
                                (Manager as IManager).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);
                            }
                        }

                        idx += length;

                    }// while
                }
            }
            catch (Exception ex)
            {
                (Manager as IManager).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);

                HTTPManager.Logger.Exception("PollingTransport", "ParseResponse", ex);
            }
        }

        #endregion
    }
}

#endif