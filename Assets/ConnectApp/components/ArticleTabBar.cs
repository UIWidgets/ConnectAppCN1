using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class ArticleTabBar : StatelessWidget {
        public ArticleTabBar(
            bool like,
            GestureTapCallback addCommentCallback = null,
            GestureTapCallback commentCallback = null,
            GestureTapCallback favorCallback = null,
            GestureTapCallback bookmarkCallback = null,
            GestureTapCallback shareCallback = null,
            Key key = null
        ) : base(key) {
            this.like = like;
            this.addCommentCallback = addCommentCallback;
            this.commentCallback = commentCallback;
            this.favorCallback = favorCallback;
            this.bookmarkCallback = bookmarkCallback;
            this.shareCallback = shareCallback;
        }

        public readonly GestureTapCallback addCommentCallback;
        public readonly GestureTapCallback commentCallback;
        public readonly GestureTapCallback favorCallback;
        public readonly GestureTapCallback bookmarkCallback;
        public readonly GestureTapCallback shareCallback;
        public readonly bool like;

        public override Widget build(BuildContext context) {
            return new Container(
                height: 49,
                padding: EdgeInsets.only(16, right: 16),
                decoration: new BoxDecoration(
                    border: new Border(new BorderSide(CColors.Separator)),
                    color: CColors.White
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: addCommentCallback,
                            child: new Container(
                                padding: EdgeInsets.only(16),
                                height: 32,
                                width: MediaQuery.of(context).size.width - 164,
                                decoration: new BoxDecoration(
                                    CColors.Separator2,
                                    borderRadius: BorderRadius.all(16)
                                ),
                                alignment: Alignment.centerLeft,
                                child: new Text(
                                    "说点想法...",
                                    style: CTextStyle.PRegularBody3
                                )
                            )
                        ),
                        //评论
                        new GestureDetector(
                            onTap: commentCallback,
                            child: new Icon(Icons.comment, null, 24, CColors.icon3)
                        ),
                        new GestureDetector(
                            onTap: favorCallback,
                            child: new Icon(Icons.favorite, null, 24, like ? CColors.PrimaryBlue : CColors.icon3)
                        ),
//                        new GestureDetector(
//                            onTap:bookmarkCallback,
//                            child:new Icon(Icons.bookmark,null,24,CColors.icon3)
//                        ),
                        new GestureDetector(
                            onTap: shareCallback,
                            child: new Icon(Icons.share, null, 24, CColors.icon3)
                        )
                    }
                )
            );
        }
    }
}