using UnityEngine;
using System.Runtime.InteropServices;

namespace ConnectApp.Plugins {
    public static class TextInputPlugin {
        public static void TextInputShow() {
            if (Application.isEditor) {
                return;
            }

            UIWidgetsTextInputShow();
        }

        public static void TextInputHide() {
            if (Application.isEditor) {
                return;
            }

            UIWidgetsTextInputHide();
        }

#if UNITY_IOS
        [DllImport ("__Internal")]
        static extern void UIWidgetsTextInputShow();
        
        [DllImport ("__Internal")]
        static extern void UIWidgetsTextInputHide();
#elif UNITY_ANDROID
        static void UIWidgetsTextInputShow() {
            using (
                AndroidJavaClass pluginClass =
                    new AndroidJavaClass("com.unity.uiwidgets.plugin.editing.TextInputPlugin")
            ) {
                pluginClass.CallStatic("show");
            }
        }

        static void UIWidgetsTextInputHide() {
            using (
                AndroidJavaClass pluginClass =
                    new AndroidJavaClass("com.unity.uiwidgets.plugin.editing.TextInputPlugin")
            ) {
                pluginClass.CallStatic("hide");
            }
        }
#else
        static void UIWidgetsTextInputShow() {
        }

        static void UIWidgetsTextInputHide() {
        }
#endif
    }
}