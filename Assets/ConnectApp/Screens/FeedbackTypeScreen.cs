using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class FeedbackTypeScreenConnector : StatelessWidget {
        public FeedbackTypeScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, FeedbackTypeScreenViewModel>(
                converter: state => new FeedbackTypeScreenViewModel {
                    feedbackType = state.feedbackState.feedbackType
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new FeedbackTypeScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        changeFeedbackType = type => {
                            dispatcher.dispatch(new ChangeFeedbackTypeAction {type = type});
                            dispatcher.dispatch(new MainNavigatorPopAction());
                        }
                    };
                    return new FeedbackTypeScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class FeedbackTypeScreen : StatelessWidget {
        public FeedbackTypeScreen(
            FeedbackTypeScreenViewModel viewModel = null,
            FeedbackTypeScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        readonly FeedbackTypeScreenViewModel viewModel;
        readonly FeedbackTypeScreenActionModel actionModel;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Column(
                        children: new List<Widget> {
                            this._buildNavigationBar(),
                            new Flexible(
                                child: this._buildContent()
                            )
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.actionModel.mainRouterPop(),
                new Text(
                    "请选择反馈类型",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            var widgets = new List<Widget> {
                new CustomDivider(
                    color: CColors.White
                )
            };
            foreach (var type in FeedbackType.typesList) {
                var widget = this._buildTypeItem(type: type);
                widgets.Add(item: widget);
            }

            return new Container(
                color: CColors.Background,
                child: new ListView(
                    children: widgets
                )
            );
        }

        Widget _buildTypeItem(FeedbackType type) {
            var isCheck = this.viewModel.feedbackType.value.Equals(value: type.value);
            Widget checkWidget;
            if (isCheck) {
                checkWidget = new Icon(
                    icon: Icons.check,
                    size: 24,
                    color: CColors.PrimaryBlue
                );
            }
            else {
                checkWidget = new Container();
            }

            return new GestureDetector(
                onTap: () => this.actionModel.changeFeedbackType(obj: type),
                child: new Container(
                    color: CColors.White,
                    height: 44,
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget> {
                            new Text(
                                data: type.description,
                                style: isCheck ? CTextStyle.PLargeBlue : CTextStyle.PLargeBody
                            ),
                            checkWidget
                        }
                    )
                )
            );
        }
    }
}