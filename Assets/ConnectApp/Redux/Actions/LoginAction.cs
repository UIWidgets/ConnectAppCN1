using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Plugins;
using ConnectApp.Utils;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

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

    public class LoginByWechatAction : RequestAction {
        public BuildContext context;
        public string code;
    }

    public class LoginByWechatSuccessAction : BaseAction {
        public LoginInfo loginInfo;
    }

    public class LoginByWechatFailureAction : BaseAction {
        public BuildContext context;
    }

    public class LogoutAction : BaseAction {
        public BuildContext context;
    }

    public class CleanEmailAndPasswordAction : BaseAction {
    }

    public class JumpToCreateUnityIdAction : RequestAction {
    }

    public static partial class Actions {
        public static object loginByEmail() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var email = getState().loginState.email;
                var password = getState().loginState.password;
                return LoginApi.LoginByEmail(email, password)
                    .Then(loginInfo => {
                        var user = new User {
                            id = loginInfo.userId,
                            fullName = loginInfo.userFullName,
                            avatar = loginInfo.userAvatar
                        };
                        var dict = new Dictionary<string, User> {
                            {user.id, user}
                        };
                        dispatcher.dispatch(new UserMapAction {userMap = dict});
                        dispatcher.dispatch(new LoginByEmailSuccessAction {
                            loginInfo = loginInfo
                        });
                        dispatcher.dispatch(new MainNavigatorPopAction());
                        dispatcher.dispatch(new CleanEmailAndPasswordAction());
                        UserInfoManager.saveUserInfo(loginInfo);
                        JAnalyticsPlugin.login("email");
                        JPushPlugin.setJPushAlias(loginInfo.userId);
                    });
            });
        }

        public static object loginByWechat(string code) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return LoginApi.LoginByWechat(code)
                    .Then(loginInfo => {
                        var user = new User {
                            id = loginInfo.userId,
                            fullName = loginInfo.userFullName,
                            avatar = loginInfo.userAvatar
                        };
                        var dict = new Dictionary<string, User> {
                            {user.id, user}
                        };
                        dispatcher.dispatch(new UserMapAction {userMap = dict});
                        dispatcher.dispatch(new LoginByWechatSuccessAction {
                            loginInfo = loginInfo
                        });
                        UserInfoManager.saveUserInfo(loginInfo);
                        JAnalyticsPlugin.login("wechat");
                        JPushPlugin.setJPushAlias(loginInfo.userId);
                    });
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