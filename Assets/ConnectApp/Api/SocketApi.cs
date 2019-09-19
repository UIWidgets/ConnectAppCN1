using System;
using ConnectApp.Models.Api;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Api {
    public static class SocketApi {

        const bool DebugSocketApi = true;

        static string m_lastSessionId;

        public static void ResetState() {
            m_lastSessionId = null;
        }

        static string sessionId {
            get {
                return HttpManager.getCookie().isNotEmpty() ? HttpManager.getCookie("LS") : "";
            }
        }
        
        public static void OnCookieChanged() {
            var newSessionId = sessionId;
            if (m_lastSessionId == newSessionId) {
                return;
            }

            m_lastSessionId = newSessionId;
            
            //Logout
            if (m_lastSessionId == "") {
                DisConnectFromWSS();
            }
            //Login
            else {
                ReConnectToWSS(m_lastSessionId);
            }
        }

        public static void OnNetworkConnected() {
            //init session id by default
            if (m_lastSessionId == null) {
                m_lastSessionId = sessionId;
            }
            
            //Logout
            if (m_lastSessionId == "") {
                DisConnectFromWSS();
            }
            //Login
            else {
                ReConnectToWSS(m_lastSessionId);
            }
        }

        public static void OnNetworkDisconnected() {
            DisConnectFromWSS();
        }


        static void DisConnectFromWSS() {
            SocketGateway.instance.Close();
        }

        static void ReConnectToWSS(string sessionId) {
            DisConnectFromWSS();
            
            try {
                DoConnectToWSS(sessionId);
            }
            catch (Exception e) {
                if (DebugSocketApi) {
                    Debug.Log("Failed to connect to wss: error = " + e);
                }
            }
        }

        static void DoConnectToWSS(string sessionId) {
            //Ready-state check
            if (DebugSocketApi) {
                Debug.Assert(SocketGateway.instance != null, "fatal error: socket gateway is null, cannot connect !");
                Debug.Assert(SocketGateway.instance.readyForConnect, "fatal error: socket gateway is not ready for connection !");
            }
            if (SocketGateway.instance == null || !SocketGateway.instance.readyForConnect) {
                return;
            }

            SocketGateway.instance.Connect(
                onConnected: 
                commitId => {
                    SocketGateway.instance.Identify(sessionId, commitId);
                },
                onMessage: 
                (type, data) => {
                    switch (type) {
                        case DispatchMsgType.INVALID_LS:
                            break;
                        case DispatchMsgType.READY:
                            var sessionData = (SocketResponseSessionData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushReadyAction {
                                readyData = sessionData
                            });
                            break;
                        case DispatchMsgType.RESUMED:
                            break;
                        case DispatchMsgType.MESSAGE_CREATE:
                            var messageData = (SocketResponseMessageData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushNewMessageAction {
                                messageData = messageData
                            });
                            break;
                        case DispatchMsgType.MESSAGE_UPDATE:
                            var updateMessageData = (SocketResponseMessageData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushModifyMessageAction {
                                messageData = updateMessageData
                            });
                            break;
                        case DispatchMsgType.MESSAGE_DELETE:
                            var deleteMessageData = (SocketResponseMessageData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushDeleteMessageAction {
                                messageData = deleteMessageData
                            });
                            break;
                        case DispatchMsgType.PING:
                            //var pingData = (SocketResponsePingData) data;
                            //do nothing
                            break;
                        case DispatchMsgType.PRESENCE_UPDATE:
                            var presenceUpdateData = (SocketResponsePresentUpdateData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushPresentUpdateAction {
                                presentUpdateData = presenceUpdateData
                            });
                            break;
                        case DispatchMsgType.CHANNEL_MEMBER_ADD:
                            var memberAddData = (SocketResponseChannelMemberChangeData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushChannelAddMemberAction {
                                memberData = memberAddData
                            });
                            break;
                        case DispatchMsgType.CHANNEL_MEMBER_REMOVE:
                            var memberRemoveData = (SocketResponseChannelMemberChangeData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushChannelRemoveMemberAction {
                                memberData = memberRemoveData
                            });
                            break;
                    }
                });
        }
    }
}