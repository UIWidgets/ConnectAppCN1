package com.unity3d.unityconnect;

import android.app.Application;

import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.unity3d.unityconnect.plugins.CommonPlugin;
import com.unity3d.unityconnect.plugins.JAnalyticsPlugin;
import com.unity3d.unityconnect.plugins.JPushPlugin;
import com.unity3d.unityconnect.plugins.QRScanPlugin;
import com.unity3d.unityconnect.plugins.UUIDUtils;
import com.unity3d.unityconnect.plugins.WechatPlugin;

import cn.jiguang.analytics.android.api.JAnalyticsInterface;
import cn.jpush.android.api.JPushInterface;

public class CustomApplication extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        IWXAPI iwxapi = WXAPIFactory.createWXAPI(this, "wx0ab79f0c7db7ca52", true);
        WechatPlugin.getInstance().iwxapi = iwxapi;
        iwxapi.registerApp("wx0ab79f0c7db7ca52");

        JPushPlugin.getInstance().mContext = this;
        JAnalyticsPlugin.getInstance().context = this;
        QRScanPlugin.getInstance().context = this;
        CommonPlugin.mContext = this;

        JPushInterface.init(this);     		// 初始化 JPush
        JAnalyticsInterface.init(this);

        UUIDUtils.buidleID(this).check();
    }
}
