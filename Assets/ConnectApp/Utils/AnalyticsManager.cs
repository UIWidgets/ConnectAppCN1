using System;
using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Plugins;
using Unity.UIWidgets.foundation;
using UnityEngine;
#if UNITY_IOS
using System.Runtime.InteropServices;

#endif

namespace ConnectApp.Utils {
    public enum QRState {
        click,
        check,
        confirm,
        cancel
    }

    public enum FavoriteTagType {
        create,
        edit,
        delete
    }

    public static class AnalyticsManager {
        public static string foucsTime;

        // tab点击统计
        public static void ClickHomeTab(int fromIndex, int toIndex) {
            if (Application.isEditor) {
                return;
            }

            List<string> tabs = new List<string> {
                "Article", "Event", "Notification", "Mine"
            };
            List<string> entries = new List<string> {
                "Article_EnterArticle", "Event_EnterEvent", "Notification_EnterNotification", "Mine_EnterMine"
            };
            var mEventId = $"Click_Tab_{entries[index: toIndex]}";
            var extras = new Dictionary<string, string>();
            extras.Add("from", tabs[index: fromIndex]);
            extras.Add("to", tabs[index: toIndex]);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        // tab点击统计
        public static void ClickEventSegment(string from, string type) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Event_Segment";
            var extras = new Dictionary<string, string>();
            extras.Add("type", value: type);
            extras.Add("from", value: from);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        //search点击事件统计
        public static void ClickEnterSearch(string from) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Enter_Search";
            var extras = new Dictionary<string, string>();
            extras.Add("from", value: from);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        //进入文章详情
        public static void ClickEnterArticleDetail(string from, string articleId, string articleTitle) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Enter_ArticleDetail";
            var extras = new Dictionary<string, string>();
            extras.Add("from", value: from);
            extras.Add("id", value: articleId);
            extras.Add("title", value: articleTitle);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickReturnArticleDetail(string articleId, string articleTitle) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Return_ArticleDetail";
            var extras = new Dictionary<string, string>();
            extras.Add("id", value: articleId);
            extras.Add("title", value: articleTitle);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        //进入活动详情
        public static void ClickEnterEventDetail(string from, string eventId, string eventTitle, string type) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Enter_EventDetail";
            var extras = new Dictionary<string, string>();
            extras.Add("from", value: from);
            extras.Add("id", value: eventId);
            extras.Add("title", value: eventTitle);
            extras.Add("type", value: type);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        //eventClick
        public static void ClickShare(ShareType shareType, string type, string objectId, string title) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Event_Share";
            var extras = new Dictionary<string, string>();
            extras.Add("shareType", shareType.ToString());
            extras.Add("type", value: type);
            extras.Add("id", value: objectId);
            extras.Add("title", value: title);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }


        public static void ClickLike(string type, string articleId, string commentId = null) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Event_Like";
            var extras = new Dictionary<string, string>();
            extras.Add("type", value: type);
            extras.Add("id", value: articleId);
            if (commentId != null) {
                extras.Add("commentId", value: commentId);
            }

            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickComment(string type, string channelId, string title, string commentId = null) {
            if (Application.isEditor) {
                return;
            }

            const string mEventId = "Click_Event_Comment";
            var extras = new Dictionary<string, string> {
                {"type", type},
                {"channelId", channelId},
                {"title", title}
            };
            if (commentId != null && commentId.isNotEmpty()) {
                extras.Add("commentId", value: commentId);
            }

            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickPublishComment(string type, string channelId, string commentId = null) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Event_PublishComment";
            var extras = new Dictionary<string, string>();
            extras.Add("type", value: type);
            extras.Add("channelId", value: channelId);
            if (commentId != null) {
                extras.Add("commentId", value: commentId);
            }

            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickNotification(string type, string subtype, string id) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Notification";
            var extras = new Dictionary<string, string>();
            extras.Add("type", value: type);
            extras.Add("subtype", value: subtype);
            extras.Add("id", value: id);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickSplashPage(string id, string name, string url) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Splash_Page";
            var extras = new Dictionary<string, string>();
            extras.Add("id", value: id);
            extras.Add("name", value: name);
            extras.Add("url", value: url);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickSkipSplashPage(string id, string name, string url) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Skip_Splash_Page";
            var extras = new Dictionary<string, string>();
            extras.Add("id", value: id);
            extras.Add("name", value: name);
            extras.Add("url", value: url);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }


        public static void ClickHottestSearch(string keyWord) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Search_Hottest_Search";
            var extras = new Dictionary<string, string>();
            extras.Add("keyWord", value: keyWord);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickHistorySearch(string keyWord) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Search_History_Search";
            var extras = new Dictionary<string, string>();
            extras.Add("keyWord", value: keyWord);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void SignUpOnlineEvent(string eventId, string title) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Sign_Up_Online_Event";
            var extras = new Dictionary<string, string>();
            extras.Add("id", value: eventId);
            extras.Add("title", value: title);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public enum MineType {
            Event,
            History,
            Settings
        }

