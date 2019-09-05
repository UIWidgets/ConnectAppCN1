using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;
using ConnectApp.Constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
using EventType = ConnectApp.Models.State.EventType;

namespace ConnectApp.Plugins {
    public class WechatPlugin {
        public static WechatPlugin instance(Action<string> codeCallBack = null) {
            if (plugin == null) {
                plugin = new WechatPlugin();
            }

            if (codeCallBack != null) {
                plugin.codeCallBack = codeCallBack;
            }

            return plugin;
        }

        static WechatPlugin plugin;

        bool isListen;

        public BuildContext context;

        Action<string> codeCallBack;

        public string currentEventId;

        void addListener() {
            if (!this.isListen) {
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("wechat", this._handleMethodCall);
                this.isListen = true;
            }
        }

        void _handleMethodCall(string method, List<JSONNode> args) {
            using (WindowProvider.of(this.context).getScope()) {
                switch (method) {
                    case "callback": {
                        if (args.isEmpty()) {
                            return;
                        }

                        var node = args[0];
                        var dict = JSON.Parse(node);
                        var type = dict["type"];
                        if (type == "code") {
                            var code = dict["code"];
                            if (this.codeCallBack != null) {
                                this.codeCallBack(code);
                            }
                        }
                    }
                        break;
                    case "openUrl": {
                        if (args.isEmpty()) {
                            return;
                        }

                        openUrl(args.first());
                    }
                        break;
                }
            }
        }

        public static void openUrl(string schemeUrl) {
            if (schemeUrl.isEmpty()) {
                return;
            }

            var uri = new Uri(schemeUrl);
            if (uri.Scheme.Equals("unityconnect")) {
                if (uri.Host.Equals("connectapp")) {
                    var type = "";
                    if (uri.AbsolutePath.Equals("/project_detail")) {
                        type = "project";
                    }
                    else if (uri.AbsolutePath.Equals("/event_detail")) {
                        type = "event";
                    }
                    else {
                        return;
                    }

                    var subType = HttpUtility.ParseQueryString(uri.Query).Get("type");
                    var id = HttpUtility.ParseQueryString(uri.Query).Get("id");
                    if (id != instance().currentEventId) {
                        if (type == "event") {
                            var eventType = EventType.offline;
                            if (subType == "online") {
                                eventType = EventType.online;
                            }

                            StoreProvider.store.dispatcher.dispatch(
                                new MainNavigatorPushToEventDetailAction {eventId = id, eventType = eventType});
                        }
                        else if (type == "project") {
                            if (subType == "article") {
                                StoreProvider.store.dispatcher.dispatch(
                                    new MainNavigatorPushToArticleDetailAction {articleId = id, isPush = false});
                            }
                        }
                    }
                }
            }
            else {
                JPushPlugin.openUrl(schemeUrl);
            }
        }

        public void login(string stateId) {
            if (!Application.isEditor) {
                this.addListener();
                loginWechat(stateId);
            }
        }

        public void shareToFriend(string title, string description, string url, string imageBytes) {
            if (!Application.isEditor) {
                this.addListener();
                toFriends(title, description, url, imageBytes);
            }
        }

        public void shareToTimeline(string title, string description, string url, string imageBytes) {
            if (!Application.isEditor) {
                this.addListener();
                toTimeline(title, description, url, imageBytes);
            }
        }

        public void shareToMiniProgram(string title, string description, string url, string imageBytes, string path) {
            if (!Application.isEditor) {
                this.addListener();
                toMiNiProgram(title, description, url, imageBytes, Config.MINIID, path, Config.miniProgramType);
            }
        }


        public bool isInstalled() {
            if (!Application.isEditor) {
                this.addListener();
                return isInstallWechat();
            }

            return false;
        }

        public void toOpenMiNi(string path) {
            if (!Application.isEditor) {
                this.addListener();
                openMiNi(Config.MINIID, path, Config.miniProgramType);
            }
        }
#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void loginWechat(string stateId);

        [DllImport("__Internal")]
        static extern bool isInstallWechat();

        [DllImport("__Internal")]
        static extern void toFriends(string title, string description, string url, string imageBytes);

        [DllImport("__Internal")]
        static extern void toTimeline(string title, string description, string url, string imageBytes);

        [DllImport("__Internal")]
        internal static extern void toMiNiProgram(string title, string description, string url, string imageBytes,
            string ysId, string path, int miniProgramType);

        [DllImport("__Internal")]
        internal static extern void openMiNi(string ysId, string path, int miniProgramType);

#elif UNITY_ANDROID
        static AndroidJavaObject _plugin;

        static AndroidJavaObject Plugin() {
            if (_plugin == null) {
                using (
                    AndroidJavaClass managerClass =
                        new AndroidJavaClass("com.unity3d.unityconnect.plugins.WechatPlugin")
                ) {
                    _plugin = managerClass.CallStatic<AndroidJavaObject>("getInstance");
                }
            }

            return _plugin;
        }

        static void loginWechat(string stateId) {
            Plugin().Call("loginWechat", stateId);
        }

        static bool isInstallWechat() {
            return Plugin().Call<bool>("isInstallWechat");
        }

        static void toFriends(string title, string description, string url, string imageBytes) {
            Plugin().Call("shareToFriends", title, description, url, imageBytes);
        }

        static void toTimeline(string title, string description, string url, string imageBytes) {
            Plugin().Call("shareToTimeline", title, description, url, imageBytes);
        }

        static void toMiNiProgram(string title, string description, string url, string imageBytes, string ysId,
            string path,int miniProgramType) {
            Plugin().Call("shareToMiNiProgram", title, description, url, imageBytes, ysId, path,miniProgramType);
        }

        static void openMiNi(string ysId, string path,int miniProgramType) {
            Plugin().Call("openMiNi", Config.wechatAppId, ysId, path,miniProgramType);
        }
#else
        static bool isInstallWechat() {return true;}
        static void loginWechat(string stateId) {}
        static void toFriends(string title, string description, string url,string imageBytes) {}
        static void toTimeline(string title, string description, string url,string imageBytes) {}
        static void toMiNiProgram(string title, string description, string url, string imageBytes,string ysId, string path) {}
        static void openMiNi(string ysId, string path) {}
#endif
    }
}