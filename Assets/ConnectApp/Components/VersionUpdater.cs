using ConnectApp.Api;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.screens;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class VersionUpdater : StatefulWidget {
        public VersionUpdater(
            Widget child = null,
            Key key = null
        ) : base(key: key) {
            this.child = child;
        }

        public readonly Widget child;

        public override State createState() {
            return new _VersionUpdaterState();
        }
    }

    public class _VersionUpdaterState : State<VersionUpdater> {
        public override void initState() {
            base.initState();
            SplashManager.hiddenAndroidSpalsh();
            fetchInitData();
            VersionManager.checkForUpdates(type: CheckVersionType.initialize);
            StatusBarManager.hideStatusBar(false);
            SplashManager.fetchSplash();
            AnalyticsManager.AnalyticsOpenApp();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (UserInfoManager.isLogin()) {
                    var userId = UserInfoManager.getUserInfo().userId ?? "";
                    if (userId.isNotEmpty()) {
                        StoreProvider.store.dispatcher.dispatch(Actions.fetchUserProfile(userId: userId));
                    }

                    StoreProvider.store.dispatcher.dispatch(Actions.fetchChannels(1));
                    StoreProvider.store.dispatcher.dispatch(Actions.fetchCreateChannelFilter());
                }

                StoreProvider.store.dispatcher.dispatch(Actions.fetchReviewUrl());
            });
        }

        static void fetchInitData() {
            LoginApi.InitData().Then(initDataResponse => {
                var vs = initDataResponse.VS;
                var serverConfig = initDataResponse.config;
                if (vs.isNotEmpty()) {
                    HttpManager.updateCookie($"VS={vs}");
                }
                if (serverConfig.tinyGameUrl.isNotEmpty()) {
                    LocalDataManager.saveTinyGameUrl(url: serverConfig.tinyGameUrl);
                }
                if (serverConfig.minVersionCode.isNotEmpty()) {
                    if (!int.TryParse(serverConfig.minVersionCode, out var minVersionCode)) {
                        return;
                    }
                    if (minVersionCode > 0 && minVersionCode > Config.versionCode) {
                        // need update
                        StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToAction{routeName = MainNavigatorRoutes.ForceUpdate});
                        VersionManager.saveMinVersionCode(versionCode: minVersionCode);
                    }
                }
            }).Catch(exception => {
                StoreProvider.store.dispatcher.dispatch(new NetworkAvailableStateAction {available = false});
                Debuger.LogError(message: exception);
            });
        }

        public override Widget build(BuildContext context) {
            return this.widget.child;
        }
    }
}