using System.Collections.Generic;
using ConnectApp.Components.LikeButton.Utils;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class ArticleTabBar : StatelessWidget {
        public ArticleTabBar(
            bool like,
            GestureTapCallback addCommentCallback = null,
            GestureTapCallback commentCallback = null,
            GestureTapCallback favorCallback = null,
            GestureTapCallback shareCallback = null,
            Key key = null
        ) : base(key: key) {
            this.like = like;
            this.addCommentCallback = addCommentCallback;
            this.commentCallback = commentCallback;
            this.favorCallback = favorCallback;
            this.shareCallback = shareCallback;
        }

        readonly GestureTapCallback addCommentCallback;
        readonly GestureTapCallback commentCallback;
        readonly GestureTapCallback favorCallback;
        readonly GestureTapCallback shareCallback;
        readonly bool like;

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
                            onTap: this.addCommentCallback,
                            child: new Container(
                                padding: EdgeInsets.only(16),
                                height: 32,
                                width: MediaQuery.of(context).size.width - 164,
                                decoration: new BoxDecoration(
                                    CColors.Separator2,
                                    borderRadius: BorderRadius.all(16)
                                ),
                                alignment: Alignment.centerLeft,
                                child: new Container(
                                    child: new Text(
                                        "说点想法...",
                                        style: CTextStyle.PKeyboardTextStyle
                                    )
                                )
                            )
                        ),
                        //评论
                        new CustomButton(
                            padding: EdgeInsets.symmetric(12, 10),
                            onPressed: this.commentCallback,
                            child: new Icon(Icons.comment, size: 24, color: CColors.Icon)
                        ),
                        //点赞
                        new LikeButton.LikeButton(
                            isLiked => new Icon(
                                isLiked ? Icons.favorite : Icons.favorite_border,
                                color: isLiked ? CColors.SecondaryPink : CColors.Icon,
                                size: 24
                            ),
                            size: 24,
                            circleColor: new CircleColor(
                                CColors.SecondaryPink,
                                CColors.SecondaryPink
                            ),
                            likeButtonPadding: EdgeInsets.symmetric(12, 10),
                            isLiked: this.like,
                            isShowBubbles: false,
                            onTap: () => this.favorCallback()
                        ),
                        //分享
                        new CustomButton(
                            padding: EdgeInsets.symmetric(12, 10),
                            onPressed: this.shareCallback,
                            child: new Icon(Icons.share, size: 24, color: CColors.Icon)
                        )
                    }
                )
            );
        }
    }
}