package com.unity3d.unityconnect.plugins;

import android.content.Context;

import com.google.gson.Gson;
import java.util.Map;

import cn.jiguang.analytics.android.api.BrowseEvent;
import cn.jiguang.analytics.android.api.CalculateEvent;
import cn.jiguang.analytics.android.api.CountEvent;
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
        LoginEvent event = new LoginEvent(type,true);
        JAnalyticsInterface.onEvent(context,event);
    }

    public void countEvent(String eventId,String extra){
        CountEvent event = new CountEvent(eventId);
        if (extra != null) {
            Map map = new Gson().fromJson(extra,Map.class);
            event.setExtMap(map);
        }
        JAnalyticsInterface.onEvent(context,event);
    }

    public void calculateEvent(String eventId,String value,String extra){
        CalculateEvent event = new CalculateEvent(eventId,Double.parseDouble(value));
        if (extra != null) {
            Map map = new Gson().fromJson(extra,Map.class);
            event.setExtMap(map);
        }
        JAnalyticsInterface.onEvent(context,event);
    }

    public void browseEvent(String eventId,String name, String type,String duration, String extra){
        BrowseEvent event = new BrowseEvent();
        event.setBrowseId(eventId);
        event.setBrowseName(name);
        event.setBrowseType(type);
        event.setBrowseDuration(Float.parseFloat(duration));
        if (extra != null) {
            Map map = new Gson().fromJson(extra,Map.class);
            event.setExtMap(map);
        }
        JAnalyticsInterface.onEvent(context,event);
    }
}
