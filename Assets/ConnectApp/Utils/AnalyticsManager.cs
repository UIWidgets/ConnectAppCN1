using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Plugins;
using UnityEngine;

namespace ConnectApp.Utils {
    public class AnalyticsManager {
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
            var mEventId = $"Click_Tab_{entries[toIndex]}";
            Dictionary<string, string> extras = new Dictionary<string, string>();
            extras.Add("from", tabs[fromIndex]);
            extras.Add("to", tabs[toIndex]);
            JAnalyticsPlugin.CountEvent(mEventId, extras);
        }

        //search点击事件统计
        public static void ClickEnterSearch(string from) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Enter_Search";
            Dictionary<string, string> extras = new Dictionary<string, string>();
            extras.Add("from", from);
            JAnalyticsPlugin.CountEvent(mEventId, extras);
        }

        //进入文章详情
        public static void ClickEnterArticleDetail(string from, string articleId, string articleTitle) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Enter_ArticleDetail";
            Dictionary<string, string> extras = new Dictionary<string, string>();
            extras.Add("from", from);
            extras.Add("id", articleId);
            extras.Add("title", articleTitle);
            JAnalyticsPlugin.CountEvent(mEventId, extras);
        }

        //进入活动详情
        public static void ClickEnterEventDetail(string from, string eventId, string eventTitle, string type) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Enter_EventDetail";
            Dictionary<string, string> extras = new Dictionary<string, string>();
            extras.Add("from", from);
            extras.Add("id", eventId);
            extras.Add("title", eventTitle);
            extras.Add("type", type);
            JAnalyticsPlugin.CountEvent(mEventId, extras);
        }

        //eventClick
        public static void ClickShare(ShareType shareType, string type, string objectId, string title) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Event_Share";
            Dictionary<string, string> extras = new Dictionary<string, string>();
            extras.Add("shareType", shareType.ToString());
            extras.Add("type", type);
            extras.Add("id", objectId);
            extras.Add("title", title);
            JAnalyticsPlugin.CountEvent(mEventId, extras);
        }


        public static void ClickLike(string type, string articleId, string commentId = null) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Event_Like";
            Dictionary<string, string> extras = new Dictionary<string, string>();
            extras.Add("type", type);
            extras.Add("id", articleId);
            if (commentId != null) {
                extras.Add("commentId", commentId);
            }

            JAnalyticsPlugin.CountEvent(mEventId, extras);
        }

        public static void ClickComment(string type, string channelId, string title, string commentId = null) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Event_Comment";
            Dictionary<string, string> extras = new Dictionary<string, string>();
            extras.Add("type", type);
            extras.Add("channelId", channelId);
            extras.Add("title", title);
            if (commentId != null) {
                extras.Add("commentId", commentId);
            }

            JAnalyticsPlugin.CountEvent(mEventId, extras);
        }

        public static void ClickPublishComment(string type, string channelId, string commentId = null) {
            if (Application.isEditor) {
                return;
            }

            var mEventId = "Click_Event_PublishComment";
            Dictionary<string, string> extras = new Dictionary<string, string>();
            extras.Add("type", type);
            extras.Add("channelId", channelId);
            if (commentId != null) {
                extras.Add("commentId", commentId);
            }

            JAnalyticsPlugin.CountEvent(mEventId, extras);
        }

        public static void BrowseArtileDetail(string id, string name, DateTime startTime, DateTime endTime) {
            if (Application.isEditor) {
                return;
            }

            string duration = (endTime - startTime).TotalSeconds.ToString("0.0");
            JAnalyticsPlugin.BrowseEvent(id, name, "ArticleDetail", duration, null);
        }

        public static void BrowseEventDetail(string id, string name, DateTime startTime, DateTime endTime) {
            if (Application.isEditor) {
                return;
            }

            string duration = (endTime - startTime).TotalSeconds.ToString("0.0");
            JAnalyticsPlugin.BrowseEvent(id, name, "EventDetail", duration, null);
        }
    }
}