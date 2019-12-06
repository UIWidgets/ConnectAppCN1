package com.unity3d.unityconnect;

import com.google.gson.Gson;
import com.huawei.hms.push.HmsMessageService;
import com.huawei.hms.push.RemoteMessage;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;

import java.util.Arrays;
import java.util.HashMap;


public class HuaWeiMessageService extends HmsMessageService {

    @Override
    public void onNewToken(String s) {
        super.onNewToken(s);
        HashMap<String, String> hashMap = new HashMap<>();
        hashMap.put("token", s);
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "RegisterToken", Arrays.asList(new Gson().toJson(hashMap)));
    }

    @Override
    public void onMessageReceived(RemoteMessage remoteMessage) {
        super.onMessageReceived(remoteMessage);

    }

    @Override
    public void onMessageSent(String s) {
        super.onMessageSent(s);

    }

    @Override
    public void onSendError(String s, Exception e) {
        super.onSendError(s, e);

    }
}
