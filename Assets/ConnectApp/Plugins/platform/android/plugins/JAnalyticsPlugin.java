package com.unity3d.unityconnect.plugins;

import android.content.Context;
import cn.jiguang.analytics.android.api.JAnalyticsInterface;
import cn.jiguang.analytics.android.api.LoginEvent;

public class JAnalyticsPlugin {

    private static JAnalyticsPlugin instance;

    public Context context;

    public static JAnalyticsPlugin getInstance() {
        synchronized (JAnalyticsPlugin.class) {
            if (instance == null) {
                instance = new JAnalyticsPlugin();
            }
            return instance;
        }
    }
    public void startLogPageView(String pageName){
        JAnalyticsInterface.onPageStart(context,pageName);
    }

    public void stopLogPageView(String pageName){
        JAnalyticsInterface.onPageEnd(context,pageName);
    }

    public void loginEvent(String type){
        LoginEvent loginEvent = new LoginEvent(type,true);
        JAnalyticsInterface.onEvent(context,loginEvent);
    }
}
