using System;
using System.Collections.Generic;
using ConnectApp.Components;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class PreferencesManager {
        const string _initTabBarIndex = "InitTabBarIndex";
        const string _initVibrate = "InitVibrate";
        const string _initKingKong = "InitKingKong";
        static int _tabIndex = 0;
        static bool _isVibrate = false;

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

        public static void updateVibrate(bool vibrate) {
            if (_isVibrate == vibrate) {
                return;
            }

            _isVibrate = vibrate;
            PlayerPrefs.SetInt(key: _initVibrate, value: Convert.ToInt32(_isVibrate));
            PlayerPrefs.Save();
        }

        public static bool initVibrate() {
            if (PlayerPrefs.HasKey(_initVibrate)) {
                var _vibrate = PlayerPrefs.GetInt(key: _initVibrate);
                _isVibrate = Convert.ToBoolean(_vibrate);
                return _isVibrate;
            }

            _isVibrate = true;
            return true;
        }

        public static void updateKingKongType(KingKongType type) {
            var kingKongTypes = initKingKongType();
            if (kingKongTypes.Contains(item: type)) {
                return;
            }

            kingKongTypes.Add(item: type);
            var newKingKongType = JsonConvert.SerializeObject(value: kingKongTypes);
            PlayerPrefs.SetString(key: _initKingKong, value: newKingKongType);
            PlayerPrefs.Save();
        }

        public static List<KingKongType> initKingKongType() {
            var kingKongType = PlayerPrefs.GetString(key: _initKingKong);
            return kingKongType.isNotEmpty()
                ? JsonConvert.DeserializeObject<List<KingKongType>>(value: kingKongType)
                : new List<KingKongType>();
        }
    }
}