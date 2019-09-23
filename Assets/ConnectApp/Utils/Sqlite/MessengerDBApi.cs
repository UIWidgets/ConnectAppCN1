using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;
using ConnectApp.Models.ViewModel;


namespace ConnectApp.Utils {
    public static class MessengerDBApi {
        const string DBName = "messenger";
        
        static SQLiteDBManager m_SqlManager;

        static SQLiteDBManager SqlManager {
            get { return m_SqlManager ?? (m_SqlManager = new SQLiteDBManager(DBName)); }
        }

        public static List<ChannelMessageView> SyncLoadMessages(string channelId, long messageNonceFrom = -1, int messageCount = 10) {
            var msgLites = SqlManager.QueryMessages(channelId, messageNonceFrom, messageCount);

            var msgs = new List<ChannelMessageView>();
            foreach (var msgLite in msgLites) {
                msgs.Add(new ChannelMessageView {
                    id = msgLite.messageId,
                    content = msgLite.content,
                    author = new User {
                        fullName = msgLite.authorName,
                        avatar = msgLite.authorThumb
                    },
                    channelId = msgLite.channelId,
                    nonce = msgLite.nonce
                });
            }

            return msgs;
        }

        public static void SyncSaveMessage(ChannelMessageView messages) {
            SyncSaveMessages(new List<ChannelMessageView> {messages});
        }
        

        public static void SyncSaveMessages(List<ChannelMessageView> messages) {
            var msgLites = new List<DBMessageLite>();

            foreach (var message in messages) {
                msgLites.Add(new DBMessageLite {
                    messageId = message.id,
                    content = message.content,
                    authorName = message.author.fullName,
                    authorThumb = message.author.avatar,
                    channelId = message.channelId,
                    nonce = message.nonce
                });
            }
            
            SqlManager.SaveMessages(msgLites);
        }

        public static void SyncSaveMessages(List<ChannelMessage> messages) {
            var msgLites = new List<DBMessageLite>();

            foreach (var message in messages) {
                msgLites.Add(new DBMessageLite {
                    messageId = message.id,
                    content = message.content,
                    authorName = message.author.fullName,
                    authorThumb = message.author.avatar,
                    channelId = message.channelId,
                    nonce = string.IsNullOrEmpty(message.nonce) ? 0 : Convert.ToInt64(message.nonce, 16)
                });
            }
            
            SqlManager.SaveMessages(msgLites);
        }
    }
}