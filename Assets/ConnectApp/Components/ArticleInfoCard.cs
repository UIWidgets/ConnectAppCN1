using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class ArticleInfoCard : StatefulWidget {
        public ArticleInfoCard(
            string fullName,
            DateTime time,
            int viewCount,
            Key key = null
        ) : base(key: key) {
            this.fullName = fullName;
            this.time = time;
            this.viewCount = viewCount;
        }
        public readonly string fullName;
        public readonly DateTime time;
        public readonly int viewCount;
        
        public override State createState() {
            return new  _ArticleInfoCardState();
        }
    }

    class _ArticleInfoCardState : State<ArticleInfoCard> {
        readonly GlobalKey contentKey = GlobalKey.key("article-infoContent");
        readonly GlobalKey fullNameKey = GlobalKey.key("article-fullName");
        readonly GlobalKey timeKey = GlobalKey.key("article-time");
        readonly GlobalKey viewCountKey = GlobalKey.key("article-viewCount");

        float? _contentWidth;
        float? _fullNameWidth;
        float? _timeWidth;
        float? _viewCountWidth;
        
        public override void initState() {
            base.initState();
            WidgetsBinding.instance.addPostFrameCallback(callback: this._afterLayout);
            this._contentWidth = null;
            this._fullNameWidth = null;
            this._timeWidth = null;
            this._viewCountWidth = null;
        }

        void _afterLayout(TimeSpan duration) {
            RenderObject contentRenderObject = this.contentKey.currentContext?.findRenderObject();
            float? contentWidth = null;
            if (contentRenderObject != null) {
                var size = contentRenderObject.paintBounds.size;
                contentWidth = size.width;
            }
            
            RenderObject fullNameRenderObject = this.fullNameKey.currentContext?.findRenderObject();
            float? fullNameWidth = null;
            if (fullNameRenderObject != null) {
                var size = fullNameRenderObject.paintBounds.size;
                fullNameWidth = size.width;
            }
            
            RenderObject timeRenderObject = this.timeKey.currentContext?.findRenderObject();
            float? timeWidth = null;
            if (timeRenderObject != null) {
                var size = timeRenderObject.paintBounds.size;
                timeWidth = size.width;
            }
            
            RenderObject viewCountRenderObject = this.viewCountKey.currentContext?.findRenderObject();
            float? viewCountWidth = null;
            if (viewCountRenderObject != null) {
                var size = viewCountRenderObject.paintBounds.size;
                viewCountWidth = size.width;
            }
            this.setState(() => {
                this._contentWidth = contentWidth;
                this._fullNameWidth = fullNameWidth;
                this._timeWidth = timeWidth;
                this._viewCountWidth = viewCountWidth;
            });
        }

        public override Widget build(BuildContext context) {
            var fullNameWidth = this._contentWidth - this._timeWidth - this._viewCountWidth;
            if (fullNameWidth > this._fullNameWidth) {
                fullNameWidth = this._fullNameWidth;
            }
            return new Container(
                key: this.contentKey,
                child: new Row(
                    children: new List<Widget> {
                        new Container(
                            width: fullNameWidth,
                            child: new Text(
                                $"{this.widget.fullName}",
                                key: this.fullNameKey,
                                style: CTextStyle.PSmallBody3,
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis
                            )
                        ),
                        new Container(
                            width: this._timeWidth,
                            child: new Text(
                                $" · {DateConvert.DateStringFromNow(dt: this.widget.time)}",
                                key: this.timeKey,
                                style: CTextStyle.PSmallBody3
                            )
                        ),
                        new Container(
                            width: this._viewCountWidth,
                            child: new Text(
                                $" · 阅读 {this.widget.viewCount}",
                                key: this.viewCountKey,
                                style: CTextStyle.PSmallBody3
                            )
                        )
                    }
                )
            );
        }
    }
}