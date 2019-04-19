using System.Collections.Generic;
using ConnectApp.models;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.utils {
    public static class UserInfoManager {
        private const string _userInfo = "UserInfo";

        public static void saveUserInfo(LoginInfo loginInfo) {
            if (loginInfo == null) return;

            var list = new List<LoginInfo> {loginInfo};
            var infoStr = JsonConvert.SerializeObject(list);
            PlayerPrefs.SetString(_userInfo, infoStr);
            PlayerPrefs.Save();
        }

        public static LoginInfo initUserInfo() {
            var info = PlayerPrefs.GetString(_userInfo);
            if (info.isNotEmpty()) {
                var list = JsonConvert.DeserializeObject<List<LoginInfo>>(info);
                var loginInfo = list.first();
                return loginInfo;
            }

            return new LoginInfo();
        }

        public static bool isLogin() {
            var info = PlayerPrefs.GetString(_userInfo);
            return info.isNotEmpty();
        }

        public static Dictionary<string, User> initUserDict() {
            var info = initUserInfo();
            if (info.userId.isEmpty()) return new Dictionary<string, User>();
            var user = new User {
                fullName = info.userFullName,
                id = info.userId,
                avatar = info.userAvatar
            };
            return new Dictionary<string, User> {{user.id, user}};
        }

        public static void clearUserInfo() {
            if (PlayerPrefs.HasKey(_userInfo))
                PlayerPrefs.DeleteKey(_userInfo);
        }
    }
}