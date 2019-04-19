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

        private readonly GestureTapCallback addCommentCallback;
        private readonly GestureTapCallback commentCallback;
        private readonly GestureTapCallback favorCallback;
        private readonly GestureTapCallback bookmarkCallback;
        private readonly GestureTapCallback shareCallback;
        private readonly bool like;

        public override Widget build(BuildContext context) {
            return new Container(
                height: 49,
                padding: EdgeInsets.only(16, right: 8),
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
                        new CustomButton(
                            padding: EdgeInsets.symmetric(12, 10),
                            onPressed: commentCallback,
                            child: new Icon(Icons.comment, size: 24, color: CColors.icon3)
                        ),
                        //点赞
                        new CustomButton(
                            padding: EdgeInsets.symmetric(12, 10),
                            onPressed: favorCallback,
                            child: new Icon(like ? Icons.favorite : Icons.favorite_border, size: 24,
                                color: like ? CColors.PrimaryBlue : CColors.icon3)
                        ),
                        //分享
                        new CustomButton(
                            padding: EdgeInsets.symmetric(12, 10),
                            onPressed: shareCallback,
                            child: new Icon(Icons.share, size: 24, color: CColors.icon3)
                        )
                    }
                )
            );
        }
    }
}