using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class ForceUpdateScreen : StatefulWidget {
        public ForceUpdateScreen(
            Key key = null
        ) : base(key: key) {
        }

        public override State createState() {
            return new _ForceUpdateScreen();
        }
    }
    
    public class _ForceUpdateScreen : State<ForceUpdateScreen> {
        
        bool isLoading = true;
        bool needFetchAgain;
        string url = "";

        public override void initState() {
            base.initState();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this._checkNewVersion();
            });
        }

        void _checkNewVersion() {
            SettingApi.CheckNewVersion(platform: Config.platform, store: Config.store, $"{Config.versionCode}")
                .Then(versionResponse => {
                    this.url = versionResponse.url;
                    this.isLoading = false;
                    this.setState(() => {});
                })
                .Catch(error => {
                    this.isLoading = false;
                    this.needFetchAgain = true;
                    this.setState(() => {});
                });
        }

        public override Widget build(BuildContext context) {
            Widget content;
            if (this.isLoading) {
                content = new GlobalLoading();
            }
            else if (this.needFetchAgain) {
                content = new Container(
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            new Container(
                                margin: EdgeInsets.only(bottom: 24),
                                child: Image.asset(
                                    name: "image/default-network",
                                    width: 128,
                                    height: 128
                                )
                            ),
                            new Container(
                                margin: EdgeInsets.only(bottom: 24),
                                child: new Text(
                                    data: "数据获取失败",
                                    style: new TextStyle(
                                        fontSize: 16,
                                        fontFamily: "Roboto-Regular",
                                        color: CColors.TextBody5
                                    )
                                )
                            ),
                            new Container(height: 32),
                            new CustomButton(
                                padding: EdgeInsets.zero,
                                onPressed: () => {
                                    this.isLoading = true;
                                    this.needFetchAgain = false;
                                    this.setState(() => {});
                                    this._checkNewVersion();
                                },
                                child: new Container(
                                    height: 40,
                                    width: 120,
                                    alignment: Alignment.center,
                                    decoration: new BoxDecoration(
                                        color: CColors.PrimaryBlue,
                                        borderRadius: BorderRadius.all(20)
                                    ),
                                    child: new Text(
                                        "重新获取",
                                        style: CTextStyle.PLargeMediumWhite.merge(new TextStyle(height: 1))
                                    )
                                )
                            )
                        }
                    )
                );
            }
            else {
                content = new Container(
                    padding: EdgeInsets.only(top: 120, bottom: 40),
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget> {
                            new Column(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(bottom: 16),
                                        alignment: Alignment.center,
                                        child: Image.asset(
                                            name: "image/unity-icon-logo",
                                            width: 128,
                                            height: 128,
                                            fit: BoxFit.cover
                                        )
                                    ),
                                    new Container(
                                        alignment: Alignment.center,
                                        child: Image.asset(
                                            name: "image/unity-text-logo",
                                            width: 149,
                                            height: 24,
                                            fit: BoxFit.cover
                                        )
                                    )
                                }
                            ),
                            new Text(
                                "当前版本过低，请升级后使用",
                                style: CTextStyle.PXLargeMedium
                            ),
                            new Container(
                                padding: EdgeInsets.symmetric(horizontal: 16),
                                child: new CustomButton(
                                    onPressed: () => {
                                        Application.OpenURL(url: this.url);
                                    },
                                    child: new Container(
                                        height: 48,
                                        alignment: Alignment.center,
                                        decoration: new BoxDecoration(
                                            color: CColors.PrimaryBlue,
                                            borderRadius: BorderRadius.all(24)
                                        ),
                                        child: new Text(
                                            "获取最新版本",
                                            style: CTextStyle.PLargeMediumWhite.merge(new TextStyle(height: 1))
                                        )
                                    )
                                )
                            )
                        }
                    )
                );   
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.White,
                        child: content
                    )
                )
            );
        }
    }
}