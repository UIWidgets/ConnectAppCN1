using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class ChannelIntroductionScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelIntroductionScreenViewModel>(
                converter: state => new ChannelIntroductionScreenViewModel {
                    channel = new ChannelView {
                        thumbnail = 
                            "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                        name = "UI Widgets 技术交流",
                        live = true,
                        isTop = true,
                        atMe = true,
                        topic = "UIWidgets是一个可以独立使用的 Unity Package (https://github.com/UnityTech/UIWidgets)。"
                                       + "它将Flutter(https://flutter.io/)的App框架与Unity渲染引擎相结合，"
                                       + "让您可以在Unity编辑器中使用一套代码构建出可以同时在PC、网页及移动设备上运行的原生应用。"
                                       + "此外，您还可以在您的3D游戏或者Unity编辑器插件中用它来构建复杂的UI层，替换UGUI和IMGUI。",
                        atAll = true,
                        members = new List<User> {
                            new User {
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                        }
                    }
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelIntroductionScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToChannelMembers = () => dispatcher.dispatch(new MainNavigatorPushToChannelMembersAction())
                    };
                    return new ChannelIntroductionScreen(actionModel, viewModel);
                }
            );
        }
    }

    public class ChannelIntroductionScreen : StatelessWidget {
        public ChannelIntroductionScreen(
            ChannelIntroductionScreenActionModel actionModel = null,
            ChannelIntroductionScreenViewModel viewModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        readonly ChannelIntroductionScreenActionModel actionModel;
        readonly ChannelIntroductionScreenViewModel viewModel;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: new Color(0xFFFAFAFA),
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Expanded(
                                    child: this._buildContent()
                                )
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
                    "群简介",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            return new Container(
                color: CColors.Background,
                child: new ListView(
                    children: new List<Widget> {
                        new Container(
                            color: CColors.White,
                            padding: EdgeInsets.all(16),
                            height: 80,
                            child: new Row(
                                children: new List<Widget> {
                                    new ClipRRect(
                                        borderRadius: BorderRadius.all(4),
                                        child: new Container(
                                            width: 48,
                                            height: 48,
                                            child: Image.network(this.viewModel.channel.thumbnail, fit: BoxFit.cover)
                                        )
                                    ),
                                    new Expanded(
                                        child: new Container(
                                            padding: EdgeInsets.only(16),
                                            child: new Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget> {
                                                    new Text(this.viewModel.channel.name,
                                                        style: CTextStyle.PLargeMedium,
                                                        maxLines: 1, overflow: TextOverflow.ellipsis),
                                                    new Expanded(
                                                        child: new Text(
                                                            $"{this.viewModel.channel.members.Count}名群成员",
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
                        new Container(
                            color: CColors.White,
                            padding: EdgeInsets.only(16, 0, 16, 16),
                            child: new Text(this.viewModel.channel.topic)
                        ),
                    }
                )
            );
        }
    }
}