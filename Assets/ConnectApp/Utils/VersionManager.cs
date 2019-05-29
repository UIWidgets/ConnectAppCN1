using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.Components;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.utils {
    public enum CheckVersionType {
        first,
        setting
    }

    public static class VersionManager {
        const string _ignoreUpdaterKey = "ignoreUpdaterKey";

        public static void checkForUpdates(CheckVersionType type) {
            if (type == CheckVersionType.setting) {
                CustomDialogUtils.showCustomDialog(
                    child: new CustomLoadingDialog(message: "正在检查更新")
                );
            }

            SettingApi.FetchVersion(Config.platform, Config.store, $"{Config.versionCode}")
                .Then(versionResponse => {
                    if (type == CheckVersionType.setting) {
                        CustomDialogUtils.hiddenCustomDialog();
                    }

                    var status = versionResponse["status"];
                    if (status.ToLower() == "need_update" && versionResponse.ContainsKey("url")) {
                        var changeLog = "发现新版本，立即更新体验吧！";
                        if (versionResponse.ContainsKey("changeLog")) {
                            if (versionResponse["changeLog"].isNotEmpty()) {
                                changeLog = versionResponse["changeLog"];
                            }
                        }

                        CustomDialogUtils.showCustomDialog(
                            barrierColor: Color.fromRGBO(0, 0, 0, 0.5f),
                            child: new CustomAlertDialog(
                                "版本更新",
                                changeLog,
                                new List<Widget> {
                                    new CustomButton(
                                        child: new Text(
                                            "稍后再说",
                                            style: new TextStyle(
                                                height: 1.33f,
                                                fontSize: 16,
                                                fontFamily: "Roboto-Regular",
                                                color: new Color(0xFF959595)
                                            ),
                                            textAlign: TextAlign.center
                                        ),
                                        onPressed: _ignoreUpdater
                                    ),
                                    new CustomButton(
                                        child: new Text(
                                            "立即更新",
                                            style: CTextStyle.PLargeBlue,
                                            textAlign: TextAlign.center
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
                    }
                    else {
                        if (type == CheckVersionType.setting) {
                            var customSnackBar = new CustomSnackBar(
                                "当前是最新版本",
                                color: CColors.TextBody
                            );
                            customSnackBar.show();
                        }
                    }
                })
                .Catch(error => {
                    if (type == CheckVersionType.setting) {
                        CustomDialogUtils.hiddenCustomDialog();
                    }
                });
        }

        public static bool needCheckUpdater() {
            return !PlayerPrefs.HasKey(_ignoreUpdaterKey);
        }

        static void _ignoreUpdater() {
            CustomDialogUtils.hiddenCustomDialog();
            PlayerPrefs.SetString(_ignoreUpdaterKey, "true");
            PlayerPrefs.Save();
        }
    }
}