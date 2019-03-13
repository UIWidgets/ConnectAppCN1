using ConnectApp.models;

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

    public class LoginByEmailFailedAction : BaseAction {
    }
}