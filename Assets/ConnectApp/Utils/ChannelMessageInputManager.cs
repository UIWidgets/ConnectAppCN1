using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Utils {
    static class ChannelMessageMentionHelper {
        public static string parseMention(this string text, Dictionary<string, string> replacements) {
            if (text.isEmpty()) {
                return "";
            }

            if (replacements.isNullOrEmpty()) {
                return text;
            }

            var regex = new Regex(String.Join("|",replacements.Keys.Select(k => "@" + Regex.Escape(k))));
            var replaced = regex.Replace(input: text, m => {
                if (m.Value.isEmpty()) {
                    return "";
                }

                if (m.Value == "@所有人") {
                    return "@everyone";
                }

                if (replacements.ContainsKey(m.Value.Substring(1))) {
                    return "<@" + replacements[m.Value.Substring(1)] + ">";
                }

                return "";
            });
            return replaced;
        }
    }
}