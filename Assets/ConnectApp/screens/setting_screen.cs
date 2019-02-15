using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens
{
    public class SettingScreen : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                child: new Column(
                    children: new List<Widget>
                    {
                        new CustomAppBar(
                            title: new Text(
                                "Setting",
                                style: new TextStyle(
                                    fontSize: 17.0,
                                    color: CColors.text1
                                )
                            )
                        ),
                        _content(context)
                    }
                )
            );
        }

        private Widget _content(BuildContext context)
        {
            return new Flexible(
                child: new Container(
                    decoration: new BoxDecoration(
                        CColors.background2
                    ),
                    child: new ListView(
                        physics: new AlwaysScrollableScrollPhysics(),
                        children: new List<Widget>
                        {
                            _gapView(),
                            _cellView("绑定Unity ID", ""),
                            _gapView(),
                            _cellView("自动播放视频", "开"),
                            _cellView("语言", "简体中文"),
                            _cellView("清除缓存", "110MB"),
                            _gapView(),
                            _cellView("评分", ""),
                            _cellView("检查更新", "当前版本 1.1.1"),
                            _gapView(),
                            _logoutBtn()
                        }
                    )
                )
            );
        }

        private Widget _gapView()
        {
            return new Container(
                height: 16,
                decoration: new BoxDecoration(
                )
            );
        }

        private Widget _logoutBtn()
        {
            return new CustomButton(
                padding: EdgeInsets.zero,
                child: new Container(
                    height: 60,
                    decoration: new BoxDecoration(
                        color: CColors.background1
                    ),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: new List<Widget>
                        {
                            new Text(
                                "退出登录",
                                style: new TextStyle(
                                    fontSize: 16,
                                    color: CColors.warning
                                )
                            )
                        }
                    )
                )
            );
        }

        private Widget _cellView(string title, string desc)
        {
            return new Container(
                height: 60,
                padding: EdgeInsets.symmetric(0, 16),
                decoration: new BoxDecoration(
                    CColors.background1
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget>
                    {
                        new Text(
                            title,
                            style: new TextStyle(
                                fontSize: 13,
                                color: CColors.text1
                            )
                        ),
                        new Flexible(child: new Container()),
                        new Text(
                            desc,
                            style: new TextStyle(
                                fontSize: 13,
                                color: CColors.text1
                            )
                        ),
                        new Icon(
                            Icons.chevron_right,
                            size: 16.0,
                            color: CColors.icon1
                        )
                    }
                )
            );
        }
    }
}