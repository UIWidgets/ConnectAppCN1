using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class FavoriteCard : StatelessWidget {
        public FavoriteCard(
            FavoriteTag favoriteTag,
            bool isShowCheckBox = false,
            GestureTapCallback onTap = null,
            bool checkBoxValue = false,
            Key key = null
        ) : base(key: key) {
            this.favoriteTag = favoriteTag;
            this.isShowCheckBox = isShowCheckBox;
            this.onTap = onTap;
            this.checkBoxValue = checkBoxValue;
        }

        readonly FavoriteTag favoriteTag;
        readonly bool isShowCheckBox;
        readonly GestureTapCallback onTap;
        readonly bool checkBoxValue;

        public override Widget build(BuildContext context) {
            if (this.favoriteTag == null) {
                return new Container();
            }

            return new GestureDetector(
                onTap: this.onTap,
                child: new Container(
                    color: CColors.White,
                    padding: EdgeInsets.only(16, 10, 16, 10),
                    child: new Row(
                        children: new List<Widget> {
                            this._buildFavoriteIcon(),
                            this._buildFavoriteInfo(),
                            this._buildFavoriteCheck()
                        }
                    )
                )
            );
        }

        Widget _buildFavoriteIcon() {
            string imageName;
            Color color;
            if (this.favoriteTag.type == "default") {
                imageName = $"{CImageUtils.FavoriteCoverImagePath}/{CImageUtils.FavoriteCoverImages[0]}";
                color = CColorUtils.FavoriteCoverColors[0];
            }
            else {
                imageName = $"{CImageUtils.FavoriteCoverImagePath}/{this.favoriteTag.iconStyle.name}";
                color = new Color(long.Parse(s: this.favoriteTag.iconStyle.bgColor));
            }
            
            return new FavoriteTagCoverImage(
                coverImage: imageName,
                coverColor: color,
                margin: EdgeInsets.only(right: 16)
            );
        }

        Widget _buildFavoriteInfo() {
            string title = this.favoriteTag.type == "default" ? "默认" : this.favoriteTag.name ?? "";
            return new Expanded(
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            data: title,
                            style: CTextStyle.PLargeBody,
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis
                        ),
                        new Text(
                            $"{this.favoriteTag.stasitics?.count ?? 0}个内容",
                            style: CTextStyle.PSmallBody4
                        )
                    }
                )
            );
        }

        Widget _buildFavoriteCheck() {
            if (!this.isShowCheckBox) {
                return new Container();
            }
            return new CustomCheckbox(
                value: this.checkBoxValue,
                value => this.onTap(),
                size: 20
            );
        }
    }
}