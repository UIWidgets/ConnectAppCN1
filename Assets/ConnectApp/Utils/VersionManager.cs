using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.utils {
    public enum CheckVersionType {
        first,
        setting
    }
    
    public static class VersionManager {
        
        private static string ignoreUpdaterKey = "ignoreUpdaterKey";
        public static void checkForUpdates(CheckVersionType type) {
            SettingApi.FetchVersion(Config.platform, Config.store, $"{Config.versionCode}")
                .Then(versionResponse => {
                    var status = versionResponse["status"];
                    if (status.ToLower() == "need_update" && versionResponse.ContainsKey("url")) {
                        CustomDialogUtils.showCustomDialog(
                            barrierColor: Color.fromRGBO(0, 0, 0, 0.5f),
                            child: new CustomAlertDialog(
                                "是否更新connect live",
                                actions: new List<Widget> {
                                    new CustomButton(
                                        child: new Text(
                                            "忽略",
                                            style: CTextStyle.PLargeError
                                        ),
                                        onPressed: _ignoreUpdater
                                    ),
                                    new CustomButton(
                                        child: new Text(
                                            "更新",
                                            style: CTextStyle.PLargeBody
                                        ),
                                        onPressed: () => {
                                            _ignoreUpdater();
                                            var url = versionResponse["url"];
                                            Application.OpenURL(url);
                                        }
                                    )
                                }
                            )
                        );
                    } else {
                        if (type == CheckVersionType.setting) {
                            var customSnackBar = new CustomSnackBar(
                                "当前是最新版本",
                                color: CColors.TextBody
                            );
                            customSnackBar.show();
                        }
                    }
                }
            );
        }

        public static bool needCheckUpdater() {
            return !PlayerPrefs.HasKey(ignoreUpdaterKey);
        }

        private static void _ignoreUpdater() {
            CustomDialogUtils.hiddenCustomDialog();
            PlayerPrefs.SetString(ignoreUpdaterKey, "true");
            PlayerPrefs.Save();
        }
    }
}