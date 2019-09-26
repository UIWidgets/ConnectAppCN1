using System.Text.RegularExpressions;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Utils {
    public static class CStringUtils {
        public static string JointProjectShareLink(string projectId) {
            return $"{Config.apiAddress}/p/{projectId}?app=true";
        }

        public static string CountToString(int count, string placeholder = "") {
            var countString = "";
            if (count == 0) {
                countString = placeholder;
            }

            if (count > 0 && count < 1000) {
                countString = count.ToString();
            }
            else if (count >= 1000 && count <= 10000) {
                countString = $"{count / 1000f:f1}k";
            }
            else if (count > 10000) {
                countString = "10k+";
            }

            return countString;
        }

        public static string genAvatarName(string name) {
            var avatarName = "";
            name = name.Trim();
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
    }
}