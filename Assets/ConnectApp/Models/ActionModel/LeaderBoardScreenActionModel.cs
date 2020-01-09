using System;

namespace ConnectApp.Models.ActionModel {
    public class LeaderBoardScreenActionModel : BaseActionModel {
        public Action pushToAlbumAction;
        public Action<string> pushToUserDetail;
    }
}