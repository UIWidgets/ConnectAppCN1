using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class AboutUsScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, object>(
                converter: state => null,
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new AboutUsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction())
                    };
                    return new AboutUsScreen(actionModel);
                }
            );
        }
    }

    public class AboutUsScreen : StatelessWidget {
        public AboutUsScreen(
            AboutUsScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.actionModel = actionModel;
        }

        readonly AboutUsScreenActionModel actionModel;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                _buildContent()
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
                    "关于",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        static Widget _buildContent() {
            return new Container(
                margin: EdgeInsets.only(top: 80),
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(bottom: 16),
                            alignment: Alignment.center,
                            width: 120,
                            height: 120,
                            decoration: new BoxDecoration(
                                borderRadius: BorderRadius.all(14),
                                border: Border.all(CColors.Separator)
                            ),
                            child: new Icon(
                                Icons.UnityLogo,
                                size: 80
                            )
                        ),
                        new Text(
                            "Unity Connect",
                            style: CTextStyle.H4
                        ),
                        new Container(height: 20),
                        new Text(
                            $"版本号：{Config.versionNumber}",
                            style: CTextStyle.PLargeBody4
                        )
                    }
                )
            );
        }
    }
}