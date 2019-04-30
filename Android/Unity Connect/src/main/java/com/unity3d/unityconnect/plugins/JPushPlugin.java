package com.unity3d.unityconnect.plugins;

import android.content.Context;

import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;

import java.util.Arrays;

import cn.jpush.android.api.JPushInterface;

public class JPushPlugin {

    private static JPushPlugin instance;

    public Context context;

    public static JPushPlugin getInstance() {
        synchronized (JPushPlugin.class) {
            if (instance == null) {
                instance = new JPushPlugin();
            }
            return instance;
        }
    }

    public String pushJson;

    public void listenCompleted(){
        if (pushJson!=null){
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnOpenNotification", Arrays.asList(pushJson));
        }
    }
    public void setChannel(String channel){
        JPushInterface.setChannel(context, channel);
    }

}
