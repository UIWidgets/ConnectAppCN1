using System;
using System.Collections.Generic;
using System.Text;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using Newtonsoft.Json;

namespace ConnectApp.Utils {
    enum GatewayState {
        Init,
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
        public const int maxTryStickyCount = 3;
        public const bool defaultSticky = true;

        public static long DateTimeToUnixTimestamp(DateTime dateTime) {
            return (long) (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                           new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }


    public class SocketGateway {
        static SocketGateway m_Instance;

        public static SocketGateway instance {
            get {
                if (m_Instance == null) {
                    Debuger.LogError("fatal error: socket gateway has not been initialized yet !");
                    return null;
                }

                return m_Instance;
            }
        }

        GatewayState m_ReadyState;

        readonly List<string> m_PayloadQueue;
        readonly WebSocketHost m_Host;
        readonly BackOff m_BackOff;

        //Gateway Data
        List<string> m_CandidateURLs;
        int m_CandidateIndex;
        string m_GatewayUrl;

        //Callback Cache
        Action<string> m_OnIdentify;
        Action<string, SocketResponseDataBase> m_OnMessage;

        //Socket Context
        WebSocket m_Socket;
        string m_SessionId;
        string m_CommitId;
        int m_Sequence;
        bool m_CloseRequired;
        int m_HeartBeater = -1;
        bool m_SslHandShakeError = false;


        public bool connected {
            get { return this.m_ReadyState == GatewayState.OPEN && (this.m_Socket?.connected ?? false); }
        }

        public bool readyForConnect {
            get { return this.m_ReadyState == GatewayState.Init; }
        }

        bool resumable {
            get { return this.m_SessionId != null && this.m_Sequence > 0; }
        }

        public SocketGateway(WebSocketHost host) {
            if (m_Instance != null) {
                DebugerUtils.DebugAssert(false, "fatal error: duplicated socket gateways is not allowed!");
                return;
            }

            m_Instance = this;
            this.m_Host = host;

            this.m_CandidateURLs = new List<string>();
            this.m_CandidateIndex = 0;
            this.m_PayloadQueue = new List<string>();
            this.m_BackOff = new BackOff(host, 1000, 30000);
            this.m_CommitId = null;
            this.m_Socket = null;

            this._Reset(isInit: true);
        }

        public void Close() {
            this.m_GatewayUrl = null;

            this.m_CommitId = null;
            this.m_CloseRequired = true;
            this.m_Socket?.Close();
            this.m_Socket = null;

            this._CancelHeartBeat();
            this._Reset(isInit: true);
            this.m_BackOff.Cancel();
            
            NetworkStatusManager.isConnected = false;
        }


        int _OnFail(bool reset = true) {
            this._Reset(isInit: false, reset);
            this.m_BackOff.Cancel();
            int delay = this.m_BackOff.OnFail(() => { this.Connect(this.m_OnIdentify, this.m_OnMessage, true); });
            return delay;
        }

        void _OnClose(int code = 0) {
            this._CancelHeartBeat();

            if (this.m_CloseRequired) {
                this.m_ReadyState = GatewayState.Init;
                return;
            }

            if (this.m_Socket != null) {
                //https://github.com/sta/websocket-sharp/issues/219  
                if (code == 1015 && this.m_Socket.checkSslProtocolHackFlag()) {
                    this.m_SslHandShakeError = true;
                }

                this.m_Socket.Close();
                this.m_Socket = null;
            }

            this._Reset(isInit: false);

            bool reset = !this.resumable || this.m_BackOff.fail > 0;
            if (code != 0) {
                this.m_CandidateIndex++;
            }
            var delay = this._OnFail(reset);
            NetworkStatusManager.isConnected = false;
            DebugerUtils.DebugAssert(false, $"connection failed, retry in {delay / 1000f} seconds");
        }

        SocketResponseDataBase _OnMessage(byte[] bytes, ref string type) {
            this.m_BackOff.OnSucceed();

            if (bytes == null) {
                return null;
            }

            var content = Encoding.UTF8.GetString(bytes);
            var response = JsonConvert.DeserializeObject<IFrame>(content);

            if (response.sequence > 0) {
                this.m_Sequence = response.sequence;
            }

            type = response.type;
            SocketResponseDataBase data = null;

            if (response.opCode == SocketGatewayUtils.OPCODE_DISPATCH) {
                if (type == DispatchMsgType.READY || type == DispatchMsgType.RESUMED) {
                    switch (type) {
                        case DispatchMsgType.READY:
                            var sessionResponse = (SocketResponseSession) response;
                            data = sessionResponse.data;
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
                        case DispatchMsgType.CHANNEL_CREATE:
                        case DispatchMsgType.CHANNEL_DELETE:
                        case DispatchMsgType.CHANNEL_UPDATE:
                            var createChannelResponse = (SocketResponseUpdateChannel) response;
                            data = createChannelResponse.data;
                            break;
                        case DispatchMsgType.MESSAGE_ACK:
                            var messageAckResponse = (SocketResponseMessageAck) response;
                            data = messageAckResponse.data;
                            break;

                        default:
                            data = null;
                            break;
                    }
                }
            }

            return data;
        }

        public void Connect(Action<string> onIdentify, Action<string, SocketResponseDataBase> onMessage,
            bool reconnect = false) {
            if (this.m_ReadyState != GatewayState.CLOSED && this.m_ReadyState != GatewayState.Init) {
                return;
            }

            if (!reconnect) {
                this.m_OnIdentify = onIdentify;
                this.m_OnMessage = onMessage;
                this.m_CloseRequired = false;
            }
            else {
                DebugerUtils.DebugAssert(this.m_OnIdentify != null, "fatal error: reconnect before initial connect !");
                DebugerUtils.DebugAssert(this.m_OnMessage != null, "fatal error: reconnect before initial connect !");
            }

            this.m_ReadyState = GatewayState.CONNECTING;
            this.m_BackOff.Cancel();

            this._SelectGateway(
                createWebSocketFunc:
                url => {
                    if (url != null) {
                        this._Reset(isInit: true);
                        this.m_Socket?.Close();
                        this.m_Socket = null;
                        
                        this.m_Socket = new WebSocket(this.m_Host, this.m_SslHandShakeError);
                        this.m_Socket.Connect(url,
                            OnError: msg => { this._OnClose(1); },
                            OnClose: this._OnClose,
                            OnConnected: () => {
                                NetworkStatusManager.isConnected = true;
                                DebugerUtils.DebugAssert(this.m_CommitId != null,
                                    "fatal error: commit Id is not correctly set before connection!");

                                this.m_ReadyState = GatewayState.OPEN;
                                if (!this._Resume()) {
                                    this.m_OnIdentify.Invoke(this.m_CommitId);
                                }

                                this._StartHeartBeat();
                            },
                            OnMessage: bytes => {
                                NetworkStatusManager.isConnected = true;
                                string type = "";
                                var data = this._OnMessage(bytes, ref type);
                                if (data != null) {
                                    this.m_OnMessage?.Invoke(type, data);
                                }
                            }
                        );
                    }
                    else {
                        NetworkStatusManager.isConnected = false;
                        var delay = this._OnFail();
                        DebugerUtils.DebugAssert(false, $"gateway discovery failed, retry in {delay / 1000f} seconds");
                    }
                });
        }

        void _SelectGateway(Action<string> createWebSocketFunc, bool sticky = SocketGatewayUtils.defaultSticky) {
            if (this.m_GatewayUrl != null && sticky && this.m_BackOff.fail < SocketGatewayUtils.maxTryStickyCount) {
                createWebSocketFunc(this.m_GatewayUrl);
                return;
            }

            if (this.m_CandidateIndex < this.m_CandidateURLs.Count) {
                var url = this.m_CandidateURLs[this.m_CandidateIndex];
                this.m_GatewayUrl = $"{url}/v1";
                createWebSocketFunc(this.m_GatewayUrl);
                return;
            }

            this.m_CandidateIndex = 0;
            this.m_CandidateURLs.Clear();

            var requestUrl = $"{Config.apiAddress_cn}{Config.apiPath}/socketgw";
            var request = HttpManager.GET(requestUrl, null);

            HttpManager.resumeAll(request).Then(responseContent => {
                var responseText = responseContent.text;
                var header = responseContent.headers;
                this.m_CommitId = header["X-Last-Commmit-Hash"];

                var socketGatewayResponse = JsonConvert.DeserializeObject<SocketGatewayResponse>(responseText);
                this.m_CandidateURLs = socketGatewayResponse.urls ?? new List<string> {socketGatewayResponse.url};
                this._SelectGateway(createWebSocketFunc, false);
            }).Catch(
                exception => { createWebSocketFunc(null); });
        }

        bool _Resume() {
            if (!this.resumable) {
                return false;
            }

            var requestPayload = new SocketRequestPayload {
                op = SocketGatewayUtils.OPCODE_RESUME,
                d = new SocketResumeRequest {
                    sessionId = this.m_SessionId,
                    seq = this.m_Sequence
                }
            };
            this._Send(requestPayload);
            return true;
        }

        void _Reset(bool isInit, bool reset = true) {
            this.m_ReadyState = isInit ? GatewayState.Init : GatewayState.CLOSED;
            this.m_PayloadQueue.Clear();
            if (reset) {
                this.m_SessionId = null;
                this.m_Sequence = 0;
            }
        }

        void _CancelHeartBeat() {
            if (this.m_HeartBeater == -1) {
                return;
            }

            this.m_Host.CancelDelayCall(this.m_HeartBeater);
            this.m_HeartBeater = -1;
        }

        void _StartHeartBeat() {
            if (this.m_HeartBeater != -1) {
                return;
            }

            this.m_HeartBeater = this.m_Host.DelayCall(SocketGatewayUtils.heartBeatInterval / 1000f, () => {
                this.Ping();
                this.m_HeartBeater = -1;
                this._StartHeartBeat();
            });
        }

        void Ping() {
            var requestPayload = new SocketRequestPayload {
                op = SocketGatewayUtils.OPCODE_PING,
                d = new SocketPingRequest {
                    ts = SocketGatewayUtils.DateTimeToUnixTimestamp(DateTime.Now)
                }
            };

            this._Send(requestPayload);
        }


        public void Identify(string sessionId, string commitId) {
            //cache the current m_SessionId for resume
            this.m_SessionId = sessionId;

            var requestPayload = new SocketRequestPayload {
                op = SocketGatewayUtils.OPCODE_IDENTIFY,
                d = new SocketIdentifyRequest {
                    ls = this.m_SessionId,
                    commitId = commitId,
                    clientType = "connect",
                    isApp = true,
                    properties = new Dictionary<string, string>()
                }
            };

            this._Send(requestPayload);
        }

        void _Send(SocketRequestPayload requestPayload) {
            var payload = JsonConvert.SerializeObject(requestPayload);
            if (this.connected) {
                this.m_Socket?.Send(payload);
            }
            else {
                this.m_PayloadQueue.Add(payload);
            }
        }
    }
}