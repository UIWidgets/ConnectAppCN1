using System;
using System.Collections;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using UnityEngine;

namespace ConnectApp.Api {
    public static class SocketApi {
        public static void DisConnectFromWSS() {
            SocketGateway.instance.Close();
        }
        
        
        public static void ConnectToWSS(bool forceConnect = true) {
            if (HttpManager.getCookie().isNotEmpty()) {
                var sessionId = HttpManager.getCookie("LS");
                if (sessionId == null) {
                    Debug.Log("Connect to Message Feed Failed: no sessionId available !");
                    return;
                }
            }
            
            D.assert(SocketGateway.instance != null);
            if (SocketGateway.instance == null) {
                return;
            }
            if (!forceConnect && !SocketGateway.instance.readyForConnect) {
                return;
            }
            
            SocketGateway.instance.Connect(commitId => 
            {
                if (HttpManager.getCookie().isNotEmpty()) {
                    var sessionId = HttpManager.getCookie("LS");
                    SocketGateway.instance.Identify(sessionId, commitId);
                }
            },
                (type, data) => {
                    switch (type) {
                        case DispatchMsgType.INVALID_LS:
                            break;
                        case DispatchMsgType.READY:
                            var sessionData = (SocketResponseSessionData) data;
                            var sessionId = sessionData.sessionId;
                            
                            StoreProvider.store.dispatcher.dispatch(new PushReadyAction {
                                readyData = sessionData});
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
        
        
        public static Promise<string> FetchSocketUrl() {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<string>();
            Window.instance.startCoroutine(_FetchSocketUrl(promise));
            return promise;
        }

        static IEnumerator
            _FetchSocketUrl(Promise<string> promise) {
            var request = HttpManager.GET(Config.apiAddress + "/api/socketgw");
            yield return request.SendWebRequest();

            if (request.isNetworkError) {
                // something went wrong
                promise.Reject(new Exception(request.error));
            }
            else if (request.responseCode != 200) {
                // or the response is not OK
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else {
                // Format output and resolve promise
                var responseText = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<FetchSocketUrlResponse>(responseText);
                if (!string.IsNullOrEmpty(response.url)) {
                    promise.Resolve(response.url);
                }
                else {
                    promise.Reject(new Exception("No user under this username found!"));
                }
            }
        }
    }
}