using System.Collections.Generic;
using ConnectApp.canvas;
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
                    reportType = reportType
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
                });
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
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    child: new Container(
                        child: new Column(
                            children: new List<Widget> {
                                _buildNavigationBar(context),
                                _buildContent()
                            }
                        )
                    )
                )
            );
        }
        
        private Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(CColors.White),
                width: MediaQuery.of(context).size.width,
                height: 140,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            child: new CustomButton(
                                padding: EdgeInsets.only(16, 10, 16),
                                onPressed: () => widget.actionModel.mainRouterPop(),
                                child: new Icon(
                                    Icons.arrow_back,
                                    size: 24,
                                    color: CColors.icon3
                                )
                            ),
                            height: 44
                        ),
                        new Container(
                            margin: EdgeInsets.only(16, bottom: 12),
                            child: new Text(
                                "举报",
                                style: CTextStyle.H2
                            )
                        )
                    }
                )
            );
        }

        private Widget _buildContent() {
            var reportItems = new List<string> {
                "垃圾信息",
                "涉嫌侵权",
                "不友善行为",
                "有害信息"
            };
            var widgets = new List<Widget>();
            reportItems.ForEach(item => {
                var index = reportItems.IndexOf(item);
                var widget = _buildReportItem(item, index);
                widgets.Add(widget);
            });
            return new Container(
                child: new Column(
                    children: widgets
                )
            );
        }

        private Widget _buildReportItem(string title, int index) {
            return new Container(
                height: 60,
                color: CColors.White,
                child: new Row(
                    children: new List<Widget> {
                        new Text(title)
                    }
                )
            );
        }
    }
}