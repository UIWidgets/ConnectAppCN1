package com.unity3d.unityconnect;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.util.Base64;

import com.google.gson.Gson;
import com.jph.takephoto.app.TakePhoto;
import com.jph.takephoto.app.TakePhotoActivity;
import com.jph.takephoto.compress.CompressConfig;
import com.jph.takephoto.model.CropOptions;
import com.jph.takephoto.model.TResult;
import com.jph.takephoto.model.TakePhotoOptions;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;

public class PickImageActivity extends TakePhotoActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        String type = this.getIntent().getStringExtra("type");
        TakePhoto takePhoto = getTakePhoto();
        
        CropOptions.Builder builder = new CropOptions.Builder();
        builder.setAspectX(800).setAspectY(800);
        builder.setWithOwnCrop(true);
        File file = new File(Environment.getExternalStorageDirectory(),
                "/temp/" + System.currentTimeMillis() + ".jpg");
        if (!file.getParentFile().exists()) {
            boolean mkdirs = file.getParentFile().mkdirs();
            if (!mkdirs) {
            }
        }
        Uri imageUri = Uri.fromFile(file);
        
        CompressConfig config = new CompressConfig.Builder()
                .setMaxSize(102400)
                .setMaxPixel(400)
                .enableReserveRaw(true)
                .create();
        takePhoto.onEnableCompress(config, true);

        TakePhotoOptions takePhotoOptions = new TakePhotoOptions.Builder().create();
        takePhotoOptions.setCorrectImage(true);
        takePhotoOptions.setWithOwnGallery(false);
        takePhoto.setTakePhotoOptions(takePhotoOptions);

        //在这里判断是打开本地相册还是直接照相
        if(type.equals("takePhoto"))
        {
            takePhoto.onPickFromCaptureWithCrop(imageUri,builder.create());
        }else{
            takePhoto.onPickFromGalleryWithCrop(imageUri,builder.create());
        }
    }


    @Override
    public void takeSuccess(TResult result) {
        super.takeSuccess(result);
        String path = result.getImage().getCompressPath();
        Bitmap bm = BitmapFactory.decodeFile(path);
        String imageStr = bitmapToBase64(bm);
        Map<String, String> map = new HashMap<String, String>();
        map.put("image", imageStr);
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("pickImage","success",Arrays.asList(new Gson().toJson(map)));
        finish();
    }

    @Override
    public void takeCancel() {
        super.takeCancel();
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("pickImage","cancel",null);

        finish();
    }

    @Override
    public void takeFail(TResult result, String msg) {
        super.takeFail(result, msg);
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("pickImage","cancel",null);
        finish();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

    }

    public String bitmapToBase64(Bitmap bitmap) {

        String result = null;
        ByteArrayOutputStream baos = null;
        try {
            if (bitmap != null) {
                baos = new ByteArrayOutputStream();
                bitmap.compress(Bitmap.CompressFormat.JPEG, 100, baos);

                baos.flush();
                baos.close();

                byte[] bitmapBytes = baos.toByteArray();
                result = Base64.encodeToString(bitmapBytes, Base64.DEFAULT);
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                if (baos != null) {
                    baos.flush();
                    baos.close();
                }
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        return result;
    }

}
