using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.painting;

namespace SyntaxHighlight {
    public class Definition {
        public Definition(string name, bool caseSensitive, Style style, IDictionary<string, Pattern> patterns) {
            this.Name = name;
            this.CaseSensitive = caseSensitive;
            this.Style = style;
            this.Patterns = patterns;
        }

        public string Name { get; private set; }
        public bool CaseSensitive { get; private set; }
        public Style Style { get; private set; }
        public IDictionary<string, Pattern> Patterns { get; private set; }

        public string GetRegexPattern() {
            var allPatterns = new StringBuilder();
            var blockPatterns = new StringBuilder();
            var markupPatterns = new StringBuilder();
            var wordPatterns = new StringBuilder();

            foreach(var pattern in this.Patterns.Values) {
                if(pattern is BlockPattern) {
                    if(blockPatterns.Length > 1) {
                        blockPatterns.Append("|");
                    }
                    blockPatterns.AppendFormat("(?'{0}'{1})", pattern.Name, pattern.GetRegexPattern());
                } else if(pattern is MarkupPattern) {
                    if(markupPatterns.Length > 1) {
                        markupPatterns.Append("|");
                    }
                    markupPatterns.AppendFormat("(?'{0}'{1})", pattern.Name, pattern.GetRegexPattern());
                } else if(pattern is WordPattern) {
                    if(wordPatterns.Length > 1) {
                        wordPatterns.Append("|");
                    }
                    wordPatterns.AppendFormat("(?'{0}'{1})", pattern.Name, pattern.GetRegexPattern());
                }
            }

            if(blockPatterns.Length > 0) {
                allPatterns.AppendFormat("(?'blocks'{0})+?", blockPatterns);
            }
            if(markupPatterns.Length > 0) {
                allPatterns.AppendFormat("|(?'markup'{0})+?", markupPatterns);
            }
            if(wordPatterns.Length > 0) {
                allPatterns.AppendFormat("|(?'words'{0})+?", wordPatterns);
            }

            return allPatterns.ToString();
        }

        public override string ToString() {
            return this.Name;
        }
    }

    public abstract class Pattern {
        public string Name { get; private set; }
        public Style Style { get; private set; }

        internal Pattern(string name, Style style) {
            this.Name = name;
            this.Style = style;
        }

        public abstract string GetRegexPattern();
    }

    public class ColorPair {
        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }

        public ColorPair() {
        }

        public ColorPair(Color foreColor, Color backColor) {
            this.ForeColor = foreColor;
            this.BackColor = backColor;
        }
    }

    public sealed class BlockPattern : Pattern {
        public string BeginsWith { get; private set; }
        public string EndsWith { get; private set; }
        public string EscapesWith { get; private set; }

        public BlockPattern(string name, Style style, string beginsWith, string endsWith, string escapesWith)
            : base(name, style) {
            this.BeginsWith = beginsWith;
            this.EndsWith = endsWith;
            this.EscapesWith = escapesWith;
        }

        public override string GetRegexPattern() {
            if(String.IsNullOrEmpty(this.EscapesWith)) {
                if(this.EndsWith.CompareTo(@"\n") == 0) {
                    return String.Format(@"{0}[^\n\r]*", Escape(this.BeginsWith));
                }

                return String.Format(@"{0}[\w\W\s\S]*?{1}", Escape(this.BeginsWith), Escape(this.EndsWith));
            }

            return String.Format("{0}(?>{1}.|[^{2}]|.)*?{3}", new object[] { Regex.Escape(this.BeginsWith), Regex.Escape(this.EscapesWith.Substring(0, 1)), Regex.Escape(this.EndsWith.Substring(0, 1)), Regex.Escape(this.EndsWith) });
        }

        public static string Escape(string str) {
            if(str.CompareTo(@"\n") != 0) {
                str = Regex.Escape(str);
            }

            return str;
        }
    }

    public sealed class MarkupPattern : Pattern {
        public bool HighlightAttributes { get; set; }
        public ColorPair BracketColors { get; set; }
        public ColorPair AttributeNameColors { get; set; }
        public ColorPair AttributeValueColors { get; set; }

        public MarkupPattern(string name, Style style, bool highlightAttributes, ColorPair bracketColors, ColorPair attributeNameColors, ColorPair attributeValueColors)
            : base(name, style) {
            this.HighlightAttributes = highlightAttributes;
            this.BracketColors = bracketColors;
            this.AttributeNameColors = attributeNameColors;
            this.AttributeValueColors = attributeValueColors;
        }

        public override string GetRegexPattern() {
            return @"
                (?'openTag'&lt;\??/?)
                (?'ws1'\s*?)
                (?'tagName'[\w\:]+)
                (?>
                    (?!=[\/\?]?&gt;)
                    (?'ws2'\s*?)
                    (?'attribute'
                        (?'attribName'[\w\:-]+)
                        (?'attribValue'(\s*=\s*(?:&\#39;.*?&\#39;|&quot;.*?&quot;|\w+))?)
                        # (?:(?'ws3'\s*)(?'attribSign'=)(?'ws4'\s*))
                        # (?'attribValue'(?:&\#39;.*?&\#39;|&quot;.*?&quot;|\w+))
                    )
                )*
                (?'ws5'\s*?)
                (?'closeTag'[\/\?]?&gt;)
            ";
        }
    }

    public class Style {
        public ColorPair Colors { get; private set; }
        public TextStyle Font { get; private set; }

        public Style(ColorPair colors, TextStyle font) {
            this.Colors = colors;
            this.Font = font;
        }

        public TextStyle toTextStyle() {
            return this.Font.copyWith(color: this.Colors.ForeColor, backgroundColor: this.Colors.BackColor);
        }
    }

    public sealed class WordPattern : Pattern {
        public IEnumerable<string> Words { get; private set; }

        public WordPattern(string name, Style style, IEnumerable<string> words)
            : base(name, style) {
            this.Words = words;
        }

        public override string GetRegexPattern() {
            var str = String.Empty;
            if(this.Words.Count() > 0) {
                var nonWords = this.GetNonWords();
                str = String.Format(@"(?<![\w{0}])(?=[\w{0}])({1})(?<=[\w{0}])(?![\w{0}])", nonWords, String.Join("|", this.Words.ToArray()));
            }

            return str;
        }

        private string GetNonWords() {
            var input = String.Join("", this.Words.ToArray());
            var list = new List<string>();
            foreach(var match in Regex.Matches(input, @"\W").Cast<Match>().Where(x => !list.Contains(x.Value))) {
                list.Add(match.Value);
            }

            return String.Join("", list.ToArray());
        }
    }
}