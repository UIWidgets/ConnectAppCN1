package com.unity3d.unityconnect;

import android.app.Activity;
import android.content.Context;
import android.content.res.Configuration;
import android.view.View;
import com.dueeeke.videocontroller.StandardVideoController;
import com.dueeeke.videoplayer.player.VideoView;
import com.dueeeke.videoplayer.util.PlayerUtils;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;
import com.unity3d.unityconnect.plugins.AVPlayerPlugin;

public class CustomVideoController extends StandardVideoController {
    public CustomVideoController(Context context) {
        super(context);
    }

    public boolean showBack;

    public void setShowBack(boolean showBack) {
        this.showBack = showBack;
        mTopContainer.setVisibility(showBack?VISIBLE:GONE);
    }

    @Override
    protected void initView() {
        super.initView();
    }

    @Override
    public void onClick(View v) {
        int i = v.getId();
        if (i == com.dueeeke.videocontroller.R.id.fullscreen || i == com.dueeeke.videocontroller.R.id.stop_fullscreen) {
            startStopFullScreen();
        } else if (i == com.dueeeke.videocontroller.R.id.lock) {
            doLockUnlock();
        } else if (i == com.dueeeke.videocontroller.R.id.iv_play || i == com.dueeeke.videocontroller.R.id.thumb) {
            doPauseResume();
        } else if (i == com.dueeeke.videocontroller.R.id.iv_replay || i == com.dueeeke.videocontroller.R.id.iv_refresh) {
            mMediaPlayer.replay(true);
        }else if(i == com.dueeeke.videocontroller.R.id.back){
            if (!mMediaPlayer.isFullScreen()){
                UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("player", "PopPage", null);
            }else{
                startStopFullScreen();
            }
        }else if(i == com.dueeeke.videocontroller.R.id.share){
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("player", "Share", null);
        }else if(i == com.dueeeke.videocontroller.R.id.upgrade){
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("player", "BuyLincese", null);
        }else if(i == com.dueeeke.videocontroller.R.id.subscribe){
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("player", "UpdateLincese", null);
        }
    }

    @Override
    public void showStatusView() {
        this.removeView(mStatusView);
    }

    /**
     * 横竖屏切换
     */
    protected void startStopFullScreen() {
        Activity activity = PlayerUtils.scanForActivity(getContext());
        int orientation = activity.getRequestedOrientation();
        if (activity == null) return;
        int currentOrientation = getResources().getConfiguration().orientation;
        //判断并设置用户点击全屏/半屏按钮的显示逻辑
        if (currentOrientation == Configuration.ORIENTATION_LANDSCAPE) {
            //如果屏幕当前是横屏显示，则设置屏幕锁死为竖屏显示
            mMediaPlayer.stopFullScreen();
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("player", "Portrait", null);
        } else if (currentOrientation == Configuration.ORIENTATION_PORTRAIT) {
            //如果屏幕当前是竖屏显示，则设置屏幕锁死为横屏显示
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("player", "LandscapeLeft", null);
            mMediaPlayer.startFullScreen();
        }
    }

    @Override
    public void setPlayerState(int playerState) {
        super.setPlayerState(playerState);
        switch (playerState) {
            case VideoView.PLAYER_FULL_SCREEN:
                mLockButton.setVisibility(GONE);
                mShareButton.setVisibility(GONE);
                hideStatusView();
                break;
            case VideoView.PLAYER_NORMAL:
                showStatusView();
                mTopContainer.setVisibility(showBack?VISIBLE:GONE);
                mLockButton.setVisibility(GONE);
                mBackButton.setVisibility(showBack?VISIBLE:GONE);
                mShareButton.setVisibility(showBack?VISIBLE:GONE);
                break;
        }
    }

    @Override
    public void setPlayState(int playState) {
        super.setPlayState(playState);
        if (playState == VideoView.STATE_PLAYBACK_COMPLETED){
            mVideoProgress.setProgress(mVideoProgress.getMax());
        }else if (playState == VideoView.STATE_PREPARING){
            mVideoProgress.setProgress(0);

        }
    }

    @Override
    protected int setProgress() {
        int position = (int) mMediaPlayer.getCurrentPosition();
        if (AVPlayerPlugin.getInstance().needUpdate&&AVPlayerPlugin.getInstance().limitSeconds*1000<position){
            AVPlayerPlugin.getInstance().videoView.release();
            mStartPlayButton.setVisibility(GONE);
            mUpdateContainer.setVisibility(VISIBLE);
        }
        return super.setProgress();
    }

    public void hiddenUpdateView(){
        mUpdateContainer.setVisibility(GONE);
    }
}
