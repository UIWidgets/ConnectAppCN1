using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components.Swiper;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class GameBrief : StatelessWidget {
        public GameBrief(
            RankData game,
            Key titleKey = null,
            Key briefKey = null,
            Widget playButton = null,
            Widget shareWidget = null,
            Key key = null
        ) : base(key: key) {
            this.game = game;
            this.titleKey = titleKey;
            this.briefKey = briefKey;
            this.playButton = playButton;
            this.shareWidget = shareWidget;
        }

        readonly RankData game;
        readonly Key titleKey;
        readonly Key briefKey;
        readonly Widget playButton;
        readonly Widget shareWidget;

        public override Widget build(BuildContext context) {
            if (this.game == null) {
                return new Container();
            }

            return new Container(
                key: this.briefKey,
                padding: EdgeInsets.symmetric(24, 16),
                child: new Row(
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(right: 12),
                            child: new PlaceholderImage(
                                imageUrl: this.game.image,
                                104,
                                104,
                                12,
                                fit: BoxFit.cover,
                                true
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                height: 104,
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new SizedBox(height: 6),
                                        new Text(
                                            data: this.game.resetTitle,
                                            key: this.titleKey,
                                            style: CTextStyle.H5,
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis
                                        ),
                                        new SizedBox(height: 2),
                                        new Text(
                                            data: this.game.resetSubLabel ?? "Unity Tiny官方示例项目",
                                            style: CTextStyle.PRegularBody4,
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis
                                        ),
                                        new Flexible(child: new Container()),
                                        new Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget> {
                                                this.playButton,
                                                this.shareWidget ?? new Container()
                                            }
                                        )
                                    }
                                )
                            )
                        )
                    }
                )
            );
        }
    }


    public class GameImageGalleryHeader : StatelessWidget {
        public GameImageGalleryHeader(
            RankData game,
            Key key = null
        ) : base(key: key) {
            this.game = game;
        }

        readonly RankData game;

        public override Widget build(BuildContext context) {
            var attachmentUrLs = this.game.attachmentURLs;
            if (attachmentUrLs.isNullOrEmpty()) {
                return new Container();
            }

            Widget swiperContent;
            if (attachmentUrLs.Count == 1) {
                var imageUrl = attachmentUrLs.FirstOrDefault();
                swiperContent = new GestureDetector(
                    onTap: () => { },
                    child: new PlaceholderImage(
                        imageUrl: imageUrl,
                        fit: BoxFit.fill,
                        useCachedNetworkImage: true,
                        color: CColorUtils.GetSpecificDarkColorFromId(id: imageUrl)
                    )
                );
            }
            else {
                swiperContent = new Swiper.Swiper(
                    (cxt, index) => {
                        var imageUrl = attachmentUrLs[index: index];
                        return new PlaceholderImage(
                            CImageUtils.SizeToScreenImageUrl(imageUrl: imageUrl),
                            fit: BoxFit.fill,
                            useCachedNetworkImage: true,
                            color: CColorUtils.GetSpecificDarkColorFromId(id: imageUrl)
                        );
                    },
                    itemCount: attachmentUrLs.Count,
                    autoplay: true,
                    onTap: index => { },
                    pagination: new SwiperPagination(margin: EdgeInsets.only(bottom: 5))
                );
            }

            return new Container(
                child: new AspectRatio(
                    aspectRatio: 16 / 9f,
                    child: swiperContent
                )
            );
        }
    }

    public class GameImageGallery : StatelessWidget {
        public GameImageGallery(
            RankData game,
            Key key = null
        ) : base(key: key) {
            this.game = game;
        }

        readonly RankData game;

        public override Widget build(BuildContext context) {
            if (this.game == null || this.game.attachmentURLs.isNullOrEmpty()) {
                return new Container();
            }

            var attachmentURLs = this.game.attachmentURLs;

            return new Container(
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            height: 1,
                            margin: EdgeInsets.symmetric(horizontal: 16),
                            color: CColors.Separator
                        ),
                        new Padding(
                            padding: EdgeInsets.all(16),
                            child: new Text(
                                "预览",
                                style: CTextStyle.H5
                            )
                        ),
                        new Container(
                            height: MediaQuery.of(context: context).size.width * 0.4f,
                            child: ListView.builder(
                                scrollDirection: Axis.horizontal,
                                itemCount: attachmentURLs.Count,
                                itemBuilder: (cxt, index) => new Container(
                                    margin: EdgeInsets.only(index == 0 ? 16 : 8,
                                        right: index == attachmentURLs.Count - 1 ? 16 : 0),
                                    child: new AspectRatio(
                                        aspectRatio: 16 / 9f,
                                        child: new PlaceholderImage(
                                            attachmentURLs[index: index],
                                            borderRadius: 8,
                                            fit: BoxFit.cover,
                                            useCachedNetworkImage: true
                                        )
                                    )
                                )
                            )
                        )
                    }
                )
            );
        }
    }

    public class GameDescription : StatelessWidget {
        public GameDescription(
            RankData game,
            Key key = null
        ) : base(key: key) {
            this.game = game;
        }

        readonly RankData game;

        public override Widget build(BuildContext context) {
            if (this.game == null) {
                return new Container();
            }

            return new Container(
                padding: EdgeInsets.only(16, 0, 16, 25),
                color: CColors.White,
                child: new Container(
                    decoration: new BoxDecoration(
                        color: CColors.Separator2,
                        borderRadius: BorderRadius.circular(8)
                    ),
                    padding: EdgeInsets.all(16),
                    child: new Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            new Text(
                                "简介",
                                style: CTextStyle.H5.defaultHeight()
                            ),
                            new Padding(
                                padding: EdgeInsets.only(top: 16),
                                child: new Text(
                                    this.game.resetDesc ?? "暂无简介",
                                    style: CTextStyle.PLargeBody2
                                )
                            )
                        }
                    )
                )
            );
        }
    }
}