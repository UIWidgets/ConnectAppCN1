using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class SettingScreenActionModel : BaseActionModel {
        public Action<string> openUrl;
        public Action pushToAboutUs;
        public Action clearCache;
        public Action logout;
        public Func<IPromise> fetchReviewUrl;
    }
}