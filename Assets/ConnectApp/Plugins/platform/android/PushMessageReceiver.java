package com.unity3d.unityconnect;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;

import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;
import com.unity3d.unityconnect.plugins.JPushPlugin;

import java.util.Arrays;

import cn.jpush.android.api.CustomMessage;
import cn.jpush.android.api.JPushInterface;
import cn.jpush.android.api.NotificationMessage;
import cn.jpush.android.service.JPushMessageReceiver;

public class PushMessageReceiver extends JPushMessageReceiver{
    @Override
    public void onMessage(Context context, CustomMessage customMessage) {

    }

    @Override
    public void onNotifyMessageOpened(Context context, NotificationMessage message) {
        try{
            //打开自定义的Activity
            Intent i = new Intent(context, UnityPlayerActivityStatusBar.class);
            Bundle bundle = new Bundle();
            bundle.putString(JPushInterface.EXTRA_NOTIFICATION_TITLE,message.notificationTitle);
            bundle.putString(JPushInterface.EXTRA_ALERT,message.notificationContent);
            i.putExtras(bundle);
            i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TOP );
            context.startActivity(i);

            //获取接收到推送的消息数据
            String JSON = message.notificationExtras;//得到的是服务器返回的json串,需要解析才能得到消息内容
            JPushPlugin.getInstance().pushJson = JSON;
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnOpenNotification", Arrays.asList(JSON));
        }catch (Throwable throwable){

        }
    }

    @Override
    public void onNotifyMessageArrived(Context context, NotificationMessage message) {
        String JSON = message.notificationExtras;
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnReceiveNotification", Arrays.asList(JSON));
    }
}
