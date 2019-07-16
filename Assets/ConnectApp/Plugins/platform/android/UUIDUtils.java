package com.unity3d.unityconnect.plugins;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Environment;
import android.util.Log;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.RandomAccessFile;


public class UUIDUtils {
    private static final String TAG = UUIDUtils.class.getName();
    private static UUIDUtils device;
    private Context context;
    private final static String DEFAULT_NAME = "system_device_id";
    private final static String DEFAULT_FILE_NAME = "system_device_id.txt";
    private final static String DEFAULT_DEVICE_ID = "dervice_id";
    private String FILE_ANDROID = Environment.getExternalStorageDirectory()+ File.separator;
    private static SharedPreferences preferences = null;

    public UUIDUtils(Context context) {
            this.context = context;
    }

    private String uuid;

    public static UUIDUtils buidleID(Context context) {
        if (device == null) {
            synchronized (UUIDUtils.class) {
                if (device == null) {
                    device = new UUIDUtils(context);
                }
            }
        }
        return device;
    }

    public static String getUUID() {
        if (preferences == null) {
            Log.d(TAG, "Please check the UUIDS.buidleID in Application (this).Check ()");
            return "dervice_id";
        }
        return preferences.getString("dervice_id", "dervice_id");
    }

    //生成一个128位的唯一标识符
    private String createUUID() {
        return java.util.UUID.randomUUID().toString();
    }


    public void check() {
        preferences = context.getSharedPreferences(DEFAULT_NAME, 0);
        uuid = preferences.getString(DEFAULT_DEVICE_ID, null);
        if (uuid == null||uuid.length()==0) {
            if (checkAndroidFile() == null ) {
                uuid = createUUID();
                saveAndroidFile(uuid);
                Log.d(TAG, "new devices,create only id");
            }
            SharedPreferences.Editor editor = preferences.edit();
            editor.putString(DEFAULT_DEVICE_ID, uuid);
            editor.commit();
            Log.d(TAG,"save uuid SharePref:" + uuid);
        } else {
            if (checkAndroidFile() == null) {
                saveAndroidFile(uuid);
            }
        }
        Log.d(TAG,"result uuid:" + uuid);
    }

    private String checkAndroidFile() {

        return  getFileContent(new File(FILE_ANDROID+DEFAULT_FILE_NAME));
    }

    private void saveAndroidFile(String inputText) {

        writeTxtToFile(inputText,FILE_ANDROID,DEFAULT_FILE_NAME);
    }

    // 将字符串写入到文本文件中
    public static void writeTxtToFile(String strcontent, String filePath, String fileName) {
        if (strcontent==null||strcontent.length()==0) return;

        //生成文件夹之后，再生成文件，不然会出错
        makeFilePath(filePath, fileName);

        String strFilePath = filePath + fileName;
        // 每次写入时，都换行写
        String strContent = strcontent + "\r\n";
        try {
            File file = new File(strFilePath);
            if (!file.exists()) {
                Log.d("TestFile", "Create the file:" + strFilePath);
                file.getParentFile().mkdirs();
                file.createNewFile();
            }
            RandomAccessFile raf = new RandomAccessFile(file, "rwd");
            raf.seek(file.length());
            raf.write(strContent.getBytes());
            raf.close();
        } catch (Exception e) {
            Log.e("TestFile", "Error on write File:" + e);
        }
    }

    //生成文件
    public static File makeFilePath(String filePath, String fileName) {
        File file = null;
        makeRootDirectory(filePath);
        try {
            file = new File(filePath + fileName);
            if (!file.exists()) {
                file.createNewFile();
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return file;
    }

    //生成文件夹
    public static void makeRootDirectory(String filePath) {
        File file = null;
        try {
            file = new File(filePath);
            if (!file.exists()) {
                file.mkdir();
            }
        } catch (Exception e) {
            Log.i("error:", e + "");
        }
    }

    //读取指定目录下的所有TXT文件的文件内容
    public static String getFileContent(File file) {
        String content = "";
        if (!file.isDirectory()) {  //检查此路径名的文件是否是一个目录(文件夹)
            if (file.getName().endsWith("txt")) {//文件格式为""文件
                try {
                    InputStream instream = new FileInputStream(file);
                    if (instream != null) {
                        InputStreamReader inputreader
                                = new InputStreamReader(instream, "UTF-8");
                        BufferedReader buffreader = new BufferedReader(inputreader);
                        String line = "";
                        //分行读取
                        while ((line = buffreader.readLine()) != null) {
                            content += line + "\n";
                        }
                        instream.close();//关闭输入流
                    }
                } catch (java.io.FileNotFoundException e) {
                    Log.d("TestFile", "The File doesn't not exist.");
                } catch (IOException e) {
                    Log.d("TestFile", e.getMessage());
                }
            }
        }
        return content.length()==0?null:content;
    }


}
