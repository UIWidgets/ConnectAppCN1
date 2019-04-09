using System;
using ConnectApp.models;

namespace ConnectApp.Models.ActionModel {
    public class HistoryScreenActionModel {
        public Action mainRouterPop;
        public Action<string> pushToArticleDetail;
        public Action<string, EventType> pushToEventDetail;
        public Action getArticleHistory;
        public Action getEventHistory;
        public Action<string> deleteArticleHistory;
        public Action<string> deleteEventHistory;
    }
}