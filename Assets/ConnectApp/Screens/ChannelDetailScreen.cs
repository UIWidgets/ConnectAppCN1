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
    public class ChannelDetailScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelDetailScreenViewModel>(
                converter: state => new ChannelDetailScreenViewModel {
                    channel = new ChannelView {
                        thumbnail = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                        name = "UI Widgets 技术交流",
                        live = true,
                        lastMessage = new ChannelMessageView {
                            content = "kgu: 嗨，大家好",
                            time = DateConvert.DateTimeFromNonce("0604553dcdbfffff"),
                        },
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
                    var actionModel = new ChannelDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToChannelMembers = () => dispatcher.dispatch(new MainNavigatorPushToChannelMembersAction()),
                        pushToChannelIntroduction = () =>
                            dispatcher.dispatch(new MainNavigatorPushToChannelIntroductionAction())
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
                                new Expanded(
                                    child: this._buildContent(context)
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
                    "群聊资料",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent(BuildContext context) {
            List<Widget> avatars = new List<Widget>();
            float avatarSize = (MediaQuery.of(context).size.width - 32 - 16 * 4) / 5;
            for (int i = 0; i < 5; i++) {
                avatars.Add(this.viewModel.channel.members.Count > i
                    ? Avatar.User(this.viewModel.channel.members[i], avatarSize)
                    : (Widget) new Container(width: 56, height: 56)
                );
            }

            return new Container(
                color: CColors.Background,
                child: new ListView(
                    children: new List<Widget> {
                        new Container(
                            color: CColors.White,
                            padding: EdgeInsets.only(left: 16, right: 16, top: 16),
                            height: 64,
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
                        new GestureDetector(
                            onTap: () => { this.actionModel.pushToChannelIntroduction(); },
                            child: this._tapRow(this.viewModel.channel.topic, 2, 16, 16, true)
                        ),
                        new Container(height: 16),
                        new Container(
                            color: CColors.White,
                            padding: EdgeInsets.only(16, 16, 8, 16),
                            child: new Row(
                                children: new List<Widget> {
                                    new Text("群聊成员", style: CTextStyle.PLargeBody),
                                    new Expanded(
                                        child: new Container()
                                    ),
                                    new GestureDetector(
                                        onTap: () => { this.actionModel.pushToChannelMembers(); },
                                        child: new Container(
                                            color: CColors.Transparent,
                                            child: new Row(
                                                children: new List<Widget> {
                                                    new Text(
                                                        $"查看{this.viewModel.channel.members.Count}名群成员",
                                                        style: new TextStyle(
                                                            fontSize: 12,
                                                            fontFamily: "Roboto-Regular",
                                                            color: CColors.TextBody4
                                                        )
                                                    ),
                                                    new Icon(
                                                        icon: Icons.chevron_right,
                                                        size: 20,
                                                        color: Color.fromRGBO(199, 203, 207, 1)
                                                    )
                                                }
                                            )
                                        )
                                    )
                                }
                            )
                        ),
                        new Container(
                            color: CColors.White,
                            padding: EdgeInsets.only(16, 0, 16, 16),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: avatars
                            )
                        ),
                        new Container(height: 16),
//                        new GestureDetector(
//                            child: this._tapRow("查找聊天内容", 1, 18, 18)
//                        ),
                        this._switchRow("设为置顶", this.viewModel.channel.isTop, value => { }),
                        this._switchRow("消息免打扰", this.viewModel.channel.isMute, value => { }),
                        new Container(height: 16),
                        new Container(
                            color: CColors.White,
                            height: 60,
                            child: new Center(
                                child: new Text("退出群聊", style: CTextStyle.PLargeError)
                            )
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

        Widget _switchRow(string content, bool value, ValueChanged<bool> onChanged) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.symmetric(16, 18),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new Expanded(
                            child: new Text(content,
                                style: CTextStyle.PLargeBody,
                                overflow: TextOverflow.ellipsis)
                        ),
                        new CustomSwitch(value, onChanged, activeColor: CColors.PrimaryBlue),
                    }));
        }
    }
}