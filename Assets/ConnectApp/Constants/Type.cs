using System.Collections.Generic;
using System.ComponentModel;

namespace ConnectApp.Constants {

    public class FeedbackType {
        FeedbackType(string value, string description) {
            this.Value = value;
            this.Description = description;
        }

        public string Value { get; }
        public string Description { get; }

        public static List<FeedbackType> typesList {
            get {
                return new List<FeedbackType> {
                    Advice,
                    Bug,
                    Other
                };
            }
        }

        public static FeedbackType Advice { get { return new FeedbackType("advice", "意见建议"); } }
        public static FeedbackType Bug { get { return new FeedbackType("bug", "BUG 相关"); } }
        public static FeedbackType Other { get { return new FeedbackType("other", "其他"); } }
    }
}