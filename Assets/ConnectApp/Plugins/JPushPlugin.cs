using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
using EventType = ConnectApp.models.EventType;

namespace ConnectApp.plugins {
    public static class JPushPlugin {
        public static BuildContext context;
        public static bool isListen;

        public static void addListener() {
            if (!isListen) {
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("jpush", _handleMethodCall);
                completed();
                setJPushChannel(Config.store);
                isListen = true;
            }
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (context != null) {
                using (WindowProvider.of(context).getScope()) {
                    switch (method) {
                        case "OnOpenNotification": {
                            var node = args[0];
                            var dict = JSON.Parse(node);
                            var type = dict["type"];
                            var id = dict["id"];
                            if (type == "article") {
                                StoreProvider.store.dispatcher.dispatch(
                                    new MainNavigatorPushToArticleDetailAction {articleId = id});
                            }
                            else if (type == "event") {
                                var eventType = EventType.offline;
                                if (dict["eventType"] == "online") {
                                    eventType = EventType.online;
                                }

                                StoreProvider.store.dispatcher.dispatch(
                                    new MainNavigatorPushToEventDetailAction {eventId = id, eventType = eventType});
                            }
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

            setAlias(alias);
        }


#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void listenCompleted();

        [DllImport("__Internal")]
        static extern void setChannel(string channel);
        
        [DllImport("__Internal")]
        static extern void setAlias(string alias);

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

        static void setAlias(string alias) {
            Plugin().Call("setAlias", alias);
        }


#else
        static void listenCompleted() {}
        static void setChannel(string channel) {}
        static void setAlias(string channel) {}
#endif
    }
}