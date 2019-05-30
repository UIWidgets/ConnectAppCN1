using System;

namespace ConnectApp.Models.State {
    [Serializable]
    public class LiveState {
        public bool loading;
        public string detailId;
        public bool showChatWindow;
        public bool openChatWindow;
    }
}