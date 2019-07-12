using System;
using ConnectApp.Components;
using ConnectApp.Models.State;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class HistoryScreenActionModel : BaseActionModel {
        public Action<string> pushToArticleDetail;
        public Action<string, EventType> pushToEventDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
        public Action<string> deleteArticleHistory;
        public Action<string> deleteEventHistory;
        public Action deleteAllArticleHistory;
        public Action deleteAllEventHistory;
    }
}