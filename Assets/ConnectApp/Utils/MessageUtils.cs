using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;

namespace ConnectApp.Utils {
    public static class MessageUtils {
        public delegate void MentionTapCallback(string userId);

        public static string AnalyzeMessage(string content, List<User> mentions, bool mentionEveryone) {
            if (content.isEmpty()) {
                return "";
            }

            var parsingContent = content;
            if (mentionEveryone) {
                parsingContent = parsingContent.Replace("@everyone", "@所有人");
            }

            if (parsingContent.Contains("<@") && mentions != null && mentions.Count > 0) {
                mentions.ForEach(mention => {
                    var mentionId = mention.id;
                    var mentionFullName = mention.fullName;
                    parsingContent = parsingContent.Replace($"<@{mentionId}>", $"@{mentionFullName}");
                });
            }

            return parsingContent;
        }
                
        public static IEnumerable<TextSpan> messageToTextSpans(string content, List<User> mentions, bool mentionEveryone, MentionTapCallback onTap) {
            var textSpans = new List<TextSpan>();

            if (content.isEmpty()) {
                return textSpans;
            }

            var parsingContent = content;
            if (mentionEveryone) {
                parsingContent = parsingContent.Replace("@everyone", "@所有人");
            }

            if (parsingContent.Contains("<@") && mentions != null && mentions.Count > 0) {
                int startIndex = 0;
                mentions.ForEach(mention => {
                    var mentionId = mention.id;
                    var mentionFullName = mention.fullName;
                    if (parsingContent.Contains($"<@{mentionId}>")) {
                        parsingContent = parsingContent.Replace($"<@{mentionId}>", $"@{mentionFullName}");
                    }
                    var index = parsingContent.IndexOf($"@{mentionFullName}", startIndex: startIndex);
                    var length = $"@{mentionFullName}".Length;
                    textSpans.Add(new TextSpan(
                        parsingContent.Substring(startIndex: startIndex, index - startIndex),
                        style: CTextStyle.PLargeBody
                    ));
                    textSpans.Add(new TextSpan(
                        parsingContent.Substring(startIndex: index, length: length),
                        style: CTextStyle.PLargeBlue,
                        recognizer: new TapGestureRecognizer {
                            onTap = () => onTap(userId: mentionId)
                        }
                    ));
                    startIndex = index + length;
                });
                textSpans.Add(new TextSpan(
                    parsingContent.Substring(startIndex: startIndex, parsingContent.Length - startIndex),
                    style: CTextStyle.PLargeBody
                ));
            }
            else {
                textSpans.Add(new TextSpan(
                    text: parsingContent,
                    style: CTextStyle.PLargeBody
                ));
            }

            return textSpans;
        }
    }
}