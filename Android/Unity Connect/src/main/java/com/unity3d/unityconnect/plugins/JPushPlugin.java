package com.unity3d.unityconnect.plugins;

import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;

import java.util.Arrays;

public class JPushPlugin {

    private static JPushPlugin instance;

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
}
