package com.unity3d.unityconnect;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;

import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;

import java.util.Arrays;
import java.util.HashMap;

import cn.jpush.android.api.JPushInterface;

import com.google.gson.Gson;

/**
 * 自定义接收器
 *
 * 如果不定义这个 Receiver，则：
 * 1) 默认用户会打开主界面
 * 2) 接收不到自定义消息
 */
public class CustomReceiver extends BroadcastReceiver {
    final Gson gson = new Gson();

    @Override
    public void onReceive(Context context, Intent intent) {
        try {
            if (JPushInterface.ACTION_NOTIFICATION_OPENED.equals(intent.getAction())) {

                Intent mainIntent = new Intent(context, UnityPlayerActivityStatusBar.class);
                mainIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                context.startActivity(mainIntent);
                //获取接收到推送的消息数据
                Bundle bundle = intent.getExtras();
                String JSON = bundle.getString(JPushInterface.EXTRA_EXTRA);//得到的是服务器返回的json串,需要解析才能得到消息内容
                UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnOpenNotification", Arrays.asList(JSON));

            }
        } catch (Exception e){

        }
    }
}
