using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.redux;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public delegate void MentionTapCallback(string userId);

    public static class MessageUtils {
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

        public static string truncateMessage(string content, int head = 1000, int tail = 1000,
            string connector = "...") {
            if (content.isEmpty() || content.Length <= head + tail + connector.Length) {
                return content;
            }

            return content.Substring(0, head > 0 && char.IsHighSurrogate(content[head - 1])
                       ? head - 1
                       : head) +
                   connector +
                   content.Substring(tail > 0 && char.IsLowSurrogate(content[content.Length - tail])
                       ? content.Length - tail + 1
                       : content.Length - tail);
        }

        public static IEnumerable<TextSpan> messageToTextSpans(string content, List<User> mentions,
            bool mentionEveryone, MentionTapCallback onTap) {
            var userDict = StoreProvider.store.getState().userState.userDict;

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
                    var mentionFullName = mention.fullName.isNotEmpty() ? mention.fullName :
                        userDict.ContainsKey(mention.id) ? userDict[mention.id].fullName : "";
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
                        recognizer: onTap == null
                            ? null
                            : new TapGestureRecognizer {
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

        public static IEnumerable<TextSpan> messageWithMarkdownToTextSpans(
            string content,
            List<User> mentions,
            bool mentionEveryone,
            MentionTapCallback onTap,
            TextStyle bodyStyle = null,
            TextStyle linkStyle = null,
            string url = null,
            Action<string> onClickUrl = null) {
            bodyStyle = bodyStyle ?? CTextStyle.PLargeBody;
            linkStyle = linkStyle ?? CTextStyle.PLargeBlue;
            var textSpans = messageToTextSpans(content, mentions, mentionEveryone, onTap);
            List<TextSpan> result = new List<TextSpan>();
            foreach (var textSpan in textSpans) {
                if (textSpan.recognizer != null) {
                    result.Add(new TextSpan(
                        text: textSpan.text,
                        style: textSpan.style.copyWith(
                            fontSize: bodyStyle.fontSize,
                            color: bodyStyle.color),
                        recognizer: textSpan.recognizer));
                    continue;
                }

                stripMarkdown(result, textSpan.text, bodyStyle, linkStyle, url, onClickUrl);
            }

            return result;
        }

        public static void stripMarkdown(
            List<TextSpan> result,
            string text,
            TextStyle bodyStyle,
            TextStyle linkStyle,
            string url = null,
            Action<string> onClickUrl = null) {
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrEmpty(text)) {
                result.Add(new TextSpan(stripPairs(text), style: bodyStyle));
                return;
            }

            var index = text.IndexOf(url, StringComparison.Ordinal);
            if (index >= 0) {
                if (index > 0) {
                    result.Add(new TextSpan(
                        stripPairs(text.Substring(0, index)),
                        style: bodyStyle));
                }

                result.Add(new TextSpan(url, style: linkStyle,
                    recognizer: onClickUrl == null
                        ? null
                        : new TapGestureRecognizer {
                            onTap = () => onClickUrl(url)
                        }));
                if (index + url.Length < text.Length) {
                    result.Add(new TextSpan(
                        stripPairs(text.Substring(index + url.Length)),
                        style: bodyStyle));
                }
            }
        }

        public static string stripPairs(string text) {
            text = stripPair(text, "**");
            text = stripPair(text, "~~");
            text = stripPair(text, "_");
            text = stripPair(text, "```");
            text = stripPair(text, "`");
            return text;
        }

        public static string stripPair(string text, string symbol) {
            while (true) {
                var first = text.IndexOf(symbol, StringComparison.Ordinal);
                var last = text.LastIndexOf(symbol, StringComparison.Ordinal);
                if (first >= 0 && last >= 0 && first + symbol.Length <= last) {
                    text = (first > 0 ? text.Substring(0, first) : "") +
                           (first + symbol.Length < last
                               ? text.Substring(first + symbol.Length, last - first - symbol.Length)
                               : "") +
                           (last + symbol.Length < text.Length ? text.Substring(last + symbol.Length) : "");
                    continue;
                }

                return text;
            }
        }

        public static void parseMarkdown(List<TextSpan> result, string text, string url = null,
            Action<string> onClickUrl = null) {
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

                    result.Add(new TextSpan(url, style: CTextStyle.PLargeBlue.merge(markdownStyle),
                        recognizer: onClickUrl == null
                            ? null
                            : new TapGestureRecognizer {
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
                else if (text.Length - curr >= 3 && text[curr] == '`' && text[curr + 1] == '`' &&
                         text[curr + 2] == '`') {
                    int end = text.IndexOf("```", curr + 3, StringComparison.Ordinal);
                    if (end >= curr + 3) {
                        if (curr > last) {
                            result.Add(new TextSpan(text.Substring(last, curr - last),
                                style: CTextStyle.PLargeBody.merge(markdownStyle)));
                        }

                        var start = curr + 3;
                        var length = end - start;
                        if (text[curr + 3] == '\n') {
                            start += 1;
                            length -= 1;
                        }

                        if (text[end - 1] == '\n') {
                            length -= 1;
                        }

                        result.Add(new TextSpan((curr > 0 && text[curr - 1] != '\n' ? "\n" : "") +
                                                text.Substring(start, length) +
                                                (end + 3 < text.Length && text[end + 3] != '\n' ? "\n" : ""),
                            style: CTextStyle.PLargeBody.merge(markdownStyle.copyWith(fontFamily: "Menlo"))));

                        curr = end + 3;
                        last = curr;
                    }
                    else {
                        curr += 3;
                    }
                }
                else if (text.Length - curr >= 1 && text[curr] == '`') {
                    if (curr > last) {
                        result.Add(new TextSpan(text.Substring(last, curr - last),
                            style: CTextStyle.PLargeBody.merge(markdownStyle)));
                    }

                    markdownStyle = markdownStyle.copyWith(
                        fontFamily: markdownStyle.fontFamily?.StartsWith("Roboto") ?? true
                            ? "Menlo"
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