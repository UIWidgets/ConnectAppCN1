using System;

namespace ConnectApp.models {
    [Serializable]
    public class LiveState {
        
        public bool loading;
        public string detailId;
        public LiveInfo liveInfo;
        public bool showChatWindow;
        public bool openChatWindow;
    }
}