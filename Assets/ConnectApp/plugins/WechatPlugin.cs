using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using UnityEngine;

namespace ConnectApp.plugins
{
    public class WechatPlugin
    {
        private WechatPlugin() {
            
        }
        
        public static readonly WechatPlugin instance = new WechatPlugin();

        private bool isListen;
        
        public UIWidgetsPanel panel;

        
#if UNITY_IOS        

        public void addListener()
        {
            if (!isListen)
            {
                UIWidgetsMessageManager.instance.
                    AddChannelMessageDelegate("wechat", this._handleMethodCall);
                isListen = true;
            }
        }

        void _handleMethodCall(string method, List<JSONNode> args)
        {
            using (panel.getScope())
            {
                switch (method)
                {
                    case "callback":
                    {
                        var dict = JSON.Parse(args.first());
                        var type = dict["type"];
                        if (type == "code")
                        {
                            var code = dict["code"];
                            loginByWechat(code);
                        }
                        else if(type == "cancel")
                        {
                     
                     
                        }
                    }
                        break;
                    
                }
            }
            
           
        }

        public void loginByWechat(string code)
        {
           StoreProvider.store.Dispatch(new LoginByWechatAction {code = code});
        }

        public void init(string appId)
        {
            if (!Application.isEditor)
                initWechat(appId);
        }
        public void login(string stateId)
        {
            if (!Application.isEditor)
            {
                loginWechat(stateId);
                addListener();
            }
                
        }

        public void shareToFriend(string title, string description, string url,string imageBytes)
        {
            if (!Application.isEditor)
            {
                toFriends(title,description,url,imageBytes); 
                addListener();
            }
        }
        public void shareToTimeline(string title, string description, string url,string imageBytes)
        {
            if (!Application.isEditor)
            {
                toTimeline(title,description,url,imageBytes); 
                addListener();
            }
        }


        public bool inInstalled()
        {
            if (!Application.isEditor)
            {
                addListener();
                return isInstallWechat();
            }
            return false;
        }

        [DllImport ("__Internal")]
        internal static extern void initWechat(string appId);
         
        [DllImport ("__Internal")]
        internal static extern void loginWechat(string stateId);
        
        [DllImport ("__Internal")]
        internal static extern bool isInstallWechat();
        
        [DllImport ("__Internal")]
        internal static extern void toFriends(string title, string description, string url,string imageBytes);
        
        [DllImport ("__Internal")]
        internal static extern void toTimeline(string title, string description, string url,string imageBytes);
        
#endif
       
    }
            

}