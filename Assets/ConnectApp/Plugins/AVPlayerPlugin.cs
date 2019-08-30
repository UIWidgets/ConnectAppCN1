using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConnectApp.Components;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Plugins {
    public class AVPlayerPlugin {

        public static void initVideoPlayer(string url,string cookie,float left,float top,float width,float height,bool isPop) {
            if (Application.isEditor) {
                return;
            }

            VideoPlayerManager.instance.isRotation = true;
            InitPlayer(url,cookie,left,top,width,height,isPop);
            UIWidgetsMessageManager.instance.AddChannelMessageDelegate("player", _handleMethodCall);
        }

        public static void removePlayer() {
            if (Application.isEditor) {
                return;
            }
            VideoPlayerManager.instance.isRotation = false;
            VideoRelease();
            UIWidgetsMessageManager.instance.RemoveChannelMessageDelegate("player", _handleMethodCall);

        }
        
        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (JPushPlugin.context != null) {
                using (WindowProvider.of(JPushPlugin.context).getScope()) {
                    switch (method) {
                        case "PopPage": {
                            Debug.Log("PopPage ============  ");
                            StoreProvider.store.dispatcher.dispatch(new MainNavigatorPopAction());
                        }
                            break;
                        case "Share": {
                            
                        }
                            break;
                    }
                }
            }

            
        }
        


#if UNITY_IOS

        [DllImport("__Internal")]
        internal static extern void InitPlayer(string url,string cookie,float left,float top,float width,float height,bool isPop);
        
        [DllImport("__Internal")]
        internal static extern void VideoRelease();

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

        static void InitPlayer(string url,string cookie,float left,float top,float width,float height,bool isPop) {
            Plugin().Call("InitPlayer",url,cookie,left,top,width,height,isPop);
        }

        static void VideoRelease() {
            Plugin().Call("VideoRelease");
        }
#else
        static void InitPlayer(string url,string cookie,float left,float top,float width,float height,bool isPop) {}
        static void VideoRelease() {}
        
#endif
    }
    
    
    
}