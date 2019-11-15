using System;
using ConnectApp.Models.Api;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Api {
    public static class SocketApi {
        static string m_lastSessionId;

        public static void ResetState() {
            m_lastSessionId = null;
        }

        static string sessionId {
            get { return HttpManager.getCookie().isNotEmpty() ? HttpManager.getCookie("LS") : ""; }
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

        public static void OnApplicationFocus(bool focus) {
            if (focus) {
                if (!string.IsNullOrEmpty(m_lastSessionId)) {
                    ReConnectToWSS(m_lastSessionId);
                }
            }
            else {
                DisConnectFromWSS();
            }
        }

        public static void OnNetworkConnected() {
            NetworkStatusManager.isAvailable = true;
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
            NetworkStatusManager.isAvailable = false;
            DisConnectFromWSS();
        }


        static void DisConnectFromWSS() {
            SocketGateway.instance?.Close();
        }

        static void ReConnectToWSS(string sessionId) {
            DisConnectFromWSS();

            try {
                DoConnectToWSS(sessionId);
            }
            catch (Exception e) {
                Debuger.Log("Failed to connect to wss: error = " + e);
            }
        }

        static void DoConnectToWSS(string sessionId) {
            //Ready-state check
            DebugerUtils.DebugAssert(SocketGateway.instance != null,
                "fatal error: socket gateway is null, cannot connect !");
            DebugerUtils.DebugAssert(SocketGateway.instance.readyForConnect,
                "fatal error: socket gateway is not ready for connection !");

            if (SocketGateway.instance == null || !SocketGateway.instance.readyForConnect) {
                return;
            }

            SocketGateway.instance.Connect(
                onIdentify:
                commitId => { SocketGateway.instance.Identify(sessionId, commitId); },
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
                            StoreProvider.store.dispatcher.dispatch(Actions.saveReadyStateToDB(sessionData));
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
                        case DispatchMsgType.CHANNEL_CREATE:
                            var createChannelData = (SocketResponseUpdateChannelData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushChannelCreateChannelAction {
                                channelData = createChannelData
                            });
                            break;
                        case DispatchMsgType.CHANNEL_DELETE:
                            var deleteChannelData = (SocketResponseUpdateChannelData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushChannelDeleteChannelAction {
                                channelData = deleteChannelData
                            });
                            break;
                        case DispatchMsgType.CHANNEL_UPDATE:
                            var updateChannelData = (SocketResponseUpdateChannelData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushChannelUpdateChannelAction {
                                channelData = updateChannelData
                            });
                            break;
                        case DispatchMsgType.MESSAGE_ACK:
                            var messageAckData = (SocketResponseMessageAckData) data;

                            StoreProvider.store.dispatcher.dispatch(new PushChannelMessageAckAction {
                                ackData = messageAckData
                            });
                            break;
                    }
                });
        }
    }
}