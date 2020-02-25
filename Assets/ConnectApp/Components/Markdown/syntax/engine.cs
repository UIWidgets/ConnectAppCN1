using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConnectApp.Constants;
using Unity.UIWidgets.painting;

namespace SyntaxHighlight {
    public interface IEngine {
        TextSpan Highlight(Definition definition, string input);
    }

    public class Engine : IEngine {
        private const RegexOptions DefaultRegexOptions = RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace;

        List<TextSpan> textSpans;
        int processedLength;
        string source;

        public TextSpan Highlight(Definition definition, string input) {
            if(definition == null) {
                throw new ArgumentNullException("definition");
            }

            this.textSpans = new List<TextSpan>();
            this.processedLength = 0;
            this.source = input;
            this.HighlightUsingRegex(definition, input);
            if(this.processedLength < this.source.Length) {
                this.textSpans.Add(new TextSpan(this.source.Substring(this.processedLength)));
            }
            return new TextSpan(
                children: this.textSpans,
                style: definition.Style.toTextStyle()
            );
        }

        private void HighlightUsingRegex(Definition definition, string input) {
            var regexOptions = this.GetRegexOptions(definition);
            var evaluator = this.GetMatchEvaluator(definition);
            var regexPattern = definition.GetRegexPattern();
            Regex.Replace(input, regexPattern, evaluator, regexOptions);
        }

        private RegexOptions GetRegexOptions(Definition definition) {
            if(definition.CaseSensitive) {
                return DefaultRegexOptions | RegexOptions.IgnoreCase;
            }

            return DefaultRegexOptions;
        }

        private string ElementMatchHandler(Definition definition, Match match) {
            if(definition == null) {
                throw new ArgumentNullException("definition");
            }
            if(match == null) {
                throw new ArgumentNullException("match");
            }

            var pattern = definition.Patterns.First(x => match.Groups[x.Key].Success).Value;
            if(match.Index > this.processedLength) {
                this.textSpans.Add(new TextSpan(this.source.Substring(this.processedLength, match.Index - this.processedLength)));
            }
            if(pattern != null) {
                if(pattern is BlockPattern blockPattern) {
                    this.textSpans.Add(this.ProcessBlockPatternMatch(definition, blockPattern, match));
                }
                else if(pattern is MarkupPattern markupPattern) {
                    this.textSpans.Add(this.ProcessMarkupPatternMatch(definition, markupPattern, match));
                }
                else if(pattern is WordPattern wordPattern) {
                    this.textSpans.Add(this.ProcessWordPatternMatch(definition, wordPattern, match));
                }
            }

            this.processedLength = match.Index + match.Length;

            return "";
        }

        private MatchEvaluator GetMatchEvaluator(Definition definition) {
            return match => this.ElementMatchHandler(definition, match);
        }

        protected TextSpan ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match) {
            return new TextSpan(match.Value, style: pattern.Style.toTextStyle());
        }

        protected TextSpan ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match) {
            return new TextSpan(match.Value, style: pattern.Style.toTextStyle());
        }

        protected TextSpan ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match) {
            return new TextSpan(match.Value, style: pattern.Style.toTextStyle());
        }
    }
}