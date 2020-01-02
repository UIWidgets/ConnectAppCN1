using System;
using System.Collections.Generic;

namespace ConnectApp.Utils {
    public delegate void _Delegate(List<object> args);

    public static class EventBus {
        static readonly Dictionary<string, Dictionary<string, _Delegate>> _globalSubscription
            = new Dictionary<string, Dictionary<string, _Delegate>>();

        public static string subscribe(string sName, _Delegate _delegate) {
            if (!_globalSubscription.ContainsKey(key: sName)) {
                _globalSubscription[key: sName] = new Dictionary<string, _Delegate>();
            }

            var id = Guid.NewGuid().ToString();
            _globalSubscription[key: sName][key: id] = _delegate;
            return id;
        }

        public static bool unSubscribe(string sName, string id) {
            if (id == null || id.Length <= 0) {
                return false;
            }

            if (_globalSubscription.ContainsKey(key: sName) &&
                _globalSubscription[key: sName].ContainsKey(key: id)) {
                _globalSubscription[key: sName].Remove(key: id);
                return true;
            }

            return false;
        }

        public static bool publish(string sName, List<object> args) {
            if (_globalSubscription.ContainsKey(key: sName)) {
                foreach (var keyValuePair in _globalSubscription[key: sName].Values) {
                    keyValuePair(args: args);
                }

                return true;
            }

            return false;
        }
    }

    public static class EventBusConstant {
        public const string login_success = "LOGIN_SUCCEED";
        public const string logout_success = "LOGOUT_SUCCEED";
        public const string pauseVideoPlayer = "PAUSE_VIDEO_PLAYER";
        public const string fullScreen = "FULL_SCREEN";
        public const string changeOrientation = "CHANGE_ORIENTATION";
        public const string follow_user = "FOLLOW_USER";
        public const string shareAction = "SHARE_ACTION";
        public const string article_refresh = "ARTICLE_REFRESH";
        public const string article_tab = "ARTICLE_TAB";
    }
}