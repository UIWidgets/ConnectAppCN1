using System;
using System.Collections.Generic;
using System.Text;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using Newtonsoft.Json;
using UnityEngine;

namespace ConnectApp.Utils {

    enum GatewayState {
        CONNECTING,
        OPEN,
        CLOSED
    }

    static class SocketGatewayUtils {
        public const int OPCODE_DISPATCH = 0;
        public const int OPCODE_IDENTIFY = 1;
        public const int OPCODE_RESUME = 2;
        public const int OPCODE_STATUS_UPDATE = 3;
        public const int OPCODE_PING = 9;

        public const int heartBeatInterval = 15000;
    }
    
    public class SocketGateway {
        static SocketGateway m_Instance;

        public static SocketGateway instance {
            get {
                if (m_Instance == null) {
                    Debug.Log("fatal error: socket gateway has not been initialized yet !");
                    return null;
                }

                return m_Instance;
            }
        }
        
        List<string> m_CandidateURLs;

        List<string> m_PayloadQueue;

        GatewayState readyState;

        WebSocketHost m_Host;

        WebSocket m_Socket;

        BackOff backoff;

        Action<string> onConnect;
        Action<string, SocketResponseDataBase> onMessage;

        string sessionId;
        string m_GatewayUrl;
        string m_CommitId;
        int seq;

        bool _closeRequired;

        int _heartBeater;

        bool connected {
            get { return this.readyState == GatewayState.OPEN; }
        }

        public bool readyForConnect {
            get { return this.readyState == GatewayState.CLOSED; }
        }

        bool resumable {
            get { return this.seq > 0; }
        }

        public SocketGateway(WebSocketHost host) {
            if (m_Instance != null) {
                Debug.Assert(false, "fatal error: duplicated socket gateways is not allowed !");
                return;
            }

            m_Instance = this;
            this.m_Host = host;
            this.m_CommitId = null;
            
            this.m_CandidateURLs = new List<string>();
            this.m_PayloadQueue = new List<string>();
            this.backoff = new BackOff(host, 1000, 60000);
            this._heartBeater = -1;
            this._Close();
        }


        int OnFail(bool reset = true) {
            this._Close(reset);
            this.backoff.Cancel();
            int delay = this.backoff.OnFail(() => {
                this.Connect(this.onConnect, this.onMessage, true);
            });
            return delay;
        }

        bool _sslHandShakeError = false;

        public void Connect(Action<string> onConnected, Action<string, SocketResponseDataBase> onMessage, bool reconnect = false) {
            if (this.readyState != GatewayState.CLOSED) {
                Debug.Log("fatal error: cannot Connect to WS when the current connection is still alive");
                return;
            }

            if (!reconnect) {
                this.onConnect = onConnected;
                this.onMessage = onMessage;
                this._closeRequired = false;
            }
            else {
                Debug.Assert(this.onConnect != null, "fatal error: reconnect before initial connect !");
                Debug.Assert(this.onMessage != null, "fatal error: reconnect before initial connect !");
            }

            this.readyState = GatewayState.CONNECTING;

            if (this.backoff.pending && !this.resumable) {
                //TODO: notify reconnecting
            }
            
            this.backoff.Cancel();
            
            this._SelectGateway(url => {
                if (url != null) {
                    this._CreateWebSocket(url, 
                        OnConnected: () => {
                            Debug.Assert(this.m_CommitId != null, "fatal error: commit Id is not correctly set before connection!");
                            this.readyState = GatewayState.OPEN;
                            if (!this._Resume()) {
                                onConnected.Invoke(this.m_CommitId);
                            }
                        },
                        OnMessage: (type, data) => {
                            this.backoff.OnSucceed();
                            onMessage?.Invoke(type, data);
                        },
                        OnClose: code => {
                            
                            //https://github.com/sta/websocket-sharp/issues/219
                            var sslProtocolHack = (System.Security.Authentication.SslProtocols)(WebSocket.SslProtocolsHack.Tls12 | WebSocket.SslProtocolsHack.Tls11 | WebSocket.SslProtocolsHack.Tls);
                            //TlsHandshakeFailure
                            if (code == 1015 && this.m_Socket.checkSslProtocolHackFlag(sslProtocolHack)) {
                                this._sslHandShakeError = true;
                            }
                            
                            Debug.Log("OnClose");
                            this.m_Socket.Close();
                            this.m_Socket = null;
                            this._Close();
                            
                            bool reset = !this.resumable || this.backoff.fail > 0;
                            var delay = this.OnFail(reset) / 1000f;
                            
                            Debug.Log($"connection failed, retry in {delay} seconds");
                            return reset;
                        });
                }
                else {
                    var delay = this.OnFail();
                    Debug.Log($"gateway discovery failed, retry in {delay} seconds");
                }
            });
        }


