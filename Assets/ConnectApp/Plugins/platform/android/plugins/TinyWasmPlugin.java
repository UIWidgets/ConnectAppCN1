package com.unity3d.unityconnect.plugins;

import android.content.Context;
import android.content.Intent;

public class TinyWasmPlugin {

    private static TinyWasmPlugin instance;

    public static Context context;

    public static TinyWasmPlugin getInstance() {
        synchronized (TinyWasmPlugin.class) {
            if (instance == null) {
                instance = new TinyWasmPlugin();
            }
            return instance;
        }
    }

    public void pushToTinyWasmScreen(String url, String name, boolean showFPS) {
    }
}