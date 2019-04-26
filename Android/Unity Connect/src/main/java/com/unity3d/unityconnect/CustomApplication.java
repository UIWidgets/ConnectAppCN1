package com.unity3d.unityconnect;

import android.app.Application;

import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.unity3d.unityconnect.plugins.WechatPlugin;
import cn.jpush.android.api.JPushInterface;

public class CustomApplication extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        IWXAPI iwxapi = WXAPIFactory.createWXAPI(this, "wx0ab79f0c7db7ca52", true);
        WechatPlugin.getInstance().iwxapi = iwxapi;
        iwxapi.registerApp("wx0ab79f0c7db7ca52");

        JPushInterface.init(this);     		// 初始化 JPush

    }
}
