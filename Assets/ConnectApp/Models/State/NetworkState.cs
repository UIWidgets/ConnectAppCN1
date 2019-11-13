using System;

namespace ConnectApp.Models.State {
    [Serializable]
    public class NetworkState {
        public bool networkConnected;
        public bool dismissNoNetworkBanner;
    }
}