using UnityEngine;

namespace ConnectApp.Utils {
    public static class PreferencesManager {
        const string _initTabBarIndex = "InitTabBarIndex";
        static int _tabIndex = 0;

        public static void updateTabIndex(int tabIndex) {
            var newTabIndex = tabIndex == 2 ? 2 : 0;
            if (_tabIndex == newTabIndex) {
                return;
            }
            _tabIndex = newTabIndex;
            PlayerPrefs.SetInt(key: _initTabBarIndex, value: _tabIndex);
            PlayerPrefs.Save();
        }

        public static int initTabIndex() {
            _tabIndex = PlayerPrefs.GetInt(key: _initTabBarIndex);
            return _tabIndex;
        }
    }
}