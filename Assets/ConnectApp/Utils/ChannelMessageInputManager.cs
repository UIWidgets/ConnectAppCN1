using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConnectApp.Utils {

    static class ChannelMessageMentionHelper {
        
        public static string parseMention(string text, Dictionary<string, string> replacements) {
            var pattern = String.Join("|", replacements.Keys.Select(k => "@" + Regex.Escape(k)));
            if (string.IsNullOrEmpty(pattern)) {
                return text;
            }
            var regex = new Regex(pattern);
            var replaced = regex.Replace(text,m => {
                return "<@" + replacements[m.Value.Substring(1)] + ">";
            });
            return replaced;
        }
    }
}