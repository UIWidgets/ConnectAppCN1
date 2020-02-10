using System.Collections.Generic;
using System.Linq;
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
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ReactionsDetailScreenConnector : StatelessWidget {
        public ReactionsDetailScreenConnector(
            string messageId,
            Key key = null
        ) : base(key: key) {
            this.messageId = messageId;
        }

        readonly string messageId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ReactionsDetailScreenViewModel>(
                converter: state => new ReactionsDetailScreenViewModel {
                    reactionsUsernameDict =
                        state.channelState.messageDict[key: this.messageId]?.reactionsUsernameListDict ??
                        new Dictionary<string, List<string>>()
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new BaseActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction())
                    };
                    return new ReactionsDetailScreen(viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class ReactionsDetailScreen : StatelessWidget {
        public ReactionsDetailScreen(
            ReactionsDetailScreenViewModel viewModel = null,
            BaseActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        readonly ReactionsDetailScreenViewModel viewModel;
        readonly BaseActionModel actionModel;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new GestureDetector(
                        onTap: () => this.actionModel.mainRouterPop(),
                        child: new Column(
                            children: new List<Widget> {
                                new Container(
                                    height: CustomAppBarUtil.appBarHeight,
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        children: new List<Widget> {
                                            new Container(width: 56),
                                            new Text(
                                                "详情",
                                                style: CTextStyle.PXLargeMedium.merge(new TextStyle(height: 1))
                                            ),
                                            new CustomButton(
                                                padding: EdgeInsets.symmetric(10, 16),
                                                onPressed: () => this.actionModel.mainRouterPop(),
                                                child: new Icon(
                                                    icon: Icons.close,
                                                    size: 24,
                                                    color: CColors.Icon
                                                )
                                            )
                                        }
                                    )
                                ),
                                new CustomDivider(color: CColors.Separator2, height: 1),
                                new Expanded(
                                    child: this._buildContent()
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildContent() {
            if (this.viewModel.reactionsUsernameDict.isEmpty()) {
                return new Container();
            }

            return new Container(
                child: new ListView(
                    children: new List<Widget>(
                        ReactionType.typesList.Select(type =>
                            _buildCard(type: type, this.viewModel.reactionsUsernameDict[key: type.value])
                        ).ToArray()
                    )
                )
            );
        }

        static Widget _buildCard(ReactionType type, List<string> nameList) {
            if (nameList.isEmpty()) {
                return new Container();
            }

            var content = "";
            var title = "";
            if (nameList != null && nameList.isNotEmpty()) {
                content = string.Join(", ", nameList.ToArray());
            }

            if (type.value == ReactionType.Thumb.value) {
                title = $"{nameList?.Count ?? 0} 人表示喜欢";
            }
            else if (type.value == ReactionType.Oppose.value) {
                title = $"{nameList?.Count ?? 0} 人表示反对";
            }
            else if (type.value == ReactionType.Coverface.value) {
                title = $"{nameList?.Count ?? 0} 人表示捂脸";
            }
            else if (type.value == ReactionType.Heartbeat.value) {
                title = $"{nameList?.Count ?? 0} 人表示心动";
            }
            else if (type.value == ReactionType.Doubt.value) {
                title = $"{nameList?.Count ?? 0} 人表示疑惑";
            }

            return new Container(
                padding: EdgeInsets.only(8, 16, 8),
                child: new Column(
                    children: new List<Widget> {
                        new Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Container(
                                    height: 36,
                                    width: 36,
                                    margin: EdgeInsets.only(right: 8),
                                    child: Image.asset(
                                        name: type.gifImagePath
                                    )
                                ),
                                new Expanded(
                                    child: new Column(
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Container(
                                                child: new Text(
                                                    data: title,
                                                    style: CTextStyle.PSmall.merge(new TextStyle(height: 1))
                                                )
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(top: 4),
                                                child: new Text(
                                                    data: content,
                                                    style: CTextStyle.PLargeMedium
                                                )
                                            )
                                        }
                                    )
                                )
                            }
                        ),
                        new Container(height: 15),
                        new CustomDivider(color: CColors.Separator2, height: 1)
                    }
                )
            );
        }
    }
}