        public static void ClickEnterMine(MineType type) {
            //进入我的
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Enter_Mine";
            var extras = new Dictionary<string, string>();
            extras.Add("type", type.ToString());
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickSetGrade() {
            //评分
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Set_Grade";
            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickEnterAboutUs() {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Enter_AboutUs";
            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickCheckUpdate() {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Check_Update";
            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void ClickClearCache() {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Clear_Cache";
            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void EnterOnOpenUrl(string url) {
            //通过openurl方式打开app
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Enter_On_OpenUrl";
            var extras = new Dictionary<string, string>();
            extras.Add("url", value: url);
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void EnterApp() {
//            进入app事件
            if (Application.isEditor) {
                return;
            }

            foucsTime = DateTime.UtcNow.ToString();
            var mEventId = "Enter_App";
            var extras = new Dictionary<string, string>();
            extras.Add("app", "unity connect");
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }


        public static void LoginEvent(string loginType) {
            if (Application.isEditor) {
                return;
            }

            JAnalyticsPlugin.Login(loginType: loginType);
        }

        public static void ClickLogout() {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Logout";
            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        public static void BrowseArtileDetail(string id, string name, DateTime startTime, DateTime endTime) {
            if (Application.isEditor) {
                return;
            }

            string duration = (endTime - startTime).TotalSeconds.ToString("0.0");
            JAnalyticsPlugin.BrowseEvent(eventId: id, name: name, "ArticleDetail", duration: duration, null);
        }

        public static void BrowseEventDetail(string id, string name, DateTime startTime, DateTime endTime) {
            if (Application.isEditor) {
                return;
            }

            string duration = (endTime - startTime).TotalSeconds.ToString("0.0");
            JAnalyticsPlugin.BrowseEvent(eventId: id, name: name, "EventDetail", duration: duration, null);
        }

        public static void AnalyticsOpenApp() {
            if (Application.isEditor) {
                return;
            }

            var type = "OpenApp";
            var userId = UserInfoManager.isLogin() ? UserInfoManager.initUserInfo().userId : null;
            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "enableNotification"}, {"dataType", "bool"}, {"value", enableNotification().ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, device: device, eventType: type, appTime: DateTime.UtcNow,
                data: data);
        }

        public static void AnalyticsWakeApp(string mode, string id = null, string type = null, string subtype = null) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.initUserInfo().userId : null;
            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>>();
            if (id.isNotEmpty()) {
                data.Add(new Dictionary<string, string> {
                    {"key", "id"}, {"dataType", "string"}, {"value", id}
                });
            }

            if (type.isNotEmpty()) {
                data.Add(new Dictionary<string, string> {
                    {"key", "type"}, {"dataType", "string"}, {"value", type}
                });
            }

            if (subtype.isNotEmpty()) {
                data.Add(new Dictionary<string, string> {
                    {"key", "subtype"}, {"dataType", "string"}, {"value", subtype}
                });
            }

            AnalyticsApi.AnalyticsApp(userId: userId, device: device, eventType: mode, appTime: DateTime.UtcNow,
                data: data);
        }


        public static void AnalyticsLogin(string type, string userId) {
            if (Application.isEditor) {
                return;
            }

            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "type"}, {"dataType", "string"}, {"value", type}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, device: device, "UserLogin", appTime: DateTime.UtcNow,
                data: data);
        }

        public static void AnalyticsActiveTime(int timespan) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.initUserInfo().userId : null;
            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "duration"}, {"dataType", "int"}, {"value", timespan.ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, device: device, "ActiveTime", appTime: DateTime.UtcNow,
                data: data);
        }

        public static void AnalyticsClickEgg(int index) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.initUserInfo().userId : null;
            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "index"}, {"dataType", "int"}, {"value", index.ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, device: device, "ClickEgg", appTime: DateTime.UtcNow, data: data);
        }

        public static void AnalyticsQRScan(QRState state, bool success = true) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.initUserInfo().userId : null;
            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "state"}, {"dataType", "string"}, {"value", state.ToString()}
                },
                new Dictionary<string, string> {
                    {"key", "success"}, {"dataType", "bool"}, {"value", success.ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, device: device, "QRScan", appTime: DateTime.UtcNow, data: data);
        }

        public static void AnalyticsHandleFavoriteTag(FavoriteTagType type) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.initUserInfo().userId : null;
            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "type"}, {"dataType", "string"}, {"value", type.ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, device: device, "HandleFavoriteTag", appTime: DateTime.UtcNow,
                data: data);
        }

        public static void AnalyticsFavoriteArticle(string articleId, IEnumerable<string> favoriteTagIds) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.initUserInfo().userId : null;
            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "articleId"}, {"dataType", "string"}, {"value", articleId}
                },
                new Dictionary<string, string> {
                    {"key", "state"}, {"dataType", "string"}, {"value", string.Join(",", values: favoriteTagIds)}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, device: device, "FavoriteArticle", appTime: DateTime.UtcNow,
                data: data);
        }

        public static void AnalyticsUnFavoriteArticle(string favoriteId) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.initUserInfo().userId : null;
            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "favoriteId"}, {"dataType", "string"}, {"value", favoriteId}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, device: device, "UnFavoriteArticle", appTime: DateTime.UtcNow,
                data: data);
        }

        public static void AnalyticsClickHomeFocus() {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.initUserInfo().userId : null;
            var device = deviceId() + (SystemInfo.deviceModel ?? "");
            var data = new List<Dictionary<string, string>>();
            AnalyticsApi.AnalyticsApp(userId: userId, device: device, "ClickHomeFocus", appTime: DateTime.UtcNow,
                data: data);
        }

        public static string deviceId() {
            return Application.isEditor ? "Editor" : getDeviceID();
        }

        public static bool enableNotification() {
            return isEnableNotification();
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        static extern string getDeviceID();

        [DllImport("__Internal")]
        static extern bool isEnableNotification();

#elif UNITY_ANDROID
        static AndroidJavaClass _plugin;

        static AndroidJavaClass Plugin() {
            if (_plugin == null) {
                _plugin = new AndroidJavaClass("com.unity3d.unityconnect.plugins.CommonPlugin");
            }

            return _plugin;
        }

        static string getDeviceID() {
            return Plugin().CallStatic<string>("getDeviceID");
        }
        static bool isEnableNotification() {
            return Plugin().CallStatic<bool>("isEnableNotification");
        }
#else
        static string getDeviceID() {
            return "Unity Editor";
        }

        static bool isEnableNotification() {
            return false;
        }
#endif
    }
}