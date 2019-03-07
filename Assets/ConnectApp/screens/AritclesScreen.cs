using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.screens {
    public class ArticleScreen : StatefulWidget {
        public override State createState() {
            return new _ArticleScreenState();
        }

        public ArticleScreen(
            Key key = null
        ) : base(key) {
        }
    }


    public class _ArticleScreenState : State<ArticleScreen> {
        private const float headerHeight = 140;
        private float _offsetY = 0;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Stack(
                    children: new List<Widget> {
                        new NotificationListener<ScrollNotification>(
                            onNotification: (ScrollNotification notification) => {
                                return _OnNotification(context, notification);
                            },
                            child: new Container(
                                padding: EdgeInsets.only(0, headerHeight - _offsetY, 0, 49),
                                child: new ListView(
                                    scrollDirection: Axis.vertical,
                                    children: new List<Widget> {
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard(),
                                        new ArticleCard()
                                    }
                                )
                            )
                        ),
                        new Positioned(
                            top: 0,
                            left: 0,
                            right: 0,
                            child: new CustomNavigationBar(new Text("文章", style: new TextStyle(
                                height: 1.25f,
                                fontSize: 32 / headerHeight * (headerHeight - _offsetY),
                                fontFamily: "PingFang-Semibold",
                                color: CColors.TextTitle
                            )), new List<Widget> {
                                new Container(child: new Icon(Icons.search, null, 28,
                                    Color.fromRGBO(255, 255, 255, 0.8f))),
                                new GestureDetector(
                                    onTap: () => { StoreProvider.store.Dispatch(new LoginByEmailAction()); },
                                    child: new Container(
                                        color: CColors.BrownGrey,
                                        child: new Text(
                                            "LoginByEmail",
                                            style: CTextStyle.H2
                                        )
                                    )
                                )
                            }, CColors.White, _offsetY)
                        )
                    }
                )
            );
        }


        private bool _OnNotification(BuildContext context, ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            if (pixels >= 0) {
                if (pixels <= headerHeight) setState(() => { _offsetY = pixels / 2.0f; });
            }
            else {
                if (_offsetY != 0) setState(() => { _offsetY = 0; });
            }

            return true;
        }
    }
}