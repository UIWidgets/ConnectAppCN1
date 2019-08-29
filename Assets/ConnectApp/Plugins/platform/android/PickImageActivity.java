package com.unity3d.unityconnect;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;

import com.jph.takephoto.app.TakePhoto;
import com.jph.takephoto.app.TakePhotoActivity;
import com.jph.takephoto.compress.CompressConfig;
import com.jph.takephoto.model.CropOptions;
import com.jph.takephoto.model.TResult;

import java.io.File;

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
        //在这里判断是打开本地相册还是直接照相
        if(type.equals("takePhoto"))
        {
            takePhoto.onPickFromCaptureWithCrop(imageUri,builder.create());
        }else{
            takePhoto.onPickFromDocumentsWithCrop(imageUri,builder.create());
        }
    }


    @Override
    public void takeSuccess(TResult result) {
        super.takeSuccess(result);
        System.out.print("takeSuccess");
    }

    @Override
    public void takeCancel() {
        super.takeCancel();
        System.out.print("takeCancel");
        finish();
    }

    @Override
    public void takeFail(TResult result, String msg) {
        super.takeFail(result, msg);
        System.out.print("takeCancel");

    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

    }


}
