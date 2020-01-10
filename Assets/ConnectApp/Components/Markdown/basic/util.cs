using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace markdown {
    public static class Utils {
        public static string escapeHtml(string html) {
            //TODO: add content
            return html;
        }

// Escape the contents of [value], so that it may be used as an HTML attribute.

// Based on http://spec.commonmark.org/0.28/#backslash-escapes.
        public static string escapeAttribute(string value) {
            var result = new StringBuilder();
            int ch;
            for (var i = 0; i < value.codeUnits().Count; i++) {
                ch = value.codeUnitAt(i);
                if (ch == CharCode.backslash) {
                    i++;
                    if (i == value.codeUnits().Count) {
                        result.writeCharCode(ch);
                        break;
                    }

                    ch = value.codeUnitAt(i);
                    switch (ch) {
                        case CharCode.quote: // "
                            result.write('"');
                            break;
                        case CharCode.exclamation: // !
                        case CharCode.hash: // #
                        case CharCode.dollar: // $
                        case CharCode.percent: // %
                        case CharCode.ampersand: // &
                        case CharCode.apostrophe: // '.
                        case CharCode.lparen: // (
                        case CharCode.rparen: // )
                        case CharCode.asterisk: // * 
                        case CharCode.plus: // +
                        case CharCode.comma: // ,
                        case CharCode.dash: // -
                        case CharCode.dot: // .
                        case CharCode.slash: // \
                        case CharCode.colon: // :
                        case CharCode.semicolon: // ;
                        case CharCode.lt: // <
                        case CharCode.equal: // =
                        case CharCode.gt: // >
                        case CharCode.question: // ?
                        case CharCode.at: // @
                        case CharCode.lbracket: // [
                        case CharCode.backslash: // \
                        case CharCode.rbracket: // ]
                        case CharCode.caret: // ^
                        case CharCode.underscore: // _
                        case CharCode.backquote: // `
                        case CharCode.lbrace: // {
                        case CharCode.bar: // |
                        case CharCode.rbrace: // }
                        case CharCode.tilde: // ~
                            result.writeCharCode(ch);
                            break;
                        default:
                            result.write('\\');
                            result.writeCharCode(ch);
                            break;
                    }
                }
                else if (ch == CharCode.quote) {
                    result.write('"');
                }
                else {
                    result.writeCharCode(ch);
                }
            }

            return result.ToString();
        }

        public static List<int> codeUnits(this string input) {
            return input.Select((t, i) => char.ConvertToUtf32(input, i)).ToList();
        }

        public static int codeUnitAt(this string input, int i) {
            return char.ConvertToUtf32(input, i);
        }

        public static void writeCharCode(this StringBuilder input, int i) {
            input.Append((char) i);
        }

        public static void write(this StringBuilder input, char i) {
            input.Append(i);
        }


        public static bool startsWith(this string input, Regex pattern) {
            return pattern.Match(input).Success;
        }

        public static bool startsWith(this string input, string str) {
            return input.StartsWith(str);
        }

        public static string replaceFirst(this string input, Regex from, string to, int startIndex = 0) {
            return from.Replace(input, to, 1, startIndex);
        }

        public static string replaceFirst(this string input, Regex from, char to, int startIndex = 0) {
            return from.Replace(input, to.ToString(), 1, startIndex);
        }

        public static string replaceFirst(this string input, string from, string to, int startIndex = 0) {
            return new Regex(from).Replace(input, to, 1, startIndex);
        }

        public static string replaceAll(this string input, string from, string to, int startIndex = 0) {
            return new Regex(from).Replace(input, to, int.MaxValue, startIndex);
        }

        public static string replaceAll(this string input, string from, char to, int startIndex = 0) {
            return new Regex(from).Replace(input, to.ToString(), int.MaxValue, startIndex);
        }

        public static string replaceAll(this string input, Regex from, char to, int startIndex = 0) {
            return from.Replace(input, to.ToString(), int.MaxValue, startIndex);
        }

        public static string[] split(this string input, Regex regex, int count = int.MaxValue, int startat = 0) {
            return regex.Split(input, count, startat);
        }

        public static string substring(this string input, int start, int end = 0) {
            var length = end - start;
            if (length > 0 && end < input.Length) {
                return input.Substring(start, end - start);
            }

            if (start < 0 || start >= input.Length) {
                return string.Empty;
            }

            return input.Substring(start);
        }

        public static string join<T>(this IEnumerable<T> input, string sperate) {
            return string.Join(sperate, input);
        }

        public static List<T> remove<T>(this List<T> input, Func<T, bool> predicate) {
            var removals = input.Where(predicate).ToArray();
            foreach (var removal in removals) {
                input.Remove(removal);
            }

            return input;
        }

        public static string join<T>(this IEnumerable<T> input, char cha) {
            return string.Join(cha.ToString(), input);
        }


        public static IEnumerable<string> getRange(this List<string> input, int start, int end) {
            if (end - start < input.Count - 1) {
                return input.GetRange(start, end - start + 1);
            }
            else {
                return input.GetRange(start, end - start);
            }
        }

        public static List<T> sublist<T>(this List<T> input, int start, int end = -1) {
            if (end == -1) {
                end = input.Count - 1;
            }

            return input.GetRange(start, end - start + 1);
        }

        public static List<T> reversed<T>(this List<T> input) {
            var arrar = input.ToArray().Reverse().ToList();
            return arrar;
        }


        public static void removeRange<T>(this List<T> input, int start, int count) {
            if (start < 0 || start > input.Count) {
                Debug.LogWarning("out of range");
                return;
            }

            if (start + count > input.Count) {
                count = input.Count - start;
            }

            input.RemoveRange(start, count);
        }

        public static Match matchAsPrefix(this Regex pattern, string content, int start = 0) {
            var match = pattern.Match(content, start);
            if (match.Success && match.Index != start) {
                return Match.Empty;
            }

            return match;
        }

        public static bool hasMatch(this Regex pattern, string content) {
            if (content == null) {
                return false;
            }

            return pattern.Match(content).Success;
        }

        public static bool endsWith(this string input, char cha) {
            return input.EndsWith(cha.ToString());
        }
    }
}