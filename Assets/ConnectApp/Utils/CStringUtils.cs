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

        public static bool IsLetterOrNumber(string str) {
            Regex reg1 = new Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str);
        }

        public static string CreateMiniPath(string id, string title) {
            if (id.isNotEmpty() && title.isNotEmpty()) {
                return $"pages/Home/Home?type=toDetail&app=true&id={id}&title={title}";
            }

            return "";
        }
    }
}