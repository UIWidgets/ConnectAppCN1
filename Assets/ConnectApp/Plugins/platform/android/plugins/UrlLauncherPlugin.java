package com.unity3d.unityconnect.plugins;

import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.provider.Browser;

import com.custom.webview.WebViewActivity;

import java.util.HashMap;
import java.util.Map;

public class UrlLauncherPlugin {
    private static UrlLauncherPlugin instance;

    public static Context context;

    public static UrlLauncherPlugin getInstance() {
        synchronized (UrlLauncherPlugin.class) {
            if (instance == null) {
                instance = new UrlLauncherPlugin();
            }
            return instance;
        }
    }

    private static void launch(String url, boolean forceSafariVC, boolean forceWebView, String cookie) {
        if (context == null) {
            return;
        }

        Map<String, String> headersMap = new HashMap<String, String>();
        if (!cookie.isEmpty()) {
            headersMap.put("Cookie", cookie);
        }
        Bundle bundle = extractBundle(headersMap);
        Intent launchIntent;
        if (forceWebView) {
            launchIntent = WebViewActivity.createIntent(context, url, false, false, bundle);
        } else {
            Intent intent = new Intent(Intent.ACTION_VIEW);
            intent.setData(Uri.parse(url));
            intent.putExtra(Browser.EXTRA_HEADERS, bundle);
            launchIntent = intent;
        }
        launchIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        context.startActivity(launchIntent);
    }

    private static boolean canLaunch(String url) {
        Intent launchIntent = new Intent(Intent.ACTION_VIEW);
        launchIntent.setData(Uri.parse(url));
        ComponentName componentName = launchIntent.resolveActivity(context.getPackageManager());

        return componentName != null
                && !"{com.android.fallback/com.android.fallback.Fallback}"
                .equals(componentName.toShortString());
    }

    private static void closeWebView() {
        context.sendBroadcast(new Intent(WebViewActivity.ACTION_CLOSE));
    }

    private static void clearCache() {
        WebViewActivity.clearCache(true);
    }

    private static void clearHistory() {
        WebViewActivity.clearHistory();
    }

    private static Bundle extractBundle(Map<String, String> headersMap) {
        final Bundle headersBundle = new Bundle();
        for (String key : headersMap.keySet()) {
            final String value = headersMap.get(key);
            headersBundle.putString(key, value);
        }
        return headersBundle;
    }
}
