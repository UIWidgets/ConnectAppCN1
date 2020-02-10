using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Plugins;
using ConnectApp.screens;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class LoginChangeEmailAction : BaseAction {
        public string changeText;
    }

    public class LoginChangePasswordAction : BaseAction {
        public string changeText;
    }

    public class StartLoginByEmailAction : RequestAction {
    }

    public class LoginByEmailSuccessAction : BaseAction {
        public LoginInfo loginInfo;
    }

    public class LoginByEmailFailureAction : BaseAction {
    }

    public class LoginByWechatSuccessAction : BaseAction {
        public LoginInfo loginInfo;
    }

    public class LoginByWechatFailureAction : BaseAction {
    }

    public class LogoutAction : BaseAction {
    }

    public class CleanEmailAndPasswordAction : BaseAction {
    }

    public static partial class Actions {
        public static object loginByEmail() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var email = getState().loginState.email;
                var password = getState().loginState.password;
                return LoginApi.LoginByEmail(email: email, password: password)
                    .Then(loginInfo => {
                        var user = new User {
                            id = loginInfo.userId,
                            fullName = loginInfo.userFullName,
                            avatar = loginInfo.userAvatar,
                            title = loginInfo.title,
                            coverImage = loginInfo.coverImageWithCDN
                        };
                        var dict = new Dictionary<string, User> {
                            {user.id, user}
                        };
                        dispatcher.dispatch(new UserMapAction {userMap = dict});
                        dispatcher.dispatch(new LoginByEmailSuccessAction {
                            loginInfo = loginInfo
                        });
                        dispatcher.dispatch(fetchChannels(1));
                        dispatcher.dispatch(fetchCreateChannelFilter());
                        dispatcher.dispatch<IPromise>(fetchUserProfile(loginInfo.userId));
                        dispatcher.dispatch(new CleanEmailAndPasswordAction());
                        UserInfoManager.saveUserInfo(loginInfo);
                        AnalyticsManager.LoginEvent("email");
                        AnalyticsManager.AnalyticsLogin("email", loginInfo.userId);
                        JPushPlugin.setJPushAlias(loginInfo.userId);
                        BuglyAgent.SetUserId(loginInfo.userId);
                        EventBus.publish(sName: EventBusConstant.login_success, new List<object> {loginInfo.userId});
                        dispatcher.dispatch(new MainNavigatorPopAction());
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new LoginByEmailFailureAction());
                        Debuger.LogError(message: error);
                        var customSnackBar = new CustomSnackBar(
                            "登录失败，请重试。"
                        );
                        customSnackBar.show();
                    });
            });
        }

        public static object loginByWechat(string code) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return LoginApi.LoginByWechat(code: code)
                    .Then(loginInfo => {
                        CustomDialogUtils.hiddenCustomDialog();
                        var user = new User {
                            id = loginInfo.userId,
                            fullName = loginInfo.userFullName,
                            avatar = loginInfo.userAvatar,
                            title = loginInfo.title,
                            coverImage = loginInfo.coverImageWithCDN
                        };
                        var dict = new Dictionary<string, User> {
                            {user.id, user}
                        };
                        dispatcher.dispatch(new UserMapAction {userMap = dict});
                        dispatcher.dispatch(new LoginByWechatSuccessAction {
                            loginInfo = loginInfo
                        });
                        dispatcher.dispatch(fetchChannels(1));
                        dispatcher.dispatch(fetchCreateChannelFilter());
                        UserInfoManager.saveUserInfo(loginInfo);
                        AnalyticsManager.LoginEvent("wechat");
                        AnalyticsManager.AnalyticsLogin("wechat", loginInfo.userId);
                        JPushPlugin.setJPushAlias(loginInfo.userId);
                        if (loginInfo.anonymous) {
                            LoginScreen.navigator.pushReplacementNamed(routeName: LoginNavigatorRoutes
                                .WechatBindUnity);
                        }
                        else {
                            dispatcher.dispatch(new MainNavigatorPopAction());
                            EventBus.publish(sName: EventBusConstant.login_success,
                                new List<object> {loginInfo.userId});
                        }
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new LoginByWechatFailureAction());
                    });
            });
        }

        public static object loginByQr(string token, string action) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return LoginApi.LoginByQr(token: token, action: action).Then(success => {
                    if (action == "cancel") {
                        AnalyticsManager.AnalyticsQRScan(state: QRState.cancel);
                        return;
                    }

                    CustomDialogUtils.hiddenCustomDialog();
                    dispatcher.dispatch(new MainNavigatorPopAction());
                    CustomDialogUtils.showToast(
                        success ? "登录成功" : "登录失败",
                        success ? Icons.sentiment_satisfied : Icons.sentiment_dissatisfied
                    );
                    if (success) {
                        AnalyticsManager.AnalyticsQRScan(state: QRState.confirm);
                    }
                    else {
                        AnalyticsManager.AnalyticsQRScan(state: QRState.confirm, false);
                    }
                }).Catch(error => {
                        Debuger.LogError($"confirm api error: {error}, action: {action}");
                        if (action == "cancel") {
                            AnalyticsManager.AnalyticsQRScan(state: QRState.cancel, false);
                            return;
                        }

                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new MainNavigatorPopAction());
                        CustomDialogUtils.showToast("登录失败", iconData: Icons.sentiment_dissatisfied);
                        AnalyticsManager.AnalyticsQRScan(state: QRState.confirm, false);
                    }
                );
            });
        }

        public static object openCreateUnityIdUrl() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return LoginApi.FetchCreateUnityIdUrl()
                    .Then(url => { dispatcher.dispatch(new OpenUrlAction {url = url}); });
            });
        }
    }
}