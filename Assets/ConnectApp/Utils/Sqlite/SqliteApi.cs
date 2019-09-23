using System.Collections.Generic;
using ConnectApp.Models.ViewModel;
using UnityEngine;


namespace ConnectApp.Utils {
    public static class SqliteApi {
        const string DBName = "message";
        
        static SQLiteDBManager m_SqlManager;

        static SQLiteDBManager SqlManager {
            get { return m_SqlManager ?? (m_SqlManager = new SQLiteDBManager(DBName)); }
        }

        public static List<DBMessageLite> LoadMessages(string channelId, int messageIdFrom = -1) {
            return null;
        }

        public static void SaveMessages(List<ChannelMessageView> messages) {
            
        }

        public static void TestQuery() {
            var begin = Time.time;

            var results = SqlManager.QueryMessages("Channel0", 5, 10);

            foreach (var result in results) {
                Debug.Log("query result: " + result.content + " " + result.nonce + " " + result.channelId);
            }
            
            var end = Time.time;
            
            Debug.Log($"Test Save Cost Time = {end - begin}");
        }

        public static void TestSave() {
            var begin = Time.time;
            List<DBMessageLite> testMsgs = new List<DBMessageLite>();
            for (int i = 0; i < 100; i++) {
                testMsgs.Add(new DBMessageLite {
                    nonce = i,
                    messageId = $"Id{i}",
                    content = $"Test Msg{i}",
                    authorName = $"Test User{i}",
                    authorThumb = "",
                    channelId = $"Channel{i%3}"
                });
            }
            
            SqlManager.SaveMessages(testMsgs);

            var end = Time.time;

            Debug.Log($"Test Save Cost Time = {end - begin}");
        }
    }
}