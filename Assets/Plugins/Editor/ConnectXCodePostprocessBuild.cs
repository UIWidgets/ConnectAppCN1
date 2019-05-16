using System.IO;
using ConnectApp.constants;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

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

            // 读取 Preprocessor.h 文件
            var preprocessor = new XClass(path + "/Classes/Preprocessor.h");
            preprocessor.Replace("#define UNITY_USES_REMOTE_NOTIFICATIONS 0",
                "#define UNITY_USES_REMOTE_NOTIFICATIONS 1");

            // 执行修改操作
            File.WriteAllText(path: projPath, proj.WriteToString());
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
            var wechat_appid = Config.wechatAppId;
            PlistElementDict wxUrl = urlTypes.AddDict();
            wxUrl.SetString("CFBundleTypeRole", "Editor");
            wxUrl.SetString("CFBundleURLName", "weixin");
            wxUrl.SetString("CFBundleURLSchemes", val: wechat_appid);
            PlistElementArray wxUrlScheme = wxUrl.CreateArray("CFBundleURLSchemes");
            wxUrlScheme.AddString(val: wechat_appid);
            
            // 白名单 for wechat
            PlistElementArray queriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
            queriesSchemes.AddString("wechat");
            queriesSchemes.AddString("weixin");
            queriesSchemes.AddString(val: wechat_appid);

            // HTTP 设置
            const string atsKey = "NSAppTransportSecurity";
            PlistElementDict dictTmp = rootDict.CreateDict(key: atsKey);
            dictTmp.SetBoolean("NSAllowsArbitraryLoads", true);

            PlistElementArray backModes = rootDict.CreateArray("UIBackgroundModes");
            backModes.AddString("remote-notification");
            
            // 写入
            File.WriteAllText(path: plistPath, plist.WriteToString());
        }
    }
}