package com.unity3d.unityconnect;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.view.WindowManager;
public class UnityPlayerActivityStatusBar extends UnityPlayerActivity
{
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        getWindow().clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN);
        // Clear low profile flags to apply non-fullscreen mode before splash screen
        showSystemUi();
        addUiVisibilityChangeListener();
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
