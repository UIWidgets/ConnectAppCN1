using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ConnectApp.Models.Model;
using ConnectApp.Plugins;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class UserInfoManager {
        const string UserInfoKey = "UserInfo";

        public static void saveUserInfo(LoginInfo loginInfo) {
            if (loginInfo == null) {
                return;
            }

            var list = new List<LoginInfo> {loginInfo};
            var infoStr = JsonConvert.SerializeObject(list);
            PlayerPrefs.SetString(key: UserInfoKey, infoStr);
            PlayerPrefs.Save();
        }

        public static LoginInfo getUserInfo() {
            var info = PlayerPrefs.GetString(key: UserInfoKey);
            if (info.isNotEmpty()) {
                var list = JsonConvert.DeserializeObject<List<LoginInfo>>(info);
                var loginInfo = list.first();
                return loginInfo;
            }

            return new LoginInfo();
        }

        public static bool isLogin() {
            var info = PlayerPrefs.GetString(key: UserInfoKey);
            return info.isNotEmpty();
        }

        public static Dictionary<string, User> getUserInfoDict() {
            var info = getUserInfo();
            if (info.userId.isEmpty()) {
                return new Dictionary<string, User>();
            }

            var user = new User {
                fullName = info.userFullName,
                id = info.userId,
                avatar = info.userAvatar,
                title = info.title,
                coverImage = info.coverImageWithCDN
            };
            return new Dictionary<string, User> {{user.id, user}};
        }

        public static void clearUserInfo() {
            if (PlayerPrefs.HasKey(key: UserInfoKey)) {
                PlayerPrefs.DeleteKey(key: UserInfoKey);
            }

            JPushPlugin.registerHmsToken();
        }

        public static bool isMe(string userId) {
            return userId == getUserInfo().userId;
        }
    }
}