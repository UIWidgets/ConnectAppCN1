using System.IO;
using ConnectApp.constants;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Plugins.Editor {
    public class ConnectXCodePostprocessBuild {
        [PostProcessBuild(999)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
            if (BuildTarget.iOS != buildTarget) {
                return;
            }

            ModifyPBXProject(path);

            ModifyPlist(path);
        }

        static void ModifyPBXProject(string path) {
            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject proj = new PBXProject();

            proj.ReadFromString(File.ReadAllText(projPath));
            string target = proj.TargetGuidByName("Unity-iPhone");

            proj.SetBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(inherited)");
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)");
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(PROJECT_DIR)/Libraries");
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(PROJECT_DIR)/Libraries/Plugins/iOS");
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS",
                "$(PROJECT_DIR)/Libraries/Plugins/iOS/WeChatSDK1.8.4");

            //add framework
            proj.AddFrameworkToProject(target, "libz.tbd", true);
            proj.AddFrameworkToProject(target, "libc++.tbd", true);
            proj.AddFrameworkToProject(target, "libsqlite3.0.tbd", true);
            proj.AddFrameworkToProject(target, "CoreFoundation.framework", false);
            proj.AddFrameworkToProject(target, "libresolv.tbd", false);
            proj.AddFrameworkToProject(target, "UserNotifications.framework", false);
            proj.AddFrameworkToProject(target, "CoreTelephony.framework", true);

            proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
            proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-all_load");

            //读取Preprocessor.h文件
            var Preprocessor = new XClass(path + "/Classes/Preprocessor.h");
            Preprocessor.Replace("#define UNITY_USES_REMOTE_NOTIFICATIONS 0",
                "#define UNITY_USES_REMOTE_NOTIFICATIONS 1");


            //执行修改操作

            File.WriteAllText(projPath, proj.WriteToString());
        }

        static void ModifyPlist(string path) {
            //Info.plist
            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            //ROOT
            PlistElementDict rootDict = plist.root;

            PlistElementArray urlTypes = rootDict.CreateArray("CFBundleURLTypes");
            // add url scheme for wechat
            string wechat_appid = Config.wechatAppId;
            PlistElementDict wxUrl = urlTypes.AddDict();
            wxUrl.SetString("CFBundleTypeRole", "Editor");
            wxUrl.SetString("CFBundleURLName", "weixin");
            wxUrl.SetString("CFBundleURLSchemes", wechat_appid);
            PlistElementArray wxUrlScheme = wxUrl.CreateArray("CFBundleURLSchemes");
            wxUrlScheme.AddString(wechat_appid);
            // 白名单 for wechat
            PlistElementArray queriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
            queriesSchemes.AddString("wechat");
            queriesSchemes.AddString("weixin");
            queriesSchemes.AddString(wechat_appid);

            //http设置
            var atsKey = "NSAppTransportSecurity";
            PlistElementDict dictTmp = rootDict.CreateDict(atsKey);
            dictTmp.SetBoolean("NSAllowsArbitraryLoads", true);

            PlistElementArray backModes = rootDict.CreateArray("UIBackgroundModes");
            backModes.AddString("remote-notification");
            //写入
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}