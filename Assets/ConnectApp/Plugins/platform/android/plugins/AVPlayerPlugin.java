package com.unity3d.unityconnect.plugins;

import android.content.Context;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;

import com.dueeeke.videoplayer.player.VideoView;
import com.unity3d.player.UnityPlayer;
import com.unity3d.unityconnect.CustomVideoController;

import java.util.HashMap;
import java.util.Map;

public class AVPlayerPlugin {
    private static AVPlayerPlugin instance;

    public Context context;

    public VideoView videoView;

    public CustomVideoController controller;


    public static AVPlayerPlugin getInstance() {
        synchronized (AVPlayerPlugin.class) {
            if (instance == null) {
                instance = new AVPlayerPlugin();
            }
            return instance;
        }
    }
    public void InitPlayer(String url, String cookie, float left, float top, float width, float height, boolean isPop){
        Map<String, String> header = new HashMap<>();
        header.put("Cookie", cookie);
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                controller.showBack = isPop;
                RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams(
                        ViewGroup.LayoutParams.MATCH_PARENT, (int)height);
                if (isPop){
                    lp.setMargins((int)left, (int)top, 0, 0);//设置margin,此处单位为px
                }else{
                    lp.addRule(RelativeLayout.CENTER_IN_PARENT);
                }
                videoView.setLayoutParams(lp);//动态改变布局
                videoView.setVisibility(View.VISIBLE);
                videoView.setUrl(url,header);
                if(!isPop) videoView.start();
            }
        });
    }

    public void VideoRelease(){
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                videoView.release();
                videoView.setVisibility(View.GONE);
            }
        });

    }

    public void VideoPlay(){
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                videoView.start();
                videoView.setVisibility(View.GONE);
            }
        });

    }

    public void VideoPause(){
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                videoView.pause();
                videoView.setVisibility(View.GONE);
            }
        });

    }
}
