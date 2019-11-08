package com.unity3d.unityconnect;

import android.Manifest;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Bundle;
import android.provider.Settings;
import android.support.v4.app.ActivityCompat;

import com.google.zxing.activity.CaptureActivity;
import com.google.zxing.decoding.FinishListener;
import com.unity.uiwidgets.plugin.UIWidgetsMessageManager;

import java.util.Arrays;

public class QRScanActivity extends Activity {
    private static final int REQUEST_CODE_SCAN = 0x0000;
    private String type = "";

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        int cameraPermission = ActivityCompat.checkSelfPermission(this, Manifest.permission.CAMERA);
        if (cameraPermission != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.CAMERA}, 1);
            return;
        }
        startQrCode();
    }

    @Override
    protected void onRestart() {
        super.onRestart();
        if (!type.equals("scan")) {
            int cameraPermission = ActivityCompat.checkSelfPermission(this, Manifest.permission.CAMERA);
            if (cameraPermission != PackageManager.PERMISSION_GRANTED) {
                ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.CAMERA}, 1);
                return;
            }
            startQrCode();
        }
    }

    private void startQrCode() {
        Intent intent = new Intent(this, CaptureActivity.class);
        startActivityForResult(intent, REQUEST_CODE_SCAN);
    }

    private void buildAlertDialog() {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setMessage(this.getString(R.string.app_name) + "没有获得照相机的使用权限，请在设置中开启");
        builder.setPositiveButton("取消", new FinishListener(this));
        builder.setNeutralButton("开启", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                Intent intent = new Intent();
                intent.setAction(Settings.ACTION_APPLICATION_DETAILS_SETTINGS);
                Uri uri = Uri.fromParts("package", getPackageName(), null);
                intent.setData(uri);
                startActivity(intent);
            }
        });
        builder.show();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == REQUEST_CODE_SCAN && resultCode == RESULT_OK) {
            if (data != null) {
                String qrCode = data.getStringExtra("qr_scan_result");
                type = data.getStringExtra("type");
                UIWidgetsMessageManager.getInstance().UIWidgetsMethodMessage("QRScan", "OnReceiveQRCode", Arrays.asList(qrCode));
            }
            finish();
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        switch (requestCode) {
            case 1:
                if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    startQrCode();
                } else {
                    buildAlertDialog();
                }
                break;
        }
    }
}
