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
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = ConnectApp.Components.Avatar;

namespace ConnectApp.screens {
    public class ChannelMentionScreenConnector : StatelessWidget {
        public ChannelMentionScreenConnector(
            string channelId,
            Key key = null
        ) : base(key: key) {
            this.channelId = channelId;
        }

        readonly string channelId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelMentionScreenViewModel>(
                converter: state => {
                    return new ChannelMentionScreenViewModel {
                        channel = state.channelState.channelDict[this.channelId]
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelMentionScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        chooseMentionCancel = () => {
                            dispatcher.dispatch(new ChannelChooseMentionCancelAction());
                            dispatcher.dispatch(new MainNavigatorPopAction());
                        },
                        chooseMentionConfirm = mentionUserId => {
                            dispatcher.dispatch(new ChannelChooseMentionConfirmAction {
                                mentionUserId = mentionUserId
                            });
                            dispatcher.dispatch(new MainNavigatorPopAction());
                        }
                    };
                    return new ChannelMentionScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }
    
    public class ChannelMentionScreen : StatefulWidget {
        public ChannelMentionScreen(
            ChannelMentionScreenViewModel viewModel = null,
            ChannelMentionScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }
        
        public readonly ChannelMentionScreenViewModel viewModel;
        public readonly ChannelMentionScreenActionModel actionModel;

        public override State createState() {
            return new _ChannelMentionScreenState();
        }
    }

    class _ChannelMentionScreenState : State<ChannelMentionScreen> {
        
        readonly TextEditingController _editingController = new TextEditingController();
        readonly List<ChannelMember> mentionList = new List<ChannelMember>();

        public override void initState() {
            this.updateMentionList("");
            base.initState();
        }

        void updateMentionList(string query) {
            this.mentionList.Clear();
            foreach(var memberKey in this.widget.viewModel.channel.membersDict.Keys) {
                var member = this.widget.viewModel.channel.membersDict[memberKey];
                if (query == "" || member.user.fullName.Contains(query)) {
                    this.mentionList.Add(member);
                }
            }
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                this._buildSearchBar(),
                                new Expanded(
                                    child: this._buildMentionList()
                                )
                            }
                        )
                    )
                )
            );
        }

        void _onSearch(string query) {
            this.setState(() => {
                this.updateMentionList(query);
            });
        }

        Widget _buildMentionList() {
            Widget ret = new Container(
                color: CColors.White,
                    child: ListView.builder(
                        itemCount: this.mentionList.Count,
                        itemBuilder: this._buildMentionTile
                    )
            );

            return ret;
        }


        void _onChooseMention(ChannelMember member) {
            this.widget.actionModel.chooseMentionConfirm(member.id);
        }

        Widget _buildMentionTile(BuildContext context, int index) {
            ChannelMember member = this.mentionList[index];
            return new GestureDetector(
                onTap: () => {
                    this._onChooseMention(member);
                },
                child: new Container(
                    color: CColors.White,
                    height: 72,
                    padding: EdgeInsets.symmetric(12, 16),
                    child: new Row(
                        children: new List<Widget> {
                            Avatar.User(user: member.user, 48),
                            new Expanded(
                                child: new Container(
                                    padding: EdgeInsets.symmetric(0, 16),
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Flexible(child: new Text(
                                                data: member.user.fullName,
                                                style: CTextStyle.PMediumBody,
                                                maxLines: 1,
                                                overflow: TextOverflow.ellipsis
                                            ))
                                        }
                                    )
                                )
                            )
                        }
                    )
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
                    controller: this._editingController,
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
                    onChanged: this._onSearch,
                    onSubmitted: this._onSearch
                )
            );
        }
        
        Widget _buildNavigationBar() {
            return new Container(
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(
                        bottom: new BorderSide(
                            color: CColors.Separator2
                        )
                    )
                ),
                height: 44,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new Container(),
                        new Text(
                            "群聊成员",
                            style: CTextStyle.PXLargeMedium
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(8, 8, 8, 8),
                            onPressed: () => this.widget.actionModel.chooseMentionCancel(),
                            child: new Text(
                                "取消",
                                style: CTextStyle.PLargeBlue
                            )
                        ),
                    }
                )
            );
        }
    }
}