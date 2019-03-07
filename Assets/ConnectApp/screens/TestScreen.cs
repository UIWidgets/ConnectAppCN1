using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class TestScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                child: new Container(
                    padding: EdgeInsets.all(10),
                    decoration: new BoxDecoration(
                        CColors.background1,
                        border: Border.all(CColors.Red)
                    ),
                    child: renderTestView()
                )
            );
        }

        private static Widget renderTestView() {
            return new ListView(
                children: new List<Widget> {
                    new Container(
                        color: CColors.White,
                        child: new StoreConnector<AppState, string>(
                            converter: (state, dispatch) => $"Count: {state.Count}",
                            builder: (context1, countText) => new Text(
                                countText,
                                style: CTextStyle.H1
                            )
                        )
                    ),
                    new Container(height: 20),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.Dispatch(new FetchArticlesAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchArticles",
                                style: CTextStyle.H2
                            )
                        )
                    ),
                    new Container(height: 20),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.Dispatch(new FetchArticleDetailAction
                                {articleId = "59c8cdfe09091500294d1bb9"});
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchArticleDetail",
                                style: CTextStyle.H2
                            )
                        )
                    ),
                    new Container(height: 20),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.Dispatch(new LoginByEmailAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "LoginByEmail",
                                style: CTextStyle.H2
                            )
                        )
                    ),
                    new Container(height: 20),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.Dispatch(new FetchEventsAction {pageNumber = 1}); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "EventsRequest",
                                style: CTextStyle.H2
                            )
                        )
                    ),
                    new Container(height: 20),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.Dispatch(new FetchEventDetailAction
                                {eventId = "5b9753f22920c6002ed2c22d"});
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "LiveRequest",
                                style: CTextStyle.H2
                            )
                        )
                    ),
                    new Container(height: 20),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.Dispatch(new FetchNotificationsAction {pageNumber = 1}); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchNotifications",
                                style: CTextStyle.H2
                            )
                        )
                    )
                }
            );
        }

        private Widget renderListView() {
            return new ListView(
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
                            CColors.White,
                            border: Border.all(CColors.Green)
                        ),
                        child: new Text(
                            "Xtra: 多重GPU粒子力场",
                            style: CTextStyle.Xtra
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        decoration: new BoxDecoration(
                            CColors.White,
                            border: Border.all(CColors.Green)
                        ),
                        child: new Text(
                            "H1: 多重GPU粒子力场",
                            style: CTextStyle.H1
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        decoration: new BoxDecoration(
                            CColors.White,
                            border: Border.all(CColors.Green)
                        ),
                        child: new Text(
                            "H2: 多重GPU粒子力场",
                            style: CTextStyle.H2
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        decoration: new BoxDecoration(
                            CColors.White,
                            border: Border.all(CColors.Green)
                        ),
                        child: new Text(
                            "H3: 多重GPU粒子力场",
                            style: CTextStyle.H3
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        decoration: new BoxDecoration(
                            CColors.White,
                            border: Border.all(CColors.Green)
                        ),
                        child: new Text(
                            "H4: 多重GPU粒子力场",
                            style: CTextStyle.H4
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        decoration: new BoxDecoration(
                            CColors.White,
                            border: Border.all(CColors.Green)
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
                            CColors.White,
                            border: Border.all(CColors.Green)
                        ),
                        child: new Text(
                            "P-Large: 设置粒子系统的方法是把任何附加了该组件的游戏对象的任何子Transform都视为作力场对象。然后我们可以从父对象动态添加或移除Transform，来创建或销毁力场。",
                            style: CTextStyle.PLarge
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        decoration: new BoxDecoration(
                            CColors.White,
                            border: Border.all(CColors.Green)
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
                            CColors.White,
                            border: Border.all(CColors.Green)
                        ),
                        child: new Text(
                            "P-Small: 设置粒子系统的方法是把任何附加了该组件的游戏对象的任何子Transform都视为作力场对象。然后我们可以从父对象动态添加或移除Transform，来创建或销毁力场。",
                            style: CTextStyle.PSmall
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        decoration: new BoxDecoration(
                            CColors.White,
                            border: Border.all(CColors.Green)
                        ),
                        child: new Text(
                            "P-Caption: 即将开始",
                            style: CTextStyle.Caption
                        )
                    )
                }
            );
        }
    }
}