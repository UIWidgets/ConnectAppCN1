using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;
using ConnectApp.Main;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Plugins {
    public static class QRScanPlugin {

        public static BuildContext context;
        static bool isListen;
        static string _token;
        static string _loginSubId;

        static void addListener() {
            if (Application.isEditor) {
                return;
            }

            if (!isListen) {
                isListen = true;
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("QRScan", del: _handleMethodCall);
                if (_loginSubId.isNotEmpty()) {
                    EventBus.unSubscribe(sName: EventBusConstant.login_success, id: _loginSubId);
                }
                _loginSubId = EventBus.subscribe(sName: EventBusConstant.login_success, args => {
                    if (_token.isNotEmpty()) {
                        StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToQRScanLoginAction {
                            token = _token
                        });
                        _token = null;
                    }
                });
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
                            string qrCode = args[0];
                            if (qrCode.StartsWith("http://") || qrCode.StartsWith("https://")) {
                                var uri = new Uri(uriString: qrCode);
                                if (uri.AbsoluteUri.StartsWith("https://connect")) {
                                    var token = HttpUtility.ParseQueryString(query: uri.Query).Get("token");
                                    if (token.isNotEmpty()) {
                                        var isLoggedIn = StoreProvider.store.getState().loginState.isLoggedIn;
                                        if (isLoggedIn) {
                                            StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToQRScanLoginAction {
                                                token = token
                                            });
                                        }
                                        else {
                                            _token = token;
                                            StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToAction {
                                                routeName = MainNavigatorRoutes.Login
                                            });
                                        }
                                    }
                                }
                                else {
                                    StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToWebViewAction {url = qrCode});
                                }
                            }
                            removeListener();
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