using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Plugins {
    public static class QRScanPlugin {
        static bool isListen;
        public static string qrCodeToken;
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

                _loginSubId = EventBus.subscribe(sName: EventBusConstant.login_success, _ => {
                    if (qrCodeToken.isNotEmpty()) {
                        checkToken(token: qrCodeToken);
                        qrCodeToken = null;
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

        static void checkToken(string token) {
            CustomDialogUtils.showCustomDialog(
                child: new CustomLoadingDialog(
                    message: "验证中"
                )
            );
            LoginApi.LoginByQr(token: token, "check").Then(success => {
                CustomDialogUtils.hiddenCustomDialog();
                StoreProvider.store.dispatcher.dispatch(
                    new MainNavigatorPushToQRScanLoginAction {
                        token = token
                    }
                );
                CustomDialogUtils.showToast("验证成功", iconData: Icons.sentiment_satisfied);
                AnalyticsManager.AnalyticsQRScan(state: QRState.check);
            }).Catch(error => {
                CustomDialogUtils.hiddenCustomDialog();
                CustomDialogUtils.showToast("验证失败", iconData: Icons.sentiment_dissatisfied);
                Promise.Delayed(new TimeSpan(0, 0, 1))
                    .Then(() => {
                        PushToQRScan();
                        AnalyticsManager.AnalyticsQRScan(state: QRState.check, false);
                    });
            });
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (GlobalContext.context != null) {
                using (WindowProvider.of(context: GlobalContext.context).getScope()) {
                    switch (method) {
                        case "OnReceiveQRCode": {
                            string qrCode = args[0];
                            if (qrCode.isUrl()) {
                                var uri = new Uri(uriString: qrCode);
                                if (uri.AbsoluteUri.StartsWith("https://connect")) {
                                    var token = HttpUtility.ParseQueryString(query: uri.Query).Get("token");
                                    if (token.isNotEmpty()) {
                                        var isLoggedIn = StoreProvider.store.getState().loginState.isLoggedIn;
                                        if (isLoggedIn) {
                                            checkToken(token: token);
                                        }
                                        else {
                                            qrCodeToken = token;
                                            StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToAction {
                                                routeName = MainNavigatorRoutes.Login
                                            });
                                        }
                                    }
                                    else {
                                        CustomDialogUtils.showToast("暂不支持该二维码类型", Icons.sentiment_dissatisfied);
                                    }
                                }
                                else {
                                    CustomDialogUtils.showToast("暂不支持该二维码类型", Icons.sentiment_dissatisfied);
                                }
                            }
                            else if (!qrCode.Equals("pop")) {
                                CustomDialogUtils.showToast("暂不支持该二维码类型", Icons.sentiment_dissatisfied);
                            }

                            StatusBarManager.hideStatusBar(false);
                            StatusBarManager.statusBarStyle(isLight: StoreProvider.store.getState().loginState
                                .isLoggedIn);
                            removeListener();
                        }
                            break;
                    }
                }
            }
        }

        public static void PushToQRScan() {
            addListener();
            if (!Application.isEditor) {
                pushToQRScan();
                AnalyticsManager.AnalyticsQRScan(state: QRState.click);
            }
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