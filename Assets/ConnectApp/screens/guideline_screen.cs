using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class GuidelineScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                child: new Container(
                    padding: EdgeInsets.all(10),
                    decoration: new BoxDecoration(
                        CColors.background1,
                        border: Border.all(CColors.red)
                    ),
                    child: new ListView(
                        physics: new BouncingScrollPhysics(),
                        children: new List<Widget> {
                            new Container(
                                alignment: Alignment.center,
                                padding: EdgeInsets.only(top: 40),
                                child: new Text(
                                    "标题",
                                    style: CTextStyle.Xtra
                                )
                            ),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "Xtra: 多重GPU粒子力场",
                                    style: CTextStyle.Xtra
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "H1: 多重GPU粒子力场",
                                    style: CTextStyle.H1
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "H2: 多重GPU粒子力场",
                                    style: CTextStyle.H2
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "H3: 多重GPU粒子力场",
                                    style: CTextStyle.H3
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "H4: 多重GPU粒子力场",
                                    style: CTextStyle.H4
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "H2: 多重GPU粒子力场",
                                    style: CTextStyle.H5
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                alignment: Alignment.center,
                                padding: EdgeInsets.only(top: 40),
                                child: new Text(
                                    "段落",
                                    style: CTextStyle.Caption
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "P-Large: 设置粒子系统的方法是把任何附加了该组件的游戏对象的任何子Transform都视为作力场对象。然后我们可以从父对象动态添加或移除Transform，来创建或销毁力场。",
                                    style: CTextStyle.PLarge
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "P-Regular: 设置粒子系统的方法是把任何附加了该组件的游戏对象的任何子Transform都视为作力场对象。然后我们可以从父对象动态添加或移除Transform，来创建或销毁力场。 然后我们可以从父对象动态添加或移除Transform，来创建或销毁力场。",
                                    softWrap: true,
                                    style: CTextStyle.PRegular
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "P-Small: 设置粒子系统的方法是把任何附加了该组件的游戏对象的任何子Transform都视为作力场对象。然后我们可以从父对象动态添加或移除Transform，来创建或销毁力场。",
                                    style: CTextStyle.PSmall
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "P-Caption: 即将开始",
                                    style: CTextStyle.Caption
                                )
                            )
                        }
                    )
                )
            );
        }
    }
}