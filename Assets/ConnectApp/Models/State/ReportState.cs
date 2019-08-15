using System;
using ConnectApp.Constants;

namespace ConnectApp.Models.State {
    [Serializable]
    public class ReportState {
        public bool loading;
    }

    public class FeedbackState {
        public FeedbackType feedbackType;
        public bool loading;
    }
}