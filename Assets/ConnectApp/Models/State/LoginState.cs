using System;

namespace ConnectApp.models {
    [Serializable]
    public class LoginState : IEquatable<LoginState> {
        public LoginInfo loginInfo;
        public string email;
        public string password;
        public bool loading;
        public bool isLoggedIn;

        public bool Equals(LoginState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(loginInfo, other.loginInfo) && string.Equals(email, other.email) && string.Equals(password, other.password) && loading == other.loading && isLoggedIn == other.isLoggedIn;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LoginState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (loginInfo != null ? loginInfo.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (email != null ? email.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (password != null ? password.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ loading.GetHashCode();
                hashCode = (hashCode * 397) ^ isLoggedIn.GetHashCode();
                return hashCode;
            }
        }
    }
}