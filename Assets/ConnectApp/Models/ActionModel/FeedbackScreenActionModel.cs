using System;
using ConnectApp.Constants;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class FeedbackScreenActionModel : BaseActionModel {
        public Action pushToFeedbackType;
        public Action startFeedback;
        public Action<FeedbackType> changeFeedbackType;
        public Func<string, string, string, IPromise> sendFeedbak;
    }

    public class FeedbackTypeScreenActionModel : BaseActionModel {
        public Action<FeedbackType> changeFeedbackType;
    }
}