        public void Close() {
            this._closeRequired = true;
            if (this.m_Socket != null) {
                this.m_Socket.Close();
                this._Close();
            }

            this.m_Socket = null;
        }

        void Reconnect() {
            if (this.backoff.pending) {
                this.Connect(this.onConnect, this.onMessage, true);
            }
        }


        void _SelectGateway(Action<string> callback, bool sticky = true) {
            if (sticky && this.m_GatewayUrl != null && this.backoff.fail < 3) {
                callback(this.m_GatewayUrl);
                return;
            }
            
            if (this.m_CandidateURLs.Count > 0) {
                var url = this.m_CandidateURLs[0];
                this.m_CandidateURLs.RemoveAt(0);
                this.m_GatewayUrl = $"{url}/v1";
                callback(this.m_GatewayUrl);
                return;
            }

            var requestUrl = $"{Config.apiAddress}/api/socketgw";
            var request = HttpManager.GET(requestUrl, null);
            
            //Debug.Log("request url = " + requestUrl);
            
            HttpManager.resumeAll(request).Then(responseContent => {
                var responseText = responseContent.text;
                var header = responseContent.headers;
                this.m_CommitId = header["X-Last-Commmit-Hash"];
                
                var socketGatewayResponse = JsonConvert.DeserializeObject<SocketGatewayResponse>(responseText);
                this.m_CandidateURLs = socketGatewayResponse.urls ?? new List<string> { socketGatewayResponse.url };
                
                //TEST INVALID URL:
                //this.m_CandidateURLs.Insert(0, "wss://connect-test-gw.unity.com");
                this._SelectGateway(callback, false);
            }).Catch(exception => {
                callback(null);
            });
        }

        
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) - 
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
        

        public void Ping() {
            var requestPayload = new SocketRequestPayload {
                op = SocketGatewayUtils.OPCODE_PING,
                d = new SocketPingRequest {
                    ts = DateTimeToUnixTimestamp(DateTime.Now)
                }
            };
            
            this._Send(requestPayload);
        }
        

        public void Identify(string sessionId, string commitId) {
            var requestPayload = new SocketRequestPayload {
                op = SocketGatewayUtils.OPCODE_IDENTIFY,
                d = new SocketIdentifyRequest {
                    ls = sessionId,
                    commitId = commitId,
                    properties = new Dictionary<string, string>()
                }
            };
            this._Send(requestPayload);
        }

        void _Send(SocketRequestPayload requestPayload) {
            var payload = JsonConvert.SerializeObject(requestPayload);
            if (this.connected) {
                this.m_Socket.Send(payload);
            }
            else {
                this.m_PayloadQueue.Add(payload);
            }
        }

        bool _Resume() {
            if (this.resumable) {
                var requestPayload = new SocketRequestPayload {
                    op = SocketGatewayUtils.OPCODE_RESUME,
                    d = new SocketResumeRequest {
                        sessionId = this.sessionId,
                        seq = this.seq
                    }
                };
                this._Send(requestPayload);
                return true;
            }

            return false;
        }

