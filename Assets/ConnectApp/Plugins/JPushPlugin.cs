using System;
using System.Collections.Generic;
using System.Web;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using EventType = ConnectApp.Models.State.EventType;
#if UNITY_IOS
using System.Runtime.InteropServices;

#endif

namespace ConnectApp.Plugins {
    public static class JPushPlugin {
        public static bool isListen;
        public static bool isShowPushAlert;
        public static string hmsToken;

        static int callbackId = 0;

        public static void addListener() {
            if (Application.isEditor) {
                return;
            }

            if (!isListen) {
                isListen = true;
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("jpush", del: _handleMethodCall);
                completed();
                setJPushChannel(channel: Config.store);
                if (StoreProvider.store.getState().loginState.isLoggedIn) {
                    setJPushAlias(alias: StoreProvider.store.getState().loginState.loginInfo.userId);
                }
                else {
                    deleteJPushAlias();
                }

                setJPushTags(
                    new List<string> {Config.versionCode.ToString(), Config.messengerTag, Config.versionName});
            }
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (GlobalContext.context != null) {
                using (WindowProvider.of(GlobalContext.context).getScope()) {
                    switch (method) {
                        case "OnOpenNotification": {
                            if (args.isEmpty()) {
                                return;
                            }

                            //点击应用通知栏
                            var node = args.first();
                            var dict = JSON.Parse(node);
                            var type = dict["type"];
                            var subType = dict["subtype"];
                            string id = dict["id"] ?? "";
                            if (id.isEmpty()) {
                                id = dict["channelId"] ?? "";
                            }

                            AnalyticsManager.AnalyticsWakeApp("OnOpenNotification", id, type, subType);
                            AnalyticsManager.ClickNotification(type, subType, id);
                            pushPage(type, subType, id, true);
                            break;
                        }
                        case "OnReceiveNotification": {
                            //接收到推送
                            if (args.isEmpty()) {
                                return;
                            }

                            var node = args.first();
                            var dict = JSON.Parse(node);
                            var type = dict["type"] ?? "";
                            if (type != "messenger") {
                                StoreProvider.store.dispatcher.dispatch(new UpdateNewNotificationAction {
                                    notification = ""
                                });
                            }

                            var id = dict["id"] ?? "";
                            if (CTemporaryValue.currentPageModelId.isEmpty() ||
                                id != CTemporaryValue.currentPageModelId) {
                                playMessageSound();
                            }

                            break;
                        }
                        case "OnReceiveMessage": {
                            //接收到应用内消息
                            break;
                        }
                        case "OnOpenUrl": {
                            if (args.isEmpty()) {
                                return;
                            }

                            AnalyticsManager.AnalyticsWakeApp("OnOpenUrl", args.first());
                            openUrlScheme(args.first());
                            break;
                        }
                        case "OnOpenUniversalLinks": {
                            if (args.isEmpty()) {
                                return;
                            }

                            AnalyticsManager.AnalyticsWakeApp("OnOpenUniversalLinks", args.first());
                            openUniversalLink(args.first());
                            break;
                        }
                        case "CompletedCallback": {
                            if (args.isEmpty()) {
                                return;
                            }

                            clearIconBadge();
                            var node = args.first();
                            var dict = JSON.Parse(node);
                            var isPush = (bool) dict["push"];
                            if (isPush) {
                                StoreProvider.store.dispatcher.dispatch(new MainNavigatorReplaceToAction {
                                    routeName = MainNavigatorRoutes.Main
                                });
                            }
                            else {
                                if (VersionManager.needForceUpdate()) {
                                    SplashManager.hiddenAndroidSpalsh();
                                }
                                else {
                                    if (SplashManager.isExistSplash()) {
                                        SplashManager.hiddenAndroidSpalsh();
                                        StoreProvider.store.dispatcher.dispatch(
                                            new MainNavigatorPushReplaceSplashAction());
                                    }
                                    else {
                                        StoreProvider.store.dispatcher.dispatch(
                                            new MainNavigatorPushReplaceMainAction());
                                    }
                                }
                            }

                            break;
                        }
                        case "RegisterToken": {
                            if (args.isEmpty()) {
                                return;
                            }

                            var node = args.first();
                            var dict = JSON.Parse(node);
                            var token = (string) dict["token"];
                            hmsToken = token;
                            registerHmsToken(StoreProvider.store.getState().loginState.isLoggedIn
                                ? StoreProvider.store.getState().loginState.loginInfo.userId
                                : "");
                            break;
                        }
                        case "SaveImageSuccess": {
                            CustomDialogUtils.showToast("保存成功", iconData: Icons.sentiment_satisfied);
                            break;
                        }
                        case "SaveImageError": {
                            CustomDialogUtils.showToast("保存失败，请检查权限", iconData: Icons.sentiment_dissatisfied);
                            break;
                        }
                    }
                }
            }
        }

