using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Plugins {
    public class AVPlayerPlugin {
        public static bool isExistPlayer;

        public static void initVideoPlayer(string url, string cookie, float left, float top, float width, float height,
            bool isPop, bool needUpdate = false) {
            if (Application.isEditor) {
                return;
            }

            if (isExistPlayer) {
                return;
            }

            isExistPlayer = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
            var ratio = Application.platform == RuntimePlatform.Android ? Window.instance.devicePixelRatio : 1.0f;
            InitPlayer(url, cookie, left, top * ratio, width * ratio, height * ratio, isPop, needUpdate);
            UIWidgetsMessageManager.instance.AddChannelMessageDelegate("player", _handleMethodCall);
        }

        public static void configVideoPlayer(string url, string cookie) {
            if (Application.isEditor) {
                return;
            }

            ConfigPlayer(url, cookie);
        }

        public static void removePlayer() {
            if (Application.isEditor) {
                return;
            }

            Screen.orientation = ScreenOrientation.Portrait;
            isExistPlayer = false;
            VideoRelease();
            UIWidgetsMessageManager.instance.RemoveChannelMessageDelegate("player", _handleMethodCall);
        }

        public static void hiddenPlayer() {
            if (Application.isEditor) {
                return;
            }

            Screen.orientation = ScreenOrientation.Portrait;
            VideoPause();
            VideoHidden();
        }

        public static void showPlayer() {
            if (Application.isEditor) {
                return;
            }

            if (isExistPlayer) {
                Screen.orientation = ScreenOrientation.AutoRotation;
            }

            VideoShow();
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (JPushPlugin.context != null) {
                using (WindowProvider.of(JPushPlugin.context).getScope()) {
                    switch (method) {
                        case "PopPage": {
                            StoreProvider.store.dispatcher.dispatch(new MainNavigatorPopAction());
                            break;
                        }
                        case "Share": {
                            EventBus.publish(EventBusConstant.shareAction, null);
                            break;
                        }
                        case "BuyLincese": {
                            StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToWebViewAction {
                                url = "https://www.baidu.com"
                            });
                            break;
                        }
                        case "UpdateLincese": {
                            StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToWebViewAction {
                                url = "https://www.baidu.com"
                            });
                            break;
                        }
                    }
                }
            }
        }


#if UNITY_IOS
        [DllImport("__Internal")]
        internal static extern void InitPlayer(string url, string cookie, float left, float top, float width,
            float height, bool isPop, bool needUpdate);

        [DllImport("__Internal")]
        internal static extern void ConfigPlayer(string url, string cookie);

        [DllImport("__Internal")]
        internal static extern void VideoRelease();

        [DllImport("__Internal")]
        internal static extern void VideoPause();

        [DllImport("__Internal")]
        internal static extern void VideoPlay();

        [DllImport("__Internal")]
        internal static extern void VideoHidden();

        [DllImport("__Internal")]
        internal static extern void VideoShow();

#elif UNITY_ANDROID
        static AndroidJavaObject _plugin;

        static AndroidJavaObject Plugin() {
            if (_plugin == null) {
                using (
                    AndroidJavaClass managerClass =
                        new AndroidJavaClass("com.unity3d.unityconnect.plugins.AVPlayerPlugin")
                ) {
                    _plugin = managerClass.CallStatic<AndroidJavaObject>("getInstance");
                }
            }

            return _plugin;
        }

        static void InitPlayer(string url, string cookie, float left, float top, float width, float height,
            bool isPop) {
            Plugin().Call("InitPlayer", url, cookie, left, top, width, height, isPop);
        }

        static void VideoRelease() {
            Plugin().Call("VideoRelease");
        }
        
        static void VideoPause() {
            Plugin().Call("VideoPause");
        }

        static void VideoPlay() {
            Plugin().Call("VideoPlay");
        }
        
        static void VideoHidden() {
            Plugin().Call("VideoHidden");
        }

        static void VideoShow() {
            Plugin().Call("VideoShow");
        }
#else
        static void InitPlayer(string url,string cookie,float left,float top,float width,float height,bool isPop) {}
        static void VideoRelease() {}
        
#endif
    }
}