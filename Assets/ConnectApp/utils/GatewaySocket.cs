using System;
using System.Net.Http;
using RSG;
using BestHTTP;
using BestHTTP.WebSocket;
using BestHTTP.WebSocket.Frames;
using ConnectApp.api;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;

namespace ConnectApp.utils {
    delegate string GetCurrentChannelId();
    delegate string GetLoginSession();
    
    public class GatewaySocket {
        
        private static WebSocket _webSocket;
        
        Promise Connect(
            string url,
            GetCurrentChannelId getCurrentChannelId,
            GetLoginSession getLoginSession,
            bool isDebug = false
        ) {
            SocketApi.FetchSocketUrl()
                .Then(socketUrl => {
                    _webSocket = new WebSocket(new Uri($"{socketUrl}/v1"));
                    _webSocket.OnOpen += OnWebSocketOpen;
                    _webSocket.OnMessage += OnMessageReceived;
                    _webSocket.OnIncompleteFrame += OnIncompleteFrame;
                });
            return null;
        }
        
        private void OnWebSocketOpen(WebSocket webSocket) {
            Debug.Log("WebSocket Open!");
        }
        
        private void OnMessageReceived(WebSocket webSocket, string message) {
            Debug.Log("Text Message received from server: " + message);
        }
        
        private void OnIncompleteFrame(WebSocket webSocket, WebSocketFrameReader frame) {
            Debug.Log("Frame received from server: " + frame);
        }
        
        
        
//        void Add(object frame /*String|List<int>|Frame*/) {
//            if (frame is WebSocketFrameReader) {
//                var frameBytes = frame;
//                if (this._debug) {
//                    _info('<- $frameBytes');
//                }
//                this._socket.add(frameBytes);
//            } else {
//                this._socket.add(frame);
//            }
//        }
        
        void _Info(string msg) {
            Debug.Log($"[INFO] GatewaySocket: {msg}");
        }
    }

    

//        WebSocket _socket;
//        GetCurrentChannelId _getCurrentChannelId;
//        GetLoginSession _getLoginSession;
//        Timer _pingTimer;
//        bool _debug;
//        int _lastPingTimeStamp;
//        string _url;
//
//        _GatewaySocket() {
//            _Ping();
        }

//        void _Ping() {
//            this._pingTimer = Window.instance.run(TimeSpan.FromSeconds(15),() => {
//                if (this._lastPingTimeStamp != null) {
//                    return ;
//                }
//                PingFrame frame;
//            })
//            this._pingTimer = Timer(Duration(seconds: 15), () {
//                if (this._lastPingTimeStamp != null) {
//                    return;
//                }
//                PingFrame frame;
//                if (this._getCurrentChannelId == null) {
//                    frame = PingFrame.generate();
//                } else {
//                    frame = PingFrame.withChannelId(this._getCurrentChannelId());
//                }
//                Timer(Duration(seconds: 5), () {
//                    // _lastPingTimeStamp needs to be clear in 5 seconds, unless we consider it is disconnected.
//                    if (this._lastPingTimeStamp != null) {
//                        this.reconnect();
//                    }
//                });
//                this._lastPingTimeStamp = frame.data.timestamp;
//                this.add(frame);
//                _ping();
//            });
//        }
//    }
//}