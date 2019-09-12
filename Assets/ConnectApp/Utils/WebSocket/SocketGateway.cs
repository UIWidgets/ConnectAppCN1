using System;
using System.Collections.Generic;
using System.Text;
using ConnectApp.Api;
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
        public const int OPCODE_PING = 4;

        public const int heartBeatInterval = 15000;    //ms
    }
    
    public class SocketGateway {
        static SocketGateway m_Instance;

        public static SocketGateway instance {
            get {
                if (m_Instance == null) {
                    Debug.Assert(false, "fatal error: socket gateway has not been initialized yet !");
                    return null;
                }

                return m_Instance;
            }
        }
        
        
        List<string> m_CandidateURLs;

        List<string> m_PayloadQueue;

        GatewayState readyState;

        WebSocket m_Socket;

        int m_lastDispatchTs;

        string sessionId;
        string m_GatewayUrl;
        int seq;

        bool _closeRequired = false;

        bool connected {
            get { return this.readyState == GatewayState.OPEN; }
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
            
            this.m_CandidateURLs = new List<string>();
            this.m_PayloadQueue = new List<string>();

            this.m_Socket = new WebSocket(host);
            this._Close();
        }

        public void Connect(Action onConnected, Action<string, SocketResponseDataBase> onMessage) {
            if (this.readyState != GatewayState.CLOSED) {
                return;
            }

            this.readyState = GatewayState.CONNECTING;
            
            this._SelectGateway(url => {
                if (url != null) {
                    this._CreateWebSocket(url, 
                        OnConnected: () => {
                            this.readyState = GatewayState.OPEN;
                            if (!this._Resume()) {
                                onConnected.Invoke();
                            }
                        },
                        OnMessage: (type, data) => {
                            onMessage?.Invoke(type, data);
                        },
                        OnClose: () => {
                            Debug.Log("OnClose");
                            this.m_Socket.Close();
                            this.m_Socket = null;
                            this._Close();
                        });
                }
                else {
                    Debug.Assert(false, "url is null, Cannot connect!");
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

        public void Reconnect() {
            //TODO
        }


        void _SelectGateway(Action<string> callback, bool sticky = true) {
            if (sticky && this.m_GatewayUrl != null) {
                callback(this.m_GatewayUrl);
                return;
            }
            
            if (this.m_CandidateURLs.Count > 0) {
                var url = this.m_CandidateURLs[0];
                this.m_CandidateURLs.RemoveAt(0);
                this.m_CandidateURLs.Add(url);

                this.m_GatewayUrl = $"{url}/v1";
                callback(this.m_GatewayUrl);
                return;
            }

            var request = HttpManager.GET($"{Config.apiAddress}/api/socketgw", null);
            
            HttpManager.resume(request).Then(responseText => {
                var socketGatewayResponse = JsonConvert.DeserializeObject<SocketGatewayResponse>(responseText);
                this.m_CandidateURLs = socketGatewayResponse.urls ?? new List<string> { socketGatewayResponse.url };
                this._SelectGateway(callback, false);
            }).Catch(exception => {
                callback(null);
                Debug.Log(exception);
            });
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

        void _CreateWebSocket(string url, Action OnConnected, Action<string, SocketResponseDataBase> OnMessage, Action OnClose) {
            this.m_Socket.Connect(url, 
                OnConnected: () => {
                    this.m_lastDispatchTs = Time.frameCount;
                    OnConnected.Invoke();
                },
                
                OnMessage: bytes => {
                    if (bytes == null) {
                        return;
                    }
                    var content = Encoding.UTF8.GetString (bytes);
                    Debug.Log(content);
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
                                    var messageResponse = (SocketResponseCreateMsg) response;
                                    data = messageResponse.data;
                                    break;
                            }
                        }
                    }
                    
                    //Debug.Log("On Message =" + content);
                    
                    OnMessage?.Invoke(type, data);
                },
                OnError: msg => {
                    OnClose?.Invoke();
                    Debug.Log("OnError" + msg);
                },
                OnClose: () => {
                    OnClose?.Invoke();
                    Debug.Log("OnClose");
                });
        }
    }
}