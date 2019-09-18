using System;
using System.Security.Authentication;
using System.Text;
using UnityEngine;
using WebSocketSharp;

namespace ConnectApp.Utils {
    
    enum WebSocketState
    {
        NotConnected,
        Connecting,
        Connected,
        Closed
    }
    
    public class WebSocket {
        
        WebSocketState m_State;
        
        readonly WebSocketHost m_Host;

        WebSocketSharp.WebSocket m_Socket;

        readonly bool m_useSslHandShakeHack;

        const bool EnableWebSocketSharpLog = false;

        public WebSocket(WebSocketHost host, bool useSslHandShakeHack)
        {
            this.m_State = WebSocketState.NotConnected;
            this.m_Host = host;
            this.m_useSslHandShakeHack = useSslHandShakeHack;
        }

        public void Send(string content) {
            this.Send(Encoding.UTF8.GetBytes (content));
        }

        public void Send(byte[] buffer) {
            Debug.Assert(this.m_State == WebSocketState.Connected, "fatal error: Cannot send data before connect!");
            Debug.Assert(this.m_Socket != null, "fatal error: Cannot send data because the websocket is null.");
            this.m_Socket.Send(buffer);
        }
        
        public enum SslProtocolsHack {
            Tls = 192,
            Tls11 = 768,
            Tls12 = 3072
        }

        public bool checkSslProtocolHackFlag(SslProtocols flags) {
            return this.m_Socket.SslConfiguration.EnabledSslProtocols != flags;
        }

        //add callbacks here as parameters
        public void Connect(string url, Action OnConnected, Action<byte[]> OnMessage,
            Action<string> OnError, Action<int> OnClose)
        {
            Debug.Assert(this.m_State == WebSocketState.NotConnected, $"fatal error: Cannot Connect to {url} because the socket is already set up.");
            this.m_State = WebSocketState.Connecting;
            
            this.m_Socket = new WebSocketSharp.WebSocket(url);
            if (this.m_useSslHandShakeHack) {
                var sslProtocolHack = (SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
                this.m_Socket.SslConfiguration.EnabledSslProtocols = sslProtocolHack;
            }

            if (EnableWebSocketSharpLog) {
                this.m_Socket.Log.Level = LogLevel.Debug;
                this.m_Socket.Log.File = @"log.txt";
            }


            this.m_Socket.OnOpen += (sender, e) => {
                this.m_State = WebSocketState.Connected;
                this.m_Host.Enqueue(() => {
                    OnConnected?.Invoke();
                });
            };
            
            this.m_Socket.OnMessage += (sender, e) => {
                this.m_Host.Enqueue(() => {
                    OnMessage(e.RawData);
                });
            };

            this.m_Socket.OnError += (sender, e) => {
                this.m_Host.Enqueue(() => {
                    OnError(e.Message);
                });
            };

            this.m_Socket.OnClose += (sender, e) => {
                this.m_Host.Enqueue(() => {
                    //Debug.Log("error code: " + e.Code + " : reason: " + e.Reason);
                    OnClose?.Invoke(e.Code);
                });
            };
            
            //try to open connection
            this.m_Socket.ConnectAsync();
        }


        public void Close() {
            if (this.m_State == WebSocketState.Connected ||
                this.m_State == WebSocketState.Connecting) {
                this.m_Socket.CloseAsync();
            }   
                            
            this.m_State = WebSocketState.Closed;
        }
    }
}