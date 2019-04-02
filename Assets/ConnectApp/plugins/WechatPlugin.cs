using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using UnityEngine;

namespace ConnectApp.plugins
{
    
    public class WechatPlugin
    {
        private WechatPlugin() {
            UIWidgetsMessageManager.instance.
                AddChannelMessageDelegate("wechat", this._handleMethodCall);
        }
        
        public static readonly WechatPlugin instance = new WechatPlugin();

        void _handleMethodCall(string method, List<JSONNode> args) {
            switch (method)
            {
                case "callback":
                {
                    StoreProvider.store.Dispatch(new LoginNavigatorPushToAction());
                }
                    break;
                    
            }
           
        } 


#if UNITY_IOS 
        
        public void login(string stateId)
        {
            if (!Application.isEditor)
                loginWechat(stateId);
        }
        public bool inInstalled()
        {
            if (!Application.isEditor)
                return isInstallWechat();
            return false;
        }

        [DllImport ("__Internal")]
        internal static extern void InitWechat(string appId);
         
        [DllImport ("__Internal")]
        internal static extern void loginWechat(string stateId);
        
        [DllImport ("__Internal")]
        internal static extern bool isInstallWechat();
        
#endif
       
    }
            

}