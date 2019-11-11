package com.unity3d.unityconnect.plugins;

import android.content.Context;
import android.os.Vibrator;
import android.text.TextUtils;
import com.google.gson.Gson;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.util.Arrays;
import java.util.HashMap;
import java.util.LinkedHashSet;
import java.util.Map;
import java.util.Set;
import cn.jpush.android.api.JPushInterface;

public class JPushPlugin {

    private static JPushPlugin instance;

    public Context mContext;

    public static JPushPlugin getInstance() {
        synchronized (JPushPlugin.class) {
            if (instance == null) {
                instance = new JPushPlugin();
            }
            return instance;
        }
    }

    public String pushJson;

    public String schemeUrl;

    public boolean isListenCompleted;

    public void listenCompleted(){
        isListenCompleted = true;
        Boolean needPush = false;
        if (pushJson != null || schemeUrl != null){
            needPush = true;
        }
        Map<String, Boolean> map = new HashMap<String, Boolean>();
        map.put("push", needPush);
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "CompletedCallback", Arrays.asList(new Gson().toJson(map)));
        if (pushJson != null){
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnOpenNotification", Arrays.asList(pushJson));
        }
        if (schemeUrl != null){
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnOpenUrl", Arrays.asList(schemeUrl));
        }
    }

    public void setChannel(String channel){
        JPushInterface.setChannel(mContext, channel);
    }

    public void setAlias(int sequence, String alias){
        JPushInterface.setAlias(mContext, sequence, alias);
    }

    public void deleteAlias(int sequence){
        JPushInterface.deleteAlias(mContext, sequence);
    }

    public void setTags(int sequence, String tagsJsonStr){
        if (TextUtils.isEmpty(tagsJsonStr)) {
            return;
        }

        Set<String> tagSet = new LinkedHashSet<String>();

        try {
            JSONObject itemsJsonObj = new JSONObject(tagsJsonStr);
            JSONArray tagsJsonArr = itemsJsonObj.getJSONArray("Items");

            for (int i = 0; i < tagsJsonArr.length(); i++) {
                tagSet.add(tagsJsonArr.getString(i));
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }

        JPushInterface.setTags(mContext, sequence, tagSet);
    }
    
    public void playSystemSound(){
        Vibrator vibrator = (Vibrator)mContext.getSystemService(Context.VIBRATOR_SERVICE);
        vibrator.vibrate(200L);
    }
}
