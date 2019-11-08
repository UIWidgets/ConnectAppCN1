using UnityEngine;

namespace ConnectApp.Utils {
    public class PreferencesManager {
        const string _initTabBarIndex = "InitTabBarIndex";
        static int _tabIndex = 0;

        public static void updateTabIndex(int tabIndex) {
            if (_tabIndex == tabIndex) {
                return;
            }

            _tabIndex = tabIndex == 2 ? 2 : 0;
            PlayerPrefs.SetInt(_initTabBarIndex, _tabIndex);
            PlayerPrefs.Save();
        }

        public static int initTabIndex() {
            _tabIndex = PlayerPrefs.GetInt(_initTabBarIndex);
            return _tabIndex;
        }
    }
}