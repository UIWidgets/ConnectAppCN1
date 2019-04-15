using System;
using ConnectApp.components;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ReportScreenActionModel : BaseActionModel {
        public Action startReportItem;
        public Func<string, IPromise> reportItem;
    }
}