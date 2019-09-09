using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class DiscoverChannelsScreenConnector : StatelessWidget {
        public DiscoverChannelsScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, DiscoverChannelsScreenViewModel>(
                converter: state => {
                    return new DiscoverChannelsScreenViewModel {
                        discoverChannelInfo = new List<Channel> {
                            new Channel {
                                imageUrl =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                                name = "Unite Shanghai 技术会场",
                                members = new List<User>(),
                                isHot = true,
                                joined = true,
                            },
                            new Channel {
                                imageUrl =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                                name = "游戏开发日常吐槽",
                                members = new List<User>(),
                                isHot = true,
                            },
                            new Channel {
                                imageUrl =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                                name = "我们都爱玩游戏",
                                members = new List<User>(),
                                joined = true,
                            },
                            new Channel {
                                imageUrl =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                                name = "今天你学Unity了吗",
                                members = new List<User>(),
                            },
                            new Channel {
                                imageUrl =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                                name = "Unity深圳Meetup",
                                members = new List<User>(),
                            }
                        }
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new DiscoverChannelsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                    };
                    return new DiscoverChannelsScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class DiscoverChannelsScreen : StatefulWidget {
        public DiscoverChannelsScreen(
            DiscoverChannelsScreenViewModel viewModel = null,
            DiscoverChannelsScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly DiscoverChannelsScreenViewModel viewModel;
        public readonly DiscoverChannelsScreenActionModel actionModel;

        public override State createState() {
            return new _DiscoverChannelsScreenState();
        }
    }

    class _DiscoverChannelsScreenState : State<DiscoverChannelsScreen> {
        TextEditingController _fullNameController;
        TextEditingController _titleController;

        readonly FocusNode _fullNameFocusNode = new FocusNode();
        readonly FocusNode _titleFocusNode = new FocusNode();
        Dictionary<string, string> _jobRole;

        public override void initState() {
            base.initState();
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
                onBack: () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    "发现群聊",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            return new Container(
                color: CColors.White,
                child: new ListView(
                    padding: EdgeInsets.symmetric(16, 0),
                    children: this.widget.viewModel.discoverChannelInfo
                        .Select(MessageBuildUtils.buildDiscoverChannelItem).ToList()
                )
            );
        }
    }
}