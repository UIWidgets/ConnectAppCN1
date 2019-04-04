using System;
using ConnectApp.screens;

namespace ConnectApp.Models.Screen {

    public class BindUnityScreenModel : IEquatable<BindUnityScreenModel> {
        public FromPage fromPage = FromPage.setting;
        public bool loginLoading;
        public string loginEmail;
        public string loginPassword;
        public bool loginBtnEnable;

        public bool Equals(BindUnityScreenModel other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return fromPage == other.fromPage && loginLoading == other.loginLoading && string.Equals(loginEmail, other.loginEmail) && string.Equals(loginPassword, other.loginPassword) && loginBtnEnable == other.loginBtnEnable;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BindUnityScreenModel) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (int) fromPage;
                hashCode = (hashCode * 397) ^ loginLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ (loginEmail != null ? loginEmail.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (loginPassword != null ? loginPassword.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ loginBtnEnable.GetHashCode();
                return hashCode;
            }
        }
    }
}