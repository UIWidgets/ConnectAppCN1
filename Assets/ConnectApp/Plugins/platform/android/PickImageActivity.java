package com.unity3d.unityconnect;
import android.Manifest;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.v4.app.ActivityCompat;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.zxing.decoding.FinishListener;
import com.jph.takephoto.app.TakePhoto;
import com.jph.takephoto.app.TakePhotoActivity;
import com.jph.takephoto.compress.CompressConfig;
import com.jph.takephoto.model.CropOptions;
import com.jph.takephoto.model.TResult;
import com.jph.takephoto.model.TakePhotoOptions;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;

import java.io.File;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;

public class PickImageActivity extends TakePhotoActivity {

    boolean mFinished;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        String source = this.getIntent().getStringExtra("source");
        mFinished = false;
        if (source.equals("0")) {
            int cameraPermission = ActivityCompat.checkSelfPermission(this, android.Manifest.permission.CAMERA);
            if (cameraPermission != PackageManager.PERMISSION_GRANTED) {
                ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.CAMERA}, 1);
            } else {
                startPick();
            }
        } else if (source.equals("1")) {
            String[] PERMISSIONS_STORAGE = {
                    android.Manifest.permission.READ_EXTERNAL_STORAGE,
                    android.Manifest.permission.WRITE_EXTERNAL_STORAGE };
            int permission = ActivityCompat.checkSelfPermission(getApplicationContext(), android.Manifest.permission.WRITE_EXTERNAL_STORAGE);
            if (permission != PackageManager.PERMISSION_GRANTED) {
                ActivityCompat.requestPermissions(this, PERMISSIONS_STORAGE, 2);
            } else {
                startPick();
            }
        }
    }

    @Override
    public void takeSuccess(TResult result) {
        super.takeSuccess(result);
        if (mFinished)return;
        mFinished = true;
        String compressPath = result.getImage().getCompressPath();
        String path = compressPath.isEmpty() ? result.getImage().getOriginalPath() : result.getImage().getCompressPath();
        Map<String, String> map = new HashMap<String, String>();
        map.put("imagePath", path);
        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("pickImage","pickImageSuccess",Arrays.asList(new Gson().toJson(map)));
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
        if (requestCode == 66) {
            if (resultCode == RESULT_OK) {
                if (data == null) {
                    finish();
                } else {
                    if (mFinished)return;
                    mFinished = true;
                    Uri selectedVideo = data.getData();
                    String[] filePathColumn = {MediaStore.Video.Media.DATA};

                    Cursor cursor = getContentResolver().query(selectedVideo,
                            filePathColumn, null, null, null);
                    cursor.moveToFirst();

                    int columnIndex = cursor.getColumnIndex(filePathColumn[0]);
                    String videoPath = cursor.getString(columnIndex);

                    File file = new File(videoPath);
                    if (file.length()>30*1024*1024){
                        Toast.makeText(this,"该视频过大，无法上传",Toast.LENGTH_LONG).show();
                    }else{
                        cursor.close();
                        Map<String, String> map = new HashMap<String, String>();
                        map.put("videoPath", videoPath);
                        UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("pickImage","pickVideoSuccess", Arrays.asList(new Gson().toJson(map)));
                    }
                    finish();

                }
            }
            if (resultCode == RESULT_CANCELED) {
                finish();
            }
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        switch (requestCode) {
            case 1:
                if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    startPick();
                } else {
                    buildAlertDialog("照相机");
                }
                break;
            case 2:
                if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    startPick();
                } else {
                    buildAlertDialog("存储");
                }
                break;
        }
    }

    private void startPick(){
        String type = this.getIntent().getStringExtra("type");
        if (type.equals("image")) {
            String source = this.getIntent().getStringExtra("source");
            boolean cropped = this.getIntent().getBooleanExtra("cropped", false);

            TakePhoto takePhoto = getTakePhoto();
            File file = new File(Environment.getExternalStorageDirectory(),
                    "/temp/" + System.currentTimeMillis() + ".jpg");
            if (!file.getParentFile().exists()) {
                boolean mkdirs = file.getParentFile().mkdirs();
                if (!mkdirs) {
                }
            }
            Uri imageUri = Uri.fromFile(file);
            // 压缩图片
            takePhoto.onEnableCompress(compressConfig(), true);

            TakePhotoOptions takePhotoOptions = new TakePhotoOptions.Builder().create();
            takePhotoOptions.setCorrectImage(true);
            takePhotoOptions.setWithOwnGallery(false);
            takePhoto.setTakePhotoOptions(takePhotoOptions);

            if (source.equals("0")) {
                if (cropped) {
                    takePhoto.onPickFromCaptureWithCrop(imageUri, cropOptions());
                } else {
                    takePhoto.onPickFromCapture(imageUri);
                }

            } else {
                if (cropped) {
                    takePhoto.onPickFromGalleryWithCrop(imageUri, cropOptions());
                } else {
                    takePhoto.onPickFromGallery();
                }
            }
        }
        if (type.equals("video")) {
            Intent intent = new Intent(Intent.ACTION_PICK);
            intent.setType("video/*");
            startActivityForResult(intent, 66);
        }
    }

    private CropOptions cropOptions() {
        CropOptions.Builder builder = new CropOptions.Builder();
        builder.setAspectX(800);
        builder.setAspectY(800);
        builder.setWithOwnCrop(true);
        return builder.create();
    }

    private CompressConfig compressConfig() {
        int maxSize = this.getIntent().getIntExtra("maxSize", 0);
        int imageMaxSize = maxSize == 0 ? 5*1024*1024 : maxSize;
        CompressConfig config = new CompressConfig.Builder()
                .setMaxSize(imageMaxSize)
                .setMaxPixel(2048)
                .enableReserveRaw(true)
                .create();
        return config;
    }

    private void buildAlertDialog(String message) {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setMessage(this.getString(R.string.app_name) + "没有获得" + message + "的使用权限，请在设置中开启");
        builder.setPositiveButton("取消", new FinishListener(this));
        builder.setNeutralButton("开启", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                Intent intent = new Intent();
                intent.setAction(Settings.ACTION_APPLICATION_DETAILS_SETTINGS);
                Uri uri = Uri.fromParts("package", getPackageName(), null);
                intent.setData(uri);
                startActivity(intent);
                finish();
            }
        });
        builder.show();
    }
}
