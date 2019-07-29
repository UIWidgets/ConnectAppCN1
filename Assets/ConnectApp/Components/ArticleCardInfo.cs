using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class ArticleCardInfo : StatelessWidget {
        public ArticleCardInfo(
            string fullName,
            DateTime time,
            int viewCount,
            Key key = null
        ) : base(key: key) {
            this.fullName = fullName;
            this.time = time;
            this.viewCount = viewCount;
        }

        readonly string fullName;
        readonly DateTime time;
        readonly int viewCount;

        public override Widget build(BuildContext context) {
            return new Row(
                children: new List<Widget> {
                    new Flexible(
                        child: new Container(
                            child: new Text(
                                $"{this.fullName}",
                                style: CTextStyle.PSmallBody5,
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis
                            )
                        )
                    ),
                    new Container(
                        child: new Text(
                            $" · {DateConvert.DateStringFromNow(dt: this.time)}",
                            style: CTextStyle.PSmallBody5
                        )
                    ),
                    new Container(
                        child: new Text(
                            $" · 阅读 {this.viewCount}",
                            style: CTextStyle.PSmallBody5
                        )
                    )
                }
            );
        }
    }
}