using System.Collections.Generic;
using ConnectApp.Utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace ConnectApp.Plugins {
    public static class TinyWasmPlugin {
        public static bool isListen;
        public static void PushToTinyWasmScreen(string url, string name) {
            if (!CCommonUtils.isIPhone || !url.isNotEmpty() || !name.isNotEmpty()) {
                return;
            }
            if (!isListen) {
                isListen = true;
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("tinyWasm", _handleMethodCall);
            }
            pushToTinyWasmScreen(url: url, name: name);
            AnalyticsManager.AnalyticsTinyWasm(url: url, name: name);
        }
        
        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (GlobalContext.context != null) {
                using (WindowProvider.of(GlobalContext.context).getScope()) {
                    switch (method) {
                        case "init": {
                            Screen.orientation = ScreenOrientation.AutoRotation;
                            break;
                        }
                        case "popPage": {
                            Screen.orientation = ScreenOrientation.Portrait;
                            break;
                        }
                    }
                }
            }
        }
        
#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void pushToTinyWasmScreen(string url, string name);
#elif UNITY_ANDROID
        // TODO: TinyWasm support Android not yet 
        static void pushToTinyWasmScreen(string url, string name) { }
#else
        static void pushToTinyWasmScreen(string url, string name) { }
#endif
    }
}