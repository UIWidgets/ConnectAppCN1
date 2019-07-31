using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;
using ConnectApp.Api;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Plugins {
    public static class QRScanPlugin {

        public static BuildContext context;
        static bool isListen;

        static void addListener() {
            if (Application.isEditor) {
                return;
            }

            if (!isListen) {
                isListen = true;
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("QRScan", del: _handleMethodCall);
            }
        }

        static void removeListener() {
            if (Application.isEditor) {
                return;
            }

            if (isListen) {
                isListen = false;
                UIWidgetsMessageManager.instance.RemoveChannelMessageDelegate("QRScan", del: _handleMethodCall);
            }
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (context != null) {
                using (WindowProvider.of(context: context).getScope()) {
                    switch (method) {
                        case "OnReceiveQRCode": {
                            removeListener();
                            string qrCode = args[0];
                            if (qrCode.StartsWith("http://") || qrCode.StartsWith("https://")) {
                                var uri = new Uri(uriString: qrCode);
                                if (uri.AbsolutePath.StartsWith("https://connect")) {
                                    var token = HttpUtility.ParseQueryString(query: uri.Query).Get("token");
                                    LoginApi.LoginByQr(token: token);
                                }
                            }
                        }
                            break;
                    }
                }
            }
        }

        public static void PushToQRScan() {
            addListener();
            pushToQRScan();
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void pushToQRScan();
#elif UNITY_ANDROID

        static AndroidJavaObject _plugin;

        static AndroidJavaObject Plugin() {
            if (_plugin == null) {
                using (
                    AndroidJavaClass managerClass =
                        new AndroidJavaClass("com.unity3d.unityconnect.plugins.QRScanPlugin")
                ) {
                    _plugin = managerClass.CallStatic<AndroidJavaObject>("getInstance");
                }
            }

            return _plugin;
        }

        static void pushToQRScan() {
            Plugin().Call("pushToQRScan");
        }
#else
        static void pushToQRScan() {}
#endif
    }
}