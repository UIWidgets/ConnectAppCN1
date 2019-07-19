using System;
using ConnectApp.Components;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class TeamDetailScreenActionModel : BaseActionModel {
        public Action<string> pushToArticleDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action<string> pushToTeamFollower;
        public Action<string> pushToTeamMember;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
        public Action startFetchTeam;
        public Func<IPromise> fetchTeam;
        public Action startFetchTeamArticle;
        public Func<int, IPromise> fetchTeamArticle;
        public Action startFollowTeam;
        public Func<string, IPromise> followTeam;
        public Action startUnFollowTeam;
        public Func<string, IPromise> unFollowTeam;
    }
}