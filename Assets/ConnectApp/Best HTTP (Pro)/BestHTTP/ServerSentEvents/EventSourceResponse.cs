#if !BESTHTTP_DISABLE_SERVERSENT_EVENTS && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using System.IO;
using System.Threading;

using System.Text;
using System.Collections.Generic;

namespace BestHTTP.ServerSentEvents
{
    /// <summary>
    /// A low-level class to receive and parse an EventSource(http://www.w3.org/TR/eventsource/) stream.
    /// Higher level protocol representation is implemented in the EventSource class.
    /// </summary>
    public sealed class EventSourceResponse : HTTPResponse, IProtocol
    {
        public bool IsClosed { get; private set; }

        #region Public Events

        public Action<EventSourceResponse, BestHTTP.ServerSentEvents.Message> OnMessage;
        public Action<EventSourceResponse> OnClosed;

        #endregion

        #region Privates

        /// <summary>
        /// Thread sync object
        /// </summary>
        private object FrameLock = new object();

        /// <summary>
        /// Buffer for the read data.
        /// </summary>
        private byte[] LineBuffer;

        /// <summary>
        /// Buffer position.
        /// </summary>
        private int LineBufferPos = 0;

        /// <summary>
        /// The currently receiving and parsing message
        /// </summary>
        private BestHTTP.ServerSentEvents.Message CurrentMessage;

        /// <summary>
        /// Completed messages that waiting to be dispatched
        /// </summary>
        private List<BestHTTP.ServerSentEvents.Message> CompletedMessages = new List<BestHTTP.ServerSentEvents.Message>();

        #endregion

        public EventSourceResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache)
            :base(request, stream, isStreamed, isFromCache)
        {
            base.IsClosedManually = true;
        }

        public override bool Receive(int forceReadRawContentLength = -1, bool readPayloadData = true)
        {
            bool received = base.Receive(forceReadRawContentLength, false);

            string contentType = this.GetFirstHeaderValue("content-type");
            base.IsUpgraded = received &&
                              this.StatusCode == 200 &&
                              !string.IsNullOrEmpty(contentType) &&
                              contentType.ToLower().StartsWith("text/event-stream");

            // If we didn't upgraded to the protocol we have to read all the sent payload because
            // next requests may read these datas as HTTP headers and will fail
            if (!IsUpgraded)
                ReadPayload(forceReadRawContentLength);

            return received;
        }

        internal void StartReceive()
        {
            if (IsUpgraded)
            {
#if NETFX_CORE
                #pragma warning disable 4014
                    Windows.System.Threading.ThreadPool.RunAsync(ReceiveThreadFunc);
                #pragma warning restore 4014
#else
                ThreadPool.QueueUserWorkItem(ReceiveThreadFunc);
                //new Thread(ReceiveThreadFunc)
                //    .Start();
#endif
            }
        }

        #region Private Threading Functions

        private void ReceiveThreadFunc(object param)
        {
            try
            {
                if (HasHeaderWithValue("transfer-encoding", "chunked"))
                    ReadChunked(Stream);
                else
                    ReadRaw(Stream, -1);
            }
#if !NETFX_CORE
            catch (ThreadAbortException)
            {
                this.baseRequest.State = HTTPRequestStates.Aborted;
            }
#endif
            catch (Exception e)
            {
                if (HTTPUpdateDelegator.IsCreated)
                {
                    this.baseRequest.Exception = e;
                    this.baseRequest.State = HTTPRequestStates.Error;
                }
                else
                    this.baseRequest.State = HTTPRequestStates.Aborted;
            }
            finally
            {
                IsClosed = true;
            }
        }

        #endregion

        #region Read Implementations

        // http://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.6.1
        private new void ReadChunked(Stream stream)
        {
            int chunkLength = ReadChunkLength(stream);
            byte[] buffer = Extensions.VariableSizedBufferPool.Get(chunkLength, true);

            while (chunkLength != 0)
            {
                // To avoid more GC garbage we use only one buffer, and resize only if the next chunk doesn't fit.
                if (buffer.Length < chunkLength)
                    Extensions.VariableSizedBufferPool.Resize(ref buffer, chunkLength, true);

                int readBytes = 0;

                // Fill up the buffer
                do
                {
                    int bytes = stream.Read(buffer, readBytes, chunkLength - readBytes);
                    if (bytes == 0)
                        throw new Exception("The remote server closed the connection unexpectedly!");

                    readBytes += bytes;
                } while (readBytes < chunkLength);

                FeedData(buffer, readBytes);

                // Every chunk data has a trailing CRLF
                ReadTo(stream, LF);

                // read the next chunk's length
                chunkLength = ReadChunkLength(stream);
            }

            Extensions.VariableSizedBufferPool.Release(buffer);

            // Read the trailing headers or the CRLF
            ReadHeaders(stream);
        }

        private new void ReadRaw(Stream stream, long contentLength)
        {
            byte[] buffer = Extensions.VariableSizedBufferPool.Get(1024, true);
            int bytes;

            do
            {
                bytes = stream.Read(buffer, 0, buffer.Length);

                FeedData(buffer, bytes);
            } while(bytes > 0);

            Extensions.VariableSizedBufferPool.Release(buffer);
        }

        #endregion

        #region Data Parsing

