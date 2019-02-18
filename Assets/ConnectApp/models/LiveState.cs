using System;

namespace ConnectApp.models {
    [Serializable]
    public class LiveState {
        public bool loading;
        public LiveInfo liveInfo;
        public bool showChatWindow;
        public bool openChatWindow;
    }
}