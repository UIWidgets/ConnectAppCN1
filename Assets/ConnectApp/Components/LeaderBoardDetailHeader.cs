using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.screens;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class LeaderBoardDetailHeader : StatelessWidget {
        public LeaderBoardDetailHeader(
            string title,
            string subTitle,
            LeaderBoardType type = LeaderBoardType.collection,
            bool isCollected = false,
            bool isLoading = false,
            bool isHost = false,
            List<string> images = null,
            Action ClickButtonCallback = null,
            Widget followButton = null,
            Widget leftWidget = null,
            float leftWidgetTopPadding = 8,
            Key key = null
        ) : base(key) {
            this.type = type;
            this.isCollected = isCollected;
            this.images = images ?? new List<string>();
            this.isLoading = isLoading;
            this.title = title;
            this.isHost = isHost;
            this.subTitle = subTitle;
            this.ClickButtonCallback = ClickButtonCallback;
            this.followButton = followButton;
            this.leftWidget = leftWidget;
            this.leftWidgetTopPadding = leftWidgetTopPadding;
        }

        readonly string title;
        readonly string subTitle;
        readonly LeaderBoardType type;
        readonly bool isCollected;
        readonly bool isLoading;
        readonly bool isHost;
        readonly List<string> images;
        readonly Action ClickButtonCallback;
        readonly Widget followButton;
        readonly Widget leftWidget;
        readonly float leftWidgetTopPadding;


        public override Widget build(BuildContext context) {
            Widget buttonChild;
            Color buttonColor = CColors.PrimaryBlue;
            if (this.isLoading) {
                buttonColor = CColors.Disable2;
                buttonChild = new CustomActivityIndicator(
                    size: LoadingSize.xSmall
                );
            }
            else {
                string buttonText = "收藏";
                Color textColor = CColors.PrimaryBlue;
                if (this.isCollected) {
                    buttonText = $"已收藏";
                    buttonColor = CColors.Disable2;
                    textColor = new Color(0xFF959595);
                }

                buttonChild = new Text(
                    data: buttonText,
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
                                    new Padding(
                                        padding: EdgeInsets.only(top: this.leftWidgetTopPadding),
                                        child: this.leftWidget ?? new CoverImages(
                                                   images: this.images,
                                                   horizontalGap:8,
                                                   verticalGap: 0
                                               )
                                    ),

                                    new SizedBox(width: 16),
                                    new Expanded(
                                        child: new Column(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget> {
                                                new Container(
                                                    margin: EdgeInsets.only(bottom: 4),
                                                    height: 56,
                                                    child: new Text(this.title,
                                                        maxLines: 2,
                                                        overflow: TextOverflow.ellipsis,
                                                        style: CTextStyle.H5
                                                    )),
                                                this.type == LeaderBoardType.column
                                                    ? this.followButton
                                                    : this.isHost
                                                        ? (Widget) new Container()
                                                        : new CustomButton(
                                                            onPressed: () => this.ClickButtonCallback(),
                                                            padding: EdgeInsets.zero,
                                                            child: new Container(
                                                                width: 60,
                                                                height: 28,
                                                                alignment: Alignment.center,
                                                                decoration: new BoxDecoration(
                                                                    color: CColors.White,
                                                                    borderRadius: BorderRadius.circular(14),
                                                                    border: Border.all(color: buttonColor)
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
                            padding: EdgeInsets.only(16, 0, 16),
                            child: new Text(
                                this.subTitle,
                                style: CTextStyle.PSmallBody4
                            )
                        ),
                        new SizedBox(height: 16)
                    })
            );
        }
    }
}