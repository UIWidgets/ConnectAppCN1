using System.IO;
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
        }

        static void ModifyPbxProject(string path) {
            string projPath = PBXProject.GetPBXProjectPath(buildPath: path);
            PBXProject proj = new PBXProject();

            proj.ReadFromString(File.ReadAllText(path: projPath));
            string target = proj.TargetGuidByName("Unity-iPhone");

            proj.SetBuildProperty(targetGuid: target, "LIBRARY_SEARCH_PATHS", "$(inherited)");
            proj.AddBuildProperty(targetGuid: target, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)");
            proj.AddBuildProperty(targetGuid: target, "LIBRARY_SEARCH_PATHS", "$(PROJECT_DIR)/Libraries");
            proj.AddBuildProperty(targetGuid: target, "LIBRARY_SEARCH_PATHS", "$(PROJECT_DIR)/Libraries/Plugins/iOS");
            proj.AddBuildProperty(targetGuid: target, "LIBRARY_SEARCH_PATHS",
                "$(PROJECT_DIR)/Libraries/Plugins/iOS/WeChatSDK1.8.4");

            // Add Framework
            proj.AddFrameworkToProject(targetGuid: target, "libz.tbd", true);
            proj.AddFrameworkToProject(targetGuid: target, "libc++.tbd", true);
            proj.AddFrameworkToProject(targetGuid: target, "libsqlite3.0.tbd", true);
            proj.AddFrameworkToProject(targetGuid: target, "CoreFoundation.framework", false);
            proj.AddFrameworkToProject(targetGuid: target, "libresolv.tbd", false);
            proj.AddFrameworkToProject(targetGuid: target, "UserNotifications.framework", false);
            proj.AddFrameworkToProject(targetGuid: target, "CoreTelephony.framework", true);

            proj.AddBuildProperty(targetGuid: target, "OTHER_LDFLAGS", "-ObjC");
            proj.AddBuildProperty(targetGuid: target, "OTHER_LDFLAGS", "-all_load");

            //associated-domains

            string fileName = "unityconnect.entitlements";
            string filePath = Path.Combine(path, fileName);
            //Debug.Log ("filePath: " + filePath);
            string fileContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd 

"">
<plist version=""1.0"">
<dict>
    <key>com.apple.developer.associated-domains</key>
    <array>
        <string>applinks:connect-download.unity.com</string>
        <string>applinks:connect.unity.com</string>
    </array>
</dict>
</plist>";
            File.WriteAllText(filePath, fileContent);
            proj.AddFile(filePath, fileName);
            proj.SetBuildProperty(target, "CODE_SIGN_ENTITLEMENTS", fileName);
            // save changed
            File.WriteAllText(projPath, proj.WriteToString());


            // 读取 Preprocessor.h 文件
            var preprocessor = new XClass(path + "/Classes/Preprocessor.h");
            preprocessor.Replace("#define UNITY_USES_REMOTE_NOTIFICATIONS 0",
                "#define UNITY_USES_REMOTE_NOTIFICATIONS 1");

            var blackDestDict = path + "/Unity-iPhone/Images.xcassets/unityConnectBlack.imageset";
            var blackSourceFile = "iOS/unityConnectBlack.imageset";
            writeFile(blackSourceFile, blackDestDict);

            var madeDestDict = path + "/Unity-iPhone/Images.xcassets/madeWithUnity.imageset";
            var madeSourceFile = "iOS/madeWithUnity.imageset";
            writeFile(madeSourceFile, madeDestDict);

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
            File.WriteAllText(path: projPath, proj.WriteToString());
        }


        static void writeFile(string sourceFile, string destDict) {
            FileUtil.DeleteFileOrDirectory(destDict);
            FileUtil.CopyFileOrDirectory(Application.dataPath + "/ConnectApp/Resources/image/" + sourceFile, destDict);
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
            PlistElementDict jgUrl = urlTypes.AddDict();
            jgUrl.SetString("CFBundleTypeRole", "Editor");
            jgUrl.SetString("CFBundleURLName", "jiguang");
            jgUrl.SetString("CFBundleURLSchemes", val: "jiguang-" + Config.jgAppKey);
            PlistElementArray jgUrlScheme = jgUrl.CreateArray("CFBundleURLSchemes");
            jgUrlScheme.AddString(val: "jiguang-" + Config.jgAppKey);

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

            // remove exit on suspend if it exists.
            string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
            if (rootDict.values.ContainsKey(exitsOnSuspendKey)) {
                rootDict.values.Remove(exitsOnSuspendKey);
            }

            // 写入
            File.WriteAllText(path: plistPath, plist.WriteToString());
        }
    }
}