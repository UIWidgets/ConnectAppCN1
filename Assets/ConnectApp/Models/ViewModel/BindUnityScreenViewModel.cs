using System;
using ConnectApp.screens;

namespace ConnectApp.Models.ViewModel {

    public class BindUnityScreenViewModel {
        public FromPage fromPage = FromPage.setting;
        public bool loginLoading;
        public string loginEmail;
        public string loginPassword;
        public bool loginBtnEnable;
    }
}