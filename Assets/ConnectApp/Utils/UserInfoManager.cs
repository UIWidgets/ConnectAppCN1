using System.Collections.Generic;
using ConnectApp.models;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.utils
{
    public class UserInfoManager
    {
        private const string USERINFO= "UserInfo";
        
        public static void saveUserInfo(LoginInfo loginInfo)
        {
            var list = new List<LoginInfo>{loginInfo};
            var infoStr = JsonConvert.SerializeObject(list);
            PlayerPrefs.SetString(USERINFO,infoStr);
            PlayerPrefs.Save();
        }
        public static LoginInfo initUserInfo()
        {
            var info = PlayerPrefs.GetString(USERINFO);
            if (info.isNotEmpty())
            {
              var list= JsonConvert.DeserializeObject<List<LoginInfo>>(info);
              var loginInfo = list.first();
              return loginInfo;
            }
            return new LoginInfo();
        }

        public static bool isLogin()
        {
            var info = PlayerPrefs.GetString(USERINFO);
            return info.isNotEmpty();
        }

        public static Dictionary<string, User> initUserDict()
        {
            var info = initUserInfo();
            if (info.userId.isEmpty())
            {
                return new Dictionary<string, User>();
            }
            var user  = new User();
            user.fullName = info.userFullName;
            user.id = info.userId;
            user.avatar = info.userAvatar;
            return new Dictionary<string, User>{{user.id,user}};
        }

        public static void clearUserInfo()
        {
           PlayerPrefs.DeleteKey(USERINFO);

        }
    }
}
