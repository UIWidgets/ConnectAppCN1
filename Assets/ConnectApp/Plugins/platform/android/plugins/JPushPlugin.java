package com.unity3d.unityconnect.plugins;

import android.app.NotificationManager;
import android.content.Context;
import android.text.TextUtils;

import com.google.gson.Gson;
import com.google.zxing.util.FeedbackUtil;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;
import com.unity3d.unityconnect.RomUtils;
import com.xiaomi.mipush.sdk.MiPushClient;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.Arrays;
import java.util.HashMap;
import java.util.LinkedHashSet;
import java.util.Map;
import java.util.Set;

import cn.jpush.android.api.JPushInterface;

import static android.content.Context.NOTIFICATION_SERVICE;

public class JPushPlugin {

    private static JPushPlugin instance;

    private static String hmsAppId = "100771325";


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

    public String hmsToken;


    public boolean isListenCompleted;

    public void listenCompleted() {
        isListenCompleted = true;
        Boolean needPush = false;
        if (pushJson != null || schemeUrl != null) {
            needPush = true;
        }
        Map<String, Boolean> map = new HashMap<String, Boolean>();
        map.put("push", needPush);
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "CompletedCallback", Arrays.asList(new Gson().toJson(map)));
        if (pushJson != null) {
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnOpenNotification", Arrays.asList(pushJson));
        }
        if (schemeUrl != null) {
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnOpenUrl", Arrays.asList(schemeUrl));
        }
        if (RomUtils.isHuawei()) {
            getToken();
        }
    }

    public void registerToken(String token) {
        hmsToken = token;
        HashMap<String, String> hashMap = new HashMap<>();
        hashMap.put("token", token);
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "RegisterToken", Arrays.asList(new Gson().toJson(hashMap)));
    }


    /**
     * get token
     */
    private void getToken() {
    }

    public void setChannel(String channel) {
        if (RomUtils.isXiaomi() || RomUtils.isHuawei()) return;
        JPushInterface.setChannel(mContext, channel);
    }

    public void setAlias(int sequence, String alias) {
        if (RomUtils.isXiaomi()) {
            MiPushClient.setAlias(mContext, alias, null);
        } else if (RomUtils.isHuawei()) {
            return;
        } else {
            JPushInterface.setAlias(mContext, sequence, alias);
        }
    }

    public void deleteAlias(int sequence, String alias) {
        if (RomUtils.isXiaomi()) {
            MiPushClient.unsetAlias(mContext, alias, null);
        } else if (RomUtils.isHuawei()) {
            return;
        } else {
            JPushInterface.deleteAlias(mContext, sequence);
        }
    }


    public void setTags(int sequence, String tagsJsonStr) {
        if (TextUtils.isEmpty(tagsJsonStr)) {
            return;
        }
        Set<String> tagSet = new LinkedHashSet<String>();

        try {
            JSONObject itemsJsonObj = new JSONObject(tagsJsonStr);
            JSONArray tagsJsonArr = itemsJsonObj.getJSONArray("Items");
            if (RomUtils.isXiaomi()) {
                for (int i = 0; i < tagsJsonArr.length(); i++) {
                    MiPushClient.subscribe(mContext, tagsJsonArr.getString(i), "");
                }
            } else if (RomUtils.isHuawei()) {
            } else {
                for (int i = 0; i < tagsJsonArr.length(); i++) {
                    tagSet.add(tagsJsonArr.getString(i));
                }
                JPushInterface.setTags(mContext, sequence, tagSet);
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }


    }

    public void playSystemSound() {
        FeedbackUtil.playBeepSoundAndVibrate(mContext, FeedbackUtil.SoundType.NOTIFICATION, false, true);
    }

    public void clearAllAlert() {
        if (RomUtils.isXiaomi()) {
            MiPushClient.clearNotification(mContext);
        }
        NotificationManager notiManager = (NotificationManager) this.mContext.getSystemService(NOTIFICATION_SERVICE);
        notiManager.cancelAll();
    }

    public void clearBadge() {
    }
}
