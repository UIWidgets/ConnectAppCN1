using System;
using ConnectApp.Components;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class GameDetailScreenActionModel : BaseActionModel {
        public Action startFetchGameDetail;
        public Action<string> copyText;
        public Func<IPromise> fetchGameDetail;
        public Func<ShareType, string, string, string, string, string, IPromise> shareToWechat;
    }
}