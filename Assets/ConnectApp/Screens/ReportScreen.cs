using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.screens {
    public enum ReportType {
        article,
        comment
    }
    public class ReportScreenConnector : StatelessWidget {
        public ReportScreenConnector(
            string reportId,
            ReportType reportType,
            Key key = null
        ) : base(key) {
            this.reportId = reportId;
            this.reportType = reportType;
        }

        private readonly string reportId;
        private readonly ReportType reportType;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ReportScreenViewModel>(
                converter: state => new ReportScreenViewModel {
                    reportId = reportId,
                    reportType = reportType,
                    loading = state.reportState.loading
                },
                builder: (context1, viewModel, dispatcher) => {
                    var itemType = "";
                    if (reportType == ReportType.article) {
                        itemType = "project";
                    }
                    if (reportType == ReportType.comment) {
                        itemType = "comment";
                    }
                    var actionModel = new ReportScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        startReportItem = () => dispatcher.dispatch(new StartReportItemAction()),
                        reportItem = reportContext => dispatcher.dispatch<IPromise>(
                            Actions.reportItem(
                                reportId,
                                itemType,
                                reportContext
                            )
                        )
                    };
                    return new ReportScreen(viewModel, actionModel, key);
                }
            );
        }
    }
    
    public class ReportScreen : StatefulWidget {
        public ReportScreen(
            ReportScreenViewModel viewModel = null,
            ReportScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ReportScreenViewModel viewModel;
        public readonly ReportScreenActionModel actionModel;

        public override State createState() => new _ReportScreenState();
    }

    internal class _ReportScreenState : State<ReportScreen> {

        private readonly List<string> _reportItems = new List<string> {
            "垃圾信息",
            "涉嫌侵权",
            "不友善行为",
            "有害信息"
        };
        private int _selectedIndex;
        public override void initState() {
            base.initState();
            _selectedIndex = 0;
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    child: new Container(
                        child: new Column(
                            children: new List<Widget> {
                                _buildNavigationBar(),
                                _buildContent()
                            }
                        )
                    )
                )
            );
        }
        
        private Widget _buildNavigationBar() {
            return new Container(
                decoration: new BoxDecoration(
                    CColors.White,
                    border: new Border(
                        bottom: new BorderSide(
                            CColors.Separator2
                        )
                    )
                ),
                height: 44,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.only(16, 0, 16),
                            onPressed: () => widget.actionModel.mainRouterPop(),
                            child: new Icon(
                                Icons.arrow_back,
                                size: 24,
                                color: CColors.icon3
                            )
                        ),
                        new Container(
                            child: new Text(
                                "举报",
                                style: CTextStyle.H5
                            )
                        ),
                        new Container(
                            width: 56
                        )
                    }
                )
            );
        }

        private Widget _buildContent() {
            var widgets = new List<Widget>();
            _reportItems.ForEach(item => {
                var index = _reportItems.IndexOf(item);
                var widget = _buildReportItem(item, index);
                widgets.Add(widget);
            });
            widgets.Add(_buildReportButton());
            return new Container(
                margin: EdgeInsets.only(top: 15),
                child: new Column(
                    children: widgets
                )
            );
        }

        private Widget _buildReportItem(string title, int index) {
            return new GestureDetector(
                onTap: () => {
                    if (_selectedIndex != index) setState(() => { _selectedIndex = index; });
                },
                child: new Container(
                    height: 44,
                    color: CColors.White,
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: new Row(
                        children: new List<Widget> {
                            index == _selectedIndex ? _buildCheckBox() : _buildUnCheckBox(),
                            new Container(
                                margin: EdgeInsets.only(12),
                                child: new Text(
                                    title,
                                    style: CTextStyle.PLargeBody
                                )
                            )
                        }
                    )
                )
            );
        }
        
        private static Widget _buildUnCheckBox() {
            return new Container(
                width: 20,
                height: 20,
                decoration: new BoxDecoration(
                    borderRadius: BorderRadius.circular(10),
                    border: Border.all(Color.fromRGBO(216, 216, 216, 1))
                )
            );
        }
        
        private static Widget _buildCheckBox() {
            return new Container(
                width: 20,
                height: 20,
                decoration: new BoxDecoration(
                    CColors.PrimaryBlue,
                    borderRadius: BorderRadius.circular(10)
                ),
                alignment: Alignment.center,
                child: new Container(
                    width: 10,
                    height: 10,
                    decoration: new BoxDecoration(
                        CColors.White,
                        borderRadius: BorderRadius.circular(5)
                    )
                )
            );
        }
        
        private Widget _buildReportButton() {
            Widget right = new Container();
            if (widget.viewModel.loading)
                right = new CustomActivityIndicator(
                    loadingColor: LoadingColor.white,
                    size: LoadingSize.small
                );
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(top: 16),
                child: new CustomButton(
                    onPressed: () => {
                        if (widget.viewModel.loading) return;
                        widget.actionModel.startReportItem();
                        widget.actionModel.reportItem(_reportItems[_selectedIndex]);
                    },
                    padding: EdgeInsets.zero,
                    child: new Container(
                        height: 40,
                        decoration: new BoxDecoration(
                            widget.viewModel.loading
                                ? CColors.BottonActive
                                : CColors.PrimaryBlue,
                            borderRadius: BorderRadius.all(4)
                        ),
                        child: new Stack(
                            children: new List<Widget> {
                                new Align(
                                    alignment: Alignment.center,
                                    child: new Text(
                                        "举报",
                                        style: CTextStyle.PLargeMediumWhite
                                    )
                                ),
                                new Positioned(
                                    right: 24,
                                    height: 40,
                                    child: right
                                )
                            }
                        )
                    )
                )
            );
        }
    }
}