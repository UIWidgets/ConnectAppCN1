package com.unity3d.unityconnect;

import android.app.ActivityManager;
import android.app.Application;
import android.content.Context;
import android.os.Bundle;
import android.os.Process;

import com.dueeeke.videoplayer.ijk.IjkPlayerFactory;
import com.dueeeke.videoplayer.player.VideoViewConfig;
import com.dueeeke.videoplayer.player.VideoViewManager;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.unity3d.unityconnect.plugins.CommonPlugin;
import com.unity3d.unityconnect.plugins.JAnalyticsPlugin;
import com.unity3d.unityconnect.plugins.JPushPlugin;
import com.unity3d.unityconnect.plugins.QRScanPlugin;
import com.unity3d.unityconnect.plugins.UrlLauncherPlugin;
import com.unity3d.unityconnect.plugins.UUIDUtils;
import com.unity3d.unityconnect.plugins.WechatPlugin;
import com.xiaomi.mipush.sdk.MiPushClient;

import java.util.Date;
import java.util.List;

import cn.jiguang.analytics.android.api.JAnalyticsInterface;
import cn.jpush.android.api.JPushInterface;

public class CustomApplication extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        IWXAPI iwxapi = WXAPIFactory.createWXAPI(this, "wx0ab79f0c7db7ca52", true);
        WechatPlugin.getInstance().iwxapi = iwxapi;
        WechatPlugin.getInstance().context = this;

        iwxapi.registerApp("wx0ab79f0c7db7ca52");

        JPushPlugin.getInstance().mContext = this;
        JAnalyticsPlugin.getInstance().context = this;
        QRScanPlugin.getInstance().context = this;
        UrlLauncherPlugin.getInstance().context = this;
        CommonPlugin.mContext = this;

        if (RomUtils.isXiaomi()) {
            if (shouldInit()) {
                MiPushClient.registerPush(this, "2882303761517998811", "5581799889811");
            }
        } else if (RomUtils.isHuawei()) {
            initialHWAnalytics();
        } else {
            JPushInterface.init(this);            // 初始化 JPush
            JAnalyticsInterface.init(this);
        }

        UUIDUtils.buidleID(this).check();

        VideoViewManager.setConfig(VideoViewConfig.newBuilder()
                .setPlayOnMobileNetwork(true)
                .setPlayerFactory(IjkPlayerFactory.create(this))
                .setAutoRotate(false)
                .build());

    }

    void initialHWAnalytics() {
    }


    private boolean shouldInit() {
        ActivityManager am = ((ActivityManager) getSystemService(Context.ACTIVITY_SERVICE));
        List<ActivityManager.RunningAppProcessInfo> processInfos = am.getRunningAppProcesses();
        String mainProcessName = getPackageName();
        int myPid = Process.myPid();
        for (ActivityManager.RunningAppProcessInfo info : processInfos) {
            if (info.pid == myPid && mainProcessName.equals(info.processName)) {
                return true;
            }
        }
        return false;
    }
}
