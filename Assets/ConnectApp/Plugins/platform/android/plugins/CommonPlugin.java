package com.unity3d.unityconnect.plugins;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.media.AudioManager;
import android.net.Uri;
import android.os.Environment;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.v4.app.NotificationManagerCompat;
import android.util.Base64;
import android.view.View;

import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;
import com.unity3d.player.UnityPlayer;
import com.unity3d.unityconnect.PickImageActivity;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Arrays;

public class CommonPlugin {

    public static Context mContext;

    public static View splashView;

    public static void pauseAudioSession() {

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
        manager.requestAudioFocus(afChangeListener, AudioManager.STREAM_MUSIC, AudioManager.AUDIOFOCUS_GAIN_TRANSIENT);
    }

    /**
     * 判断屏幕旋转功能是否开启
     */
    public static boolean isOpenSensor() {
        boolean isOpen = false;
        if (getSensorState(mContext) == 1) {
            isOpen = true;
        } else if (getSensorState(mContext) == 0) {
            isOpen = false;
        }
        return isOpen;
    }

    public static void pickImage(String source, boolean cropped, int maxSize) {
        Intent intent = new Intent(mContext, PickImageActivity.class);
        intent.putExtra("type", "image");
        intent.putExtra("source", source);
        intent.putExtra("cropped", cropped);
        intent.putExtra("maxSize", maxSize);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        mContext.startActivity(intent);
    }

    public static void pickVideo(String source) {
        Intent intent = new Intent(mContext, PickImageActivity.class);
        intent.putExtra("type", "video");
        intent.putExtra("source", source);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        mContext.startActivity(intent);
    }

    public static void saveImage(String image) {
        byte[] bytes = Base64.decode(image, Base64.DEFAULT);
        Bitmap bitmap = BitmapFactory.decodeByteArray(bytes, 0, bytes.length);
        if (saveImageToGallery(mContext, bitmap)) {
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "SaveImageSuccess", Arrays.asList("error"));

        } else {
            UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("jpush", "SaveImageError", Arrays.asList("error"));
        }
    }

    public static boolean saveImageToGallery(Context context, Bitmap bmp) {
        String storePath = Environment.getExternalStorageDirectory().getAbsolutePath() + File.separator + "dearxy";
        File appDir = new File(storePath);
        if (!appDir.exists()) {
            appDir.mkdir();
        }
        String fileName = System.currentTimeMillis() + ".jpg";
        File file = new File(appDir, fileName);
        try {
            FileOutputStream fos = new FileOutputStream(file);
            boolean isSuccess = bmp.compress(Bitmap.CompressFormat.JPEG, 60, fos);
            fos.flush();
            fos.close();

            //把文件插入到系统图库
            MediaStore.Images.Media.insertImage(context.getContentResolver(), file.getAbsolutePath(), fileName, null);

            //保存图片后发送广播通知更新数据库
            Uri uri = Uri.fromFile(file);
            context.sendBroadcast(new Intent(Intent.ACTION_MEDIA_SCANNER_SCAN_FILE, uri));
            if (isSuccess) {
                return true;
            } else {
                return false;
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        return false;
    }


    private static int getSensorState(Context context) {
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

    public static boolean isEnableNotification() {
        NotificationManagerCompat notification = NotificationManagerCompat.from(mContext);
        boolean isEnabled = notification.areNotificationsEnabled();
        return isEnabled;
    }

    public static void hiddenSplash() {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                splashView.setVisibility(View.INVISIBLE);
            }
        });

    }
}
