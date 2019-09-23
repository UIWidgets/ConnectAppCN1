using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Utils {
    public class ChannelTopManager {
        const string _channelTop = "ChannelTop";

        public static void saveChannelTop(Dictionary<string, bool> unread) {
            if (unread == null) {
                return;
            }
            var infoStr = JsonConvert.SerializeObject(unread);
            PlayerPrefs.SetString(_channelTop, infoStr);
            PlayerPrefs.Save();
        }
        
        public static Dictionary<string, bool> getChannelTop() {
            var info = PlayerPrefs.GetString(_channelTop);
            if (info.isNotEmpty()) {
                try {
                    return JsonConvert.DeserializeObject<Dictionary<string, bool>>(info);
                }
                catch (Exception e) {
                    return new Dictionary<string, bool>();
                }
            }

            return new Dictionary<string, bool>();
        }
        
        public static void clearChannelTop() {
            if (PlayerPrefs.HasKey(_channelTop)) {
                PlayerPrefs.DeleteKey(_channelTop);
            }
        }
    }
}