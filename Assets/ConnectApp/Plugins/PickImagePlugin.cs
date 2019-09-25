using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConnectApp.Components;
using ConnectApp.Utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Plugins {
    public enum ImageSource {
        camera,
        gallery
    }

    public static class PickImagePlugin {
        static Action<string> _imageCallBack;

        static void addListener() {
            if (Application.isEditor) {
                return;
            }

            UIWidgetsMessageManager.instance.AddChannelMessageDelegate("pickImage", del: _handleMethodCall);
        }

        static void removeListener() {
            if (Application.isEditor) {
                return;
            }

            UIWidgetsMessageManager.instance.RemoveChannelMessageDelegate("pickImage", del: _handleMethodCall);
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (GlobalContext.context != null) {
                using (WindowProvider.of(context: GlobalContext.context).getScope()) {
                    switch (method) {
                        case "success": {
                            var node = args[0];
                            var dict = JSON.Parse(aJSON: node);
                            var image = (string) dict["image"];
                            if (image != null) {
                                _imageCallBack?.Invoke(obj: image);
                            }

                            removeListener();
                            break;
                        }
                        case "cancel": {
                            removeListener();
                            break;
                        }
                    }
                }
            }
        }

        public static void PickImage(
            ImageSource source,
            Action<string> imageCallBack = null,
            bool cropped = true,
            int? maxSize = null
        ) {
            if (Application.isEditor) {
                return;
            }

            addListener();
            _imageCallBack = imageCallBack;
            var sourceInt = (int) source;
            pickImage(sourceInt.ToString(), cropped: cropped, maxSize ?? 0);
        }

        public static void PickVideo(ImageSource source) {
            if (Application.isEditor) {
                return;
            }

            addListener();
            var sourceInt = (int) source;
            pickVideo(sourceInt.ToString());
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void pickImage(string source, bool cropped = true, int maxSize = 0);

        [DllImport("__Internal")]
        static extern void pickVideo(string source);
#elif UNITY_ANDROID
        static AndroidJavaClass _plugin;

        static AndroidJavaClass Plugin() {
            if (_plugin == null) {
                _plugin = new AndroidJavaClass("com.unity3d.unityconnect.plugins.CommonPlugin");
            }

            return _plugin;
        }

        static void pickImage(string source, bool cropped = true, int maxSize = 0) {
            Plugin().CallStatic("pickImage", source, cropped, maxSize);
        }

        static void pickVideo(string source) {
            Plugin().CallStatic("pickVideo", source);
        }
#else
        static void pickImage(string source, bool cropped = true, int maxSize = 0) { }
        static void pickVideo(string source) { }
#endif
    }
}