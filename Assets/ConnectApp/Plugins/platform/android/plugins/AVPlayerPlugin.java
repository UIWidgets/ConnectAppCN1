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

    public int limitSeconds;

    public boolean needUpdate;

    public static AVPlayerPlugin getInstance() {
        synchronized (AVPlayerPlugin.class) {
            if (instance == null) {
                instance = new AVPlayerPlugin();
            }
            return instance;
        }
    }

    public void InitPlayer(String url, String cookie, float left, float top, float width, float height, boolean isPop, boolean needUpdate, int limitSeconds) {
        this.limitSeconds = limitSeconds;
        this.needUpdate = needUpdate;
        Map<String, String> header = new HashMap<>();
        header.put("Cookie", cookie);
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {

                controller.showBack = isPop;
                controller.setPlayState(VideoView.STATE_PAUSED);
                controller.setPlayerState(VideoView.PLAYER_NORMAL);
                RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams(
                        ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
                if (isPop) {
                    lp.height = (int) height;
                    lp.setMargins((int) left, 0, 0, 0);//设置margin,此处单位为px
                } else {
                    lp.addRule(RelativeLayout.CENTER_IN_PARENT);
                }
                videoView.setLayoutParams(lp);//动态改变布局
                if (!url.isEmpty()) {
                    videoView.setVisibility(View.VISIBLE);
                    videoView.setUrl(url, header);
                }
                if (!isPop) videoView.start();

            }
        });
    }

    public void ConfigPlayer(String url, String cookie) {
        Map<String, String> header = new HashMap<>();
        header.put("Cookie", cookie);
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                controller.show();
                videoView.setVisibility(View.VISIBLE);
                videoView.setUrl(url, header);
                videoView.pause();
            }
        });
    }

    public void VideoRelease() {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                controller.hiddenUpdateView();
                videoView.release();
                videoView.setVisibility(View.GONE);
            }
        });

    }

    public void VideoPlay() {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                videoView.resume();
            }
        });

    }

    public void VideoPause() {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                videoView.pause();
            }
        });

    }

    public void VideoShow() {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                videoView.setVisibility(View.VISIBLE);
            }
        });

    }

    public void VideoHidden() {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                videoView.pause();
                videoView.setVisibility(View.GONE);
            }
        });
    }
}
