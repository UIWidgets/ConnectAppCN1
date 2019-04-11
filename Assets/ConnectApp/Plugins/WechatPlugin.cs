using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.plugins {
    

    public class WechatPlugin {

        public static WechatPlugin instance(Action<string> codeCallBack=null)
        {
            if (plugin==null)
            {
                plugin = new WechatPlugin();
            }

            if (codeCallBack!=null)
            {
                plugin.codeCallBack = codeCallBack;
            }
            return plugin;
        }

        public static WechatPlugin plugin;

        private bool isListen;

        public BuildContext context;

        public Action<string> codeCallBack;

        public void addListener() {
            Debug.Log("addListener");
            if (!isListen) {
                Debug.Log("!isListen addListener");
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("wechat", _handleMethodCall);
                isListen = true;
            }
        }

        private void _handleMethodCall(string method, List<JSONNode> args) {
            using (WindowProvider.of(context).getScope()) {
                switch (method) {
                    case "callback": {
                        var node = args[0];
                        var dict = JSON.Parse(node);
                        var type = dict["type"];
                        if (type == "code") {
                            var code = dict["code"];
                            if (codeCallBack!=null)
                            {
                                codeCallBack(code);
                            }
                        }
                    }
                        break;
                }
            }
        }

        public void init(string appId) {
            if (!Application.isEditor)
                initWechat(appId);
        }

        public void login(string stateId) {
            if (!Application.isEditor) {
                addListener();
                loginWechat(stateId);
            }
        }

        public void shareToFriend(string title, string description, string url, byte[] imageBytes) {
            if (!Application.isEditor) {
                addListener();
                toFriends(title, description, url, imageBytes);
            }
        }

        public void shareToTimeline(string title, string description, string url, byte[] imageBytes) {
            if (!Application.isEditor) {
                addListener();
                toTimeline(title, description, url, imageBytes);
            }
        }


        public bool inInstalled() {
            if (!Application.isEditor) {
                addListener();
                return isInstallWechat();
            }

            return false;
        }
#if UNITY_IOS

        [DllImport("__Internal")]
        internal static extern void initWechat(string appId);

        [DllImport("__Internal")]
        internal static extern void loginWechat(string stateId);

        [DllImport("__Internal")]
        internal static extern bool isInstallWechat();

        [DllImport("__Internal")]
        internal static extern void toFriends(string title, string description, string url, byte[] imageBytes);

        [DllImport("__Internal")]
        internal static extern void toTimeline(string title, string description, string url, byte[] imageBytes);

#elif UNITY_ANDROID
        static void initWechat(string appId) {
        }
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
            return true;
        }
        static void toFriends(string title, string description, string url,byte[] imageBytes) {
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
        static void toTimeline(string title, string description, string url,byte[] imageBytes) {
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
        public void init(string appId) {}
        public void login(string stateId) {}
        public void shareToFriend(string title, string description, string url,byte[] imageBytes) {}
        public void shareToTimeline(string title, string description, string url,byte[] imageBytes) {}
#endif
    }
}