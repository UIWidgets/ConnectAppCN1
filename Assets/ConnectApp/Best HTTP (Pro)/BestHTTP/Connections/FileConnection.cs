using System;
using System.Collections.Generic;

using BestHTTP.Extensions;
using BestHTTP.PlatformSupport.FileSystem;

namespace BestHTTP
{
    public sealed class StreamList : System.IO.Stream
    {
        private System.IO.Stream[] Streams;
        private int CurrentIdx;

        public StreamList(params System.IO.Stream[] streams)
        {
            this.Streams = streams;
            this.CurrentIdx = 0;
        }

        public override bool CanRead
        {
            get {
                if (CurrentIdx >= Streams.Length)
                    return false;
                return Streams[CurrentIdx].CanRead;
            }
        }

        public override bool CanSeek { get { return false; } }

        public override bool CanWrite
        {
            get {
                if (CurrentIdx >= Streams.Length)
                    return false;
                return Streams[CurrentIdx].CanWrite;
            }
        }

        public override void Flush()
        {
            if (CurrentIdx >= Streams.Length)
                return;

            // We have to call the flush to all previous streams, as we may advanced the CurrentIdx
            for (int i = 0; i <= CurrentIdx; ++i)
                Streams[i].Flush();
        }

        public override long Length
        {
            get {
                if (CurrentIdx >= Streams.Length)
                    return 0;

                long length = 0;
                for (int i = 0; i < Streams.Length; ++i)
                    length += Streams[i].Length;

                return length;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (CurrentIdx >= Streams.Length)
                return -1;

            int readCount = Streams[CurrentIdx].Read(buffer, offset, count);

            while (readCount < count && CurrentIdx++ < Streams.Length)
            {
                readCount += Streams[CurrentIdx].Read(buffer, offset + readCount, count - readCount);
            }

            return readCount;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (CurrentIdx >= Streams.Length)
                return;

            Streams[CurrentIdx].Write(buffer, offset, count);
        }

        public void Write(string str)
        {
            byte[] bytes = str.GetASCIIBytes();

            this.Write(bytes, 0, bytes.Length);
            VariableSizedBufferPool.Release(bytes);
        }

        protected override void Dispose(bool disposing)
        {
            for (int i = 0; i < Streams.Length; ++i)
            {
                try
                {
                    Streams[i].Dispose();
                }
                catch(Exception ex)
                {
                    HTTPManager.Logger.Exception("StreamList", "Dispose", ex);
                }
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException("Position get");
            }
            set
            {
                throw new NotImplementedException("Position set");
            }
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            if (CurrentIdx >= Streams.Length)
                return 0;

            return Streams[CurrentIdx].Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException("SetLength");
        }
    }

    /*public static class AndroidFileHelper
    {
        // AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        public static Stream GetAPKFileStream(string path)
        {
            UnityEngine.AndroidJavaClass up = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
            UnityEngine.AndroidJavaObject cActivity = up.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");

            UnityEngine.AndroidJavaObject assetManager = cActivity.GetStatic<UnityEngine.AndroidJavaObject>("getAssets");

            return new AndroidInputStream(assetManager.Call<UnityEngine.AndroidJavaObject>("open", path));
        }
    }

    public sealed class AndroidInputStream : Stream
    {
        private UnityEngine.AndroidJavaObject baseStream;

        public override bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public AndroidInputStream(UnityEngine.AndroidJavaObject inputStream)
        {
            this.baseStream = inputStream;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.baseStream.Call<int>("read", buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }*/

    internal sealed class FileConnection : ConnectionBase
    {
        public FileConnection(string serverAddress)
            :base(serverAddress)
        { }

        internal override void Abort(HTTPConnectionStates newState)
        {
            State = newState;

            switch (State)
            {
                case HTTPConnectionStates.TimedOut: TimedOutStart = DateTime.UtcNow; break;
            }

            throw new NotImplementedException();
        }

        protected override void ThreadFunc(object param)
        {
            try
            {
                // Step 1 : create a stream with header information
                // Step 2 : create a stream from the file
                // Step 3 : create a StreamList
                // Step 4 : create a HTTPResponse object
                // Step 5 : call the Receive function of the response object

                using (System.IO.Stream fs = HTTPManager.IOService.CreateFileStream(this.CurrentRequest.CurrentUri.LocalPath, FileStreamModes.Open))
                //using (Stream fs = AndroidFileHelper.GetAPKFileStream(this.CurrentRequest.CurrentUri.LocalPath))
                    using (StreamList stream = new StreamList(new System.IO.MemoryStream(), fs))
                    {
                        // This will write to the MemoryStream
                        stream.Write("HTTP/1.1 200 Ok\r\n");
                        stream.Write("Content-Type: application/octet-stream\r\n");
                        stream.Write("Content-Length: " + fs.Length.ToString() + "\r\n");
                        stream.Write("\r\n");

                        stream.Seek(0, System.IO.SeekOrigin.Begin);

                        base.CurrentRequest.Response = new HTTPResponse(base.CurrentRequest, stream, base.CurrentRequest.UseStreaming, false);

                        if (!CurrentRequest.Response.Receive())
                            CurrentRequest.Response = null;
                    }
            }
            catch(Exception ex)
            {
                if (CurrentRequest != null)
                {
                    // Something gone bad, Response must be null!
                    CurrentRequest.Response = null;

                    switch (State)
                    {
                        case HTTPConnectionStates.AbortRequested:
                            CurrentRequest.State = HTTPRequestStates.Aborted;
                            break;
                        case HTTPConnectionStates.TimedOut:
                            CurrentRequest.State = HTTPRequestStates.TimedOut;
                            break;
                        default:
                            CurrentRequest.Exception = ex;
                            CurrentRequest.State = HTTPRequestStates.Error;
                            break;
                    }
                }
            }
            finally
            {
                State = HTTPConnectionStates.Closed;
                if (CurrentRequest.State == HTTPRequestStates.Processing)
                {
                    if (CurrentRequest.Response != null)
                        CurrentRequest.State = HTTPRequestStates.Finished;
                    else
                        CurrentRequest.State = HTTPRequestStates.Error;
                }
            }
        }
    }
}