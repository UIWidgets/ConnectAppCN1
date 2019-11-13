package com.unity3d.unityconnect;

import android.app.Notification;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.media.AudioManager;

import com.unity3d.unityconnect.plugins.CommonPlugin;

import cn.jpush.android.api.BasicPushNotificationBuilder;
import cn.jpush.android.api.JPushInterface;

public class SystemReceiver extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        if (intent.getAction().equals(AudioManager.RINGER_MODE_CHANGED_ACTION)) {
            BasicPushNotificationBuilder builder = new BasicPushNotificationBuilder(CommonPlugin.mContext);
            AudioManager am = (AudioManager)CommonPlugin.mContext.getSystemService(Context.AUDIO_SERVICE);
            final int ringerMode = am.getRingerMode();
            switch (ringerMode) {
                case AudioManager.RINGER_MODE_NORMAL:
                    //normal
                    builder.notificationDefaults =  Notification.DEFAULT_VIBRATE
                            | Notification.DEFAULT_LIGHTS | Notification.DEFAULT_SOUND; // 设置为声音、震动、呼吸灯闪烁
                    break;
                case AudioManager.RINGER_MODE_VIBRATE:
                    //vibrate
                    builder.notificationDefaults =  Notification.DEFAULT_VIBRATE
                            | Notification.DEFAULT_LIGHTS; // 设置为震动、呼吸灯闪烁
                    JPushInterface.setPushNotificationBuilder(1, builder);
                    break;
                case AudioManager.RINGER_MODE_SILENT:
                    //silent
                    builder.notificationDefaults = Notification.DEFAULT_LIGHTS; // 设置为呼吸灯闪烁
                    JPushInterface.setPushNotificationBuilder(1, builder);
                    break;
            }
        }
    }
}
