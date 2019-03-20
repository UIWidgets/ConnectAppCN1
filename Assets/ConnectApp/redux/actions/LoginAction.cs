using ConnectApp.models;
using Unity.UIWidgets.widgets;

namespace ConnectApp.redux.actions {
    public class LoginChangeEmailAction : BaseAction {
        public string changeText;
    }

    public class LoginChangePasswordAction : BaseAction {
        public string changeText;
    }

    public class LoginByEmailAction : RequestAction {
        public BuildContext context;
    }

    public class LoginByEmailSuccessAction : BaseAction {
        public BuildContext context;
        public LoginInfo loginInfo;
    }

    public class LoginByEmailFailedAction : BaseAction {
    }
}