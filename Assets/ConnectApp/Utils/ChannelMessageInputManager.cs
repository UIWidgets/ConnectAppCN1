using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConnectApp.Utils {

    static class ChannelMessageMentionHelper {
        
        public static string parseMention(string text, Dictionary<string, string> replacements) {
            var regex = new Regex(String.Join("|",replacements.Keys.Select(k => "@" + Regex.Escape(k))));
            var replaced = regex.Replace(text,m => "<@" + replacements[m.Value.Substring(1)] + ">");
            return replaced;
        }
    }
}