        public void FeedData(byte[] buffer, int count)
        {
            if (count == -1)
                count = buffer.Length;

            if (count == 0)
                return;

            if (LineBuffer == null)
                LineBuffer = Extensions.VariableSizedBufferPool.Get(1024, true);

            int newlineIdx;
            int pos = 0;

            do {

                newlineIdx = -1;
                int skipCount = 1; // to skip CR and/or LF

                for (int i = pos; i < count && newlineIdx == -1; ++i)
                {
                    // Lines must be separated by either a U+000D CARRIAGE RETURN U+000A LINE FEED (CRLF) character pair, a single U+000A LINE FEED (LF) character, or a single U+000D CARRIAGE RETURN (CR) character.
                    if (buffer[i] == HTTPResponse.CR)
                    {
                        if (i + 1 < count && buffer[i + 1] == HTTPResponse.LF)
                            skipCount = 2;
                        newlineIdx = i;
                    }
                    else if (buffer[i] == HTTPResponse.LF)
                        newlineIdx = i;
                }

                int copyIndex = newlineIdx == -1 ? count : newlineIdx;

                if (LineBuffer.Length < LineBufferPos + (copyIndex - pos))
                {
                    int newSize = LineBufferPos + (copyIndex - pos);
                    Extensions.VariableSizedBufferPool.Resize(ref LineBuffer, newSize, true);
                }

                Array.Copy(buffer, pos, LineBuffer, LineBufferPos, copyIndex - pos);

                LineBufferPos += copyIndex - pos;

                if (newlineIdx == -1)
                    return;

                ParseLine(LineBuffer, LineBufferPos);

                LineBufferPos = 0;
                //pos += newlineIdx + skipCount;
                pos = newlineIdx + skipCount;

            }while(newlineIdx != -1 && pos < count);
        }

        void ParseLine(byte[] buffer, int count)
        {
            // If the line is empty (a blank line) => Dispatch the event
            if (count == 0)
            {
                if (CurrentMessage != null)
                {
                    lock (FrameLock)
                        CompletedMessages.Add(CurrentMessage);
                    CurrentMessage = null;
                }

                return;
            }

            // If the line starts with a U+003A COLON character (:) => Ignore the line.
            if (buffer[0] == 0x3A)
                return;

            //If the line contains a U+003A COLON character (:)
            int colonIdx = -1;
            for (int i = 0; i < count && colonIdx == -1; ++i)
                if (buffer[i] == 0x3A)
                    colonIdx = i;

            string field;
            string value;

            if (colonIdx != -1)
            {
                // Collect the characters on the line before the first U+003A COLON character (:), and let field be that string.
                field = Encoding.UTF8.GetString(buffer, 0, colonIdx);

                //Collect the characters on the line after the first U+003A COLON character (:), and let value be that string. If value starts with a U+0020 SPACE character, remove it from value.
                if (colonIdx + 1 < count && buffer[colonIdx + 1] == 0x20)
                    colonIdx++;

                colonIdx++;

                // discarded because it is not followed by a blank line
                if (colonIdx >= count)
                    return;

                value = Encoding.UTF8.GetString(buffer, colonIdx, count - colonIdx);
            }
            else
            {
                // Otherwise, the string is not empty but does not contain a U+003A COLON character (:) =>
                //      Process the field using the whole line as the field name, and the empty string as the field value.
                field = Encoding.UTF8.GetString(buffer, 0, count);
                value = string.Empty;
            }

            if (CurrentMessage == null)
                CurrentMessage = new BestHTTP.ServerSentEvents.Message();

            switch(field)
            {
                // If the field name is "id" => Set the last event ID buffer to the field value.
                case "id":
                    CurrentMessage.Id = value;
                    break;

                // If the field name is "event" => Set the event type buffer to field value.
                case "event":
                    CurrentMessage.Event = value;
                    break;

                // If the field name is "data" => Append the field value to the data buffer, then append a single U+000A LINE FEED (LF) character to the data buffer.
                case "data":
                    // Append a new line if we already have some data. This way we can skip step 3.) in the EventSource's OnMessageReceived.
                    // We do only null check, because empty string can be valid payload
                    if (CurrentMessage.Data != null)
                        CurrentMessage.Data += Environment.NewLine;

                    CurrentMessage.Data += value;
                    break;

                // If the field name is "retry" => If the field value consists of only ASCII digits, then interpret the field value as an integer in base ten,
                //  and set the event stream's reconnection time to that integer. Otherwise, ignore the field.
                case "retry":
                    int result;
                    if (int.TryParse(value, out result))
                        CurrentMessage.Retry = TimeSpan.FromMilliseconds(result);
                    break;

                // Otherwise: The field is ignored.
                default:
                    break;
            }
        }

        #endregion

        void IProtocol.HandleEvents()
        {
            lock(FrameLock)
            {
                // Send out messages.
                if (CompletedMessages.Count > 0)
                {
                    if (OnMessage != null)
                        for (int i = 0; i < CompletedMessages.Count; ++i)
                        {
                            try
                            {
                                OnMessage(this, CompletedMessages[i]);
                            }
                            catch(Exception ex)
                            {
                                HTTPManager.Logger.Exception("EventSourceMessage", "HandleEvents - OnMessage", ex);
                            }
                        }

                    CompletedMessages.Clear();
                }
            }

            // We are closed
            if (IsClosed)
            {
                CompletedMessages.Clear();

                if (OnClosed != null)
                {
                    try
                    {
                        OnClosed(this);
                    }
                    catch (Exception ex)
                    {
                        HTTPManager.Logger.Exception("EventSourceMessage", "HandleEvents - OnClosed", ex);
                    }
                    finally
                    {
                        OnClosed = null;
                    }
                }
            }
        }
    }
}

#endif