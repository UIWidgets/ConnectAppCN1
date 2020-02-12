using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            textSpans = new List<TextSpan>();
            processedLength = 0;
            source = input;
            HighlightUsingRegex(definition, input);
            if(processedLength < source.Length) {
                textSpans.Add(new TextSpan(source.Substring(processedLength)));
            }
            return new TextSpan(
                children: textSpans,
                style: definition.Style.toTextStyle()
            );
        }

        private void HighlightUsingRegex(Definition definition, string input) {
            var regexOptions = GetRegexOptions(definition);
            var evaluator = GetMatchEvaluator(definition);
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
            if(match.Index > processedLength) {
                textSpans.Add(new TextSpan(source.Substring(processedLength, match.Index - processedLength)));
            }
            if(pattern != null) {
                if(pattern is BlockPattern blockPattern) {
                    textSpans.Add(ProcessBlockPatternMatch(definition, blockPattern, match));
                }
                else if(pattern is MarkupPattern markupPattern) {
                    textSpans.Add(ProcessMarkupPatternMatch(definition, markupPattern, match));
                }
                else if(pattern is WordPattern wordPattern) {
                    textSpans.Add(ProcessWordPatternMatch(definition, wordPattern, match));
                }
            }
            processedLength = match.Index + match.Length;

            return "";
        }

        private MatchEvaluator GetMatchEvaluator(Definition definition) {
            return match => ElementMatchHandler(definition, match);
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