        public static void openUniversalLink(string link) {
            if (link.isEmpty()) {
                return;
            }

            var uri = new Uri(link);
            if (uri.AbsolutePath.StartsWith("/connectapplink/")) {
                var type = "";
                if (uri.AbsolutePath.Equals("/connectapplink/project_detail")) {
                    type = "project";
                }
                else if (uri.AbsolutePath.Equals("/connectapplink/event_detail")) {
                    type = "event";
                }
                else if (uri.AbsolutePath.Equals("/connectapplink/messenger")) {
                    type = "messenger";
                }
                else {
                    return;
                }

                var subType = HttpUtility.ParseQueryString(uri.Query).Get("type");
                var id = HttpUtility.ParseQueryString(uri.Query).Get("id");
                pushPage(type, subType, id);
            }
            else {
                pushPage("webView", "", link);
            }
        }

        public static void openUrlScheme(string schemeUrl) {
            if (schemeUrl.isEmpty()) {
                return;
            }

            var uri = new Uri(schemeUrl);
            if (uri.Scheme.Equals("unityconnect")) {
                AnalyticsManager.EnterOnOpenUrl(schemeUrl);
                if (uri.Host.Equals("connectapp")) {
                    var type = "";
                    if (uri.AbsolutePath.Equals("/project_detail")) {
                        type = "project";
                    }
                    else if (uri.AbsolutePath.Equals("/event_detail")) {
                        type = "event";
                    }
                    else if (uri.AbsolutePath.Equals("/messenger")) {
                        type = "messenger";
                    }
                    else if (uri.AbsolutePath.Equals("/rank")) {
                        type = "rank";
                    }
                    else if (uri.AbsolutePath.Equals("/weixin")) {
                        type = "weixin";
                    }
                    else {
                        return;
                    }

                    var subType = HttpUtility.ParseQueryString(uri.Query).Get("type");
                    var id = HttpUtility.ParseQueryString(uri.Query).Get("id");
                    pushPage(type, subType, id);
                }
            }
            else {
                pushPage("webView", "", schemeUrl);
            }
        }

        static void pushPage(string type, string subType, string id, bool isPush = false) {
            if (type != "rank" && id.isEmpty()) {
                return;
            }

            if (VersionManager.needForceUpdate()) {
                return;
            }

            if (type == "project") {
                if (subType == "article") {
                    AnalyticsManager.ClickEnterArticleDetail("Push_Article", id, $"PushArticle_{id}");
                    if (CTemporaryValue.currentPageModelId.isNotEmpty() && id == CTemporaryValue.currentPageModelId) {
                        return;
                    }

                    StoreProvider.store.dispatcher.dispatch(
                        new MainNavigatorPushToArticleDetailAction {articleId = id, isPush = isPush});
                }
            }
            else if (type == "event") {
                var eventType = EventType.offline;
                if (subType == "online") {
                    eventType = EventType.online;
                }

                AnalyticsManager.ClickEnterEventDetail("Push_Event", id, $"PushEvent_{id}", eventType.ToString());

                if (CTemporaryValue.currentPageModelId.isNotEmpty() && id == CTemporaryValue.currentPageModelId) {
                    return;
                }

                StoreProvider.store.dispatcher.dispatch(
                    new MainNavigatorPushToEventDetailAction {eventId = id, eventType = eventType});
            }
            else if (type == "team") {
                if (CTemporaryValue.currentPageModelId.isNotEmpty() && id == CTemporaryValue.currentPageModelId) {
                    return;
                }

                if (subType == "follower") {
                    StoreProvider.store.dispatcher.dispatch(
                        new MainNavigatorPushToTeamDetailAction {teamId = id});
                }
            }
            else if (type == "user") {
                if (CTemporaryValue.currentPageModelId.isNotEmpty() && id == CTemporaryValue.currentPageModelId) {
                    return;
                }

                if (subType == "follower") {
                    StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {userId = id});
                }
            }
            else if (type == "webView") {
                StoreProvider.store.dispatcher.dispatch(
                    new MainNavigatorPushToWebViewAction {url = id});
            }
            else if (type == "messenger") {
                if (CTemporaryValue.currentPageModelId.isNotEmpty() && id == CTemporaryValue.currentPageModelId) {
                    return;
                }

                if (isPush) {
                    if (UserInfoManager.isLogin()) {
                        StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToChannelAction {channelId = id});
                    }
                    else {
                        Router.navigator.pushNamed(routeName: MainNavigatorRoutes.Login);
                    }
                }
                else {
                    StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToChannelShareAction {channelId = id});
                }
            }
            else if (type == "rank") {
                if (CTemporaryValue.currentPageModelId.isNotEmpty() && id == CTemporaryValue.currentPageModelId) {
                    return;
                }
                var initIndex = 0;
                switch (subType) {
                    case "column": {
                        initIndex = 1;
                        break;
                    }
                    case "blogger": {
                        initIndex = 2;
                        break;
                    }
                }
                StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToLeaderBoardAction {
                    initIndex = initIndex
                });
            }
            else if (type == "weixin") {
                if (subType == "miniprogram") {
                    if (WechatPlugin.instance().isInstalled()) {
                        var path = CStringUtils.CreateMiniPath(id: id,
                            title: "");
                        WechatPlugin.instance().toOpenMiNi(path);
                    }  
                }
            }
        }

        static void completed() {
            if (Application.isEditor) {
                return;
            }

            listenCompleted();
        }


        public static void registerHmsToken(string userId = "") {
            // if userid is "" mean unregister token
            if (hmsToken.isNotEmpty()) {
                UserApi.RegisterToken(hmsToken, userId);
            }
        }


        static void setJPushChannel(string channel) {
            if (Application.isEditor) {
                return;
            }

            if (channel.isEmpty()) {
                return;
            }

            setChannel(channel: channel);
        }

        public static void setJPushAlias(string alias) {
            if (Application.isEditor) {
                return;
            }

            if (alias.isEmpty()) {
                return;
            }

            setAlias(sequence: callbackId++, alias: alias);
            registerHmsToken(alias);
        }

        public static void deleteJPushAlias(string alias = "") {
            if (Application.isEditor) {
                return;
            }

            deleteAlias(sequence: callbackId++, alias);
        }

        static void setJPushTags(List<string> tags) {
            if (Application.isEditor) {
                return;
            }

            string tagsJsonStr = JsonHelper.ToJson(list: tags);
            if (tagsJsonStr.isEmpty()) {
                return;
            }

            setTags(sequence: callbackId++, tagsJsonStr: tagsJsonStr);
        }

        public static void playMessageSound() {
            if (Application.isEditor) {
                return;
            }

            playSystemSound();
        }

        public static void showPushAlert(bool isShow) {
            if (!CCommonUtils.isIPhone) {
                return;
            }

            if (isShowPushAlert == isShow) {
                return;
            }

            isShowPushAlert = isShow;
            updateShowAlert(isShow);
        }

        public static void clearNotifications() {
            if (Application.isEditor) {
                return;
            }

            clearAllAlert();
        }

        public static void clearIconBadge() {
            if (Application.isEditor) {
                return;
            }

            clearBadge();
        }


