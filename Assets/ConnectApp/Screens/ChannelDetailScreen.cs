using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class ChannelDetailScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelDetailScreenViewModel>(
                converter: state => new ChannelDetailScreenViewModel {
                    channelInfo = new ChannelInfo {
                        imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                        name = "UI Widgets 技术交流",
                        size = 420,
                        isHot = true,
                        latestMessage = "kgu: 嗨，大家好",
                        time = "11:43",
                        isTop = true,
                        atMe = true,
                        introduction = "UIWidgets是一个可以独立使用的 Unity Package (https://github.com/UnityTech/UIWidgets)。"
                                     + "它将Flutter(https://flutter.io/)的App框架与Unity渲染引擎相结合，"
                                     + "让您可以在Unity编辑器中使用一套代码构建出可以同时在PC、网页及移动设备上运行的原生应用。"
                                     + "此外，您还可以在您的3D游戏或者Unity编辑器插件中用它来构建复杂的UI层，替换UGUI和IMGUI。",
                        atAll = true
                    }
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                    };
                    return new ChannelDetailScreen(actionModel, viewModel);
                }
            );
        }
    }

    public class ChannelDetailScreen : StatelessWidget {
        public ChannelDetailScreen(
            ChannelDetailScreenActionModel actionModel = null,
            ChannelDetailScreenViewModel viewModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        readonly ChannelDetailScreenActionModel actionModel;
        readonly ChannelDetailScreenViewModel viewModel;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: new Color(0xFFFAFAFA),
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                this._buildContent()
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.actionModel.mainRouterPop(),
                new Text(
                    "群聊资料",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(left: 16, right: 16, top: 16),
                            height: 64,
                            child: new Row(
                                children: new List<Widget> {
                                    new ClipRRect(
                                        borderRadius: BorderRadius.all(4),
                                        child: new Container(
                                            width: 48,
                                            height: 48,
                                            child: Image.network(this.viewModel.channelInfo.imageUrl, fit: BoxFit.cover)
                                        )
                                    ),
                                    new Expanded(
                                        child: new Container(
                                            padding: EdgeInsets.only(16),
                                            child: new Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget> {
                                                    new Text(this.viewModel.channelInfo.name,
                                                        style: CTextStyle.PLargeMedium,
                                                        maxLines: 1, overflow: TextOverflow.ellipsis),
                                                    new Expanded(
                                                        child: new Text($"{this.viewModel.channelInfo.size}名群成员",
                                                            style: CTextStyle.PRegularBody4,
                                                            maxLines: 1)
                                                    )
                                                }
                                            )
                                        )
                                    ),
                                }
                            )
                        ),
                        new GestureDetector(
                            child: this._tapRow(this.viewModel.channelInfo.introduction, 2, 16, 16, true)
                        ),
                        new Container(height: 16, color: CColors.Background),
                        new GestureDetector(
                            child: this._tapRow("查找聊天内容", 1, 18, 18)
                        )
                    }
                )
            );
        }

        Widget _tapRow(string content, int maxLines, int paddingTop, int paddingBottom, bool smallFont = false) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, top: paddingTop, bottom: paddingBottom),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new Expanded(
                            child: new Text(content,
                                style: smallFont ? CTextStyle.PRegularBody : CTextStyle.PLargeBody,
                                overflow: TextOverflow.ellipsis,
                                maxLines: maxLines)
                        ),
                        new Icon(
                            icon: Icons.arrow_forward,
                            size: 16,
                            color: CColors.Icon
                        )
                    }));
        }
    }
}
