using System;
using ConnectApp.Constants;

namespace ConnectApp.Models.ActionModel {
    public class FeedbackScreenActionModel : BaseActionModel {
        public Action pushToFeedbackType;
        public Action<FeedbackType> restoreFeedbackType;
    }
    
    public class FeedbackTypeScreenActionModel : BaseActionModel {
        public Action<FeedbackType> changeFeedbackType;
    }
}