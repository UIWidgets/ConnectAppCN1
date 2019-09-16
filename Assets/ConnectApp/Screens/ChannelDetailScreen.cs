using System.Collections.Generic;
using System.Linq;
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
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class ChannelDetailScreenConnector : StatelessWidget {
        public ChannelDetailScreenConnector(string channelId, Key key = null) : base(key : key) {
            this.channelId = channelId;
        }

        public readonly string channelId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelDetailScreenViewModel>(
                converter: state => new ChannelDetailScreenViewModel {
                    channel = state.channelState.channelDict[this.channelId],
                    members = state.channelState.channelDict[this.channelId].memberIds.Select(
                        memberId => state.channelState.membersDict[memberId]
                    ).ToList()
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToChannelMembers = () => dispatcher.dispatch(new MainNavigatorPushToChannelMembersAction {
                            channelId = this.channelId
                        }),
                        pushToChannelIntroduction = () =>
                            dispatcher.dispatch(new MainNavigatorPushToChannelIntroductionAction {
                                channelId = this.channelId
                            }),
                        fetchMembers = () => dispatcher.dispatch<IPromise>(Actions.fetchChannelMembers(this.channelId))
                    };
                    return new ChannelDetailScreen(actionModel, viewModel);
                }
            );
        }
    }

    public class ChannelDetailScreen : StatefulWidget {
        public ChannelDetailScreen(
            ChannelDetailScreenActionModel actionModel = null,
            ChannelDetailScreenViewModel viewModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ChannelDetailScreenActionModel actionModel;
        public readonly ChannelDetailScreenViewModel viewModel;

        public override State createState() {
            return new _ChannelDetailScreenState();
        }
    }
    
    class _ChannelDetailScreenState : State<ChannelDetailScreen> {
        public override void initState() {
            base.initState();
            this.widget.actionModel.fetchMembers();
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
                    "群聊资料",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent(BuildContext context) {
            List<Widget> avatars = new List<Widget>();
            float avatarSize = (MediaQuery.of(context).size.width - 32 - 16 * 4) / 5;
            for (int i = 0; i < 5; i++) {
                avatars.Add(this.widget.viewModel.members.Count > i
                    ? Avatar.User(this.widget.viewModel.members[i].user, avatarSize)
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
                                            child: Image.network(this.widget.viewModel.channel.thumbnail, fit: BoxFit.cover)
                                        )
                                    ),
                                    new Expanded(
                                        child: new Container(
                                            padding: EdgeInsets.only(16),
                                            child: new Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget> {
                                                    new Text(this.widget.viewModel.channel.name,
                                                        style: CTextStyle.PLargeMedium,
                                                        maxLines: 1, overflow: TextOverflow.ellipsis),
                                                    new Expanded(
                                                        child: new Text(
                                                            $"{this.widget.viewModel.channel.memberCount}名群成员",
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
                            onTap: () => { this.widget.actionModel.pushToChannelIntroduction(); },
                            child: this._tapRow(this.widget.viewModel.channel.topic, 2, 16, 16, true)
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
                                        onTap: () => { this.widget.actionModel.pushToChannelMembers(); },
                                        child: new Container(
                                            color: CColors.Transparent,
                                            child: new Row(
                                                children: new List<Widget> {
                                                    new Text(
                                                        $"查看{this.widget.viewModel.channel.memberCount}名群成员",
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
                        this._switchRow("设为置顶", this.widget.viewModel.channel.isTop, value => { }),
                        this._switchRow("消息免打扰", this.widget.viewModel.channel.isMute, value => { }),
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