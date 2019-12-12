package com.unity3d.unityconnect;

import com.google.gson.Gson;
import com.huawei.hms.push.HmsMessageService;
import com.huawei.hms.push.RemoteMessage;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;


import org.json.JSONException;
import org.json.JSONObject;

import java.util.Arrays;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;


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
        if (remoteMessage.getData().length() > 0) {
            try {
                JSONObject itemsJsonObj = new JSONObject(remoteMessage.getData());
                JSONObject msgContent = itemsJsonObj.getJSONObject("msgContent");
                Iterator<String> keyIter = msgContent.keys();
                String key;
                Object value;
                Map<String, Object> valueMap = new HashMap();
                while (keyIter.hasNext()) {
                    key = keyIter.next();
                    value = msgContent.get(key);
                    valueMap.put(key, value);
                }
                String json = (String) valueMap.get("data");
                UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnReceiveNotification", Arrays.asList(json));

            } catch (JSONException e) {
                e.printStackTrace();
            }

        }

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
