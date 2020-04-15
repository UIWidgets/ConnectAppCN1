#if UNITY_IOS
using System.IO;
using System.Text;
using ConnectApp.Constants;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace Plugins.Editor {
    public static class ConnectXCodePostprocessBuild {
        [PostProcessBuild(999)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
            if (BuildTarget.iOS != buildTarget) {
                return;
            }

            ModifyPbxProject(path: path);
            ModifyPlist(path: path);
            ModifyBuildSettings(path);
        }

        static void ModifyBuildSettings(string pathToBuiltProject) {
            var xcodeProjectPath = Path.Combine(pathToBuiltProject, "Unity-iPhone.xcodeproj");
            var pbxPath = Path.Combine(xcodeProjectPath, "project.pbxproj");
            var xcodeProjectLines = File.ReadAllLines(pbxPath);
            var sb = new StringBuilder();
            foreach (var line in xcodeProjectLines) {
                if (line.Contains("GCC_ENABLE_OBJC_EXCEPTIONS") ||
                    line.Contains("GCC_ENABLE_CPP_EXCEPTIONS") ||
                    line.Contains("CLANG_ENABLE_MODULES")) {
                    var newLine = line.Replace("NO", "YES");
                    sb.AppendLine(newLine);
                }
                else {
                    sb.AppendLine(line);
                }
            }

            File.WriteAllText(pbxPath, sb.ToString());
        }

        static void ModifyPbxProject(string path) {
            var projPath = PBXProject.GetPBXProjectPath(buildPath: path);
            var proj = new PBXProject();

            proj.ReadFromString(File.ReadAllText(path: projPath));
            var targetName = PBXProject.GetUnityTargetName();
            var targetGuid = proj.TargetGuidByName(name: targetName);

            proj.SetBuildProperty(targetGuid: targetGuid, name: "LIBRARY_SEARCH_PATHS", value: "$(inherited)");
            proj.AddBuildProperty(targetGuid: targetGuid, name: "LIBRARY_SEARCH_PATHS", value: "$(SRCROOT)");
            proj.AddBuildProperty(targetGuid: targetGuid, name: "LIBRARY_SEARCH_PATHS",
                value: "$(PROJECT_DIR)/Libraries");
            proj.AddBuildProperty(targetGuid: targetGuid, name: "LIBRARY_SEARCH_PATHS",
                value: "$(PROJECT_DIR)/Libraries/Plugins/iOS");
            proj.AddBuildProperty(targetGuid: targetGuid, name: "LIBRARY_SEARCH_PATHS",
                value: "$(PROJECT_DIR)/Libraries/Plugins/iOS/WeChatSDK1.8.4");
            proj.AddBuildProperty(targetGuid: targetGuid, name: "LIBRARY_SEARCH_PATHS",
                value: "$(PROJECT_DIR)/Libraries/Plugins/iOS/Bugly");
            proj.AddBuildProperty(targetGuid: targetGuid, name: "LIBRARY_SEARCH_PATHS",
                value: "$(PROJECT_DIR)/Libraries/Plugins/iOS/FPSLabel");

            // Add Framework
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "libz.tbd", weak: true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "libc++.tbd", weak: true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "libsqlite3.0.tbd", weak: true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "CoreFoundation.framework", weak: false);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "libresolv.tbd", weak: false);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "UserNotifications.framework", weak: false);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "CoreTelephony.framework", weak: true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "CoreServices.framework", weak: true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "MediaPlayer.framework", weak: true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "Photos.framework", weak: false);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "SafariServices.framework", weak: false);
            proj.AddFrameworkToProject(targetGuid: targetGuid, framework: "WebKit.framework", weak: false);

            // Update Build Setting
            proj.SetBuildProperty(targetGuid: targetGuid, name: "ENABLE_BITCODE", value: "NO");
            proj.AddBuildProperty(targetGuid: targetGuid, name: "OTHER_LDFLAGS", value: "-all_load");
            proj.AddBuildProperty(targetGuid: targetGuid, name: "OTHER_LDFLAGS", value: "-ObjC");
            
            // save changed
            File.WriteAllText(path: projPath, contents: proj.WriteToString());

            // 读取 Preprocessor.h 文件
            var preprocessor = new XClass(path + "/Classes/Preprocessor.h");
            preprocessor.Replace("#define UNITY_USES_REMOTE_NOTIFICATIONS 0",
                "#define UNITY_USES_REMOTE_NOTIFICATIONS 1");

            var blackDestDict = path + "/Unity-iPhone/Images.xcassets/unityConnectBlack.imageset";
            var blackSourceFile = "image/iOS/unityConnectBlack.imageset";
            writeFile(blackSourceFile, blackDestDict);

            var madeDestDict = path + "/Unity-iPhone/Images.xcassets/madeWithUnity.imageset";
            var madeSourceFile = "image/iOS/madeWithUnity.imageset";
            writeFile(madeSourceFile, madeDestDict);

            var arrowBackDestDict = path + "/Unity-iPhone/Images.xcassets/arrowBack.imageset";
            var arrowBackSourceFile = "image/iOS/arrowBack.imageset";
            writeFile(arrowBackSourceFile, arrowBackDestDict);

            var qrScanLineDestDict = path + "/Unity-iPhone/Images.xcassets/qrScanLine.imageset";
            var qrScanLineSourceFile = "image/iOS/qrScanLine.imageset";
            writeFile(qrScanLineSourceFile, qrScanLineDestDict);

            var launchImgDestDict = path + "/Unity-iPhone/Images.xcassets/launch_screen_img.imageset";
            var launchImgSourceFile = "image/iOS/launch_screen_img.imageset";
            writeFile(launchImgSourceFile, launchImgDestDict);

            var noticeWavDestDict = path + "/Data/noticeMusic.wav";
            var noticeWavSourceFile = "files/noticeMusic.wav";
            writeFile(noticeWavSourceFile, noticeWavDestDict);

            var destFile = path + "/Classes/UI/UnityVIewControllerBase+iOS.mm";

            FileUtil.DeleteFileOrDirectory(destFile);
            // 自定义覆盖controller文件，动态修改状态栏
            FileUtil.CopyFileOrDirectory(Application.dataPath + "/Plugins/Editor/UnityVIewControllerBase+iOS.mm",
                destFile);

            var destBaseFile = path + "/Classes/UI/UnityVIewControllerBase.mm";

            FileUtil.DeleteFileOrDirectory(destBaseFile);
            // 自定义覆盖controller文件，动态修改状态栏
            FileUtil.CopyFileOrDirectory(Application.dataPath + "/Plugins/Editor/UnityVIewControllerBase.mm",
                destBaseFile);

            // 执行修改操作
            File.WriteAllText(projPath, proj.WriteToString());
        }


        static void writeFile(string sourceFile, string destDict) {
            FileUtil.DeleteFileOrDirectory(destDict);
            FileUtil.CopyFileOrDirectory(Application.dataPath + "/ConnectApp/Resources/" + sourceFile, destDict);
        }

        static void ModifyPlist(string path) {
            // Info.plist
            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(path: plistPath));

            // ROOT
            PlistElementDict rootDict = plist.root;
            PlistElementArray urlTypes = rootDict.CreateArray("CFBundleURLTypes");

            // Add URLScheme For Wechat
            PlistElementDict wxUrl = urlTypes.AddDict();
            wxUrl.SetString("CFBundleTypeRole", "Editor");
            wxUrl.SetString("CFBundleURLName", "weixin");
            wxUrl.SetString("CFBundleURLSchemes", val: Config.wechatAppId);
            PlistElementArray wxUrlScheme = wxUrl.CreateArray("CFBundleURLSchemes");
            wxUrlScheme.AddString(val: Config.wechatAppId);

            // Add URLScheme For jiguang
            // 暂时注释 https://community.jiguang.cn/t/topic/33910
