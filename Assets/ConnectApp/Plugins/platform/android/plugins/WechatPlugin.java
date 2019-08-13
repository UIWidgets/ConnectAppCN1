package com.unity3d.unityconnect.plugins;

import android.content.Context;
import android.graphics.BitmapFactory;
import android.util.Base64;

import com.google.gson.Gson;
import com.tencent.mm.opensdk.modelbiz.WXLaunchMiniProgram;
import com.tencent.mm.opensdk.modelmsg.SendAuth;
import com.tencent.mm.opensdk.modelmsg.SendMessageToWX;
import com.tencent.mm.opensdk.modelmsg.WXMediaMessage;
import com.tencent.mm.opensdk.modelmsg.WXMiniProgramObject;
import com.tencent.mm.opensdk.modelmsg.WXWebpageObject;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;

import java.util.Arrays;
import java.util.HashMap;

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

    private void shareToFriends(String title, String description, String url, String imageStr) {
        byte[] imageBytes = Base64.decode(imageStr, Base64.DEFAULT);
        shareTo(SendMessageToWX.Req.WXSceneSession,title,description,url,imageBytes);
    }

    private void shareToTimeline(String title, String description, String url, String imageStr) {
        byte[] imageBytes = Base64.decode(imageStr, Base64.DEFAULT);
        shareTo(SendMessageToWX.Req.WXSceneTimeline, title, description, url, imageBytes);
    }

    private void shareToMiNiProgram(String title, String description, String url, String imageStr, String ysId, String path, int WXMiniProgramType) {
        byte[] imageBytes = Base64.decode(imageStr, Base64.DEFAULT);
        WXMiniProgramObject miniProgramObj = new WXMiniProgramObject();
        miniProgramObj.webpageUrl = url; // 兼容低版本的网页链接
        miniProgramObj.miniprogramType = WXMiniProgramType;// 正式版:0，测试版:1，体验版:2
        miniProgramObj.userName = ysId;     // 小程序原始id
        miniProgramObj.path = path;            //小程序页面路径
        WXMediaMessage msg = new WXMediaMessage(miniProgramObj);
        msg.title = title;                    // 小程序消息title
        msg.description = description;               // 小程序消息desc
        msg.thumbData = imageBytes;                      // 小程序消息封面图片，小于128k

        SendMessageToWX.Req req = new SendMessageToWX.Req();
        req.transaction = buildTransaction("miniProgram");
        req.message = msg;
        req.scene = SendMessageToWX.Req.WXSceneSession;  // 目前只支持会话
        iwxapi.sendReq(req);
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
        req.transaction = buildTransaction("webpage");

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
    public void openMiNi(String appId, String ysId, String path, int WXMiniProgramType){
        final IWXAPI iwxapi = WXAPIFactory.createWXAPI(context, appId);
        iwxapi.registerApp(appId);
        WXLaunchMiniProgram.Req req = new WXLaunchMiniProgram.Req();
        req.userName = ysId;//小程序原始id
        req.path = path;//页面路径  pages/index/index?page=1
        req.miniprogramType = WXMiniProgramType;// 可选打开 开发版，体验版和正式版
        iwxapi.sendReq(req);
    }

    private String buildTransaction(final String type) {
        return (type == null) ? String.valueOf(System.currentTimeMillis()) : type + System.currentTimeMillis();
    }

}
