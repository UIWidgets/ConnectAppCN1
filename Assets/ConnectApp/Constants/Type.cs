using System.Collections.Generic;

namespace ConnectApp.Constants {

    public class FeedbackType {
        FeedbackType(string value, string description) {
            this.value = value;
            this.description = description;
        }

        public string value { get; }
        public string description { get; }

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

    public class ReactionType {
        ReactionType(string value, string gifImagePath, string jpgImagePath, string description) {
            this.value = value;
            this.gifImagePath = gifImagePath;
            this.jpgImagePath = jpgImagePath;
            this.description = description;
        }

        public string value { get; }
        public string gifImagePath { get; }
        public string jpgImagePath { get; }
        public string description { get; }

        public static List<ReactionType> typesList {
            get {
                return new List<ReactionType> {
                    Thumb,
                    Oppose,
                    Coverface,
                    Heartbeat,
                    Doubt
                };
            }
        }

        public static ReactionType Thumb {
            get { return new ReactionType("thumb", "image/reaction-thumb.gif", "image/reaction-thumb", "喜欢"); }
        }

        public static ReactionType Oppose {
            get { return new ReactionType("oppose", "image/reaction-oppose.gif", "image/reaction-oppose", "反对"); }
        }

        public static ReactionType Coverface {
            get {
                return new ReactionType("coverface", "image/reaction-coverface.gif", "image/reaction-coverface",
                    "捂脸");
            }
        }

        public static ReactionType Heartbeat {
            get {
                return new ReactionType("heartbeat", "image/reaction-heartbeat.gif", "image/reaction-heartbeat", "心动");
            }
        }

        public static ReactionType Doubt {
            get { return new ReactionType("doubt", "image/reaction-doubt.gif", "image/reaction-doubt", "疑惑"); }
        }
    }
}