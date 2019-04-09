using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.models;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class LoginChangeEmailAction : BaseAction {
        public string changeText;
    }

    public class LoginChangePasswordAction : BaseAction {
        public string changeText;
    }

    public class LoginByEmailAction : RequestAction {
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
            return new ThunkAction<AppState>((dispatcher, getState) =>
            {
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
                        StoreProvider.store.dispatcher.dispatch(new UserMapAction {userMap = dict});
                        StoreProvider.store.dispatcher.dispatch(new LoginByEmailSuccessAction {
                            loginInfo = loginInfo
                        });
                        StoreProvider.store.dispatcher.dispatch(new MainNavigatorPopAction());
                        StoreProvider.store.dispatcher.dispatch(new CleanEmailAndPasswordAction());
                    })
                    .Catch(error => {
                        Debug.Log(error);
                        StoreProvider.store.dispatcher.dispatch(new LoginByEmailFailureAction());
                    });
            });
        }

        public static object openCreateUnityIdUrl() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
//                CustomDialogUtils.showCustomDialog(
//                    child: new CustomDialog()
//                );
                return LoginApi.FetchCreateUnityIdUrl()
                    .Then(url => {
//                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new OpenUrlAction {url = url});
                    })
                    .Catch(error => {
//                        Debug.Log(error);
//                        CustomDialogUtils.hiddenCustomDialog();
                    });
            });
        }
    }   
}