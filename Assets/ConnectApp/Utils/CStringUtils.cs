using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public static class CStringUtils {
        public static string JointProjectShareLink(string projectId) {
            return $"{Config.unity_cn_url}/projects/{projectId}?app=true";
        }
        
        public static string JointEventShareLink(string eventId) {
            return $"{Config.unity_com_url}/events/{eventId}";
        }
        public static string JointTinyGameShareLink(string gameId) {
            return $"{Config.unity_cn_url}/tinyGame/{gameId}/share";
        }

        public static string CountToString(int count, string placeholder = "") {
            if (count > 0 && count < 1000) {
                return count.ToString();
            }

            if (count >= 1000 && count <= 10000) {
                return $"{count / 1000f:f1}k";
            }

            if (count > 10000) {
                return "10k+";
            }

            return placeholder;
        }

        public static string genAvatarName(string name) {
            var avatarName = "";
            string[] nameList = Regex.Split(input: name.Trim(), @"\s{1,}");
            if (nameList.Length > 0) {
                for (int i = 0; i < 2 && i < nameList.Length; i++) {
                    var str = nameList[i].ToCharArray();
                    if (i == 0) {
                        avatarName += str.first();
                        if (char.IsHighSurrogate(str[0])) {
                            if (str.Length > 1 && char.IsLowSurrogate(str[1])) {
                                avatarName += str[1];
                                break;
                            }

                            // There is a single high surrogate char, which will cause crash.
                            // This should never happen.
                            avatarName = $"{(char) EmojiUtils.emptyEmojiCode}";
                            break;
                        }

                        if (!str.first().ToString().IsLetterOrNumber()) {
                            break;
                        }
                    }

                    if (i == 1) {
                        if (str.first().ToString().IsLetterOrNumber()) {
                            avatarName += str.first();
                        }
                    }
                }
            }

            avatarName = avatarName.ToUpper();
            return avatarName;
        }

        public static string CreateMiniPath(string id, string title) {
            if (id.isNotEmpty() && title.isNotEmpty()) {
                return $"pages/Home/Home?type=toDetail&app=true&id={id}&title={title}";
            }

            return "";
        }

        public static string FileSize(long bytes) {
            if (bytes < 1024) {
                return $"{bytes}B";
            }

            if (bytes < 1024 * 1024) {
                return $"{bytes / 1024.0f:F}K";
            }

            if (bytes < 1024 * 1024 * 1024) {
                return $"{bytes / (1024.0f * 1024):F}M";
            }

            return $"{bytes / (1024.0f * 1024 * 1024):F}G";
        }

        public static long hexToLong(this string number, long defaultValue = -1) {
            if (number.isEmpty()) {
                return defaultValue;
            }

            try {
                return Convert.ToInt64(value: number, 16);
            }
            catch (Exception e) {
                Debuger.LogWarning($"Error in converting {number}: {e}");
                return defaultValue;
            }
        }

        public static string httpToHttps(this string url) {
            if (url.isEmpty()) {
                return "";
            }

            return url.Contains("http://")
                ? url.Replace("http://", "https://")
                : url;
        }

        public static bool isUrl(this string url) {
            if (url.isEmpty()) {
                return false;
            }

            return url.StartsWith("http://") || url.StartsWith("https://");
        }

        static readonly Regex LetterOrNumberRegex = new Regex(@"^[A-Za-z0-9]+$");

        public static bool IsLetterOrNumber(this string str) {
            return LetterOrNumberRegex.IsMatch(input: str);
        }

        static readonly Regex LowercaseLetterOrNumberRegex = new Regex(@"^[a-z0-9]+$");

        static bool IsLowercaseLetterOrNumber(this string str) {
            return LowercaseLetterOrNumberRegex.IsMatch(input: str);
        }

        public static bool isSlug(this string str) {
            if (str.isEmpty()) {
                return false;
            }

            return str.Length != 24 || !str.IsLowercaseLetterOrNumber();
        }
    }

    public static class CCollectionUtils {
        public static bool isNullOrEmpty<T>(this ICollection<T> it) {
            return it == null || it.Count == 0;
        }

        public static bool isNotNullAndEmpty<T>(this ICollection<T> it) {
            return it != null && it.Count > 0;
        }

        public static bool isNullOrEmpty<T>(this Queue<T> it) {
            return it == null || it.Count == 0;
        }

        public static bool isNotNullAndEmpty<T>(this Queue<T> it) {
            return it != null && it.Count > 0;
        }

        public static bool isNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> it) {
            return it == null || it.Count == 0;
        }

        public static bool isNotNullAndEmpty<TKey, TValue>(this IDictionary<TKey, TValue> it) {
            return it != null && it.Count > 0;
        }
    }
}