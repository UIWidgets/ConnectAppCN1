using ConnectApp.Constants;

namespace ConnectApp.Utils {
    public static class CStringUtils {
        public static string JointProjectShareLink(string projectId) {
            return $"{Config.apiAddress}/p/{projectId}?app=true";
        }

        public static string likeCountToString(int likeCount) {
            var likeCountString = "";
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
    }
}