using System;
using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.Utils {
    public enum CheckVersionType {
        initialize,
        setting
    }

    public static class VersionManager {
        const string _noticeNewVersionTimeKey = "noticeNewVersionTimeKey";

        public static void checkForUpdates(CheckVersionType type) {
            if (type == CheckVersionType.setting) {
                CustomDialogUtils.showCustomDialog(
                    child: new CustomLoadingDialog(message: "正在检查更新")
                );
            }

            SettingApi.CheckNewVersion(platform: Config.platform, store: Config.store, $"{Config.versionCode}")
                .Then(versionResponse => {
                    if (type == CheckVersionType.setting) {
                        CustomDialogUtils.hiddenCustomDialog();
                    }

                    var status = versionResponse.status;
                    if (status == "NEED_UPDATE" && versionResponse.url.isNotEmpty()) {
                        if (type == CheckVersionType.initialize && !needNoticeNewVersion()) {
                            return;
                        }

                        markUpdateNoticeTime();
                        CustomDialogUtils.showCustomDialog(
                            barrierColor: Color.fromRGBO(0, 0, 0, 0.5f),
                            child: new CustomAlertDialog(
                                "版本更新",
                                message: versionResponse.changeLog,
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
                                        onPressed: CustomDialogUtils.hiddenCustomDialog
                                    ),
                                    new CustomButton(
                                        child: new Text(
                                            "立即更新",
                                            style: CTextStyle.PLargeBlue,
                                            textAlign: TextAlign.center
                                        ),
                                        onPressed: () => {
                                            CustomDialogUtils.hiddenCustomDialog();
                                            Application.OpenURL(url: versionResponse.url);
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

        static bool needNoticeNewVersion() {
            if (!PlayerPrefs.HasKey(key: _noticeNewVersionTimeKey)) {
                // when need update first check
                return true;
            }

            var timeString = PlayerPrefs.GetString(key: _noticeNewVersionTimeKey);
            var endTime = DateTime.Parse(s: timeString);
            return DateTime.Compare(t1: endTime, t2: DateTime.Now) <= 0;
        }

        static void markUpdateNoticeTime() {
            var noticeTimeString = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd HH:mm:ss");
            PlayerPrefs.SetString(key: _noticeNewVersionTimeKey, value: noticeTimeString);
            PlayerPrefs.Save();
        }
    }
}