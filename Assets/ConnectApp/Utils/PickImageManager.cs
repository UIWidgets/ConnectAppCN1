using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConnectApp.Components;
using ConnectApp.Plugins;
using UnityEngine;

namespace ConnectApp.Utils {
    public class PickImageManager {
        public static void showActionSheet() {
            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    "拍照",
                    type: ActionType.normal,
                    TakeCamera
                ),
                new ActionSheetItem("从相册选择", type: ActionType.normal, PickImage),
                new ActionSheetItem("取消", type: ActionType.cancel)
            };

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                items: items
            ));
        }


        public static void PickImage() {
            if (Application.isEditor) {
                return;
            }

            PickImagePlugin.addListener();
            pickImage();
        }

        public static void TakeCamera() {
            if (Application.isEditor) {
                return;
            }

            PickImagePlugin.addListener();
            takeCamera();
        }
#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void pickImage();

        [DllImport("__Internal")]
        static extern void takeCamera();
#elif UNITY_ANDROID
        static AndroidJavaClass _plugin;

        static AndroidJavaClass Plugin() {
            if (_plugin == null) {
                _plugin = new AndroidJavaClass("com.unity3d.unityconnect.plugins.CommonPlugin");
            }

            return _plugin;
        }

        static void pickImage() {
            Plugin().CallStatic("pickImage");
        }
        static void takeCamera() {
            Plugin().CallStatic("takeCamera");
        }
#else
        static void pickImage() {
        }
        static void takeCamera() {
        }
#endif
    }
}