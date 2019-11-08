using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Utils {
    public static class CStringUtils {
        public static string JointProjectShareLink(string projectId) {
            return $"{Config.apiAddress}/p/{projectId}?app=true";
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
            // 过滤 emoji
            name = Regex.Replace(name.Trim(), @"\p{Cs}", "?");
            string[] nameList = Regex.Split(input: name, @"\s{1,}");
            if (nameList.Length > 0) {
                for (int i = 0; i < nameList.Length; i++) {
                    if (i == 2) {
                        break;
                    }

                    var str = nameList[i].ToCharArray();
                    if (i == 0) {
                        avatarName += str.first();
                        if (!IsLetterOrNumber(str.first().ToString())) {
                            break;
                        }
                    }

                    if (i == 1) {
                        if (IsLetterOrNumber(str.first().ToString())) {
                            avatarName += str.first();
                        }
                    }
                }
            }

            avatarName = avatarName.ToUpper();
            return avatarName;
        }

        static readonly Regex LetterOrNumberRegex = new Regex(@"^[A-Za-z0-9]+$");

        public static bool IsLetterOrNumber(string str) {
            return LetterOrNumberRegex.IsMatch(input: str);
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

        public static string NotificationText(int num) {
            if (num == 0) {
                return null;
            }

            if (num < 100) {
                return $"{num}";
            }

            return "";
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