using System;
using System.Collections.Generic;

namespace ConnectApp.utils {
    public delegate void _Delegate(List<object> args);

    public static class EventBus {
        static readonly Dictionary<string, Dictionary<string, _Delegate>> _globalSubscription
            = new Dictionary<string, Dictionary<string, _Delegate>>();

        public static string subscribe(string sName, _Delegate _delegate) {
            if (!_globalSubscription.ContainsKey(sName)) {
                _globalSubscription[sName] = new Dictionary<string, _Delegate>();
            }

            var id = Guid.NewGuid().ToString();
            _globalSubscription[sName][id] = _delegate;
            return id;
        }

        public static bool unSubscribe(string sName, string id) {
            if (id == null || id.Length <= 0) {
                return false;
            }

            if (_globalSubscription.ContainsKey(sName) &&
                _globalSubscription[sName].ContainsKey(id)) {
                _globalSubscription[sName].Remove(id);
                return true;
            }

            return false;
        }

        public static bool publish(string sName, List<object> args) {
            if (_globalSubscription.ContainsKey(sName)) {
                foreach (var keyValuePair in _globalSubscription[sName].Values) {
                    keyValuePair(args);
                }

                return true;
            }

            return false;
        }
    }

    public static class EventBusConstant {
        public const string login_success = "LOGIN_SUCCEED";
        public const string pauseVideoPlayer = "PAUSE_VIDEO_PLAYER";
        public const string refreshNotifications = "REFRESH_NOTIFICATIONS";
    }
}