//            PlistElementDict jgUrl = urlTypes.AddDict();
//            jgUrl.SetString("CFBundleTypeRole", "Editor");
//            jgUrl.SetString("CFBundleURLName", "jiguang");
//            jgUrl.SetString("CFBundleURLSchemes", val: "jiguang-" + Config.jgAppKey);
//            PlistElementArray jgUrlScheme = jgUrl.CreateArray("CFBundleURLSchemes");
//            jgUrlScheme.AddString(val: "jiguang-" + Config.jgAppKey);

            // Add URLScheme For unityconnect
            PlistElementDict appUrl = urlTypes.AddDict();
            appUrl.SetString("CFBundleTypeRole", "Editor");
            appUrl.SetString("CFBundleURLName", "");
            appUrl.SetString("CFBundleURLSchemes", val: "unityconnect");
            PlistElementArray appUrlScheme = appUrl.CreateArray("CFBundleURLSchemes");
            appUrlScheme.AddString(val: "unityconnect");

            // 白名单 for wechat
            PlistElementArray queriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
            queriesSchemes.AddString("wechat");
            queriesSchemes.AddString("weixin");
            queriesSchemes.AddString(val: Config.wechatAppId);

            // HTTP 设置
            const string atsKey = "NSAppTransportSecurity";
            PlistElementDict dictTmp = rootDict.CreateDict(key: atsKey);
            dictTmp.SetBoolean("NSAllowsArbitraryLoads", true);

            // 出口合规信息
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

            rootDict.SetString("NSCameraUsageDescription", "我们需要访问您的相机，以便您正常使用修改头像、发送图片、扫一扫等功能");
            rootDict.SetString("NSPhotoLibraryUsageDescription", "我们需要访问您的相册，以便您正常使用修改头像、发送图片或者视频等功能");
            rootDict.SetString("NSPhotoLibraryAddUsageDescription", "我们需要访问您的相册，以便您正常使用保存图片功能");
//            rootDict.SetString("NSMicrophoneUsageDescription", "需要录制音频，是否允许此App打开麦克风？");

            // remove exit on suspend if it exists.
            string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
            if (rootDict.values.ContainsKey(exitsOnSuspendKey)) {
                rootDict.values.Remove(exitsOnSuspendKey);
            }

            // 写入
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif