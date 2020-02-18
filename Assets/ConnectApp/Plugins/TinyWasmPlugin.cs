using System.Runtime.InteropServices;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Plugins {
    public class TinyWasmPlugin {
        public static void PushToTinyWasmScreen(string url, string name) {
            if (!CCommonUtils.isIPhone || !url.isNotEmpty() || !name.isNotEmpty()) {
                return;
            }
            pushToTinyWasmScreen(url: url, name: name);
            AnalyticsManager.AnalyticsTinyWasm(url: url, name: name);
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