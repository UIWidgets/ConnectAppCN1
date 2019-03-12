using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens
{
    public class ArticleDetailScreen : StatefulWidget
    {
        public ArticleDetailScreen(
            Key key = null
        ) : base(key)
        {
            
        }
        
        public override State createState()
        {
            return new _ArticleDetailScreenState();
        }
        
    }

    internal class _ArticleDetailScreenState : State<ArticleDetailScreen>
    {
        public override void initState()
        {
            base.initState();
            StoreProvider.store.Dispatch(new FetchArticleDetailAction()
                {articleId = StoreProvider.store.state.articleState.detailId});
        }

        public override Widget build(BuildContext context)
        {
            return new StoreConnector<AppState, Dictionary<string, object>>(
                converter: (state, dispatcher) => new Dictionary<string, object> {
                    {"articleDetail", state.articleState.articleDetail},
                },
                builder: (context1, viewModel) => {
                    var articleDetail = (ArticleDetail) viewModel["articleDetail"];
                    if (StoreProvider.store.state.articleState.articleDetailLoading)
                    {
                        return new Container(
                            color: CColors.White,
                            child: new Container(child: new CustomActivityIndicator(radius: 16))
                        ); 
                    }
                    if (articleDetail == null) return new Container();
                    return new Container(
                        color: CColors.White,
                        child: new Stack(
                            children: new List<Widget> {
                                new Column(
                                    children: new List<Widget> {
                                        _navigationBar(context),
                                        new ArticleDetailComponent(articleDetail: articleDetail),
                                    }
                                ),
                                
                                new Positioned(
                                    bottom: 0,
                                    left: 0,
                                    right: 0,
                                    child: new ArticleTabBar(
                                        commentCallback: () => { },
                                        favorCallback: () => { },
                                        bookmarkCallback: () => { },
                                        shareCallback: () => { }
                                    )
                                )
                            }
                        )
                    );
                }
            );
        }
        private Widget _navigationBar(BuildContext context) {
            return new CustomNavigationBar(
                new GestureDetector(
                    onTap: () => {
                        Navigator.pop(context);
                        StoreProvider.store.Dispatch(new ClearEventDetailAction());
                    },
                    child: new Icon(Icons.arrow_back, size: 28, color: CColors.icon3)
                ), new List<Widget> {
                    new Container(
                        padding: EdgeInsets.all(1),
                        width: 88,
                        height: 28,
                        decoration: new BoxDecoration(
                            borderRadius: BorderRadius.all(14),
                            border: Border.all(CColors.PrimaryBlue)
                        ),
                        alignment: Alignment.center,
                        child: new Text("说点想法",
                            style: new TextStyle(color: CColors.PrimaryBlue, fontSize: 14,
                                fontFamily: "PingFangSC-Medium"))
                    )
                }, CColors.White, 52);
        }
    }

    


}