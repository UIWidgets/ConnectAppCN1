package com.unity3d.unityconnect.plugins;

import android.content.Context;
import android.media.AudioFocusRequest;
import android.media.AudioManager;

public class CommonPlugin {

    public static Context mContext;

    public static void pauseAudioSession(){

        AudioManager.OnAudioFocusChangeListener afChangeListener = new AudioManager.OnAudioFocusChangeListener() {
            public void onAudioFocusChange(int focusChange) {
                switch (focusChange) {
                    //重新获取焦点
                    case AudioManager.AUDIOFOCUS_GAIN:
                        //判断是否需要重新播放音乐

                        break;
                    //暂时失去焦点
                    case AudioManager.AUDIOFOCUS_LOSS_TRANSIENT:
                        //暂时失去焦点，暂停播放音乐（将needRestart设置为true）

                        break;
                    //时期焦点
                    case AudioManager.AUDIOFOCUS_LOSS:
                        //暂停播放音乐，不再继续播放

                        break;
                }
            }
        };
        AudioManager manager = (AudioManager) mContext.getSystemService(Context.AUDIO_SERVICE);
        manager.requestAudioFocus(afChangeListener,AudioManager.STREAM_MUSIC,AudioManager.AUDIOFOCUS_GAIN_TRANSIENT);

    }

}
