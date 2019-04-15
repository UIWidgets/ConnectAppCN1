using System;
using ConnectApp.models;
using ConnectApp.screens;

namespace ConnectApp.Models.ActionModel {
    public class HistoryScreenActionModel : BaseActionModel {
        public Action<string> pushToArticleDetail;
        public Action<string, EventType> pushToEventDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> deleteArticleHistory;
        public Action<string> deleteEventHistory;
    }
}