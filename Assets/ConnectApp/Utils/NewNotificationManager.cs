using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class NewNotificationManager {
        const string _newNotifications = "NewNotifications";

        public static void saveNewNotification(string userId, string newNotification) {
            var dict = getNewNotifications();
            if (dict.ContainsKey(userId) && newNotification == null) {
                dict.Remove(userId);
            }
            else {
                dict[userId] = newNotification;
            }
            var infoStr = JsonConvert.SerializeObject(dict);
            PlayerPrefs.SetString(_newNotifications, infoStr);
            PlayerPrefs.Save();
        }

        public static string getNewNotification(string userId) {
            var dict = getNewNotifications();
            return dict.ContainsKey(userId) ? dict[userId] : null;
        }

        public static Dictionary<string, string> getNewNotifications() {
            if (!PlayerPrefs.HasKey(_newNotifications)) {
                return new Dictionary<string, string>();
            }

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(PlayerPrefs.GetString(_newNotifications));
        }

        public static void clearNewNotifications() {
            if (PlayerPrefs.HasKey(_newNotifications)) {
                PlayerPrefs.DeleteKey(_newNotifications);
            }
        }
    }
}