using System.Collections.Generic;
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
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ChannelMembersScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelMembersScreenViewModel>(
                converter: state => new ChannelMembersScreenViewModel {
                    channel = new Channel {
                        imageUrl =
                            "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                        name = "UI Widgets 技术交流",
                        isHot = true,
                        latestMessage = "kgu: 嗨，大家好",
                        time = "11:43",
                        isTop = true,
                        atMe = true,
                        introduction = "UIWidgets是一个可以独立使用的 Unity Package (https://github.com/UnityTech/UIWidgets)。"
                                       + "它将Flutter(https://flutter.io/)的App框架与Unity渲染引擎相结合，"
                                       + "让您可以在Unity编辑器中使用一套代码构建出可以同时在PC、网页及移动设备上运行的原生应用。"
                                       + "此外，您还可以在您的3D游戏或者Unity编辑器插件中用它来构建复杂的UI层，替换UGUI和IMGUI。",
                        atAll = true,
                        members = new List<User> {
                            new User {
                                name = "毛毛",
                                title = "产品经理",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                name = "尚尚",
                                title = "独立游戏开发者",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                name = "奇甲机器人 Long Long Long Long Long Name",
                                title = "Unity开发者",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                name = "mooc mooc mooc mooc mooc mooc mooc mooc mooc mooc mooc mooc",
                                title = "原画设计师",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                name = "mooc",
                                title = "原画设计师",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                name = "mooc",
                                title = "原画设计师",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                name = "mooc",
                                title = "原画设计师",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                name = "mooc",
                                title = "原画设计师",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                name = "mooc",
                                title = "原画设计师",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                            new User {
                                name = "mooc",
                                title = "原画设计师",
                                avatar =
                                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            },
                        },
                        numAdmins = 2,
                    },
                    followed = new HashSet<string> {
                        "毛毛",
                        "尚尚",
                    }
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelMembersScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                    };
                    return new ChannelMembersScreen(actionModel, viewModel);
                }
            );
        }
    }


    public class ChannelMembersScreen : StatefulWidget {
        public ChannelMembersScreen(
            ChannelMembersScreenActionModel actionModel = null,
            ChannelMembersScreenViewModel viewModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ChannelMembersScreenActionModel actionModel;
        public readonly ChannelMembersScreenViewModel viewModel;

        public override State createState() {
            return new _ChannelMembersScreenState();
        }
    }

    class _ChannelMembersScreenState : State<ChannelMembersScreen> {
        TextEditingController _controller;
        string _title;

        public override void initState() {
            base.initState();
            this._controller = new TextEditingController("");
//            SchedulerBinding.instance.addPostFrameCallback(_ => {
//                if (this.widget.viewModel.searchFollowingKeyword.Length > 0
//                    || this.widget.viewModel.searchFollowingUsers.Count > 0) {
//                    this.widget.actionModel.clearSearchFollowingResult();
//                }
//            });
        }

        public override void dispose() {
            this._controller.dispose();
            base.dispose();
        }

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
                () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    $"群聊成员({this.widget.viewModel.channel.members.Count})",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent(BuildContext context) {
            List<Widget> specialMembers = new List<Widget>();
            List<Widget> members = new List<Widget>();
            for (int i = 0; i <= this.widget.viewModel.channel.numAdmins; i++) {
                User user = this.widget.viewModel.channel.members[i];
                specialMembers.Add(buildMemberItem(context, user, i == 0 ? 0 : 1,
                    this.widget.viewModel.followed.Contains(user.name)));
            }

            for (int i = this.widget.viewModel.channel.numAdmins + 1;
                i < this.widget.viewModel.channel.members.Count;
                i++) {
                User user = this.widget.viewModel.channel.members[i];
                members.Add(buildMemberItem(context, user, 2, this.widget.viewModel.followed.Contains(user.name)));
            }

            return new Container(
                color: CColors.Background,
                child: new ListView(
                    children: new List<Widget> {
                        // this._buildSearchBar(),
                        new Column(children: specialMembers),
                        new Container(height: 16),
                        new Column(children: members),
                    }
                )
            );
        }

        Widget _buildSearchBar() {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16, 12),
                child: new InputField(
                    decoration: new BoxDecoration(
                        color: CColors.Separator2,
                        borderRadius: BorderRadius.all(8)
                    ),
                    height: 40,
                    controller: this._controller,
                    style: CTextStyle.PLargeBody2,
                    prefix: new Container(
                        padding: EdgeInsets.only(11, 9, 7, 9),
                        child: new Icon(
                            icon: Icons.search,
                            color: CColors.BrownGrey
                        )
                    ),
                    hintText: "搜索",
                    hintStyle: CTextStyle.PLargeBody4,
                    cursorColor: CColors.PrimaryBlue,
                    textInputAction: TextInputAction.search,
                    clearButtonMode: InputFieldClearButtonMode.whileEditing,
                    onChanged: text => {
                        if (text == null || text.Length <= 0) {
                            // this.widget.actionModel.clearSearchFollowingResult();
                        }
                    }
                    // onSubmitted: this._searchFollowing
                )
            );
        }

        public static Widget buildMemberItem(BuildContext context, User user, int type, bool followed) {
            Widget title = new Text(user.name, style: CTextStyle.PMediumBody,
                maxLines: 1, overflow: TextOverflow.ellipsis);
            return new Container(
                color: CColors.White,
                height: 72,
                padding: EdgeInsets.symmetric(12, 16),
                child: new Row(
                    children: new List<Widget> {
                        Avatar.User(user, 48),
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.symmetric(0, 16),
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        type == 0 || type == 1
                                            ? new Row(
                                                children: new List<Widget> {
                                                    new Flexible(child: title),
                                                    new Container(
                                                        decoration: new BoxDecoration(
                                                            color: type == 0 ? CColors.Tan : CColors.Portage,
                                                            borderRadius: BorderRadius.all(2)
                                                        ),
                                                        padding: EdgeInsets.symmetric(0, 4),
                                                        margin: EdgeInsets.only(4),
                                                        child: new Text(type == 0 ? "群主" : "管理员",
                                                            style: CTextStyle.PSmallWhite.copyWith(height: 1.2f))
                                                    )
                                                }
                                            )
                                            : title,
                                        new Expanded(
                                            child: new Text(user.title, style: CTextStyle.PRegularBody4,
                                                maxLines: 1, overflow: TextOverflow.ellipsis)
                                        )
                                    }
                                )
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            child: new Container(
                                width: 60,
                                height: 28,
                                decoration: new BoxDecoration(
                                    border: Border.all(color: followed
                                        ? CColors.Disable2
                                        : CColors.PrimaryBlue),
                                    borderRadius: BorderRadius.all(14)
                                ),
                                child: new Center(
                                    child: followed
                                        ? new Text(
                                            "已关注",
                                            style: CTextStyle.PRegularBody5.copyWith(height: 1)
                                        )
                                        : new Text(
                                            "关注",
                                            style: CTextStyle.PRegularBlue.copyWith(height: 1)
                                        )
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}