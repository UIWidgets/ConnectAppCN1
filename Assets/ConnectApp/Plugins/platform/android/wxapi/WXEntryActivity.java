package com.unity3d.unityconnect.wxapi;

import android.app.Activity;
import android.os.Bundle;

import com.tencent.mm.opensdk.constants.ConstantsAPI;
import com.tencent.mm.opensdk.modelbase.BaseReq;
import com.tencent.mm.opensdk.modelbase.BaseResp;
import com.tencent.mm.opensdk.modelbiz.WXLaunchMiniProgram;
import com.tencent.mm.opensdk.modelmsg.SendAuth;
import com.tencent.mm.opensdk.openapi.IWXAPIEventHandler;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;
import com.unity3d.unityconnect.plugins.WechatPlugin;

import java.util.Arrays;

public class WXEntryActivity extends Activity implements IWXAPIEventHandler {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        WechatPlugin.getInstance().iwxapi.handleIntent(getIntent(), this);
        finish();
    }

    @Override
    public void onReq(BaseReq baseReq) {

    }

    @Override
    public void onResp(BaseResp baseResp) {
        if (baseResp instanceof SendAuth.Resp) {
            SendAuth.Resp resp = (SendAuth.Resp) baseResp;
            WechatPlugin.getInstance().sendBackAuthCode(resp.code,resp.state);
        }
        if (baseResp.getType() == ConstantsAPI.COMMAND_LAUNCH_WX_MINIPROGRAM) {
            WXLaunchMiniProgram.Resp launchMiniProResp = (WXLaunchMiniProgram.Resp) baseResp;
            String extraData =launchMiniProResp.extMsg; // 对应JsApi navigateBackApplication中的extraData字段数据
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("wechat", "openUrl", Arrays.asList(extraData));

        }
    }
}