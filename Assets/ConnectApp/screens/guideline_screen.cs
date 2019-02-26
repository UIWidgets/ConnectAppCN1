using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

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
                                    style: new TextStyle(
                                        fontSize: 48,
                                        color: CColors.white
                                    )
                                )
                            ),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "Semibold: 多重GPU粒子力场",
                                    style: new TextStyle(
                                        height: 1.16f,
                                        fontSize: 40,
                                        fontFamily: "PingFang-Semibold",
                                        color: CColors.black,
//                                        fontWeight: FontWeight.w400,
                                        textBaseline: TextBaseline.alphabetic
                                    )
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "Medium: 多重GPU粒子力场",
                                    style: new TextStyle(
                                        height: 1.16f,
                                        fontSize: 40,
                                        fontFamily: "PingFang-Medium",
                                        color: CColors.black,
//                                        fontWeight: FontWeight.w400,
                                        textBaseline: TextBaseline.alphabetic
                                    )
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "Regular: 多重GPU粒子力场",
                                    style: new TextStyle(
                                        height: 1.16f,
                                        fontSize: 40,
                                        fontFamily: "PingFang-Regular",
                                        color: CColors.black,
//                                        fontWeight: FontWeight.w400,
                                        textBaseline: TextBaseline.alphabetic
                                    )
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "Default: 多重GPU粒子力场",
                                    style: new TextStyle(
                                        height: 1.16f,
                                        fontSize: 40,
//                                        fontFamily: "Roboto Mono",
                                        color: CColors.black,
//                                        fontWeight: FontWeight.w400,
                                        textBaseline: TextBaseline.alphabetic
                                    )
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
                                    style: new TextStyle(
                                        height: 1.2f,
                                        fontSize: 40,
                                        color: CColors.black,
                                        fontWeight: FontWeight.w700,
                                        textBaseline: TextBaseline.alphabetic
                                    )
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
                                    style: new TextStyle(
                                        height: 1.25f,
                                        fontSize: 32,
                                        color: CColors.black,
                                        fontWeight: FontWeight.w700,
                                        textBaseline: TextBaseline.alphabetic
                                    )
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
                                    style: new TextStyle(
                                        height: 1.28f,
                                        fontSize: 28,
                                        color: CColors.black,
                                        fontWeight: FontWeight.w700,
                                        textBaseline: TextBaseline.alphabetic
                                    )
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
                                    style: new TextStyle(
                                        height: 1.33f,
                                        fontSize: 24,
                                        color: CColors.black,
                                        fontWeight: FontWeight.w700,
                                        textBaseline: TextBaseline.alphabetic
                                    )
                                )
                            ),
                            new Container(height: 10),
                            new Container(
                                decoration: new BoxDecoration(
                                    CColors.white,
                                    border: Border.all(CColors.green)
                                ),
                                child: new Text(
                                    "H5: 多重GPU粒子力场",
                                    style: new TextStyle(
                                        height: 1.4f,
                                        fontSize: 20,
                                        color: CColors.black,
                                        fontWeight: FontWeight.w700,
                                        textBaseline: TextBaseline.alphabetic
                                    )
                                )
                            ),
                            new Container(
                                alignment: Alignment.center,
                                padding: EdgeInsets.only(top: 40),
                                child: new Text(
                                    "段落",
                                    style: new TextStyle(
                                        fontSize: 48,
                                        color: CColors.white
                                    )
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
                                    style: new TextStyle(
                                        height: 1.6f,
                                        fontSize: 16,
                                        color: CColors.black,
                                        fontWeight: FontWeight.w400,
                                        textBaseline: TextBaseline.alphabetic
                                    )
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
                                    style: new TextStyle(
                                        height: 1.57f,
                                        fontSize: 16,
                                        color: CColors.black,
                                        fontWeight: FontWeight.w400,
                                        textBaseline: TextBaseline.alphabetic
                                    )
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
                                    style: new TextStyle(
                                        height: 1.66f,
                                        fontSize: 12,
                                        color: CColors.black,
                                        fontWeight: FontWeight.w400,
                                        textBaseline: TextBaseline.alphabetic
                                    )
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
                                    style: new TextStyle(
                                        height: 1.66f,
                                        fontSize: 12,
                                        color: CColors.black,
                                        fontWeight: FontWeight.w700,
                                        textBaseline: TextBaseline.alphabetic
                                    )
                                )
                            )
                        }
                    )
                )
            );
        }
    }
}