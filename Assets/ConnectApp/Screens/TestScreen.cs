using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
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
                    child: renderWebSocket()
                )
            );
        }

        static Widget renderWebSocket() {
            return new ListView(
                children: new List<Widget> {
                    new GestureDetector(
                        onTap: () => {
                            GatewaySocket.Connect(
                                $"{Config.apiAddress}/api/socketgw",
                                () => "",
                                () => "",
                                true
                            );
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "ConnectWebSocket",
                                style: CTextStyle.H4
                            )
                        )
                    )
                }
            );
        }

        static Widget renderTestText() {
            return new ListView(
                children: new List<Widget> {
                    new Container(height: 10),
                    new Container(
                        color: CColors.White,
                        child: new Text(
                            "Regular: 盘点CES 2019的Unity汽车行业解决方案及案例",
                            style: new TextStyle(
                                height: 1.33f,
                                fontSize: 24,
                                fontFamily: "Roboto-Regular",
                                color: CColors.TextTitle
                            )
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        color: CColors.White,
                        child: new Text(
                            "Medium: 盘点CES 2019的Unity汽车行业解决方案及案例",
                            style: new TextStyle(
                                height: 1.33f,
                                fontSize: 24,
                                fontFamily: "Roboto-Medium",
                                color: CColors.TextTitle
                            )
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        color: CColors.White,
                        child: new Text(
                            "Bold: 盘点CES 2019的Unity汽车行业解决方案及案例",
                            style: new TextStyle(
                                height: 1.33f,
                                fontSize: 24,
                                fontFamily: "Roboto-Bold",
                                color: CColors.TextTitle
                            )
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        color: CColors.White,
                        child: new Text(
                            "w400: 盘点CES 2019的Unity汽车行业解决方案及案例",
                            style: new TextStyle(
                                height: 1.33f,
                                fontSize: 24,
                                fontWeight: FontWeight.w400,
                                color: CColors.TextTitle
                            )
                        )
                    ),
                    new Container(height: 10),
                    new Container(
                        color: CColors.White,
                        child: new Text(
                            "w700: 盘点CES 2019的Unity汽车行业解决方案及案例",
                            style: new TextStyle(
                                height: 1.33f,
                                fontSize: 24,
                                fontWeight: FontWeight.w700,
                                color: CColors.TextTitle
                            )
                        )
                    ),
                    new Container(height: 10)
                }
            );
        }

        static Widget renderTestView(BuildContext context) {
            return new ListView(
                children: new List<Widget> {
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartLoginByEmailAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "LoginByEmail",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartFetchArticlesAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchArticles",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartFetchArticleDetailAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchArticleDetail",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.dispatcher.dispatch(new LikeArticleAction
                                {articleId = "59c8cdfe09091500294d1bb9"});
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "LikeArticle",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.dispatcher.dispatch(new StartReportItemAction
                                {itemId = "59c8cdfe09091500294d1bb9", itemType = "project"});
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "ReportArticle",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.dispatcher.dispatch(new StartFetchArticleCommentsAction());
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchArticleCommentsFirst",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.dispatcher.dispatch(new StartFetchArticleCommentsAction());
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchArticleCommentsMore",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartLikeCommentAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "LikeComment",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.dispatcher.dispatch(new StartRemoveLikeCommentAction
                                {messageId = "05d5ffd1ed800000"});
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "RemoveLikeComment",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            var nonce = Snowflake.CreateNonce();
                            StoreProvider.store.dispatcher.dispatch(new StartSendCommentAction
                                {channelId = "032f7a336d800000", content = "wow!", nonce = nonce});
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "SendComment",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            var nonce = Snowflake.CreateNonce();
                            StoreProvider.store.dispatcher.dispatch(new StartSendCommentAction {
                                channelId = "032f7a336d800000", content = "good!", nonce = nonce,
                                parentMessageId = "04c4adc7ed800000"
                            });
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "SendReplyComment",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.dispatcher.dispatch(new StartReportItemAction
                                {itemId = "05d5ffd1ed800000", itemType = "comment", reportContext = "测试举报功能测试举报功能"});
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "ReportComment",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartFetchEventOngoingAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchEvents",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartFetchEventDetailAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchEventDetail",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartFetchNotificationsAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchNotifications",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartJoinEventAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "JoinEvent",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartFetchMyFutureEventsAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchMyFutureEvents",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartFetchMyPastEventsAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchMyPastEvents",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.dispatcher.dispatch(Actions.fetchMessages("0522ffbb43000001", "",
                                true));
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchMessagesFirst",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => {
                            StoreProvider.store.dispatcher.dispatch(Actions.fetchMessages("0522ffbb43000001",
                                "0587f55b2b40c000", false));
                        },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "FetchMessagesMore",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartSendMessageAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "SendMessage",
                                style: CTextStyle.H4
                            )
                        )
                    ),
                    new Container(height: 10),
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.dispatcher.dispatch(new StartSearchArticleAction()); },
                        child: new Container(
                            color: CColors.White,
                            child: new Text(
                                "SearchArticle",
                                style: CTextStyle.H4
                            )
                        )
                    )
                }
            );
        }

        Widget renderListView() {
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
                            "H4: 多重GPU粒子力场",
                            style: CTextStyle.H4
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
                            style: CTextStyle.PLargeBody
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
                            style: CTextStyle.PRegularBody
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