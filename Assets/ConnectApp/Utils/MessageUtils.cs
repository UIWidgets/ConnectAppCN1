using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.utils {
    public static class MessageUtils {
        public static string AnalyzeMessage(string content, List<User> mentions, bool mentionEveryone) {
            if (content == null || content.Length <= 0) {
                return "";
            }

            var parsingContent = content;
            if (mentionEveryone) {
                parsingContent = parsingContent.Replace("@everyone", "@所有人");
            }

            if (mentions != null && mentions.Count > 0) {
                mentions.ForEach(mention => {
                    var mentionId = mention.id;
                    var mentionFullName = mention.fullName;
                    parsingContent = parsingContent.Replace($"<@{mentionId}>", $"@{mentionFullName}");
                });
            }

            return parsingContent;
        }
    }
}