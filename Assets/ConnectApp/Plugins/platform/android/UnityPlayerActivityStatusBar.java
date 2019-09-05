package com.unity3d.unityconnect;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.app.ActivityCompat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.WindowManager;

import com.dueeeke.videoplayer.player.VideoView;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;
import com.unity3d.unityconnect.plugins.AVPlayerPlugin;
import com.unity3d.unityconnect.plugins.JPushPlugin;

import java.util.Arrays;

public class UnityPlayerActivityStatusBar extends UnityPlayerActivity
{
    VideoView videoView;

    CustomVideoController controller;
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        if (this.getIntent().getScheme()!=null&&this.getIntent().getScheme().equals("unityconnect")){
            JPushPlugin.getInstance().schemeUrl = this.getIntent().getDataString();
        }
        getWindow().clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN);

//         Clear low profile flags to apply non-fullscreen mode before splash screen
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            getWindow().setStatusBarColor(Color.WHITE);
        }


        showSystemUi();
        addUiVisibilityChangeListener();

        addDKPlayView();

        applePermission();

        addDKPlayView();
    }

    void addDKPlayView(){
        View view = LayoutInflater.from(this).inflate(R.layout.video_view, null);
        mUnityPlayer.addView(view);
        videoView = (VideoView) view.findViewById(R.id.player);
        controller = new CustomVideoController(this);
        videoView.setVideoController(controller); //设置控制器，如需定制可继承BaseVideoController
        videoView.setLock(false);
        videoView.setVisibility(View.GONE);
        videoView.setAutoRotate(true);

        AVPlayerPlugin.getInstance().videoView = videoView;
        AVPlayerPlugin.getInstance().controller = controller;

    }

    public void applePermission(){

        final int REQUEST_EXTERNAL_STORAGE = 1;
        String[] PERMISSIONS_STORAGE = {
                "android.permission.READ_EXTERNAL_STORAGE",
                "android.permission.WRITE_EXTERNAL_STORAGE" };

        if (Build.VERSION.SDK_INT>=23){
            int permission = ActivityCompat.checkSelfPermission(getApplicationContext(),"android.permission.WRITE_EXTERNAL_STORAGE");
            if (permission != PackageManager.PERMISSION_GRANTED) {
                // 没有写的权限，去申请写的权限，会弹出对话框
                ActivityCompat.requestPermissions(UnityPlayerActivityStatusBar.this, PERMISSIONS_STORAGE,REQUEST_EXTERNAL_STORAGE);
            }
        }
    }

    // Quit Unity
    @Override
    protected void onDestroy() {
        videoView.release();
        super.onDestroy();
    }

    // Pause Unity
    @Override
    protected void onPause() {
        super.onPause();
        videoView.pause();

    }

    // Resume Unity
    @Override
    protected void onResume() {
        super.onResume();
        videoView.resume();
    }

    @Override
    public void onBackPressed() {
        if (!videoView.onBackPressed()) {
            super.onBackPressed();
        }
    }

    @Override
    protected void onRestart() {
        super.onRestart();
        if (this.getIntent().getScheme() != null && this.getIntent().getScheme().equals("unityconnect")){
            String data = this.getIntent().getDataString();
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "OnOpenUrl", Arrays.asList(data));
        }
    }

    private static int getLowProfileFlag()
    {
        return Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT
                ?
                View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY |
                        View.SYSTEM_UI_FLAG_LAYOUT_STABLE |
                        View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN |
                        View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION |
                        View.SYSTEM_UI_FLAG_HIDE_NAVIGATION |
                        View.SYSTEM_UI_FLAG_FULLSCREEN
                :
                View.SYSTEM_UI_FLAG_LOW_PROFILE;
    }

    private void showSystemUi()
    {
        // Works from API level 11
        if (android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.HONEYCOMB)
            return;

        mUnityPlayer.setSystemUiVisibility(mUnityPlayer.getSystemUiVisibility() & ~getLowProfileFlag());
    }

    private void addUiVisibilityChangeListener()
    {
        // Works from API level 11
        if (android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.HONEYCOMB)
            return;

        mUnityPlayer.setOnSystemUiVisibilityChangeListener(new View.OnSystemUiVisibilityChangeListener()
        {
            @Override
            public void onSystemUiVisibilityChange(final int visibility)
            {
                // Whatever changes - force status/nav bar to be visible
                showSystemUi();
            }
        });
    }
}
