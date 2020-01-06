using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class AlbumHeader : StatelessWidget {
        public AlbumHeader(
            Key key = null
        ) : base(key) {
        }

        public override Widget build(BuildContext context) {
            Widget buttonChild;
            Color followColor = CColors.PrimaryBlue;
            if (false) {
                followColor = CColors.Disable2;
                buttonChild = new CustomActivityIndicator(
                    size: LoadingSize.xSmall
                );
            }
            else {
                string followText = "收藏";
                Color textColor = CColors.PrimaryBlue;
                if (false) {
                    followText = "已收藏";
                    followColor = CColors.Disable2;
                    textColor = new Color(0xFF959595);
                }

                buttonChild = new Text(
                    data: followText,
                    style: new TextStyle(
                        fontSize: 14,
                        fontFamily: "Roboto-Medium",
                        color: textColor
                    )
                );
            }

            return new Container(
                color: CColors.Background,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Padding(
                            padding: EdgeInsets.all(16),
                            child: new Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new CoverImages(
                                        verticalGap: 0
                                    ),
                                    new SizedBox(width: 16),
                                    new Expanded(
                                        child: new Column(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget> {
                                                new Text(
                                                    "Unity官方博主预备营准备启动",
                                                    maxLines: 2,
                                                    overflow: TextOverflow.ellipsis,
                                                    style: CTextStyle.H5
                                                ),
                                                new CustomButton(
                                                    onPressed: () => { },
                                                    padding: EdgeInsets.zero,
                                                    child: new Container(
                                                        width: 60,
                                                        height: 28,
                                                        alignment: Alignment.center,
                                                        decoration: new BoxDecoration(
                                                            color: CColors.White,
                                                            borderRadius: BorderRadius.circular(14),
                                                            border: Border.all(color: followColor)
                                                        ),
                                                        child: buttonChild
                                                    )
                                                )
                                            }
                                        )
                                    ),
                                    new SizedBox(width: 16)
                                }
                            )
                        ),

                        new SizedBox(height: 4),
                        new Padding(
                            padding:EdgeInsets.only(16,0,16),
                            child:new Text(
                                "作者 10 • 文章 10",
                                style: CTextStyle.PSmallBody4
                            )
                            ),
                        new SizedBox(height: 16),
                        new Container(
                            height: 16,
                            decoration: new BoxDecoration(color: CColors.White, borderRadius: BorderRadius.only(12, 12))
                        )
                    })
            );
        }
    }
}