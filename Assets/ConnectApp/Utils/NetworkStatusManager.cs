using System;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;

namespace ConnectApp.Utils {
    public class NetworkStatusManager {
        public static bool isConnected {
            get { return _isConnected; }

            set {
                if (value != _isConnected) {
                    Promise.Delayed(TimeSpan.FromMilliseconds(1000))
                        .Then(_update);
                    _isConnected = value;
                }
            }
        }

        public static bool isAvailable {
            get { return _isAvailable; }

            set {
                if (value != _isAvailable) {
                    Promise.Delayed(TimeSpan.FromMilliseconds(1000))
                        .Then(_update);
                    _isAvailable = value;
                }
            }
        }

        static bool _isConnected = true;
        static bool _isConnectedInState = true;

        static bool _isAvailable = true;
        static bool _isAvailableInState = true;

        static void _update() {
            if (_isConnectedInState != _isConnected) {
                StoreProvider.store.dispatcher.dispatch(new SocketConnectStateAction {connected = _isConnected});
                _isConnectedInState = _isConnected;
            }

            if (_isAvailable != _isAvailableInState) {
                StoreProvider.store.dispatcher.dispatch(new NetWorkStateAction {available = _isAvailable});
                _isAvailableInState = _isAvailable;
            }
        }
    }
}