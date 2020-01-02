using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class BloggerScreenConnector : StatelessWidget {
        public BloggerScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, BloggerScreenViewModel>(
                converter: state => new BloggerScreenViewModel(),
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new BloggerScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction())
                    };
                    return new BloggerScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }
    public class BloggerScreen : StatefulWidget {
        public BloggerScreen(
            BloggerScreenViewModel viewModel,
            BloggerScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly BloggerScreenViewModel viewModel;
        public readonly BloggerScreenActionModel actionModel;

        public override State createState() {
            return new _BloggerScreenState();
        }
    }

    class _BloggerScreenState : State<BloggerScreen> {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar()
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                new Text(
                    "博主",
                    style: CTextStyle.H2
                ),
                onBack: () => this.widget.actionModel.mainRouterPop()
            );
        }
    }
}