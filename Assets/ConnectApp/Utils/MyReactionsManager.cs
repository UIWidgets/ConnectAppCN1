using System.Collections.Generic;

namespace ConnectApp.Utils {
    public static class MyReactionsManager {
        static readonly Dictionary<string, Dictionary<string, int>> myReactionsDict = new Dictionary<string, Dictionary<string, int>>();

        public static void initialMyReactions(string messageId, Dictionary<string, Dictionary<string, int>> allUserReactionsDict) {
            if (myReactionsDict.ContainsKey(key: messageId)) {
                return;
            }
            var myUserId = UserInfoManager.getUserInfo().userId;
            if (allUserReactionsDict.ContainsKey(key: myUserId)) {
                myReactionsDict[key: messageId] = allUserReactionsDict[myUserId];
            }
        }
        
        public static void updateMyReaction(string messageId, string type) {
            if (!myReactionsDict.ContainsKey(key: messageId)) {
                myReactionsDict.Add(key: messageId, new Dictionary<string, int>());
            }
            if (!myReactionsDict[key: messageId].ContainsKey(key: type)) {
                myReactionsDict[key: messageId].Add(key: type, 0);
            }
            myReactionsDict[key: messageId][key: type] = 1 - myReactionsDict[key: messageId][key: type];
        }

        public static Dictionary<string, int> getMessageReactions(string messageId) {
            return !myReactionsDict.ContainsKey(key: messageId) ? null : myReactionsDict[key: messageId];
        }

        public static void clearData() {
            myReactionsDict.Clear();
        }
    }
}