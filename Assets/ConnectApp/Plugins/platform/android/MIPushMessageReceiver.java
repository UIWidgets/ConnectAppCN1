package com.unity3d.unityconnect;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;

import com.google.gson.Gson;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;
import com.unity3d.unityconnect.plugins.JPushPlugin;
import com.xiaomi.mipush.sdk.MiPushMessage;
import com.xiaomi.mipush.sdk.PushMessageReceiver;

import java.util.Arrays;

import cn.jpush.android.api.JPushInterface;

public class MIPushMessageReceiver extends PushMessageReceiver {

    @Override
    public void onNotificationMessageArrived(Context context, MiPushMessage miPushMessage) {
        super.onNotificationMessageArrived(context, miPushMessage);
        String JSON = new Gson().toJson(miPushMessage.getExtra());
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnReceiveNotification", Arrays.asList(JSON));
    }

    @Override
    public void onNotificationMessageClicked(Context context, MiPushMessage miPushMessage) {
        super.onNotificationMessageClicked(context, miPushMessage);
        try {
            //打开自定义的Activity
            Intent i = new Intent(context, UnityPlayerActivityStatusBar.class);
            Bundle bundle = new Bundle();
            bundle.putString(JPushInterface.EXTRA_NOTIFICATION_TITLE, miPushMessage.getTitle());
            bundle.putString(JPushInterface.EXTRA_ALERT, miPushMessage.getContent());
            i.putExtras(bundle);
            i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TOP);
            context.startActivity(i);

            //获取接收到推送的消息数据
            String JSON = new Gson().toJson(miPushMessage.getExtra());//得到的是服务器返回的json串,需要解析才能得到消息内容
            JPushPlugin.getInstance().pushJson = JSON;
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnOpenNotification", Arrays.asList(JSON));
        } catch (Throwable throwable) {

        }
    }

    @Override
    public void onReceivePassThroughMessage(Context context, MiPushMessage miPushMessage) {
        super.onReceivePassThroughMessage(context, miPushMessage);
        String JSON = new Gson().toJson(miPushMessage.getExtra());//得到的是服务器返回的json串,需要解析才能得到消息内容
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnReceiveNotification", Arrays.asList(JSON));
    }
}
