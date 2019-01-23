using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Samples.ConnectApp.widgets;
using Unity.UIWidgets.widgets;

namespace Unity.UIWidgets.Samples.ConnectApp {
    public class SettingScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        new CustomAppBar(
                            title: new Text(
                                "Setting",
                                style: new TextStyle(
                                    fontSize: 17.0,
                                    color: CLColors.text1
                                )
                            )
                        ),
                        this._content(context)
                    }
                )
            );
        }

        Widget _content(BuildContext context) {
            return new Flexible(
                child: new Container(
                    decoration: new BoxDecoration(
                        CLColors.background2
                    ),
                    child: new ListView(
                        physics: new AlwaysScrollableScrollPhysics(),
                        children: new List<Widget> {
                            this._gapView(),
                            this._cellView("绑定Unity ID", ""),
                            this._gapView(),
                            this._cellView("自动播放视频", "开"),
                            this._cellView("语言", "简体中文"),
                            this._cellView("清除缓存", "110MB"),
                            this._gapView(),
                            this._cellView("评分", ""),
                            this._cellView("检查更新", "当前版本 1.1.1"),
                            this._gapView(),
                            this._logoutBtn()
                        }
                    )
                )
            );
        }

        Widget _gapView() {
            return new Container(
                height: 16,
                decoration: new BoxDecoration(
                )
            );
        }

        Widget _logoutBtn() {
            return new CustomButton(
                padding: EdgeInsets.zero,
                child: new Container(
                    height: 60,
                    decoration: new BoxDecoration(
                        color: CLColors.background1
                    ),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: new List<Widget> {
                            new Text(
                                "退出登录",
                                style: new TextStyle(
                                    fontSize: 16,
                                    color: CLColors.warning
                                )
                            )
                        }
                    )
                )
            );
        }

        Widget _cellView(string title, string desc) {
            return new Container(
                height: 60,
                padding: EdgeInsets.symmetric(0, 16),
                decoration: new BoxDecoration(
                    CLColors.background1
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new Text(
                            title,
                            style: new TextStyle(
                                fontSize: 13,
                                color: CLColors.text1
                            )
                        ),
                        new Flexible(child: new Container()),
                        new Text(
                            desc,
                            style: new TextStyle(
                                fontSize: 13,
                                color: CLColors.text1
                            )
                        ),
                        new Icon(
                            Icons.chevron_right,
                            size: 16.0,
                            color: CLColors.icon1
                        )
                    }
                )
            );
        }
    }
}