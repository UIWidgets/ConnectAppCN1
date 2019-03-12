using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class ArticleDetail : StatefulWidget {
        public ArticleDetail(
            Project articleDetail,
            Key key = null
        ) : base(key) {
            this.articleDetail = articleDetail;
        }

        public readonly Project articleDetail;

        public override State createState() {
            return new _ArticleDetailState();
        }
    }

    internal class _ArticleDetailState : State<ArticleDetail>
    {
        private Article _article = null;
        private Dictionary<string,ContentMap> _contentMap = null;

        public override void initState()
        {
            base.initState();
            _article  = widget.articleDetail.projectData;
            _contentMap = widget.articleDetail.contentMap;
        }

        public override Widget build(BuildContext context) {
            return new Container(child: _content(context));
        }

        private Widget _content(BuildContext context) {
            return new Flexible(
                child: new ListView(
                    physics: new AlwaysScrollableScrollPhysics(),
                    children: new List<Widget> {
                        _contentHead(context),
                        _subTitle(context),
                        _contentDetail(context)
                    }
                )
            );
        }

        private Widget _contentHead(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            _article.title,
                            style: CTextStyle.H3
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            child: new Text(
                                $"阅读 { _article.viewCount } · {DateConvert.DateStringFromNow(_article.createdTime)}",
                                style: new TextStyle(
                                    height: 1.67f,
                                    fontSize: 12,
                                    fontFamily: "PingFang-Regular",
                                    color: CColors.TextBody4
                                    )
                                )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 24, bottom: 24),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.start,
                                crossAxisAlignment: CrossAxisAlignment.center,
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: new ClipRRect(
                                            borderRadius: BorderRadius.circular(16),
                                            child: new Container(
                                                height: 32,
                                                width: 32,
                                                child: Image.network(
                                                    _article.headerImage.url,
                                                    fit: BoxFit.cover
                                                )
                                            )
                                        )
                                    ),


                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Container(height: 5),
                                            new Text(
//                                                articleDetail.projectData..fullName,
                                                "user",
                                                style: CTextStyle.PRegular
                                            ),
                                            new Container(height: 5),
                                            new Text(
                                                DateConvert.DateStringFromNow(_article.createdTime),
                                                style: CTextStyle.PSmall
                                            )
                                        }
                                    )
                                }
                            )),
                    }
                )
            );
        }


        private Widget _subTitle(BuildContext context)
        {
            return new Container(
                decoration:new BoxDecoration(
                   color:CColors.Separator2,
                   borderRadius:BorderRadius.all(4)
                ),
                   
                margin:EdgeInsets.only(bottom:24,left:16,right:16),
                child:new Container(
                    padding:EdgeInsets.only(16,12,16,12), 
                    child:new Text($"题头：{_article.subTitle}",style:CTextStyle.PLargeGray)
                    )  
            );
        }


        private Widget _contentDetail(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new EventDescription(content: _article.body, contentMap: _contentMap)
                    }
                )
            );
        }
    }
}