using ConnectApp.Constants;

namespace ConnectApp.Utils {
    public static class CStringUtils {
        public static string JointProjectShareLink(string projectId) {
            return $"{Config.apiAddress}/connectmobile/projects/{projectId}";
        }
    }
}