        void _Close(bool reset = true) {
            this.readyState = GatewayState.CLOSED;
            this.m_PayloadQueue.Clear();
            if (reset) {
                this.sessionId = null;
                this.seq = 0;
            }
        }
        
        
        void _OnClose(Func<int, bool> OnClose, int code = 0) {
            if (this._heartBeater != -1) {
                this.m_Host.CancelDelayCall(this._heartBeater);
                this._heartBeater = -1;
            }
            
            if (!this._closeRequired && OnClose != null && OnClose(code)) {
                Debug.Log("socket disconnected");
            }
        }

        void _HeartBeat() {
            Debug.Log("heart beat >>>>>>>>>>");
            if (this._heartBeater != -1) {
                return;
            }
            
            this._heartBeater = this.m_Host.DelayCall(SocketGatewayUtils.heartBeatInterval / 1000f, () => {
                this.Ping();
                this._heartBeater = -1;
                this._HeartBeat();     
            });
        }

        void _CreateWebSocket(string url, Action OnConnected, Action<string, SocketResponseDataBase> OnMessage, Func<int, bool> OnClose) {
            var heartBeater = -1;
            //Debug.Log("socket connect to " + url);
            
            this.m_Socket = this.m_Socket ?? new WebSocket(this.m_Host, this._sslHandShakeError);
            this.m_Socket.Connect(url, 
                OnConnected: () => {
                    OnConnected.Invoke();
                    this._HeartBeat();
                },
                
                OnMessage: bytes => {
                    if (bytes == null) {
                        return;
                    }
                    var content = Encoding.UTF8.GetString (bytes);
                    //Debug.Log("On Message ==>" + content);
                    var response = JsonConvert.DeserializeObject<IFrame>(content);
                    
                    if (response.sequence > 0) {
                        this.seq = response.sequence;
                    }

                    var type = response.type;
                    SocketResponseDataBase data = null;
                    
                    if (response.opCode == SocketGatewayUtils.OPCODE_DISPATCH) {
                        if (type == DispatchMsgType.READY || type == DispatchMsgType.RESUMED) {
                            switch (type) {
                                case DispatchMsgType.READY:
                                    var sessionResponse = (SocketResponseSession) response;
                                    this.sessionId = sessionResponse.data.sessionId;
                                    data = sessionResponse.data;
                                    
                                    //Debug.Log(content);
                                    break;
                                case DispatchMsgType.RESUMED:
                                    break;
                            }

                            foreach (var payload in this.m_PayloadQueue) {
                                this.m_Socket.Send(payload);
                            }
                            
                            this.m_PayloadQueue.Clear();
                        }
                        else {
                            switch (type) {
                                case DispatchMsgType.MESSAGE_CREATE:
                                case DispatchMsgType.MESSAGE_UPDATE:
                                case DispatchMsgType.MESSAGE_DELETE:
                                    var messageResponse = (SocketResponseMessage) response;
                                    data = messageResponse.data;
                                    break;
                                case DispatchMsgType.PING:
                                    var pingResponse = (SocketResponsePing) response;
                                    data = pingResponse.data;
                                    break;
                                case DispatchMsgType.PRESENCE_UPDATE:
                                    var presenceUpdateResponse = (SocketResponsePresentUpdate) response;
                                    data = presenceUpdateResponse.data;
                                    break;
                                case DispatchMsgType.CHANNEL_MEMBER_ADD:
                                case DispatchMsgType.CHANNEL_MEMBER_REMOVE:
                                    var memberChangeResponse = (SocketResponseChannelMemberChange) response;
                                    data = memberChangeResponse.data;
                                    break;
                            }
                        }
                    }
                    
                    OnMessage?.Invoke(type, data);
                },
                OnError: msg => {
                    this._OnClose(OnClose);
                },
                OnClose: code => {
                    this._OnClose(OnClose, code);
                });
        }
    }
}