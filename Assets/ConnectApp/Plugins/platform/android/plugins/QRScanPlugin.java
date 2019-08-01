package com.unity3d.unityconnect.plugins;

import android.content.Context;
import android.content.Intent;

import com.unity3d.unityconnect.QRScanActivity;

public class QRScanPlugin {

    private static QRScanPlugin instance;

    public static Context context;

    public static QRScanPlugin getInstance() {
        synchronized (QRScanPlugin.class) {
            if (instance == null) {
                instance = new QRScanPlugin();
            }
            return instance;
        }
    }

    public void pushToQRScan() {
        Intent intent = new Intent(context, QRScanActivity.class);
        context.startActivity(intent);
    }
}
