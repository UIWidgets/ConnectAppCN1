#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace ConnectApp.Plugins {
    public class FPSLabelPlugin {
        public static void SwitchFPSLabelShowStatus(bool isOpen) {
            switchFPSLabelShowStatus(isOpen: isOpen);
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void switchFPSLabelShowStatus(bool isOpen);
#elif UNITY_ANDROID
        // TODO: FPSLabel support Android not yet 
        static void switchFPSLabelShowStatus(bool isOpen) { }
#else
        static void switchFPSLabelShowStatus(bool isOpen) { }
#endif
    }
}