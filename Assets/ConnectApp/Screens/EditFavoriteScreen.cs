using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class EditFavoriteScreenConnector : StatelessWidget {
        public EditFavoriteScreenConnector(
            string tagId,
            Key key = null
        ) : base(key: key) {
            this.tagId = tagId;
        }

        readonly string tagId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EditFavoriteScreenViewModel>(
                converter: state => new EditFavoriteScreenViewModel {
                    tagId = this.tagId,
                    favoriteTagDict = state.favoriteState.favoriteTagDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new EditFavoriteScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        editFavoriteTag = (tagId, iconStyle, name, description) =>
                            dispatcher.dispatch<IPromise>(Actions.editFavoriteTag(tagId: tagId, iconStyle: iconStyle,
                                name: name, description: description)),
                        createFavoriteTag = (iconStyle, name, description) =>
                            dispatcher.dispatch<IPromise>(Actions.createFavoriteTag(iconStyle: iconStyle, name: name,
                                description: description))
                    };
                    return new EditFavoriteScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class EditFavoriteScreen : StatefulWidget {
        public EditFavoriteScreen(
            EditFavoriteScreenViewModel viewModel,
            EditFavoriteScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly EditFavoriteScreenViewModel viewModel;
        public readonly EditFavoriteScreenActionModel actionModel;

        public override State createState() {
            return new _EditFavoriteScreenState();
        }
    }

    class _EditFavoriteScreenState : State<EditFavoriteScreen> {
        const int _nameMaxLength = 30;
        const int _descriptionMaxLength = 100;
        const int _coverImageColumn = 3;
        const float _coverImagePadding = 16;
        const float _coverImageSpacing = 8;
        const float _coverImageRunSpacing = 12;
        readonly TextEditingController _nameController = new TextEditingController();
        readonly TextEditingController _descriptionController = new TextEditingController();
        readonly FocusNode _nameFocusNode = new FocusNode();
        readonly FocusNode _descriptionFocusNode = new FocusNode();
        string _favoriteImageName;
        long _favoriteImageColor;
        string _favoriteName = "";
        string _favoriteDescription = "";
        bool _enableSubmit;

        public override void initState() {
            base.initState();
            var tagId = this.widget.viewModel.tagId;
            this._favoriteImageName = CImageUtils.FavoriteCoverImages[0];
            this._favoriteImageColor = CColorUtils.FavoriteCoverColors[0].value;
            this._enableSubmit = false;
            if (tagId.isNotEmpty()) {
                if (this.widget.viewModel.favoriteTagDict.ContainsKey(key: tagId)) {
                    var favoriteTag = this.widget.viewModel.favoriteTagDict[key: tagId];
                    this._favoriteName = favoriteTag.name;
                    this._favoriteDescription = favoriteTag.description;

                    this._nameController.text = this._favoriteName;
                    this._descriptionController.text = this._favoriteDescription;
                    this._favoriteImageName = favoriteTag.iconStyle.name;
                    if (favoriteTag.iconStyle.bgColor.isNotEmpty()) {
                        this._favoriteImageColor = long.Parse(s: favoriteTag.iconStyle.bgColor);
                    }
                }
            }
        }

        void _onPressBack() {
            if (!this._enableSubmit) {
                this.widget.actionModel.mainRouterPop();
                return;
            }

            var title = this.widget.viewModel.tagId.isNotEmpty() ? "编辑收藏夹" : "新建收藏夹";
            CustomDialogUtils.showCustomDialog(
                barrierColor: Color.fromRGBO(0, 0, 0, 0.5f),
                child: new CustomAlertDialog(
                    $"您正在{title}，是否退出",
                    actions: new List<Widget> {
                        new CustomButton(
                            child: new Text(
                                "取消",
                                style: new TextStyle(
                                    height: 1.33f,
                                    fontSize: 16,
                                    fontFamily: "Roboto-Regular",
                                    color: new Color(0xFF959595)
                                ),
                                textAlign: TextAlign.center
                            ),
                            onPressed: CustomDialogUtils.hiddenCustomDialog
                        ),
                        new CustomButton(
                            child: new Text(
                                "确定",
                                style: CTextStyle.PLargeBlue,
                                textAlign: TextAlign.center
                            ),
                            onPressed: () => {
                                CustomDialogUtils.hiddenCustomDialog();
                                this.widget.actionModel.mainRouterPop();
                            }
                        )
                    }
                )
            );
        }

        void PackUpKeyboard() {
            if (this._nameFocusNode.hasFocus) {
                this._nameFocusNode.unfocus();
            }

            if (this._descriptionFocusNode.hasFocus) {
                this._descriptionFocusNode.unfocus();
            }
        }

        void _onPressSubmit() {
            this.PackUpKeyboard();
            if (this.widget.viewModel.tagId.isNotEmpty()) {
                this.widget.actionModel.editFavoriteTag(
                    arg1: this.widget.viewModel.tagId,
                    new IconStyle {
                        name = this._favoriteImageName,
                        bgColor = $"{this._favoriteImageColor}"
                    },
                    arg3: this._favoriteName,
                    arg4: this._favoriteDescription
                );
            }
            else {
                this.widget.actionModel.createFavoriteTag(
                    new IconStyle {
                        name = this._favoriteImageName,
                        bgColor = $"{this._favoriteImageColor}"
                    },
                    arg2: this._favoriteName,
                    arg3: this._favoriteDescription
                );
            }
        }

        void _setEnableSubmit() {
            if (this._enableSubmit == false) {
                this.setState(() => this._enableSubmit = true);
            }
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Flexible(
                                    child: new ListView(
                                        children: new List<Widget> {
                                            new CustomDivider(
                                                color: CColors.Background
                                            ),
                                            this._buildInputField(),
                                            new CustomDivider(
                                                color: CColors.Background
                                            ),
                                            this._buildCoverButtons(context: context),
                                            this._buildCoverColors()
                                        }
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            var disEnabled = this._favoriteName.isEmpty();

            return new Container(
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(
                        bottom: new BorderSide(
                            color: CColors.Separator2
                        )
                    )
                ),
                height: CustomAppBarUtil.appBarHeight,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.symmetric(8, 16),
                            onPressed: this._onPressBack,
                            child: new Text(
                                "取消",
                                style: CTextStyle.PLargeBody5.merge(new TextStyle(height: 1))
                            )
                        ),
                        new Text(
                            this.widget.viewModel.tagId.isNotEmpty() ? "编辑收藏夹" : "新建收藏夹",
                            style: CTextStyle.PXLargeMedium.merge(new TextStyle(height: 1))
                        ),
                        new CustomButton(
                            padding: EdgeInsets.symmetric(8, 16),
                            onPressed: () => {
                                if (!disEnabled) {
                                    this._onPressSubmit();
                                }
                            },
                            child: new Text(
                                "完成",
                                style: disEnabled
                                    ? new TextStyle(
                                        fontSize: 16,
                                        fontFamily: "Roboto-Medium",
                                        color: Color.fromRGBO(33, 150, 243, 0.5f)
                                    )
                                    : CTextStyle.PLargeMediumBlue.merge(new TextStyle(height: 1))
                            )
                        )
                    }
                )
            );
        }

        Widget _buildInputField() {
            var imageName = $"{CImageUtils.FavoriteCoverImagePath}/{this._favoriteImageName}";
            return new Container(
                padding: EdgeInsets.all(16),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Row(
                            children: new List<Widget> {
                                new FavoriteTagCoverImage(
                                    coverImage: imageName,
                                    new Color(value: this._favoriteImageColor),
                                    margin: EdgeInsets.only(right: 16)
                                ),
                                new Expanded(
                                    child: new InputField(
                                        focusNode: this._nameFocusNode,
                                        height: 48,
                                        controller: this._nameController,
                                        maxLength: _nameMaxLength,
                                        style: new TextStyle(
                                            height: 1.2f,
                                            fontSize: 18,
                                            fontFamily: "Roboto-Medium",
                                            color: CColors.TextBody2
                                        ),
                                        hintText: "收藏夹标题",
                                        hintStyle: new TextStyle(
                                            fontSize: 18,
                                            fontFamily: "Roboto-Medium",
                                            color: CColors.TextBody4
                                        ),
                                        cursorColor: CColors.PrimaryBlue,
                                        clearButtonMode: InputFieldClearButtonMode.never,
                                        onChanged: text => {
                                            this.setState(() => this._favoriteName = text);
                                            this._setEnableSubmit();
                                        }
                                    )
                                ),
                                new Text(
                                    $"{_nameMaxLength - this._favoriteName.Length}",
                                    style: new TextStyle(
                                        fontSize: 12,
                                        fontFamily: "Roboto-Regular",
                                        color: CColors.ShadyLady
                                    )
                                )
                            }
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 16),
                            child: new Stack(
                                children: new List<Widget> {
                                    new InputField(
                                        focusNode: this._descriptionFocusNode,
                                        height: 120,
                                        controller: this._descriptionController,
                                        maxLength: _descriptionMaxLength,
                                        maxLines: null,
                                        alignment: Alignment.topLeft,
                                        style: CTextStyle.PLargeBody2,
                                        hintText: "描述一下这个收藏夹…",
                                        hintStyle: CTextStyle.PLargeBody4,
                                        cursorColor: CColors.PrimaryBlue,
                                        clearButtonMode: InputFieldClearButtonMode.never,
                                        onChanged: text => {
                                            this.setState(() => this._favoriteDescription = text);
                                            this._setEnableSubmit();
                                        }
                                    ),
                                    new Positioned(
                                        right: 0,
                                        bottom: 0,
                                        child: new Text(
                                            $"{_descriptionMaxLength - this._favoriteDescription.Length}",
                                            style: new TextStyle(
                                                fontSize: 12,
                                                fontFamily: "Roboto-Regular",
                                                color: CColors.ShadyLady
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildCoverButtons(BuildContext context) {
            var width = MediaQuery.of(context: context).size.width;
            var imageSize = (float) Math.Ceiling((width - _coverImagePadding - _coverImageSpacing * 6) / 6.5f);

            var children = new List<Widget>();
            for (var i = 0; i < CImageUtils.FavoriteCoverImages.Count; i++) {
                children.Add(this._buildCoverButtonItem(index: i, buttonSize: imageSize));
            }

            return new Container(
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(16, right: 16),
                            margin: EdgeInsets.only(top: 16),
                            child: new Text(
                                "选择封面图标",
                                style: CTextStyle.PSmallBody4
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 16, bottom: 40),
                            height: imageSize * _coverImageColumn + _coverImageRunSpacing * (_coverImageColumn - 1),
                            child: new ListView(
                                scrollDirection: Axis.horizontal,
                                children: new List<Widget> {
                                    new Container(
                                        padding: EdgeInsets.only(16, right: 16),
                                        width: imageSize * 12 + _coverImagePadding * 2 + _coverImageSpacing * 11 + 1,
                                        child: new Wrap(
                                            spacing: _coverImageSpacing,
                                            runSpacing: _coverImageRunSpacing,
                                            children: children
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildCoverColors() {
            return new Container(
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(16, right: 16),
                            child: new Text(
                                "选择封面颜色",
                                style: CTextStyle.PSmallBody4
                            )
                        ),
                        new Container(
                            height: 36,
                            margin: EdgeInsets.only(top: 10, bottom: 10),
                            child: ListView.builder(
                                padding: EdgeInsets.only(10, right: 10),
                                scrollDirection: Axis.horizontal,
                                itemCount: CColorUtils.FavoriteCoverColors.Count,
                                itemBuilder: (cxt, index) => {
                                    var color = CColorUtils.FavoriteCoverColors[index: index];
                                    var isSelected = this._favoriteImageColor == color.value;
                                    return new GestureDetector(
                                        onTap: () => {
                                            this.PackUpKeyboard();
                                            this._setEnableSubmit();
                                            if (!isSelected) {
                                                this.setState(() => this._favoriteImageColor = color.value);
                                            }
                                        },
                                        child: new Container(
                                            padding: EdgeInsets.all(6),
                                            color: CColors.Transparent,
                                            child: new Container(
                                                width: 24,
                                                height: 24,
                                                decoration: new BoxDecoration(
                                                    color: color,
                                                    borderRadius: BorderRadius.circular(12),
                                                    border: Border.all(
                                                        isSelected ? CColors.TextBody : CColors.Transparent,
                                                        1.5f
                                                    )
                                                )
                                            )
                                        )
                                    );
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildCoverButtonItem(int index, float buttonSize) {
            var imageName = CImageUtils.FavoriteCoverImages[index: index];
            var isSelected = this._favoriteImageName == imageName;
            return new CustomButton(
                width: buttonSize,
                height: buttonSize,
                decoration: new BoxDecoration(
                    color: CColors.BlackHaze,
                    borderRadius: BorderRadius.circular(4),
                    border: Border.all(
                        isSelected ? CColors.TextBody : CColors.BlackHaze,
                        1.5f
                    )
                ),
                child: Image.asset(
                    $"{CImageUtils.FavoriteCoverImagePath}/{imageName}"
                ),
                onPressed: () => {
                    this.PackUpKeyboard();
                    this._setEnableSubmit();
                    if (this._favoriteImageName != imageName) {
                        this.setState(() => this._favoriteImageName = imageName);
                    }
                }
            );
        }
    }
}