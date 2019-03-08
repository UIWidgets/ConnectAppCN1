using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class NotificationCard : StatelessWidget {
        public NotificationCard(
            Key key = null
        ) : base(key) {
        }

        public override Widget build(BuildContext context) {
            return new Container(
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(16, 16, 16),
                            width: 80,
                            child: new ClipRRect(
                                borderRadius: BorderRadius.circular(24),
                                child: Image.asset("pikachu", width: 48, height: 48)
                            )
                        ),
                        new Container(
                            padding: EdgeInsets.only(0, 16, 16, 16),
                            decoration: new BoxDecoration(
                                border: new Border(bottom: new BorderSide(CColors.Separator2)
                                )
                            ),
                            width: MediaQuery.of(context).size.width - 80,
                            child: new Column(
                                mainAxisAlignment: MainAxisAlignment.start,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Container(
                                        child: new RichText(
                                            text: new TextSpan(
                                                children: new List<TextSpan> {
                                                    new TextSpan("码农小哥", CTextStyle.PMedium),
                                                    new TextSpan(" 评论了你的文章", CTextStyle.PRegular)
                                                }
                                            )
                                        )
                                    ),
                                    new Container(child: new Text("谢谢关注哦,持续产出中,谢谢关注哦持续产出中,谢谢关注哦,持续产出中,谢谢关注哦,持续产出中",
                                        style: CTextStyle.PRegular)),
                                    new Container(child: new Text("2小时前", style: CTextStyle.TextBody4))
                                }
                            )
                        )
                    }
                )
            );
        }
    }
}