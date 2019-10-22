using System;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;

namespace ConnectApp.Utils {
    public class NetworkStatusManager {
        public static bool isConnected {
            get {
                return _isConnected;
            }

            set {
                if (value != _isConnected) {
                    Promise.Delayed(TimeSpan.FromMilliseconds(1000))
                        .Then(_update);
                    _isConnected = value;
                }
            }
        }

        static bool _isConnected = true;
        static bool _isConnectedInState = true;

        static void _update() {
            if (_isConnectedInState != _isConnected) {
                StoreProvider.store.dispatcher.dispatch(new SocketConnectStateAction {connected = _isConnected});
                _isConnectedInState = _isConnected;
            }
        }
    }
}