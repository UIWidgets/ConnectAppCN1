using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;

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

        public static IEnumerable<TextSpan> messageWithMarkdownToTextSpans(string content, List<User> mentions,
            bool mentionEveryone, MentionTapCallback onTap, string url = null, Action<string> onClickUrl = null) {
            var textSpans = messageToTextSpans(content, mentions, mentionEveryone, onTap);
            List<TextSpan> result = new List<TextSpan>();
            foreach (var textSpan in textSpans) {
                if (textSpan.style != CTextStyle.PLargeBody) {
                    result.Add(textSpan);
                    continue;
                }

                parseMarkdown(result, textSpan.text, url, onClickUrl);
            }
            return result;
        }

        public static void parseMarkdown(List<TextSpan> result, string text, string url = null, Action<string> onClickUrl = null) {
            if (string.IsNullOrEmpty(text)) {
                return;
            }
            
            var markdownStyle = new TextStyle();

            int curr = 0, last = 0;
            while (curr < text.Length) {
                if (url != null && text[curr] == url[0] && text.Length - curr >= url.Length &&
                    text.Substring(curr, url.Length) == url) {
                    if (curr > last) {
                        result.Add(new TextSpan(text.Substring(last, curr - last),
                            style: CTextStyle.PLargeBody.merge(markdownStyle)));
                    }
                    result.Add(new TextSpan(url, style: CTextStyle.PLargeBlue,
                        recognizer: onClickUrl == null ? null : new TapGestureRecognizer {
                            onTap = () => onClickUrl(url)
                        }));
                    curr += url.Length;
                    last = curr;
                }
                else if (text.Length - curr >= 2 && text[curr] == '*' && text[curr + 1] == '*') {
                    if (curr > last) {
                        result.Add(new TextSpan(text.Substring(last, curr - last),
                            style: CTextStyle.PLargeBody.merge(markdownStyle)));
                    }

                    markdownStyle = markdownStyle.copyWith(
                        fontWeight: markdownStyle.fontWeight == FontWeight.bold
                            ? FontWeight.normal
                            : FontWeight.bold);

                    curr += 2;
                    last = curr;
                }
                else if (text.Length - curr >= 2 && text[curr] == '~' && text[curr + 1] == '~') {
                    if (curr > last) {
                        result.Add(new TextSpan(text.Substring(last, curr - last),
                            style: CTextStyle.PLargeBody.merge(markdownStyle)));
                    }

                    markdownStyle = markdownStyle.copyWith(
                        decoration: markdownStyle.decoration?.contains(TextDecoration.lineThrough) ?? false
                            ? TextDecoration.none
                            : TextDecoration.lineThrough);

                    curr += 2;
                    last = curr;
                }
                else if (text.Length - curr >= 1 && text[curr] == '_') {
                    if (curr > last) {
                        result.Add(new TextSpan(text.Substring(last, curr - last),
                            style: CTextStyle.PLargeBody.merge(markdownStyle)));
                    }

                    markdownStyle = markdownStyle.copyWith(
                        fontStyle: markdownStyle.fontStyle == FontStyle.italic
                            ? FontStyle.normal
                            : FontStyle.italic);

                    curr += 1;
                    last = curr;
                }
                else if (text.Length - curr >= 3 && text[curr] == '`' && text[curr + 1] == '`' && text[curr + 2] == '`') {
                    if (curr > last) {
                        result.Add(new TextSpan(text.Substring(last, curr - last),
                            style: CTextStyle.PLargeBody.merge(markdownStyle)));
                    }

                    markdownStyle = markdownStyle.copyWith(
                        fontFamily: markdownStyle.fontFamily?.StartsWith("Roboto") ?? true
                            ? "Courier"
                            : "Roboto-Regular");

                    curr += 3;
                    last = curr;
                }
                else if (text.Length - curr >= 1 && text[curr] == '`') {
                    if (curr > last) {
                        result.Add(new TextSpan(text.Substring(last, curr - last),
                            style: CTextStyle.PLargeBody.merge(markdownStyle)));
                    }

                    markdownStyle = markdownStyle.copyWith(
                        fontFamily: markdownStyle.fontFamily?.StartsWith("Roboto") ?? true
                            ? "Courier"
                            : "Roboto-Regular");

                    curr += 1;
                    last = curr;
                }
                else {
                    curr++;
                }
            }

            if (last < curr) {
                result.Add(new TextSpan(text.Substring(last, curr - last),
                    style: CTextStyle.PLargeBody.merge(markdownStyle)));
            }
        }
    }
}