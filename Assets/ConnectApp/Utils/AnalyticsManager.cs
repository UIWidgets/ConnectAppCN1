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
        delete,
        collect,
        cancelCollect
    }

    public enum EnterMineType {
        Favorite,
        Event,
        History,
        Setting
    }

    public static class AnalyticsManager {
        public static string foucsTime;

        const string ClickEventSegmentId = "Click_Event_Segment";
        const string ClickEnterSearchId = "Click_Enter_Search";
        const string ClickEnterArticleDetailId = "Click_Enter_ArticleDetail";
        const string ClickReturnArticleDetailId = "Click_Return_ArticleDetail";
        const string ClickEnterEventDetailId = "Click_Enter_EventDetail";
        const string ClickShareId = "Click_Event_Share";
        const string ClickLikeId = "Click_Event_Like";
        const string ClickCommentId = "Click_Event_Comment";
        const string ClickPublishCommentId = "Click_Event_PublishComment";
        const string ClickNotificationId = "Click_Notification";
        const string ClickSplashPageId = "Click_Splash_Page";
        const string ClickSkipSplashPageId = "Click_Skip_Splash_Page";
        const string ClickHottestSearchId = "Click_Search_Hottest_Search";
        const string ClickHistorySearchId = "Click_Search_History_Search";
        const string SignUpOnlineEventId = "Sign_Up_Online_Event";
        const string ClickEnterMineId = "Click_Enter_Mine";
        const string ClickSetGradeId = "Click_Set_Grade";
        const string ClickEnterAboutUsId = "Click_Enter_AboutUs";
        const string ClickCheckUpdateId = "Click_Check_Update";
        const string ClickClearCacheId = "Click_Clear_Cache";
        const string EnterOnOpenUrlId = "Enter_On_OpenUrl";
        const string EnterAppId = "Enter_App";
        const string ClickLogoutId = "Click_Logout";

        // tab点击统计
        public static void ClickHomeTab(int fromIndex, int toIndex) {
            if (Application.isEditor) {
                return;
            }

            List<string> tabs = new List<string> {
                "Article", "Event", "Messenger", "Mine"
            };
            List<string> entries = new List<string> {
                "Article_EnterArticle", "Event_EnterEvent", "Messenger_EnterMessenger", "Mine_EnterMine"
            };
            var mEventId = $"Click_Tab_{entries[index: toIndex]}";
            var extras = new Dictionary<string, string> {
                {"from", tabs[index: fromIndex]},
                {"to", tabs[index: toIndex]}
            };
            JAnalyticsPlugin.CountEvent(eventId: mEventId, extras: extras);
        }

        // 活动
        public static void ClickEventSegment(string from, string type) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"type", type},
                {"from", from}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickEventSegmentId, extras: extras);
        }

        //search点击事件统计
        public static void ClickEnterSearch(string from) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"from", from}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickEnterSearchId, extras: extras);
        }

        //进入文章详情
        public static void ClickEnterArticleDetail(string from, string articleId, string articleTitle) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"from", from},
                {"id", articleId},
                {"title", articleTitle}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickEnterArticleDetailId, extras: extras);
        }

        public static void ClickReturnArticleDetail(string articleId, string articleTitle) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"id", articleId},
                {"title", articleTitle}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickReturnArticleDetailId, extras: extras);
        }

        //进入活动详情
        public static void ClickEnterEventDetail(string from, string eventId, string eventTitle, string type) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"from", from},
                {"id", eventId},
                {"title", eventTitle},
                {"type", type}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickEnterEventDetailId, extras: extras);
        }

        //分享
        public static void ClickShare(ShareType shareType, string type, string objectId, string title) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"shareType", shareType.ToString()},
                {"type", type},
                {"id", objectId},
                {"title", title}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickShareId, extras: extras);
        }

        // 点赞文章或者评价
        public static void ClickLike(string type, string articleId, string commentId = null) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"type", type},
                {"id", articleId}
            };
            if (commentId != null) {
                extras.Add("commentId", value: commentId);
            }

            JAnalyticsPlugin.CountEvent(eventId: ClickLikeId, extras: extras);
        }

        public static void ClickComment(string type, string channelId, string title, string commentId = null) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"type", type},
                {"channelId", channelId},
                {"title", title}
            };
            if (commentId.isNotEmpty()) {
                extras.Add("commentId", value: commentId);
            }

            JAnalyticsPlugin.CountEvent(eventId: ClickCommentId, extras: extras);
        }

        public static void ClickPublishComment(string type, string channelId, string commentId = null) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"type", type},
                {"channelId", channelId}
            };
            if (commentId != null) {
                extras.Add("commentId", value: commentId);
            }

            JAnalyticsPlugin.CountEvent(eventId: ClickPublishCommentId, extras: extras);
        }

        public static void ClickNotification(string type, string subtype, string id) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"type", type},
                {"subtype", subtype},
                {"id", id}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickNotificationId, extras: extras);
        }

        public static void ClickSplashPage(string id, string name, string url) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"id", id},
                {"name", name},
                {"url", url}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickSplashPageId, extras: extras);
        }

        public static void ClickSkipSplashPage(string id, string name, string url) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"id", id},
                {"name", name},
                {"url", url}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickSkipSplashPageId, extras: extras);
        }

        public static void ClickHottestSearch(string keyWord) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"keyWord", keyWord}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickHottestSearchId, extras: extras);
        }

        public static void ClickHistorySearch(string keyWord) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"keyWord", keyWord}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickHistorySearchId, extras: extras);
        }

        public static void SignUpOnlineEvent(string eventId, string title) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"id", eventId},
                {"title", title}
            };
            JAnalyticsPlugin.CountEvent(eventId: SignUpOnlineEventId, extras: extras);
        }

        public static void ClickEnterMine(EnterMineType type) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"type", type.ToString()}
            };
            JAnalyticsPlugin.CountEvent(eventId: ClickEnterMineId, extras: extras);
        }

        //评分
        public static void ClickSetGrade() {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: ClickSetGradeId, extras: extras);
        }

        public static void ClickEnterAboutUs() {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: ClickEnterAboutUsId, extras: extras);
        }

        public static void ClickCheckUpdate() {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: ClickCheckUpdateId, extras: extras);
        }

        public static void ClickClearCache() {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: ClickClearCacheId, extras: extras);
        }

        //通过openurl方式打开app
        public static void EnterOnOpenUrl(string url) {
            if (Application.isEditor) {
                return;
            }

            var extras = new Dictionary<string, string> {
                {"url", url}
            };
            JAnalyticsPlugin.CountEvent(eventId: EnterOnOpenUrlId, extras: extras);
        }

        public static void EnterApp() {
            if (Application.isEditor) {
                return;
            }

            foucsTime = DateTime.UtcNow.ToString();
            var extras = new Dictionary<string, string> {
                {"app", "unity connect"}
            };
            JAnalyticsPlugin.CountEvent(eventId: EnterAppId, extras: extras);
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

            var extras = new Dictionary<string, string>();
            JAnalyticsPlugin.CountEvent(eventId: ClickLogoutId, extras: extras);
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

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "enableNotification"}, {"dataType", "bool"}, {"value", enableNotification().ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, "OpenApp", data: data);
        }

        public static void AnalyticsWakeApp(string mode, string id = null, string type = null, string subtype = null) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
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

            AnalyticsApi.AnalyticsApp(userId: userId, eventType: mode, data: data);
        }


        public static void AnalyticsLogin(string type, string userId) {
            if (Application.isEditor) {
                return;
            }

            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "type"}, {"dataType", "string"}, {"value", type}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, "UserLogin", data: data);
        }

        public static void AnalyticsActiveTime(int timespan) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "duration"}, {"dataType", "int"}, {"value", timespan.ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, "ActiveTime", data: data);
        }

        public static void AnalyticsClickEgg(int index) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "index"}, {"dataType", "int"}, {"value", index.ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, "ClickEgg", data: data);
        }

        public static void AnalyticsQRScan(QRState state, bool success = true) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "state"}, {"dataType", "string"}, {"value", state.ToString()}
                },
                new Dictionary<string, string> {
                    {"key", "success"}, {"dataType", "bool"}, {"value", success.ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, "QRScan", data: data);
        }
        
        public static void AnalyticsTinyWasm(string url, string name) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "url"}, {"dataType", "string"}, {"value", url}
                },
                new Dictionary<string, string> {
                    {"key", "name"}, {"dataType", "string"}, {"value", name}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, "TinyWasm", data: data);
        }

        public static void AnalyticsHandleFavoriteTag(FavoriteTagType type) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "type"}, {"dataType", "string"}, {"value", type.ToString()}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, "HandleFavoriteTag", data: data);
        }

        public static void AnalyticsFavoriteArticle(string articleId, IEnumerable<string> favoriteTagIds) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "articleId"}, {"dataType", "string"}, {"value", articleId}
                },
                new Dictionary<string, string> {
                    {"key", "state"}, {"dataType", "string"}, {"value", string.Join(",", values: favoriteTagIds)}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, "FavoriteArticle", data: data);
        }

        public static void AnalyticsUnFavoriteArticle(string favoriteId) {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
            var data = new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    {"key", "favoriteId"}, {"dataType", "string"}, {"value", favoriteId}
                }
            };
            AnalyticsApi.AnalyticsApp(userId: userId, "UnFavoriteArticle", data: data);
        }

        public static void AnalyticsClickHomeFocus() {
            if (Application.isEditor) {
                return;
            }

            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : null;
            var data = new List<Dictionary<string, string>>();
            AnalyticsApi.AnalyticsApp(userId: userId, "ClickHomeFocus",
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