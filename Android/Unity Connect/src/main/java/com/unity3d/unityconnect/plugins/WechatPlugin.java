package com.unity3d.unityconnect.plugins;

import android.content.Context;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.graphics.BitmapFactory;

import com.google.gson.Gson;
import com.tencent.mm.opensdk.modelmsg.SendAuth;
import com.tencent.mm.opensdk.modelmsg.SendMessageToWX;
import com.tencent.mm.opensdk.modelmsg.WXMediaMessage;
import com.tencent.mm.opensdk.modelmsg.WXWebpageObject;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;
import com.unity3d.player.UnityPlayer;

import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

//singleton
public final class WechatPlugin{

    private static WechatPlugin instance;


    public static WechatPlugin getInstance() {
        synchronized (WechatPlugin.class) {
            if (instance == null) {
                instance = new WechatPlugin();
            }
            return instance;
        }
    }

    public IWXAPI iwxapi;
    public Context context;

    private void loginWechat(String id) {
        final SendAuth.Req req = new SendAuth.Req();
        req.scope = "snsapi_userinfo";
        req.state = id;
        iwxapi.sendReq(req);
    }

    private void shareToFriends(String title, String description, String url, byte[] imageBytes) {
        shareTo(SendMessageToWX.Req.WXSceneSession,title,description,url,imageBytes);
    }

    private void shareToTimeline(String title, String description, String url, byte[] imageBytes) {
        shareTo(SendMessageToWX.Req.WXSceneTimeline, title, description, url, imageBytes);
    }

    private boolean isInstallWechat() {
        return iwxapi.isWXAppInstalled();
    }

    private void shareTo(final int scene, String title, String description, String url, byte[] imageBytes) {
        WXWebpageObject webpageObject = new WXWebpageObject();
        webpageObject.webpageUrl = url;

        final WXMediaMessage message = new WXMediaMessage(webpageObject);
        message.title = title;
        message.description = description;
        message.setThumbImage(BitmapFactory.decodeByteArray(imageBytes, 0, imageBytes.length));
        final SendMessageToWX.Req req = new SendMessageToWX.Req();

        req.scene = scene;
        req.message = message;
        req.transaction = "webpage" + String.valueOf(System.currentTimeMillis());

        iwxapi.sendReq(req);
    }

    final Gson gson = new Gson();

    private void sendBack(final HashMap<String, Object> model) {

        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("wechat", "callback", Arrays.asList(gson.toJson(model)));
    }

    public void sendBackAuthCode(final String code, final String id) {
        HashMap<String, Object> event = new HashMap<>();
        event.put("type", "code");
        event.put("code", code);
        event.put("id", id);
        sendBack(event);
    }

    private void sendBackAuthCancel() {
        HashMap<String, Object> event = new HashMap<>();
        event.put("type", "cancel");
        sendBack(event);
    }

}
