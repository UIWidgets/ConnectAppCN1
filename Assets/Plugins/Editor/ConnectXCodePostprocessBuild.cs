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

        const string Entitlements = @"
<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
    <key>aps-environment</key>
    <string>development</string>
    <key>com.apple.developer.associated-domains</key>
    <array>
        <string>applinks:connect-download.unity.com</string>
        <string>applinks:connect.unity.com</string>
    </array>
</dict>
</plist>";

        static void ModifyPbxProject(string path) {
            var projPath = PBXProject.GetPBXProjectPath(buildPath: path);
            var proj = new PBXProject();

            proj.ReadFromString(File.ReadAllText(path: projPath));
            var targetName = PBXProject.GetUnityTargetName();
            var targetGuid = proj.TargetGuidByName(name: targetName);

            proj.SetBuildProperty(targetGuid: targetGuid, "LIBRARY_SEARCH_PATHS", "$(inherited)");
            proj.AddBuildProperty(targetGuid: targetGuid, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)");
            proj.AddBuildProperty(targetGuid: targetGuid, "LIBRARY_SEARCH_PATHS", "$(PROJECT_DIR)/Libraries");
            proj.AddBuildProperty(targetGuid: targetGuid, "LIBRARY_SEARCH_PATHS",
                "$(PROJECT_DIR)/Libraries/Plugins/iOS");
            proj.AddBuildProperty(targetGuid: targetGuid, "LIBRARY_SEARCH_PATHS",
                "$(PROJECT_DIR)/Libraries/Plugins/iOS/WeChatSDK1.8.4");
            proj.AddBuildProperty(targetGuid: targetGuid, "LIBRARY_SEARCH_PATHS",
                "$(PROJECT_DIR)/Libraries/Plugins/iOS/Bugly");

            // Add Framework
            proj.AddFrameworkToProject(targetGuid: targetGuid, "libz.tbd", true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "libc++.tbd", true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "libsqlite3.0.tbd", true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "CoreFoundation.framework", false);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "libresolv.tbd", false);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "UserNotifications.framework", false);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "CoreTelephony.framework", true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "CoreServices.framework", true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "MediaPlayer.framework", true);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "Photos.framework", false);
            proj.AddFrameworkToProject(targetGuid: targetGuid, "SafariServices.framework", false);

            // Update Build Setting
            proj.SetBuildProperty(targetGuid: targetGuid, "ENABLE_BITCODE", "NO");
            proj.AddBuildProperty(targetGuid: targetGuid, "OTHER_LDFLAGS", "-all_load");
            proj.AddBuildProperty(targetGuid: targetGuid, "OTHER_LDFLAGS", "-ObjC");

            // Add Entitlements
            const string fileName = "unityconnect.entitlements";
            var filePath = Path.Combine(path1: path, path2: fileName);
            File.WriteAllText(path: filePath, contents: Entitlements);
            proj.AddFile(path: filePath, projectPath: fileName);
            proj.SetBuildProperty(targetGuid: targetGuid, "CODE_SIGN_ENTITLEMENTS", value: fileName);
            // save changed
            File.WriteAllText(path: projPath, proj.WriteToString());

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

            var messageCafDestDict = path + "/Data/notification.caf";
            var messageCafSourceFile = "files/notification.caf";
            writeFile(messageCafSourceFile, messageCafDestDict);

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

            PlistElementArray backModes = rootDict.CreateArray("UIBackgroundModes");
            backModes.AddString("remote-notification");

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