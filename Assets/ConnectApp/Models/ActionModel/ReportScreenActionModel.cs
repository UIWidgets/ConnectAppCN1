using System;
using ConnectApp.components;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ReportScreenActionModel {
        public Action mainRouterPop;
        public Action startReportItem;
        public Func<string, IPromise> reportItem;
    }
}