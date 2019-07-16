package com.unity3d.unityconnect.plugins;
import android.content.Context;
import android.media.AudioManager;
import android.provider.Settings;

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

    /**
     * 判断屏幕旋转功能是否开启
     */
    public static boolean isOpenSensor(){
        boolean isOpen = false;
        if(getSensorState(mContext) == 1){
            isOpen = true;
        }else if(getSensorState(mContext) == 0){
            isOpen = false;
        }
        return isOpen;
    }

    private static int getSensorState(Context context){
        int sensorState = 0;
        try {
            sensorState = Settings.System.getInt(context.getContentResolver(), Settings.System.ACCELEROMETER_ROTATION);
            return sensorState;
        } catch (Settings.SettingNotFoundException e) {
            e.printStackTrace();
        }
        return sensorState;
    }

    public static String getDeviceID() {
        String uuid = UUIDUtils.getUUID();
        return uuid;
    }
}
