using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.plugins {
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

        public static WechatPlugin plugin;

        bool isListen;

        public BuildContext context;

        public Action<string> codeCallBack;

        public void addListener() {
            if (!this.isListen) {
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("wechat", this._handleMethodCall);
                this.isListen = true;
            }
        }

        void _handleMethodCall(string method, List<JSONNode> args) {
            using (WindowProvider.of(this.context).getScope()) {
                switch (method) {
                    case "callback": {
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
                }
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


        public bool inInstalled() {
            if (!Application.isEditor) {
                this.addListener();
                return isInstallWechat();
            }

            return false;
        }
#if UNITY_IOS

        [DllImport("__Internal")]
        internal static extern void loginWechat(string stateId);

        [DllImport("__Internal")]
        internal static extern bool isInstallWechat();

        [DllImport("__Internal")]
        internal static extern void toFriends(string title, string description, string url, string imageBytes);

        [DllImport("__Internal")]
        internal static extern void toTimeline(string title, string description, string url, string imageBytes);

#elif UNITY_ANDROID
        static void loginWechat(string stateId) {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.WechatPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("loginWechat", stateId);
                }
            }
        }
        static bool isInstallWechat()
        {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.WechatPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                   return managerInstance.Call<bool>("isInstallWechat");
                }
            }
        }
        static void toFriends(string title, string description, string url,string imageBytes) {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.WechatPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("shareToFriends",title,description,url,imageBytes);
                }
            }
        }
        static void toTimeline(string title, string description, string url,string imageBytes) {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.WechatPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("shareToTimeline",title,description,url,imageBytes);
                }
            }
        }
#else
        public static bool isInstallWechat() {return true;}
        public static void loginWechat(string stateId) {}
        public static void toFriends(string title, string description, string url,string imageBytes) {}
        public static void toTimeline(string title, string description, string url,string imageBytes) {}
#endif
    }
}