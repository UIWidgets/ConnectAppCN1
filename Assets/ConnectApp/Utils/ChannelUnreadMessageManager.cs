using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Utils {
    public class ChannelUnreadMessageManager {
        const string _unreadAfter = "UnreadAfter";

        public static void saveUnread(Dictionary<string, long> unread) {
            if (unread == null) {
                return;
            }
            var infoStr = JsonConvert.SerializeObject(unread);
            PlayerPrefs.SetString(_unreadAfter, infoStr);
            PlayerPrefs.Save();
        }
        
        public static Dictionary<string, long> getUnread() {
            var info = PlayerPrefs.GetString(_unreadAfter);
            if (info.isNotEmpty()) {
                try {
                    return JsonConvert.DeserializeObject<Dictionary<string, long>>(info);
                }
                catch (Exception e) {
                    return new Dictionary<string, long>();
                }
            }

            return new Dictionary<string, long>();
        }
        
        public static void clearUnread() {
            if (PlayerPrefs.HasKey(_unreadAfter)) {
                PlayerPrefs.DeleteKey(_unreadAfter);
            }
        }
    }
}