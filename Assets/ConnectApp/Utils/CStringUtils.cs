using System;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Utils {
    public static class CStringUtils {
        public static string JointProjectShareLink(string projectId) {
            return $"{Config.apiAddress}/p/{projectId}?app=true";
        }

        public static string likeCountToString(int likeCount) {
            var likeCountString = "";
            if (likeCount == 0) {
                likeCountString = "点赞";
            }
            if (likeCount > 0 && likeCount < 1000) {
                likeCountString = likeCount.ToString();
            }
            else if (likeCount >= 1000 && likeCount <= 10000) {
                likeCountString = $"{likeCount / 1000f:f1}k";
            }
            else if (likeCount > 10000) {
                likeCountString = "10k+";
            }

            return likeCountString;
        }

        public static string genAvatarName(string name) {
            var avatarName = "";
            name = name.Trim();
            string[] nameList = name.Split(' ');
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
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str);
        }
    }
}