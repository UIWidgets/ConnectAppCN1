using System;
using ConnectApp.Models.State;
using ConnectApp.screens;

namespace ConnectApp.Models.ActionModel {
    public class HistoryScreenActionModel : BaseActionModel {
        public Action<string> pushToArticleDetail;
        public Action<string, EventType> pushToEventDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action<string> deleteArticleHistory;
        public Action<string> deleteEventHistory;
        public Action deleteAllArticleHistory;
        public Action deleteAllEventHistory;
    }
}