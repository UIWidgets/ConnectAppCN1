using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
using EventType = ConnectApp.Models.State.EventType;

namespace ConnectApp.Plugins {
    public static class JPushPlugin {
        public static BuildContext context;
        public static bool isListen;
        static int callbackId = 0;

        public static void addListener() {
            if (!isListen) {
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("jpush", _handleMethodCall);
                completed();
                setJPushChannel(Config.store);
                setJPushTags(new List<string> {Config.versionCode.ToString()});
                isListen = true;
            }
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (context != null) {
                using (WindowProvider.of(context).getScope()) {
                    switch (method) {
                        case "OnOpenNotification": {
                            //点击应用通知栏
                            var node = args[0];
                            var dict = JSON.Parse(node);
                            var type = dict["type"];
                            var subType = dict["subtype"];
                            var id = dict["id"];
                            if (type == "project") {
                                if (subType == "article") {
                                    StoreProvider.store.dispatcher.dispatch(
                                        new MainNavigatorPushToArticleDetailAction {articleId = id});
                                }
                            }
                            else if (type == "event") {
                                var eventType = EventType.offline;
                                if (subType == "online") {
                                    eventType = EventType.online;
                                }

                                StoreProvider.store.dispatcher.dispatch(
                                    new MainNavigatorPushToEventDetailAction {eventId = id, eventType = eventType});
                            }
                        }
                            break;
                        case "OnReceiveNotification": {
                            //接收到推送
                            EventBus.publish(EventBusConstant.refreshNotifications, new List<object>());
                        }
                            break;
                        case "OnReceiveMessage": {
                            //接收到应用内消息
                        }
                            break;
                    }
                }
            }
        }

        static void completed() {
            listenCompleted();
        }

        public static void setJPushChannel(string channel) {
            if (channel.isEmpty()) {
                return;
            }

            setChannel(channel);
        }

        public static void setJPushAlias(string alias) {
            if (alias.isEmpty()) {
                return;
            }

            setAlias(callbackId++, alias);
        }

        public static void deleteJPushAlias() {
            deleteAlias(callbackId++);
        }

        public static void setJPushTags(List<string> tags) {
            string tagsJsonStr = JsonHelper.ToJson(tags);
            if (tagsJsonStr.isEmpty()) {
                return;
            }

            setTags(callbackId++, tagsJsonStr);
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void listenCompleted();

        [DllImport("__Internal")]
        static extern void setChannel(string channel);

        [DllImport("__Internal")]
        static extern void setAlias(int sequence, string alias);

        [DllImport("__Internal")]
        static extern void deleteAlias(int sequence);

        [DllImport("__Internal")]
        static extern void setTags(int sequence, string tagsJsonStr);

#elif UNITY_ANDROID
        static AndroidJavaObject _plugin;

        static AndroidJavaObject Plugin() {
            if (_plugin == null) {
                using (
                    AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JPushPlugin")
                ) {
                    _plugin = managerClass.CallStatic<AndroidJavaObject>("getInstance");
                }
            }

            return _plugin;
        }

        static void listenCompleted() {
            Plugin().Call("listenCompleted");
        }

        static void setChannel(string channel) {
            Plugin().Call("setChannel", channel);
        }

        static void setAlias(int sequence, string alias) {
            Plugin().Call("setAlias", sequence, alias);
        }

        static void deleteAlias(int sequence) {
            Plugin().Call("deleteAlias", sequence);
        }
        
        static void setTags(int sequence, string tagsJsonStr) {
            Plugin().Call("setTags", sequence, tagsJsonStr);
        }
#else
        static void listenCompleted() {}
        static void setChannel(string channel) {}
        static void setAlias(int sequence, string channel) {}
        static void deleteAlias(int sequence) {}
        static void setTags(int sequence, string tagsJsonStr) {}
#endif
    }
}