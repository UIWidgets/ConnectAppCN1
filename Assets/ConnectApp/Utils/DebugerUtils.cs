namespace ConnectApp.Utils {
    public class DebugerUtils {
        public static void DebugAssert(bool condition, string logMsg) {
            if (!condition) {
                Debuger.Log(message: logMsg);
            }
        }
    }
}