#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void listenCompleted();

        [DllImport("__Internal")]
        static extern void setChannel(string channel);

        [DllImport("__Internal")]
        static extern void setAlias(int sequence, string alias);

        [DllImport("__Internal")]
        static extern void deleteAlias(int sequence, string alias);

        [DllImport("__Internal")]
        static extern void setTags(int sequence, string tagsJsonStr);

        [DllImport("__Internal")]
        static extern void playSystemSound();

        [DllImport("__Internal")]
        static extern void updateShowAlert(bool isShow);

        [DllImport("__Internal")]
        static extern void clearAllAlert();

        [DllImport("__Internal")]
        static extern void clearBadge();

#elif UNITY_ANDROID
        static AndroidJavaObject _plugin;

        static AndroidJavaObject Plugin() {
            if (_plugin == null) {
                using (
                    AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JPushPlugin")
                ) {
                    _plugin = managerClass.CallStatic<AndroidJavaObject>("getInstance");
                }
            }

            return _plugin;
        }

        static void listenCompleted() {
            Plugin().Call("listenCompleted");
        }

        static void setChannel(string channel) {
            Plugin().Call("setChannel", channel);
        }

        static void setAlias(int sequence, string alias) {
            Plugin().Call("setAlias", sequence, alias);
        }

        static void deleteAlias(int sequence, string alias) {
            Plugin().Call("deleteAlias", sequence, alias);
        }

        static void setTags(int sequence, string tagsJsonStr) {
            Plugin().Call("setTags", sequence, tagsJsonStr);
        }

        static void playSystemSound() {
            Plugin().Call("playSystemSound");
        }

        static void updateShowAlert(bool isShow) {
            Plugin().Call("updateShowAlert");
        }

        static void clearAllAlert() {
            Plugin().Call("clearAllAlert");
        }

        static void clearBadge() {
            Plugin().Call("clearBadge");
        }
#else
        static void listenCompleted() {
        }

        static void setChannel(string channel) {
        }

        static void setAlias(int sequence, string alias) {
        }

        static void deleteAlias(int sequence, string alias) {
        }

        static void setTags(int sequence, string tagsJsonStr) {
        }

        static void playSystemSound() {
        }

        static void updateShowAlert(bool isShow) {
        }

        static void clearAllAlert() {
        }

        static void clearBadge() {
        }
#endif
    }
}