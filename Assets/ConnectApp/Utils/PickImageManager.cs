using System.Runtime.InteropServices;
using UnityEngine;

namespace ConnectApp.Utils {
    public class PickImageManager {
        public static void PickImage() {
            if (Application.isEditor) {
                return;
            }

            pickImage();
        }

        public static void TakeCamera() {
            if (Application.isEditor) {
                return;
            }

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