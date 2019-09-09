using System;
using UnityEngine;
using WebSocketSharp;

namespace ConnectApp.Utils {
    
    //TODO: deal with reConnect
    enum WebSocketState
    {
        NotConnected,
        Connecting,
        Connected
    }
    
    public class WebSocketManager {

        static WebSocketManager m_Instance;

        public static WebSocketManager instance {
            get {
                if (m_Instance != null) {
                    return m_Instance;
                }

                Debug.Assert(false, "fatal error: There is no WebSocketManager available now!");
                return null;
            }
        }
        
        WebSocketState m_State;
        
        readonly WebSocketHost m_Host;

        WebSocket m_Socket;

        public WebSocketManager(WebSocketHost host)
        {
            Debug.Assert(m_Instance == null, "fatal error: Cannot initiate two WebSocketManagers!");
            this.m_State = WebSocketState.NotConnected;
            this.m_Host = host;

            m_Instance = this;
        }

        public void Send(byte[] buffer) {
            Debug.Assert(this.m_State == WebSocketState.Connected, "fatal error: Cannot send data before connect!");
            Debug.Assert(this.m_Socket != null, "fatal error: Cannot send data because the websocket is null.");
            this.m_Socket.Send(buffer);
        }

        //add callbacks here as parameters
        public void Connect(string url, Action OnConnected, Action<byte[]> OnMessage,
            Action<string> OnError)
        {
            Debug.Assert(this.m_State == WebSocketState.NotConnected, $"fatal error: Cannot Connect to {url} because the socket is already set up.");
            Debug.Assert(this.m_Host == null, "fatal error: The websocket has been already setup!");
            this.m_State = WebSocketState.Connecting;
            
            this.m_Socket = new WebSocket(url);
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
            
            //try to open connection
            this.m_Socket.ConnectAsync();
        }
    }
}