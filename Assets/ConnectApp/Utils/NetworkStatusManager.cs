using System;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;

namespace ConnectApp.Utils {
    public static class NetworkStatusManager {
        public static bool isConnected {
            get { return _isConnected; }

            set {
                if (value != _isConnected) {
                    _isConnected = value;
                    _updateConnectedState();
                }
            }
        }

        public static bool isAvailable {
            get { return _isAvailable; }

            set {
                if (value != _isAvailable) {
                    Promise.Delayed(TimeSpan.FromMilliseconds(_isAvailable ? 5000 : 1000))
                        .Then(onResolved: _updateShowNoNetworkBanner);
                    _isShowNoNetworkBanner = value;
                    Promise.Delayed(TimeSpan.FromMilliseconds(1000))
                        .Then(onResolved: _updateAvailableState);
                    _isAvailable = value;
                }
            }
        }

        static bool _isConnected = true;
        static bool _isConnectedInState = true;

        static bool _isAvailable;
        static bool _isAvailableInState;

        static bool _isShowNoNetworkBanner;
        static bool _isShowNoNetworkBannerInState;
        
        static void _updateConnectedState() {
            if (_isConnectedInState == _isConnected) {
                return;
            }

            StoreProvider.store.dispatcher.dispatch(new SocketConnectStateAction {connected = _isConnected});
            _isConnectedInState = _isConnected;
        }

        static void _updateAvailableState() {
            if (_isAvailable == _isAvailableInState) {
                return;
            }

            StoreProvider.store.dispatcher.dispatch(new NetworkAvailableStateAction {available = _isAvailable});
            _isAvailableInState = _isAvailable;
        }

        static void _updateShowNoNetworkBanner() {
            if (_isShowNoNetworkBanner == _isShowNoNetworkBannerInState) {
                return;
            }

            StoreProvider.store.dispatcher.dispatch(new DismissNoNetworkBannerAction
                {isDismiss = _isShowNoNetworkBanner});
            _isShowNoNetworkBannerInState = _isShowNoNetworkBanner;
        }
    }
}