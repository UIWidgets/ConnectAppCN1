package com.unity3d.unityconnect.plugins;

import android.content.Context;
import android.content.Intent;

import com.unityconnect.plugin.tinywasm.TinyWasmActivity;

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
        String wasmUrl = url + name;
        Intent intent = new Intent(context, TinyWasmActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TOP);
        intent.putExtra(TinyWasmActivity.WASM_URL, wasmUrl);
        intent.putExtra(TinyWasmActivity.SHOW_FPS, showFPS);
        context.startActivity(intent);
    }
}