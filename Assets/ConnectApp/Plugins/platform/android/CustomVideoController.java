package com.unity3d.unityconnect;

import android.content.Context;
import android.support.annotation.NonNull;
import android.view.View;

import com.dueeeke.videocontroller.StandardVideoController;
import com.dueeeke.videoplayer.player.VideoView;

public class CustomVideoController extends StandardVideoController {

    public CustomVideoController(@NonNull Context context) {
        super(context);

    }

    public boolean showBack;

    public void setShowBack(boolean showBack) {
        this.showBack = showBack;
    }

    @Override
    protected void initView() {
        super.initView();
    }


    @Override
    public void onClick(View v) {
        super.onClick(v);
        int i = v.getId();
        if (i == com.dueeeke.videocontroller.R.id.back) {
//            if (!mMediaPlayer.isFullScreen()) {
//                UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("player", "PopPage", null);
//            }
        }
    }

    @Override
    public void setPlayerState(int playerState) {
        super.setPlayerState(playerState);
        switch (playerState) {
            case VideoView.PLAYER_FULL_SCREEN:
                mLockButton.setVisibility(GONE);
                break;
            case VideoView.PLAYER_NORMAL:
                showStatusView();
                mTopContainer.setVisibility(showBack?VISIBLE:GONE);
                mLockButton.setVisibility(GONE);
                mBackButton.setVisibility(showBack?VISIBLE:GONE);
                break;
        }
    }

    @Override
    public void hide() {
        super.hide();
        mLockButton.setVisibility(GONE);
        mBackButton.setVisibility(GONE);
    }

    @Override
    public void show() {
        super.show();
        mLockButton.setVisibility(GONE);
        mBackButton.setVisibility(showBack?VISIBLE:GONE);

    }
}
